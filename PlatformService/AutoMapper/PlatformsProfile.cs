using AutoMapper;
using PlatformService.Models.DTOs;
using PlatformService.Models.Entities;

namespace PlatformService.AutoMapper{
    public class PlatformsProfile : Profile{
        public PlatformsProfile()
        {
            // Source -> Target
            CreateMap<Platform, PlatformResponseDTO>();
            CreateMap<PlatformRequestDTO, Platform>();
            CreateMap<PlatformResponseDTO, PlatformPublishDTO>();
        }
    }
}