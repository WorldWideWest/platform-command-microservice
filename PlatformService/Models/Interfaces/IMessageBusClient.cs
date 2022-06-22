using PlatformService.Models.DTOs;

namespace PlatformService.Models.Interfaces{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishDTO platform);
    }
}