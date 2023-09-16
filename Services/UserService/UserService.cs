using AutoMapper;
using MediaTrackerAuthenticationService.Data;
using MediaTrackerAuthenticationService.Dtos.User;
using MediaTrackerAuthenticationService.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaTrackerAuthenticationService.Services.UserService;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public UserService(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<ServiceResponse<GetUserDto>> AddUser(AddUserDto newUser)
    {
        var serviceResponse = new ServiceResponse<GetUserDto>();

        try
        {
            var toInsert = _mapper.Map<User>(newUser);
            _context.Users.Add(toInsert);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetUserDto>(toInsert);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetUserDto>> GetUserById(int userId)
    {
        var serviceResponse = new ServiceResponse<GetUserDto>();

        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.UserId == userId);

            if (user is null)
            {
                throw new Exception($"User with User Id '{userId}' not found");
            }

            serviceResponse.Data = _mapper.Map<GetUserDto>(user);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetUserDto>> UpdateUser(UpdateUserDto updatedUser)
    {
        var serviceResponse = new ServiceResponse<GetUserDto>();

        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.UserId == updatedUser.UserId);

            if (user is null)
            {
                throw new Exception($"User with User Id '{updatedUser.UserId}' not found");
            }

            user.Platform = updatedUser.Platform;
            user.PlatformId = updatedUser.PlatformId;

            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetUserDto>(user);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }

    public async Task<ServiceResponse<GetUserDto>> UpsertUser(UpsertUserDto upsertUser)
    {
        var serviceResponse = new ServiceResponse<GetUserDto>();

        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Platform == upsertUser.Platform && user.PlatformId == upsertUser.PlatformId);

            if (user is null)
            {
                var toAdd = _mapper.Map<AddUserDto>(upsertUser);
                return await AddUser(toAdd);
            }
            
            serviceResponse.Data = _mapper.Map<GetUserDto>(user);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }
        return serviceResponse;
    }
}