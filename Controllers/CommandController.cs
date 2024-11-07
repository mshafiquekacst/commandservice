using AutoMapper;
using CommandService.Data;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace CommandService.Controllers
{
    [Route("api/c/{platformId}/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ICommandRepo repo;
        private readonly IMapper mapper;

        public CommandController(ICommandRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Getting Commands for Platform: {platformId} from Command Service");
            if (!await repo.PlatformExists(platformId))
            {
                return NotFound();
            }
            var commands = await repo.GetCommandsForPlatform(platformId);
            return Ok(mapper.Map<IEnumerable<Dtos.CommandReadDto>>(commands));
        }
        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public async Task<ActionResult> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Getting Command for Platform: {platformId} / {commandId} from Command Service");
            if (!await repo.PlatformExists(platformId))
            {
                return NotFound();
            }
            var command = await repo.GetCommand(platformId, commandId);
            if (command == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<Dtos.CommandReadDto>(command));
        }
        [HttpPost]
        public async Task<ActionResult> CreateCommandForPlatform(int platformId, Dtos.CommandCreateDto commandDto)
        {
            Console.WriteLine($"--> Creating Command for Platform: {platformId} from Command Service");
            if (!await repo.PlatformExists(platformId))
            {
                return NotFound();
            }
            var command = mapper.Map<Command>(commandDto);
            repo.CreateCommand(platformId, command);
            await repo.SaveChangesAsync();
            var readDto = mapper.Map<Dtos.CommandReadDto>(command);
            return CreatedAtRoute(nameof(GetCommandForPlatform), new { platformId = platformId, commandId = readDto.Id }, readDto);
        }
    }
}
