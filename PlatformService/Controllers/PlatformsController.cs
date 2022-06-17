using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Models.DTOs;
using PlatformService.Models.Interfaces;

namespace PlatformService.Controllers{
    
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase{
        private readonly IPlatformRepository _repository;
        private readonly IMapper _mapper;

        public PlatformsController(IPlatformRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }        


        [HttpGet]
        public ActionResult<IEnumerable<PlatformResponseDTO>> GetPlatforms(){
            var result = _repository.GetAllPlatforms();
            if(result.Any())
                return Ok(_mapper.Map<IEnumerable<PlatformResponseDTO>>(result));
            return NotFound(result);
        }


    }
}