using System.Text.Json;
using MediaTrackerAuthenticationService.Models;
using StackExchange.Redis;

namespace MediaTrackerAuthenticationService.Data;

public class UserInformationRepository : IUserInformationRepository
{
    private readonly IConnectionMultiplexer _redis;
    public UserInformationRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }
    public async Task<bool> CreateUserInformation(UserInformation userInformation)
    {
        var db = _redis.GetDatabase();
        var serialUserInformation = JsonSerializer.Serialize(userInformation);
        return await db.StringSetAsync($"UserInformation:{userInformation.UserId}", serialUserInformation);
    }

    public async Task<bool> DeleteUserInformationByUserId(int userId)
    {
        var db = _redis.GetDatabase();
        return await db.KeyDeleteAsync($"UserInformation:{userId}");
    }

    public async Task<UserInformation?> GetUserInformationByUserId(int userId)
    {
        var db = _redis.GetDatabase();
        var userInformation = await db.StringGetAsync($"UserInformation:{userId}");

        if (!string.IsNullOrEmpty(userInformation))
        {
            return JsonSerializer.Deserialize<UserInformation>(userInformation);
        }

        return null;
    }

    public async Task<bool> UpdateUserInformation(UserInformation newUserInformation)
    {
        var exisiting = await GetUserInformationByUserId(newUserInformation.UserId);
        if (exisiting is null)
        {
            return false;
        }

        var db = _redis.GetDatabase();

        var serialUserInformation = JsonSerializer.Serialize(newUserInformation);
        return await db.StringSetAsync($"UserInformation:{newUserInformation.UserId}", serialUserInformation);
    }
}