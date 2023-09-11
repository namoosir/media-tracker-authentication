using AutoMapper;
using MediaTrackerAuthenticationService.Dtos.PlatformConnection;
using MediaTrackerAuthenticationService.Dtos.User;
using MediaTrackerAuthenticationService.Models;
using Microsoft.EntityFrameworkCore;
using MediaTrackerAuthenticationService.utils;
using System.Text.Json;
using MediaTrackerAuthenticationService.Data;
using MediaTrackerAuthenticationService.Services.RequestUrlBuilderService;
using MediaTrackerAuthenticationService.Services.SessionTokenService;
using MediaTrackerAuthenticationService.Services.HttpRequestService;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net.Http.Headers;

namespace MediaTrackerAuthenticationService.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IRequestUrlBuilderService _requestUrlBuilderService;
        private readonly ISessionTokenService _sessionTokenService;
        private readonly IHttpRequestService _httpRequestService;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        private readonly AppDbContext _dbContext;

        private readonly IMapper _mapper;

        public AuthService(
            IRequestUrlBuilderService requestUrlBuilderService,
            ISessionTokenService sessionTokenService,
            IHttpRequestService httpRequestService,
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor,
            AppDbContext appDbContext,
            IMapper mapper
        )
        {
            _requestUrlBuilderService = requestUrlBuilderService;
            _sessionTokenService = sessionTokenService;
            _httpRequestService = httpRequestService;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = appDbContext;
            _mapper = mapper;
        }

        public ServiceResponse<string> GetGoogle()
        {
            var url = _requestUrlBuilderService.BuildGoogleAuthRequest(OauthRequestType.GoogleLogin);
            return url;
        }

        public async Task<ServiceResponse<string>> GetRedirectGoogle(string? code, string? error)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    //figure out some way to do error state
                    Console.WriteLine("ISSUES" + error);
                    serviceResponse.Data = "https://google.com" + "?error=" + error;
                    throw new Exception(error);
                }

                string accessToken = (await _httpRequestService.GetTokensGoogle(OauthRequestType.GoogleLogin, code)).Data.access_token;  

                string externalUserId = (await _httpRequestService.GetUserInfoGoogle(accessToken)).Data.sub;

                var exampleDto = new AddUserDto {
                    Platform = MediaPlatform.Youtube,
                    PlatformId = externalUserId
                };

                var toInsert = _mapper.Map<User>(exampleDto);
                _dbContext.Users.Add(toInsert);
                await _dbContext.SaveChangesAsync();

                var sessionToken = _sessionTokenService.GenerateToken(toInsert.UserId);

                _httpContextAccessor.HttpContext.Response.Headers.Add("Authorization", "Bearer " + sessionToken);

                // Set the response data or message
                serviceResponse.Data = "http://localhost:5173/";

            }
            catch (Exception e) {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }
    }
}
