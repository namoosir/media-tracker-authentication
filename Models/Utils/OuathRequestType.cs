using System.Text.Json.Serialization;

namespace MediaTrackerAuthenticationService.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OauthRequestType
    {
        Login = 1,
        Youtube = 2,
    }
}
