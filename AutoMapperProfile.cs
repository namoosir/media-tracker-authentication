using AutoMapper;
using MediaTrackerAuthenticationService.Dtos.PlatformConnection;
using MediaTrackerAuthenticationService.Dtos.User;
using MediaTrackerAuthenticationService.Models;

namespace MediaTrackerAuthenticationService
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PlatformConnection, GetPlatformConnectionDto>();
            CreateMap<AddPlatformConnectionDto, PlatformConnection>();

            CreateMap<AddUserDto, User>();
        }
    }
}
