using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Models.DTOs;
using PlatformService.Models.Entities;
using PlatformService.Models.Interfaces;
using PlatformService.SyncDataService.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace PlatformService.Controllers{
    
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class PlatformsController : ControllerBase{
        private readonly IPlatformRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformsController> _logger;
        private readonly ICommandDataClient _client;
        private readonly IMessageBusClient _asyncClient;

        public PlatformsController(
            IPlatformRepository repository,
            IMapper mapper, 
            ILogger<PlatformsController> logger,
            ICommandDataClient client,
            IMessageBusClient asyncClient
            )
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _client = client;
            _asyncClient = asyncClient;
        }        

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "Get All Platofrms", typeof(IEnumerable<PlatformResponseDTO>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Get All Platofrms")]
        public ActionResult<IEnumerable<PlatformResponseDTO>> GetPlatforms(){

            try
            {
                var result = _repository.GetAllPlatforms();
                if(result.Any())
                    return Ok(_mapper.Map<IEnumerable<PlatformResponseDTO>>(result));
                return NotFound(result);    
            }
            catch (Exception ex)
            {
                _logger.LogError("GET:/Platforms", ex);
                throw;
            }
        }

        [HttpGet("{id}", Name = "GetPlatform")]
        [SwaggerResponse(StatusCodes.Status200OK, "Get Platofrm by id", typeof(PlatformResponseDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Platform not found")]
        public ActionResult<PlatformResponseDTO> GetPlatform(Guid id){
            try
            {
                var result = _repository.GetPlatform(id);
                if(result != null)
                    return Ok(_mapper.Map<PlatformResponseDTO>(result));
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("GET:/Platform", ex);
                throw;
            }
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, "Platform Create Successfuly", typeof(PlatformResponseDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request")]
        public async Task<ActionResult<PlatformResponseDTO>> CreatePlatform(PlatformRequestDTO request){
            try
            {
                var model = _mapper.Map<Platform>(request);
                _repository.CreatePlatform(model);
                _repository.SaveChanges();

                var response = _mapper.Map<PlatformResponseDTO>(model);

                // Sync Message
                // try
                // {
                //     await _client.SendPlatformToCommand(platform);
                // }
                // catch (Exception ex)
                // {
                //     _logger.LogError(ex.ToString());
                //     throw;
                // }

                // Async Message
                try
                {
                    var platform = _mapper.Map<PlatformPublishDTO>(response);
                    platform.Event = "PlatformPublished";
                    _asyncClient.PublishNewPlatform(platform);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    throw;
                }

                return CreatedAtRoute(nameof(GetPlatform), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("POST:/Platform", ex);
                throw;
            }
        }

    }
}