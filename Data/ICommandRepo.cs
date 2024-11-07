using CommandService.Models;
using Microsoft.AspNetCore.SignalR;

namespace CommandService.Data
{
    public interface ICommandRepo
    {
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<Platform>> GetAllPlatforms();
        //public Task<Platform?> GetPlatformById(int id);
        void CreatePlatform(Platform platform);
        Task<bool> PlatformExists(int platformId);
        Task<bool> ExternalPlatformExist(int externalPlatformId);
        Task<IEnumerable<Command>> GetCommandsForPlatform(int platformId);
        Task<Command> GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);
        
    }
}
