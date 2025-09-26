using M220N.Models.Projections;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace M220N.Models.Responses
{
    public class ConfigResponse
    {
        public ConfigResponse(ConfigInfo configInfo)
        {
            MaxPoolSize = configInfo.Settings.MaxConnectionPoolSize;
            WriteTimeout = configInfo.Settings.WriteConcern.WTimeout.GetValueOrDefault().TotalMilliseconds;
            AuthenticatedUserRoles = configInfo.AuthInfo.AuthenticatedUserRoles;
            Role = configInfo.AuthInfo.AuthenticatedUserRoles.Count > 0
                ? configInfo.AuthInfo.AuthenticatedUserRoles[0][0].AsString
                : string.Empty;
        }

        [JsonProperty("pool_size", NullValueHandling = NullValueHandling.Ignore)]
        public int MaxPoolSize { get; set; }

        [JsonProperty("wtimeout", NullValueHandling = NullValueHandling.Ignore)]
        public double WriteTimeout { get; set; }

        [JsonProperty("auth_info", NullValueHandling = NullValueHandling.Ignore)]
        public BsonArray AuthenticatedUserRoles { get; set; }

        public string Role { get; set; }
    }
}
