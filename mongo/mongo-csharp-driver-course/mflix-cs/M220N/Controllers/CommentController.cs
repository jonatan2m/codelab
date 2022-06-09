using System;
using System.Linq;
using System.Threading.Tasks;
using M220N.Models.Responses;
using M220N.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace M220N.Controllers
{
    public class CommentController : ControllerBase
    {
        private readonly CommentsRepository _commentsRepository;
        private readonly IOptions<JwtAuthentication> _jwtAuthentication;
        private readonly UsersRepository _userRepository;

        public CommentController(CommentsRepository commentsRepository,
            UsersRepository userRepository, IOptions<JwtAuthentication> jwtAuthentication)
        {
            _commentsRepository = commentsRepository;
            _userRepository = userRepository;
            _jwtAuthentication = jwtAuthentication ?? throw new ArgumentNullException(nameof(jwtAuthentication));
        }

        /// <summary>
        ///     Adds a comment
        /// </summary>
        /// <param name="input">A JSON object in the body of the message.</param>
        /// <returns></returns>
        [HttpPost("/api/v1/movies/comment")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> AddComment([FromBody] MovieCommentInput input)
        {
            var user = await UserController.GetUserFromTokenAsync(_userRepository, Request);

            var movieId = new ObjectId(input.MovieId);
            var result = await _commentsRepository.AddCommentAsync(user, movieId, input.Comment);

            return result != null
                ? (ActionResult) Ok(new CommentResponse(
                    result.Comments.OrderByDescending(d => d.Date).ToList()))
                : BadRequest(new CommentResponse());
        }

        /// <summary>
        ///     Updates an existing comment. Validates the user with JWT to ensure
        ///     the user updating the comment is the original commentator.
        /// </summary>
        /// <param name="input">A JSON object in the body of the message.</param>
        /// <returns></returns>
        [HttpPut("/api/v1/movies/comment")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> UpdateCommentAsync([FromBody] MovieCommentInput input)
        {
            var user = await UserController.GetUserFromTokenAsync(_userRepository, Request);

            var movieId = new ObjectId(input.MovieId);
            var commentId = new ObjectId(input.CommentId);
            var result = await _commentsRepository.UpdateCommentAsync(user, movieId, commentId, input.UpdatedComment);

            return result.IsAcknowledged && result.ModifiedCount == 1
                ? (ActionResult) Ok(new BsonDocument("status", "success"))
                : BadRequest(new BsonDocument("status", "error"));
        }

        /// <summary>
        ///     Deletes an existing comment. Validates the user with JWT to ensure
        ///     the user updating the comment is the original commentator.
        /// </summary>
        /// <param name="input">A JSON object in the body of the message.</param>
        /// <returns></returns>
        [HttpDelete("/api/v1/movies/comment")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> DeleteCommentAsync([FromBody] MovieCommentInput input)
        {
            var movieId = new ObjectId(input.MovieId);
            var commentId = new ObjectId(input.CommentId);
            var user = await UserController.GetUserFromTokenAsync(_userRepository, Request);
            var result = await _commentsRepository.DeleteCommentAsync(movieId, commentId, user);

            return result != null
                ? (ActionResult) Ok(new CommentResponse(
                    result.Comments.OrderByDescending(d => d.Date).ToList()))
                : BadRequest(new CommentResponse());
        }

    }

    /// <summary>
    ///     Helper class to define the expected JSON object passed in the
    ///     Request body.
    /// </summary>
    public class MovieCommentInput
    {
        [JsonProperty("movie_id")]
        public string MovieId { get; set; }

        [JsonProperty("comment_id")]
        public string CommentId { get; set; }

        public string Comment { get; set; }

        [JsonProperty("updated_comment")]
        public string UpdatedComment { get; set; }
    }
}
