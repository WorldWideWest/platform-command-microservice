using AutoMapper;
using CommandService.Models.DTOs;
using CommandService.Models.Entities;
using CommandService.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers{
    
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
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

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandResponseDTO> GetCommandForPlatform(Guid platformId, Guid commandId){
            try
            {
                if(!_repository.PlatformExists(platformId))
                    return NotFound(platformId);

                var result = _repository.GetCommand(platformId, commandId);
                if(result != null)
                    return Ok(_mapper.Map<CommandResponseDTO>(result));    
                return NotFound(commandId);
            }
            catch (Exception ex)
            {
                _logger.LogError("GET: Commands/{coomandId}/", ex);
                throw;
            }
        }

        [HttpPost]
        public ActionResult<CommandResponseDTO> CreateCommandForPlatform(Guid platformId, CommandRequestDTO request){
            try
            {

                if(!_repository.PlatformExists(platformId))
                    return NotFound(platformId);
                
                var command = _mapper.Map<Command>(request);
                _repository.CreateCommand(platformId, command);
                _repository.SaveChanges();

                var response = _mapper.Map<CommandResponseDTO>(command);
                return CreatedAtRoute(nameof(GetCommandForPlatform),
                    new { platformId = platformId, commandId = response.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("GET: Commands/{coomandId}/", ex);
                throw;
            }
        }

    }
}