using MediaTrackerAuthenticationService.Models.Utils;
using MediaTrackerAuthenticationService.utils;

namespace MediaTrackerAuthenticationService.Services.HttpRequestService
{
    public interface IHttpRequestService
    {
        Task<ServiceResponse<TokenResponse>> GetTokensGoogle(OauthRequestType requestType, string code);
        Task<ServiceResponse<UserInfoResponse>> GetUserInfoGoogle(string accessToken);
        Task<ServiceResponse<TokenResponse>> YoutubeGetRefreshedTokens(string refreshToken);
    }
}
