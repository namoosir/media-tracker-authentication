using System.ComponentModel.DataAnnotations;
using MediaTrackerAuthenticationService.Models;

namespace MediaTrackerAuthenticationService.Dtos.Auth
{
    public class WhoamiDto
    {
        public required string UserId { get; set; }
    }
}
