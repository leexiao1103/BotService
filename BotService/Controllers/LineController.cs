using BotService.Model.Line;
using BotService.Service.Line;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BotService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ILineService _lineService;

        public LineController(ILineService lineService, ILogger<LineController> logger)
        {
            _logger = logger;
            _lineService = lineService;
        }

        // GET api/line
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/line/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/line/webhook
        [HttpPost("webhook")]        
        public async Task<IActionResult> Post([FromHeader] string header, [FromBody] LineWebhookEvent hookEvent)
        {
            //_logger.LogInformation(JsonConvert.SerializeObject(hookEvent));            
            _lineService.HandleEventAsync(hookEvent);
            

            return Ok();
        }

        // PUT api/line/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/line/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
