using AutoMapper;
using MediaTrackerAuthenticationService.Dtos.PlatformConnection;
using MediaTrackerAuthenticationService.Dtos.User;
using MediaTrackerAuthenticationService.Models;
using Microsoft.EntityFrameworkCore;
using MediaTrackerAuthenticationService.utils;
using System.Text.Json;
using MediaTrackerAuthenticationService.Data;
using MediaTrackerAuthenticationService.Services.RequestUrlBuilderService;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net.Http.Headers;

namespace MediaTrackerAuthenticationService.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IRequestUrlBuilderService _requestUrlBuilderService;
        private readonly HttpClient _httpClient;

        private readonly AppDbContext _dbContext;

        private readonly IMapper _mapper;

        public AuthService(
            IRequestUrlBuilderService requestUrlBuilderService,
            HttpClient httpClient,
            AppDbContext appDbContext,
            IMapper mapper
        )
        {
            _requestUrlBuilderService = requestUrlBuilderService;
            _httpClient = httpClient;
            _dbContext = appDbContext;
            _mapper = mapper;
        }

        public ServiceResponse<string> GetGoogle()
        {
            var url = _requestUrlBuilderService.BuildGoogleAuthRequest(OauthRequestType.Login);
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

                Console.WriteLine($"Authorization Code: {code}");

                var request = _requestUrlBuilderService.BuildGoogleTokenRequest(
                    OauthRequestType.Login,
                    code
                );

                var response = await _httpClient.PostAsync(
                    request.Data.endpoint,
                    request.Data.body
                );

                var responseContent = await response.Content.ReadAsStringAsync();


                
                Console.WriteLine("HTTP Response:");
                Console.WriteLine($"Status Code: {response.StatusCode}");
                Console.WriteLine($"Headers: {response.Headers}");
                Console.WriteLine($"Content: {await response.Content.ReadAsStringAsync()}");

                if (response.IsSuccessStatusCode)
                {
                    var deserialzedContent = JsonSerializer.Deserialize<TokenResponse>(responseContent);
                    string accessToken = deserialzedContent.access_token;

                    var userInfoUrl = _requestUrlBuilderService.BuildGoogleUserInfoRequest();
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    response = await _httpClient.GetAsync(userInfoUrl.Data);

                    if (response.IsSuccessStatusCode) {
                        string content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"CONTENT: {await response.Content.ReadAsStringAsync()}");

                        var deserialzedContent2 = JsonSerializer.Deserialize<UserInfoResponse>(content);
                        string externalUserId = deserialzedContent2.sub;

                        var exampleDto = new AddUserDto {
                            Platform = MediaPlatform.Youtube,
                            PlatformId = externalUserId
                        };

                        Console.WriteLine($" lksflkdsmlkf {exampleDto.PlatformId}");

                        var toInsert = _mapper.Map<User>(exampleDto);
                        _dbContext.Users.Add(toInsert);
                        await _dbContext.SaveChangesAsync();
                        }
                    else {
                        throw new HttpRequestException($"Error calling userinfo endpoint: {response.StatusCode}");
                    }
                    
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
