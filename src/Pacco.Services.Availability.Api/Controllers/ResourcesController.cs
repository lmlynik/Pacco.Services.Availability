using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Microsoft.AspNetCore.Mvc;
using Pacco.Services.Availability.Application.Commands;

namespace Pacco.Services.Availability.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourcesController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public ResourcesController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }
        
        [HttpPost]
        public async Task<ActionResult> Post(AddResource command)
        {
            await _commandDispatcher.SendAsync(command);
            return Created($"resources/{command.ResourceId}", null);
        }
    }
}