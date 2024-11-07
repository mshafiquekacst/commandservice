using AutoMapper;
using CommandService.Data;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/[controller]")]
    public class PlatformController : ControllerBase
    {
        private readonly ICommandRepo repo;
        private readonly IMapper mapper;

        public PlatformController(ICommandRepo repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms from Command Service");
            var platforms = await repo.GetAllPlatforms();
            return Ok(mapper.Map<IEnumerable<Dtos.PlatformReadDto>>(platforms));
        }
        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");
            return Ok("Inbound test of from Platforms Controller");
        }
    }
}
