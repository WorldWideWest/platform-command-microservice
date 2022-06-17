using Microsoft.EntityFrameworkCore;
using PlatformService.Models.Entities;
using PlatformService.Models.Interfaces;

namespace PlatformService.Databse.Repository{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PlatformRepository> _logger;
        public PlatformRepository(ApplicationDbContext context, ILogger<PlatformRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void CreatePlatform(Platform platform)
        {
            try
            {
                if(platform == null)
                    throw new ArgumentNullException(nameof(platform));
                _context.Platforms.Add(platform);    
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
                return _context.Platforms.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetAllPlatforms));
                throw;
            }
        }

        public Platform GetPlatform(Guid id)
        {   
            try
            {
                return _context.Platforms.AsNoTracking()
                    .FirstOrDefault(x => x.Id.Equals(id));    
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetPlatform));
                throw;
            }
            
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}