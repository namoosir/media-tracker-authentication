using AutoMapper;
using MediaTrackerAuthenticationService.Dtos.PlatformConnection;
using MediaTrackerAuthenticationService.Models;
using Microsoft.EntityFrameworkCore;
using MediaTrackerAuthenticationService.utils;
using System.Text.Json;

namespace MediaTrackerAuthenticationService.Services.PlatformConnectionService
{
    public class PlatformConnectionService : IPlatformConnectionService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PlatformConnectionService(
            IMapper mapper,
            AppDbContext context,
            HttpClient httpClient,
            IConfiguration configuration
        )
        {
            _mapper = mapper;
            _context = context;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<GetPlatformConnectionDto>> AddPlatformConnection(
            AddPlatformConnectionDto newPlatformConnection
        )
        {
            var serviceResponse = new ServiceResponse<GetPlatformConnectionDto>();

            try
            {
                var toInsert = _mapper.Map<PlatformConnection>(newPlatformConnection);
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
            var url = new RedirectAuth(_configuration).CreateUrl(
                "https://www.googleapis.com/auth/youtube.readonly"
            );

            serviceResponse.Data = url;
            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> GetRedirectYoutube(string? code, string? error)
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

                Console.WriteLine($"Authorization Code: {code}");

                TokenRequestParameters requestParams = new TokenRequestParameters(
                    _configuration,
                    code
                );

                var dict = requestParams.parametersToDictionary();
                var content = new FormUrlEncodedContent(dict);

                var response = await _httpClient.PostAsync(requestParams.tokenEndpoint, content);

                Console.WriteLine("HTTP Response:");
                Console.WriteLine($"Status Code: {response.StatusCode}");
                Console.WriteLine($"Headers: {response.Headers}");
                Console.WriteLine($"Content: {await response.Content.ReadAsStringAsync()}");

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);

                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);
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
                }
                else
                {
                    // Handle the error response
                    // You may want to log the error and take appropriate action
                    throw new Exception(
                        $"Token exchange failed with status code {response.StatusCode}"
                    );
                }

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
