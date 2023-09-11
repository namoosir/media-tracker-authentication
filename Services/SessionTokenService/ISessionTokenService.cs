using MediaTrackerAuthenticationService.Models;

namespace MediaTrackerAuthenticationService.Services.SessionTokenService
{
    public interface ISessionTokenService
    {
        string GenerateToken(int userId);
    }
}
