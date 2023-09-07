using MediaTrackerAuthenticationService.Data;
using MediaTrackerAuthenticationService.Models;

namespace MediaTrackerAuthenticationService.Controllers;

public class UserInformationController
{
    private readonly IUserInformationRepository _userInformationRepository;

    public UserInformationController(IUserInformationRepository userInformationRepository)
    {
        _userInformationRepository = userInformationRepository;
    }

    public async Task<ServiceResponse<UserInformation>> GetUserIdByToken(string token)
    {
        var serviceResponse = new ServiceResponse<UserInformation>
        {
            Data = await _userInformationRepository.GetUserIdByToken(token)
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

    public async Task<ServiceResponse<bool>> DeleteUserInformation(string token)
    {
        var serviceResponse = new ServiceResponse<bool>
        {
            Data = await _userInformationRepository.DeleteUserInformation(token)
        };

        if (!serviceResponse.Data)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = "Failed to delete user";
        }
        return serviceResponse;
    }
}