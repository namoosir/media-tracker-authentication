using MediaTrackerAuthenticationService.Models.Utils;

namespace MediaTrackerAuthenticationService.Services.AuthService
{
    public interface IAuthService
    {
        ServiceResponse<string> GetGoogle();
        Task<ServiceResponse<string>> GetRedirectGoogle(string? code, string? error, string? state);

        Task<ServiceResponse<string>> RefreshSession(int userId);
        Task<ServiceResponse<string>> LogoutSession(int userId);
    }
}
