using Kafkaproducerimplemenation.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsumerImplementation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumerController : ControllerBase
    {
        private readonly ILogger<ConsumerController> _logger;

        public ConsumerController(ILogger<ConsumerController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult ProcessInventoryUpdate([FromBody] Princesrequest request)
        {
            return Ok("Princess added processed successfully.");
        }
    }

}

