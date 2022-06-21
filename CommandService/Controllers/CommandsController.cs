using AutoMapper;
using CommandService.Models.DTOs;
using CommandService.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers{
    
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{varsion:apiVersion}/c/[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CommandsController> _logger;

        public CommandsController(ICommandRepository repository, IMapper mapper, ILogger<CommandsController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandResponseDTO>> GetCommandsForPlatform(Guid platformId){
            try
            {
                if(!_repository.PlatformExists(platformId))
                    return NotFound(platformId);

                var result = _repository.GetCommandsForPlatform(platformId);
                return Ok(_mapper.Map<CommandResponseDTO>(result));    
            }
            catch (Exception ex)
            {
                _logger.LogError("GET: Commands/", ex);
                throw;
            }
        }

    }
}