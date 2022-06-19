using PlatformService.Models.DTOs;

namespace PlatformService.SyncDataService.Http{
    public interface ICommandDataClient{
        Task SendPlatformToCommand(PlatformResponseDTO response);
    }
}