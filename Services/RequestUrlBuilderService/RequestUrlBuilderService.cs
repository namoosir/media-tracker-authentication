using AutoMapper;
using MediaTrackerAuthenticationService.Dtos.PlatformConnection;
using MediaTrackerAuthenticationService.Models.Utils;
using Microsoft.EntityFrameworkCore;
using MediaTrackerAuthenticationService.utils;
using System.Text.Json;
using MediaTrackerAuthenticationService.Data;

namespace MediaTrackerAuthenticationService.Services.RequestUrlBuilderService
{
    public class RequestUrlBuilderService : IRequestUrlBuilderService
    {
        private readonly IConfiguration _configuration;

        public RequestUrlBuilderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ServiceResponse<string> BuildGoogleAuthRequest(OauthRequestType type)
        {
            var serviceResponse = new ServiceResponse<string>();
            string endpoint = _configuration["GoogleOauth:Endpoint:Auth"];
            string clientId = _configuration["GoogleOauth:ClientId"];
            string siteDomain = _configuration["Site:Domain"];
            string stateSecret = _configuration["GoogleOauth:State"];
            string responseType = "code";
            string accessType = "offline";
            string prompt = "consent";

            string scope =
                (type == OauthRequestType.GoogleLogin)
                    ? _configuration["Site:RequestScopes:Login:Google"]
                    : (type == OauthRequestType.Youtube)
                        ? _configuration["Site:RequestScopes:Resource:Youtube"]
                        : throw new ArgumentException("Invalid OauthRequestType");

            string handlerEndpoint =
                (type == OauthRequestType.GoogleLogin)
                    ? _configuration["Site:OauthRedirectPath:Login:Google"]
                    : (type == OauthRequestType.Youtube)
                        ? _configuration["Site:OauthRedirectPath:Resource:Youtube"]
                        : throw new ArgumentException("Invalid OauthRequestType");

            string oauthUrl =
                $"{endpoint}"
                + $"?client_id={clientId}"
                + $"&redirect_uri={siteDomain + '/' + handlerEndpoint}"
                + $"&scope={scope}"
                + $"&response_type={responseType}"
                + $"&access_type={accessType}"
                + $"&prompt={prompt}"
                + $"&state={stateSecret}";

            serviceResponse.Data = oauthUrl;
            return serviceResponse;
        }

        public ServiceResponse<(string endpoint, HttpContent body)> BuildGoogleTokenRequest(
            OauthRequestType type,
            string code
        )
        {
            var serviceResponse = new ServiceResponse<(string, HttpContent)>();
            string endpoint = _configuration["GoogleOauth:Endpoint:Token"];
            string clientId = _configuration["GoogleOauth:ClientId"];
            string clientSecret = _configuration["GoogleOauth:ClientSecret"];
            string siteDomain = _configuration["Site:Domain"];
            string grant_type = "authorization_code";

            string handlerEndpoint =
                (type == OauthRequestType.GoogleLogin)
                    ? _configuration["Site:OauthRedirectPath:Login:Google"]
                    : (type == OauthRequestType.Youtube)
                        ? _configuration["Site:OauthRedirectPath:Resource:Youtube"]
                        : throw new ArgumentException("Invalid OauthRequestType");

            var parameters = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "code", code },
                { "redirect_uri", siteDomain + '/' +  handlerEndpoint },
                { "grant_type", grant_type }
            };

            var body = new FormUrlEncodedContent(parameters);

            serviceResponse.Data = (endpoint, body);
            return serviceResponse;
        }

        public ServiceResponse<(string endpoint, HttpContent body)> BuildGoogleRefreshTokensReqest(
            string refresh_token
        )
        {
            string endpoint = _configuration["GoogleOauth:Endpoint:Token"];
            string clientId = _configuration["GoogleOauth:ClientId"];
            string clientSecret = _configuration["GoogleOauth:ClientSecret"];
            string grant_type = "refresh_token";

            var parameters = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "refresh_token", refresh_token },
                { "grant_type", grant_type }
            };

            var body = new FormUrlEncodedContent(parameters);
            return ServiceResponse<(string, HttpContent)>.Build((endpoint, body), true, null);
        }

        public ServiceResponse<string> BuildGoogleUserInfoRequest()
        {
            var serviceResponse = new ServiceResponse<string>();
            serviceResponse.Data = _configuration["GoogleOauth:Endpoint:UserInfo"];
            return serviceResponse;
        }
    }


}
