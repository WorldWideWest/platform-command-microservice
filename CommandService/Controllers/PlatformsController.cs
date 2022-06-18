using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CommandService.Controllers{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/c/[controller]")]
    public class PlatformsController : ControllerBase{
        private readonly ILogger<PlatformsController> _logger;
        public PlatformsController(ILogger<PlatformsController> logger)
        {
            _logger = logger;
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