using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using M220N.Models;
using M220N.Models.Responses;
using M220N.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace M220N.Controllers
{
    /// <summary>
    ///     Handles user-based requests (/api/v1/users*)
    /// </summary>
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IOptions<JwtAuthentication> _jwtAuthentication;
        private readonly UsersRepository _userRepository;
        private readonly CommentsRepository _commentsRepository;

        public UserController(UsersRepository usersRepository,
            CommentsRepository commentsRepository,
            IOptions<JwtAuthentication> jwtAuthentication)
        {
            _userRepository = usersRepository;
            _commentsRepository = commentsRepository;
            _jwtAuthentication = jwtAuthentication ?? throw new ArgumentNullException(nameof(jwtAuthentication));
        }

        /// <summary>
        ///     Returns a user
        /// </summary>
        /// <param name="email">The email of the user to return.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Get([RequiredFromQuery] string email)
        {
            var user = await _userRepository.GetUserAsync(email);
            user.AuthToken = _jwtAuthentication.Value.GenerateToken(user);
            return Ok(user);
        }

        /// <summary>
        ///     Adds a user to the system and generates a JWT token for the user
        /// </summary>
        /// <param name="user">A User object.</param>
        /// <returns></returns>
        [HttpPost("/api/v1/user/register")]
        public async Task<ActionResult> AddUser([FromBody] User user)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            if (user.Name.Length < 3)
            {
                errors.Add("name", "Your username must be at least 3 characters long.");
            }
            if (user.Password.Length < 8)
            {
                errors.Add("password", "Your password must be at least 8 characters long.");
            }
            if (errors.Count > 0)
            {
                return BadRequest(new { error = errors });
            }
            var response = await _userRepository.AddUserAsync(user.Name, user.Email, user.Password);
            if (response.User != null) response.User.AuthToken = _jwtAuthentication.Value.GenerateToken(response.User);
            if (!response.Success)
            {
                return BadRequest(new { error = response.ErrorMessage });
            }
            return Ok(response.User);
        }


        /// <summary>
        ///     Logs a user in
        /// </summary>
        /// <param name="user">The User to log in.</param>
        /// <returns></returns>
        [HttpPost("/api/v1/user/login")]
        public async Task<ActionResult> Login([FromBody] User user)
        {
            user.AuthToken = _jwtAuthentication.Value.GenerateToken(user);
            var result = await _userRepository.LoginUserAsync(user);
            return result.User != null ? Ok(new UserResponse(result.User)) : Ok(result);
        }

        /// <summary>
        ///     Logs out a user. Requires a valid JWT token for the user logging out.
        /// </summary>
        /// <returns></returns>
        [HttpPost("/api/v1/user/logout")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> Logout()
        {
            var email = GetUserEmailFromToken(Request);
            if (email.StartsWith("Error")) return BadRequest(email);

            var result = await _userRepository.LogoutUserAsync(email);
            return Ok(result);
        }

        /// <summary>
        ///     Deletes a user from the system. Requires a valid JWT token for the user being deleted.
        /// </summary>
        /// <param name="content">A Json object in the form {"password":"<pwd>"}</pwd></param>
        /// <returns></returns>
        [HttpDelete("/api/v1/user/delete")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> Delete([FromBody] PasswordObject content)
        {
            var email = GetUserEmailFromToken(Request);
            if (email.StartsWith("Error")) return BadRequest(email);

            var user = await _userRepository.GetUserAsync(email);
            if (!PasswordHashOMatic.Verify(content.Password, user.HashedPassword))
                return BadRequest("Provided password does not match user password.");

            return Ok(await _userRepository.DeleteUserAsync(email));
        }

        /// <param name="preferences">Preferences will be in the format: { preferences: {a:"b", c:"D"} }</param>
        /// <returns></returns>
        [HttpPut("/api/v1/user/update-preferences")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> UpdateUserPreferences([FromBody] PreferencesObject preferences)
        {
            var email = GetUserEmailFromToken(Request);
            if (email.StartsWith("Error")) return BadRequest(email);

            var response = await _userRepository.SetUserPreferencesAsync(email, preferences.Preferences);

            if (response.Success == true)
            {
                var user = await _userRepository.GetUserAsync(email);
                return Ok(new UserResponse(user));
            }

            return BadRequest(new UserResponse(false, ""));
        }

        [HttpGet("/api/v1/user/comment-report")]
        public async Task<ActionResult> GenerateCommentReport()
        {
            try
            {
                var checkUser = GetUserEmailFromToken(Request);
                if (checkUser == "Error: Token does not contain an email claim.")
                {
                    return BadRequest(new BsonDocument("error", "not authorized"));
                }

                var result = await _commentsRepository.MostActiveCommentersAsync();
                return (ActionResult) Ok(result);
            }
            catch (Exception)
            {
                return Unauthorized();
            }

        }

        /// <summary>
        /// IMPORTANT: this API is here for testing & verification purposes only. In a "real"
        /// app, you would never have an admin-only endpoint that isn't secured!!
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("/api/v1/user/make-admin")]
        public async Task<ActionResult> MakeAdmin([FromBody] User user)
        {
            try
            {
                var newAdmin = await _userRepository.MakeAdmin(user);
                newAdmin.AuthToken = _jwtAuthentication.Value.GenerateToken(user);
                return Ok(await _userRepository.LoginUserAsync(newAdmin));

            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating an admin user: {ex.Message}");
            }
        }

        /// <summary>
        ///     Utility method for extracting a User's email from the JWT token.
        /// </summary>
        /// <returns>The Email of the User.</returns>
        private static string GetUserEmailFromToken(HttpRequest request)
        {
            var bearer =
                request.Headers.ToArray().First(h => h.Key == "Authorization")
                    .Value.First().Substring(7);

            var jwtHandler = new JwtSecurityTokenHandler();
            var readableToken = jwtHandler.CanReadToken(bearer);
            if (readableToken != true) return "Error: No bearer in the header";

            var token = jwtHandler.ReadJwtToken(bearer);
            var claims = token.Claims;

            var userEmailClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);

            return userEmailClaim == null ? "Error: Token does not contain an email claim." : userEmailClaim.Value;
        }

        public static async Task<User> GetUserFromTokenAsync(UsersRepository _userRepository,
            HttpRequest request)
        {
            var email = GetUserEmailFromToken(request);
            return await _userRepository.GetUserAsync(email);
        }
    }

    /// <summary>
    ///     The mflix client app sends the password as a json object (not string)
    ///     in the request body, like this: {"password": "foo"}.
    ///     This class makes it easy to deserialize it.
    /// </summary>
    public class PasswordObject
    {
        public string Password { get; set; }
    }

    /// <summary>
    ///     The mflix client app sends the password as a json object (not string)
    ///     in the request body, like this: {"password": "foo"}.
    ///     This class makes it easy to deserialize it.
    /// </summary>
    public class PreferencesObject
    {
        public Dictionary<string, string> Preferences { get; set; }
    }
}
