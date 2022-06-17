using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Models.DTOs;
using PlatformService.Models.Entities;
using PlatformService.Models.Interfaces;

namespace PlatformService.Controllers{
    
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PlatformsController : ControllerBase{
        private readonly IPlatformRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformsController> _logger;

        public PlatformsController(IPlatformRepository repository, IMapper mapper, ILogger<PlatformsController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }        

        [HttpGet]
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
        public ActionResult<PlatformResponseDTO> CreatePlatform(PlatformRequestDTO request){
            try
            {
                var model = _mapper.Map<Platform>(request);
                _repository.CreatePlatform(model);
                _repository.SaveChanges();

                var platform = _mapper.Map<PlatformResponseDTO>(model);
                return CreatedAtRoute(nameof(GetPlatform), new { id = platform.Id }, platform);
            }
            catch (Exception ex)
            {
                _logger.LogError("GET:/Platform", ex);
                throw;
            }
        }

    }
}