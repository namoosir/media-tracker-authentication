using AutoMapper;
using MediaTrackerAuthenticationService.Dtos.PlatformConnection;
using MediaTrackerAuthenticationService.Models;
using Microsoft.EntityFrameworkCore;
using MediaTrackerAuthenticationService.utils;
using System.Text.Json;
using MediaTrackerAuthenticationService.Data;
using MediaTrackerAuthenticationService.Services.RequestUrlBuilderService;

namespace MediaTrackerAuthenticationService.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IRequestUrlBuilderService _requestUrlBuilderService;
        private readonly HttpClient _httpClient;

        public AuthService(
            IRequestUrlBuilderService requestUrlBuilderService,
            HttpClient httpClient
        )
        {
            _requestUrlBuilderService = requestUrlBuilderService;
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

                if (response.IsSuccessStatusCode)
                {
                    // Call User info API using token
                    // Extract localId
                    // Save user profile under a New user table ("Interal Id", "External Id", "Platform")
                    //
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
