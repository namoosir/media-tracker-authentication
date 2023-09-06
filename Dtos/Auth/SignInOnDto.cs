using System.ComponentModel.DataAnnotations;
using MediaTrackerAuthenticationService.Models;

namespace MediaTrackerAuthenticationService.Dtos.Auth
{
    public class SignInDto
    {
        public required string Token { get; set; }
    }
}
