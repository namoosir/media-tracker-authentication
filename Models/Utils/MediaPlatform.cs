using System.Text.Json.Serialization;

namespace MediaTrackerAuthenticationService.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MediaPlatform
    {
        YouTube = 1,
        Facebook = 2
    }
}
