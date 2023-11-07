using AutoMapper;
using MediaTrackerAuthenticationService.Dtos.PlatformConnection;
using MediaTrackerAuthenticationService.Models.Utils;
using Microsoft.EntityFrameworkCore;
using MediaTrackerAuthenticationService.utils;
using System.Text.Json;
using MediaTrackerAuthenticationService.Data;
using MediaTrackerAuthenticationService.Services.RequestUrlBuilderService;
using MediaTrackerAuthenticationService.Services.HttpRequestService;
using MediaTrackerAuthenticationService.Models.AuthDB;



namespace MediaTrackerAuthenticationService.Services.PlatformConnectionService
{
    public class PlatformConnectionService : IPlatformConnectionService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly IRequestUrlBuilderService _requestUrlBuilderService;
        private readonly IHttpRequestService _httpRequestService;

        public PlatformConnectionService(
            IMapper mapper,
            AppDbContext context,
            HttpClient httpClient,
            IConfiguration configuration,
            IRequestUrlBuilderService requestUrlBuilderService,
            IHttpRequestService httpRequestService
        )
        {
            _mapper = mapper;
            _context = context;
            _httpClient = httpClient;
            _configuration = configuration;
            _requestUrlBuilderService = requestUrlBuilderService;
            _httpRequestService = httpRequestService;
        }

        public async Task<ServiceResponse<GetPlatformConnectionDto>> AddPlatformConnection(
            AddPlatformConnectionDto newPlatformConnection
        )
        {
            var serviceResponse = new ServiceResponse<GetPlatformConnectionDto>();

            try
            {
                var toInsert = _mapper.Map<PlatformConnection>(newPlatformConnection);
                Console.WriteLine($"Authorization Code: {toInsert.UserId}");

                _context.PlatformConnections.Add(toInsert);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetPlatformConnectionDto>(toInsert);
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPlatformConnectionDto>> GetPlatformConnectionByUserId(
            int userID
        )
        {
            var serviceResponse = new ServiceResponse<GetPlatformConnectionDto>();

            try
            {
                var platformConnection = await _context.PlatformConnections.FirstOrDefaultAsync(
                    platform => platform.UserId == userID
                );

                if (platformConnection is null)
                {
                    throw new Exception($"Platform Connection with User Id '{userID}' not found");
                }

                serviceResponse.Data = _mapper.Map<GetPlatformConnectionDto>(platformConnection);
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPlatformConnectionDto>> UpdatePlatformConnection(
            UpdatePlatformConnectionDto updatedPlatformConnection
        )
        {
            var serviceResponse = new ServiceResponse<GetPlatformConnectionDto>();

            try
            {
                var platformConnection = await _context.PlatformConnections.FirstOrDefaultAsync(
                    platform => platform.UserId == updatedPlatformConnection.UserId
                );

                if (platformConnection is null)
                {
                    throw new Exception(
                        $"Platform Connection with User Id '{updatedPlatformConnection.UserId}' not found"
                    );
                }

                platformConnection.AccessToken = updatedPlatformConnection.AccessToken;
                platformConnection.Platform = updatedPlatformConnection.Platform;
                platformConnection.RefreshToken = updatedPlatformConnection.RefreshToken;
                platformConnection.Scopes = updatedPlatformConnection.Scopes;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetPlatformConnectionDto>(platformConnection);
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;
        }

        public ServiceResponse<string> GetYoutube()
        {
            var serviceResponse = new ServiceResponse<string>();
            var url = _requestUrlBuilderService.BuildGoogleAuthRequest(OauthRequestType.Youtube);

            serviceResponse.Data = url.Data;
            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> GetRedirectYoutube(string? code, string? error, string? state)
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

                if (string.IsNullOrEmpty(state) || state != _configuration["GoogleOauth:State"]){
                    // Someone not Google has called this endpoint, They are not authorized
                    throw new Exception("UnAuthorized call for this endpoint");
                }


                Console.WriteLine($"Authorization Code: {code}");



                var tokenResponse = (await _httpRequestService.GetTokensGoogle(OauthRequestType.Youtube, code)).Data;

                Console.WriteLine("ACCESSTOKEN:  " + tokenResponse!.access_token);
                var exampleDto = new AddPlatformConnectionDto
                {
                    Platform = MediaPlatform.Youtube, // You should replace this with the appropriate platform
                    AccessToken = tokenResponse.access_token,
                    RefreshToken = tokenResponse.refresh_token,
                    Scopes = tokenResponse.scope // Replace with the required scopes
                };

                var toInsert = _mapper.Map<PlatformConnection>(exampleDto);
                _context.PlatformConnections.Add(toInsert);
                await _context.SaveChangesAsync();

                //everything succeeded at this point so redirect properly
                serviceResponse.Data = "http://localhost:5173/";
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            // You can also use 'state' for additional validation or context if needed.

            // No content is returned to the client.
            return serviceResponse;
        }
    }
}
