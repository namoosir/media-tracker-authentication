using MediaTrackerAuthenticationService.Models.Utils;

namespace MediaTrackerAuthenticationService.Services.RequestUrlBuilderService
{
    public interface IRequestUrlBuilderService
    {
        ServiceResponse<string> BuildGoogleAuthRequest(OauthRequestType type);
        ServiceResponse<(string endpoint, HttpContent body)> BuildGoogleTokenRequest(
            OauthRequestType type,
            string code
        );

        ServiceResponse<(string endpoint, HttpContent body)> BuildGoogleRefreshTokensReqest(
            string refresh_token
        );

        ServiceResponse<string> BuildGoogleUserInfoRequest();
    }
}
