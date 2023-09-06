using MediaTrackerAuthenticationService.Models;

namespace MediaTrackerAuthenticationService.Services.AuthService
{
    public interface IAuthService
    {
        ServiceResponse<string> GetGoogle();
        Task<ServiceResponse<string>> GetRedirectGoogle(string? code, string? error);
    }
}
