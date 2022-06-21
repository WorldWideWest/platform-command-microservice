using AutoMapper;
using CommandService.Models.DTOs;
using CommandService.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CommandService.Controllers{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/c/[controller]")]
    public class PlatformsController : ControllerBase{
        private readonly ICommandRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformsController> _logger;
        public PlatformsController(ICommandRepository repository, ILogger<PlatformsController> logger, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<PlatformResponseDTO>> GetAllPlatforms(){
            try
            {   
                var result = _repository.GetAllPlatforms();
                return Ok(_mapper.Map<IEnumerable<PlatformResponseDTO>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError("GET: GetAllPlatforms", ex);
                throw;
            }
        }



        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public ActionResult TestInboundConnection(){
            try
            {
                return Ok(new {
                    connection = "Connection Ok!"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("POST: TestInboundConnection", ex);
                throw;
            }
        }
    }
}