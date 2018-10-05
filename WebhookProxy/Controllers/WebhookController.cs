using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebhookProxy.Utility;

namespace WebhookProxy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private IConfiguration _configuration;

        public WebhookController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Post([FromBody] string payload)
        {
            ServicesUtility utility = new ServicesUtility(_configuration);
            try
            {
                utility.RelayMessage(payload);
            }
            catch(Exception ex)
            {
                return new ObjectResult("Could not relay message to target: " + ex.Message);
            }

            return new OkObjectResult("Relayed message successfully.");
        }

        [HttpGet]
        public IActionResult Get()
        {
            var target = _configuration.GetSection("Target")["Location"];

            ServicesUtility utility = new ServicesUtility(_configuration);
            try
            {
                var res = utility.RelayMessage("{\"test\":\"true\"}");
            }
            catch (Exception ex)
            {
                return new ObjectResult("Could not send test message to target: " + ex.Message);
            }


            return new OkObjectResult("Sent test message successfully.");
        }
    }
}