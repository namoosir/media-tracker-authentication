using MediaTrackerAuthenticationService.Models;

namespace MediaTrackerAuthenticationService.Data;

public interface IUserInformationRepository
{
    Task<bool> CreateUserInformation(UserInformation userInformation);
    Task<UserInformation?> GetUserIdByToken(string token);
    Task<bool> DeleteUserInformation(string token);
}