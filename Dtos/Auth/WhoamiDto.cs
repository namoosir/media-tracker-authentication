using System.ComponentModel.DataAnnotations;
using MediaTrackerAuthenticationService.Models;

namespace MediaTrackerAuthenticationService.Dtos.Auth
{
    public class WhoamiDto
    {
        public required string userId { get; set; }
    }
}
