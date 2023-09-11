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


namespace MediaTrackerAuthenticationService.utils
{
    public class ServiceResponseBuilder{
    

        public static ServiceResponse<T> build<T>(T data)
        {
            var serviceResponse = new ServiceResponse<T>();
            serviceResponse.Data = data;
            return serviceResponse;
        }
        
    }
}