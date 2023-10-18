using System.ComponentModel.DataAnnotations;
using MediaTrackerAuthenticationService.Models.Utils;

namespace MediaTrackerAuthenticationService.Dtos.PlatformConnection
{
    public class UpdatePlatformConnectionDto
    {
        [Key]
        public int UserId { get; set; }

        public required MediaPlatform Platform { get; set; }

        public required string AccessToken { get; set; }

        public required string RefreshToken { get; set; }

        public required string Scopes { get; set; }
    }
}
