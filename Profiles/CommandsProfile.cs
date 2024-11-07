using AutoMapper;
using CommandService.Models;

namespace CommandService.Profiles
{
    public class CommandsProfile: Profile
    {
        public CommandsProfile()
        {
            // Source -> Target
            CreateMap<Models.Platform, Dtos.PlatformReadDto>();
            CreateMap<Dtos.CommandCreateDto, Models.Command>();
            CreateMap<Dtos.CommandReadDto, Models.Command>();
            CreateMap<Dtos.PlatformPublishedDto,Platform>()
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id));
        }
    }
}
