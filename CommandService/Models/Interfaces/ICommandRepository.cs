using CommandService.Models.Entities;

namespace CommandService.Models.Interfaces{
    public interface ICommandRepository{
        bool SaveChanges();
        // Platforms
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform platform);
        bool PlatformExists(Guid id);

        // Commands
        IEnumerable<Command> GetCommandsForPlatform(Guid platformId);
        Command GetCommand(Guid platformId, Guid commandId);
        void CreateCommand(Guid platformId, Command command);

    }
}