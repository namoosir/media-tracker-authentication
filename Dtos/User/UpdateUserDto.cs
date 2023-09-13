using System.ComponentModel.DataAnnotations;
using MediaTrackerAuthenticationService.Models;

namespace MediaTrackerAuthenticationService.Dtos.User
{
    public class UpdateUserDto
    {
        public required int  UserId   { get; set; }
        public required MediaPlatform Platform { get; set; }
        public required string PlatformId { get; set; }
    }
}
