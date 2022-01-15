using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayOcelot.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly ILogger<ConfigurationController> _logger;
        private readonly IConfiguration _configuration;

        public ConfigurationController(ILogger<ConfigurationController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var configuration = this._configuration.GetSection("ReRoutes").Value;
            return Ok(configuration);
        }
    }
}