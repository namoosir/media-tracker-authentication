using System.ComponentModel.DataAnnotations;
using MediaTrackerAuthenticationService.Models.AuthDB;

namespace MediaTrackerAuthenticationService.Dtos.PlatformConnection
{
    public class GetPlatformConnectionDto
    {
        [Key]
        public int UserId { get; set; }

        public required MediaPlatform Platform { get; set; }

        public required string AccessToken { get; set; }

        public required string RefreshToken { get; set; }

        public required string Scopes { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
