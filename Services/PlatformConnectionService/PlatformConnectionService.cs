using AutoMapper;
using MediaTrackerAuthenticationService.Dtos.PlatformConnection;
using MediaTrackerAuthenticationService.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaTrackerAuthenticationService.Services.PlatformConnectionService
{
    public class PlatformConnectionService : IPlatformConnectionService
    {
        private readonly IMapper _mapper;

        public PlatformConnectionService(IMapper mapper)
        {
            _mapper = mapper;
        }

        private static PlatformConnection youtube = new PlatformConnection
        {
            Platform = MediaPlatform.YouTube,
            AccessToken = "a",
            RefreshToken = "b",
            Scopes = "c"
        };

        public async Task<ServiceResponse<GetPlatformConnectionDto>> AddPlatformConnection(
            AddPlatformConnectionDto newPlatformConnection
        )
        {
            var serviceResponse = new ServiceResponse<GetPlatformConnectionDto>();
            //MUST DO REVERSE DTO MAPPING BEFORE INSERTING TO DB HERE AND THEN
            var temp = _mapper.Map<PlatformConnection>(newPlatformConnection);
            //inserting temp to db here
            //retrieve the item from db
            serviceResponse.Data = _mapper.Map<GetPlatformConnectionDto>(temp);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPlatformConnectionDto>> GetPlatformConnectionByUserId(
            int UserID
        )
        {
            var serviceResponse = new ServiceResponse<GetPlatformConnectionDto>();
            //retrieve the item from db based on userid and then return here
            serviceResponse.Data = _mapper.Map<GetPlatformConnectionDto>(youtube);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPlatformConnectionDto>> UpdatePlatformConnection(
            UpdatePlatformConnectionDto updatedPlatformConnection
        )
        {
            var serviceResponse = new ServiceResponse<GetPlatformConnectionDto>();

            try
            {
                //do more errorchecking
                //do some updating
                //retrieve updated item from db
                serviceResponse.Data = _mapper.Map<GetPlatformConnectionDto>(youtube);
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
