using CommandService.Models.Entities;
using CommandService.Models.Interfaces;

namespace CommandService.Database.Repository{
    public class CommandRepository : ICommandRepositroy
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<CommandRepository> _logger;

        public CommandRepository(ApplicationDbContext context, ILogger<CommandRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void CreateCommand(Guid platformId, Command command)
        {
            try
            {
                command.PlatformId = platformId;
                
                if(command != null)
                    _context.Add(command);
                throw new ArgumentNullException(nameof(command));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CreateCommand));
                throw;
            }
        }

        public void CreatePlatform(Platform platform)
        {
            try
            {
                if(platform != null)
                    _context.Add(platform);
                throw new ArgumentNullException(nameof(platform));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(CreatePlatform));
                throw;
            }
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            try
            {
                return _context.Platforms.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetAllPlatforms));
                throw;
            }
        }

        public Command GetCommand(Guid platformId, Guid commandId)
        {
            try
            {
                return _context.Commands.Where(x => 
                    x.PlatformId.Equals(platformId) && x.Id.Equals(commandId)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetCommand));
                throw;
            }
        }

        public IEnumerable<Command> GetCommandsForPlatform(Guid platformId)
        {
            try
            {
                return _context.Commands.Where(x => x.PlatformId.Equals(platformId))
                    .OrderBy(x => x.Platform.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetCommandsForPlatform));
                throw;
            }
        }

        public bool PlatformExists(Guid id)
        {
            try
            {
                return _context.Platforms.Any(x => x.Id.Equals(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(PlatformExists));
                throw;
            }
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}