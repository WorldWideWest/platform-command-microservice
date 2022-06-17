using PlatformService.Models.Entities;

namespace PlatformService.Models.Interfaces{
    public interface IPlatformRepository{
        bool SaveChanges();
        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatform(Guid id);
        void CreatePlatform(Platform platform);
    }
}