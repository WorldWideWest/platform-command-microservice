using Microsoft.EntityFrameworkCore;
using PlatformService.Models.Entities;
using PlatformService.Models.Interfaces;

namespace PlatformService.Databse.Repository{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly ApplicationDbContext _context;
        public PlatformRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreatePlatform(Platform platform)
        {
            if(platform == null)
                throw new ArgumentNullException(nameof(platform));
            _context.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.AsNoTracking().ToList();
        }

        public Platform GetPlatform(Guid id)
        {
            return _context.Platforms.AsNoTracking()
                .FirstOrDefault(x => x.Id.Equals(id));
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}