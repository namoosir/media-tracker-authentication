using System.ComponentModel.DataAnnotations;

namespace MediaTrackerAuthenticationService.Models
{
    public class PlatformConnection
    {
        [Key]
        public int UserId { get; set; }

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
