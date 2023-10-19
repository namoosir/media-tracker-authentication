using System.ComponentModel.DataAnnotations;
using MediaTrackerAuthenticationService.Models.Utils;

namespace MediaTrackerAuthenticationService.Models.AuthDB
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public required MediaPlatform Platform { get; set; }

        public required string PlatformId  { get; set; }
    }
}