using MediaTrackerAuthenticationService.Models;

namespace MediaTrackerAuthenticationService.Services.RequestUrlBuilderService
{
    public interface IRequestUrlBuilderService
    {
        ServiceResponse<string> BuildGoogleAuthRequest(OauthRequestType type);
        ServiceResponse<(string endpoint, HttpContent body)> BuildGoogleTokenRequest(
            OauthRequestType type,
            string code
        );
    }
}
