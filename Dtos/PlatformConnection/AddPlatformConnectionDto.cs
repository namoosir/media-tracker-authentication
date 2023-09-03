using System.ComponentModel.DataAnnotations;
using MediaTrackerAuthenticationService.Models;

namespace MediaTrackerAuthenticationService.Dtos.PlatformConnection
{
    public class AddPlatformConnectionDto
    {
        [Required]
        public MediaPlatform Platform { get; set; }

        [Required]
        public required string AccessToken { get; set; }

        [Required]
        public required string RefreshToken { get; set; }

        [Required]
        public required string Scopes { get; set; }
    }
}
