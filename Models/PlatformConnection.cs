using System.ComponentModel.DataAnnotations;

namespace MediaTrackerAuthenticationService.Models
{
    public class PlatformConnection
    {
        [Key]
        public int UserId { get; set; }

        public required MediaPlatform Platform { get; set; }

        public required string AccessToken { get; set; }

        public required string RefreshToken { get; set; }

        public required string Scopes { get; set; }
    }
}
