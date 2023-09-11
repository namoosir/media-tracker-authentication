using System.ComponentModel.DataAnnotations;

namespace MediaTrackerAuthenticationService.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public required MediaPlatform Platform { get; set; }

        public required string PlatformId  { get; set; }
    }
}