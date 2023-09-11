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
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net.Http.Headers;


namespace MediaTrackerAuthenticationService.Services.HttpRequestService;
public class HttpRequestService : IHttpRequestService
{
        private readonly IRequestUrlBuilderService _requestUrlBuilderService;
        private readonly HttpClient _httpClient;
        public HttpRequestService (
            IRequestUrlBuilderService requestUrlBuilderService,
            HttpClient httpClient
        )
        {
            _requestUrlBuilderService = requestUrlBuilderService;
            _httpClient = httpClient;
        }

    public async Task<ServiceResponse<TokenResponse>> GetTokensGoogle(OauthRequestType requestType, string code)
    {
        var request = _requestUrlBuilderService.BuildGoogleTokenRequest(requestType ,code);
        var response = await _httpClient.PostAsync(
            request.Data.endpoint,
            request.Data.body
        );
        var responseContentString = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode) {
            var deserialzedContent = JsonSerializer.Deserialize<TokenResponse>(responseContentString);
            return ServiceResponseBuilder.build(deserialzedContent);
        } else {
            throw new Exception($"Token exchange failed with status code {response.StatusCode}");
        }

    }

    public async Task<ServiceResponse<UserInfoResponse>> GetUserInfoGoogle(string accessToken)
    {
        var userInfoUrl = _requestUrlBuilderService.BuildGoogleUserInfoRequest();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await _httpClient.GetAsync(userInfoUrl.Data);

        if (response.IsSuccessStatusCode) {
            var responseContentString = await response.Content.ReadAsStringAsync();

            var deserialzedContent = JsonSerializer.Deserialize<UserInfoResponse>(responseContentString);
            return ServiceResponseBuilder.build(deserialzedContent);
        } else {
            throw new HttpRequestException($"Error calling userinfo endpoint: {response.StatusCode}");
        }

        

    }

}

