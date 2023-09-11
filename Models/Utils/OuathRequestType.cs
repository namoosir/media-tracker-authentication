using System.Text.Json.Serialization;

namespace MediaTrackerAuthenticationService.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OauthRequestType
    {
        GoogleLogin = 1,
        Youtube = 2,
    }
}
