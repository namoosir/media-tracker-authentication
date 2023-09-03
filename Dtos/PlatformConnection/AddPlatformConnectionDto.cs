using System.ComponentModel.DataAnnotations;
using MediaTrackerAuthenticationService.Models;

namespace MediaTrackerAuthenticationService.Dtos.PlatformConnection
{
    public class AddPlatformConnectionDto
    {
        public required MediaPlatform Platform { get; set; }

        public required string AccessToken { get; set; }

        public required string RefreshToken { get; set; }

        public required string Scopes { get; set; }
    }
}
