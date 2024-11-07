using CommandService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext dbContext;

        public CommandRepo( AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            else
            {
                command.PlatformId = platformId;
                dbContext.Commands.Add(command);
            }
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }
            else
            {
                dbContext.Platforms.Add(platform);
            }
        }

        public async Task<bool> ExternalPlatformExist(int externalPlatformId)
        {
            return await dbContext.Platforms.AnyAsync(p => p.ExternalID == externalPlatformId);
        }

        public async Task<IEnumerable<Platform>> GetAllPlatforms()
        {
            return await dbContext.Platforms.ToListAsync();
        }

        public async Task<Command> GetCommand(int platformId, int commandId)
        {
            return await dbContext.Commands.FirstOrDefaultAsync(c => c.PlatformId == platformId && c.Id == commandId);
        }

        public async Task<IEnumerable<Command>> GetCommandsForPlatform(int platformId)
        {
            return await dbContext.Commands.Where(c => c.PlatformId == platformId).OrderBy(c=>c.Platform.Name).ToListAsync();
        }

        public async Task<bool> PlatformExists(int platformId)
        {
            return await dbContext.Platforms.AnyAsync(p => p.Id == platformId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await dbContext.SaveChangesAsync()) >= 0;
        }
    }
}
