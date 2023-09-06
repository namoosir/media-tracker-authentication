using MediaTrackerAuthenticationService.Models;

namespace MediaTrackerAuthenticationService.Services.RequestUrlBuilderService
{
    public interface IRequestUrlBuilderService
    {
        string BuildGoogleAuthRequest(OauthRequestType type);
        (string endpoint, HttpContent body) BuildGoogleTokenRequest(
            OauthRequestType type,
            string code
        );
    }
}
