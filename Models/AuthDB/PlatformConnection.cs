using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediaTrackerAuthenticationService.Models;

namespace MediaTrackerAuthenticationService.Models.AuthDB
{
    public class PlatformConnection
    {
        [Key]
        public int UserId { get; set; }

        public required MediaPlatform Platform { get; set; }

        public required string AccessToken { get; set; }

        public required string RefreshToken { get; set; }

        public required string Scopes { get; set; }

        public required DateTime UpdatedAt { get; set; }
    }
}
