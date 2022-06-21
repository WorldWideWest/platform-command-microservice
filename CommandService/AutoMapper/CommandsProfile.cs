using AutoMapper;
using CommandService.Models.DTOs;
using CommandService.Models.Entities;

namespace CommandService.AutoMapper{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Source -> Target
            CreateMap<Platform, PlatformResponseDTO>();
            CreateMap<CommandRequestDTO, Command>();
            CreateMap<Command, CommandResponseDTO>();
        }
    }
}