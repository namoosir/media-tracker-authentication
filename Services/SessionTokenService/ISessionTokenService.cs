using MediaTrackerAuthenticationService.Models.Utils;

namespace MediaTrackerAuthenticationService.Services.SessionTokenService
{
    public interface ISessionTokenService
    {
        string GenerateToken(int userId);
    }
}
