using AutoMapper;
using MediaTrackerAuthenticationService.Dtos.PlatformConnection;
using MediaTrackerAuthenticationService.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaTrackerAuthenticationService.Services.PlatformConnectionService
{
    public class PlatformConnectionService : IPlatformConnectionService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public PlatformConnectionService(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<GetPlatformConnectionDto>> AddPlatformConnection(
            AddPlatformConnectionDto newPlatformConnection
        )
        {
            var serviceResponse = new ServiceResponse<GetPlatformConnectionDto>();

            try
            {
                var toInsert = _mapper.Map<PlatformConnection>(newPlatformConnection);
                _context.PlatformConnections.Add(toInsert);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetPlatformConnectionDto>(toInsert);
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPlatformConnectionDto>> GetPlatformConnectionByUserId(
            int userID
        )
        {
            var serviceResponse = new ServiceResponse<GetPlatformConnectionDto>();

            try
            {
                var platformConnection = await _context.PlatformConnections.FirstOrDefaultAsync(
                    platform => platform.UserId == userID
                );

                if (platformConnection is null)
                {
                    throw new Exception($"Platform Connection with User Id '{userID}' not found");
                }

                serviceResponse.Data = _mapper.Map<GetPlatformConnectionDto>(platformConnection);
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPlatformConnectionDto>> UpdatePlatformConnection(
            UpdatePlatformConnectionDto updatedPlatformConnection
        )
        {
            var serviceResponse = new ServiceResponse<GetPlatformConnectionDto>();

            try
            {
                var platformConnection = await _context.PlatformConnections.FirstOrDefaultAsync(
                    platform => platform.UserId == updatedPlatformConnection.UserId
                );

                if (platformConnection is null)
                {
                    throw new Exception(
                        $"Platform Connection with User Id '{updatedPlatformConnection.UserId}' not found"
                    );
                }

                platformConnection.AccessToken = updatedPlatformConnection.AccessToken;
                platformConnection.Platform = updatedPlatformConnection.Platform;
                platformConnection.RefreshToken = updatedPlatformConnection.RefreshToken;
                platformConnection.Scopes = updatedPlatformConnection.Scopes;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetPlatformConnectionDto>(platformConnection);
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;
        }
    }
}
