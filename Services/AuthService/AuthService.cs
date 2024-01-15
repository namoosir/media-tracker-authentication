using AutoMapper;
using MediaTrackerAuthenticationService.Dtos.PlatformConnection;
using MediaTrackerAuthenticationService.Dtos.User;
using MediaTrackerAuthenticationService.Models.Utils;
using MediaTrackerAuthenticationService.Models.AuthDB;
using Microsoft.EntityFrameworkCore;
using MediaTrackerAuthenticationService.utils;
using System.Text.Json;
using MediaTrackerAuthenticationService.Data;
using MediaTrackerAuthenticationService.Services.RequestUrlBuilderService;
using MediaTrackerAuthenticationService.Services.SessionTokenService;
using MediaTrackerAuthenticationService.Services.HttpRequestService;
using System.Net.Http.Headers;
using MediaTrackerAuthenticationService.Controllers;
using MediaTrackerAuthenticationService.Services.UserService;
using MediaTrackerAuthenticationService.Models.Redis;

namespace MediaTrackerAuthenticationService.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IRequestUrlBuilderService _requestUrlBuilderService;
        private readonly ISessionTokenService _sessionTokenService;
        private readonly IHttpRequestService _httpRequestService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserInformationController _userInformationController;
        private readonly IUserService _userService;
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;

        public AuthService(
            IRequestUrlBuilderService requestUrlBuilderService,
            ISessionTokenService sessionTokenService,
            IHttpRequestService httpRequestService,
            UserInformationController userInformationController,
            IUserService userService,
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor,
            AppDbContext appDbContext,
            IMapper mapper,
            IConfiguration configuration
        )
        {
            _requestUrlBuilderService = requestUrlBuilderService;
            _sessionTokenService = sessionTokenService;
            _httpRequestService = httpRequestService;
            _userInformationController = userInformationController;
            _userService = userService;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = appDbContext;
            _mapper = mapper;
            _configuration = configuration;
        }

        public ServiceResponse<string> GetGoogle()
        {
            var url = _requestUrlBuilderService.BuildGoogleAuthRequest(OauthRequestType.GoogleLogin);
            return url;
        }

        public async Task<ServiceResponse<string>> GetRedirectGoogle(string? code, string? error, string? state)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    //TODO: figure out some way to do error state
                    Console.WriteLine("ISSUES" + error);
                    serviceResponse.Data = "http://localhost:5173/" + "?error=failed";
                    throw new Exception(error);
                }

                if (string.IsNullOrEmpty(state) || state != _configuration["GoogleOauth:State"]){
                    // Someone not Google has called this endpoint, They are not authorized
                    throw new Exception("UnAuthorized call for this endpoint");
                }

                string accessToken = (await _httpRequestService.GetTokensGoogle(OauthRequestType.GoogleLogin, code)).Data!.access_token;

                string externalUserId = (await _httpRequestService.GetUserInfoGoogle(accessToken)).Data!.sub;

                var user = new UpsertUserDto
                {
                    Platform = MediaPlatform.Youtube,
                    PlatformId = externalUserId
                };

                var userId = (await _userService.UpsertUser(user)).Data!.UserId;

                string sessionToken;
                var response = await _userInformationController.GetByUserId(userId);


                if (response.Success) sessionToken = response.Data!.Token;
                else
                {
                    sessionToken = _sessionTokenService.GenerateToken(userId);
                    var createdResult = await _userInformationController.CreateUserInformation(UserInformation.Build(sessionToken, userId));
                    if (!createdResult.Success) throw new Exception(createdResult.Message);
                }

                // _httpContextAccessor.HttpContext.Response.Headers.Add("Authorization", "Bearer " + sessionToken);

                serviceResponse.Data = $"http://localhost:5173/oauthHandleRedirect?token={sessionToken}";

            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> RefreshSession(int userId)
        {
            var newSessionToken = _sessionTokenService.GenerateToken(userId);

            var updateResult = await _userInformationController.UpdateUserInformation(UserInformation.Build(newSessionToken, userId));

            if (!updateResult.Success)  return ServiceResponse<string>.Build(null, false, "Refresh failed");

            _httpContextAccessor.HttpContext!.Response.Headers.Add("Authorization", "Bearer " + newSessionToken);
            return ServiceResponse<string>.Build("http://localhost:5173/", true, null);
        }

        public async Task<ServiceResponse<string>> LogoutSession(int userId)
        {
            var deleteResult = await _userInformationController.DeleteUserInformationByUserId(userId);

            if (!deleteResult.Success)  return ServiceResponse<string>.Build(null, false, "Delete failed");

            return ServiceResponse<string>.Build("Logout Sucess", true, null);
        }
    }
}
