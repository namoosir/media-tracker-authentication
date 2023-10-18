using MediaTrackerAuthenticationService.Dtos.User;
using MediaTrackerAuthenticationService.Models.Utils;

namespace MediaTrackerAuthenticationService.Services.UserService;

public interface IUserService
{
    Task<ServiceResponse<GetUserDto>> GetUserById(int userId);
    Task<ServiceResponse<GetUserDto>> AddUser(AddUserDto newUser);
    Task<ServiceResponse<GetUserDto>> UpdateUser(UpdateUserDto updatedUser);
    Task<ServiceResponse<GetUserDto>> UpsertUser(UpsertUserDto upsertUser);
}