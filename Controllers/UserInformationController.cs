using MediaTrackerAuthenticationService.Data;
using MediaTrackerAuthenticationService.Models.Utils;
using MediaTrackerAuthenticationService.Models.Redis;
using Microsoft.AspNetCore.Mvc;

namespace MediaTrackerAuthenticationService.Controllers;

public class UserInformationController
{
    private readonly IUserInformationRepository _userInformationRepository;

    public UserInformationController(IUserInformationRepository userInformationRepository)
    {
        _userInformationRepository = userInformationRepository;
    }

    public async Task<ServiceResponse<UserInformation>> GetByUserId(int userId)
    {
        var serviceResponse = new ServiceResponse<UserInformation>
        {
            Data = await _userInformationRepository.GetUserInformationByUserId(userId)
        };

        if (serviceResponse.Data is null)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = "No entry found";
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<bool>> CreateUserInformation(UserInformation userInformation)
    {
        var serviceResponse = new ServiceResponse<bool>
        {
            Data = await _userInformationRepository.CreateUserInformation(userInformation)
        };

        if (!serviceResponse.Data)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = "Failed to add user";
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<bool>> DeleteUserInformationByUserId(int userId)
    {
        var serviceResponse = new ServiceResponse<bool>
        {
            Data = await _userInformationRepository.DeleteUserInformationByUserId(userId)
        };

        if (!serviceResponse.Data)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = "Failed to delete user";
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<bool>> UpdateUserInformation(UserInformation newUserInformation)
    {
        var serviceResponse = new ServiceResponse<bool>
        {
            Data = await _userInformationRepository.UpdateUserInformation(newUserInformation)
        };

        if (!serviceResponse.Data)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = "Failed to update user";
        }
        return serviceResponse;
    }
}