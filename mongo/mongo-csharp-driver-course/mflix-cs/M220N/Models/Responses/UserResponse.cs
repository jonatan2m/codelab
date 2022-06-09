using Newtonsoft.Json;

namespace M220N.Models.Responses
{
    public class UserResponse
    {
        public UserResponse(User user)
        {
            Success = true;
            User = user;
            AuthToken = user.AuthToken;
        }

        public UserResponse(bool success, string message)
        {
            Success = success;
            if (success) SuccessMessage = message;
            else ErrorMessage = message;
        }

        public bool Success { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        [JsonProperty("auth_token")]
        public string AuthToken { get; set; }

        [JsonProperty("info")]
        public User User { get; set; }
    }
}
