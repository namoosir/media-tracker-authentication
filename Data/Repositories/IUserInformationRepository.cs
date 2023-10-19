using MediaTrackerAuthenticationService.Models.Utils;
using MediaTrackerAuthenticationService.Models.Redis;

namespace MediaTrackerAuthenticationService.Data;

public interface IUserInformationRepository
{
    Task<bool> CreateUserInformation(UserInformation userInformation);
    Task<UserInformation?> GetUserInformationByUserId(int userId);
    Task<bool> DeleteUserInformationByUserId(int userId);
    Task<bool> UpdateUserInformation(UserInformation newUserInformation);
}