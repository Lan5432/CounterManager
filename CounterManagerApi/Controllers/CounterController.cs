using CounterManagerApi.Api;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CounterManagerApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CounterController : ControllerBase {

        private readonly ICounterApi counterApi;

        public CounterController(ICounterApi counterApi) => this.counterApi = counterApi;


        // GET: api/<CounterController>
        [HttpGet]
        public async Task<IEnumerable<Counter>> GetAllCounters()
        {
            return await counterApi.GetAllCounters();
        }

        // GET api/<CounterController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Counter>> GetCounter(string id)
        {
            try {
                return await counterApi.GetCounter(id);
            } catch (ArgumentException) {
                return NotFound();
            }
        }

        // POST api/<CounterController>
        [HttpPost]
        public async void CreateCounter([FromBody] CounterRequest newCounter)
        {
            await counterApi.CreateCounter(newCounter);
        }

        // PUT api/<CounterController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Counter>> UpdateCounter(string id, [FromBody] CounterRequest counter)
        {
            try {
                return await counterApi.UpdateCounter(id, counter);
            } catch (ArgumentException) {
                return NotFound();
            }
        }

        // DELETE api/<CounterController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCounter(string id)
        {
            try {
                await counterApi.DeleteCounter(id);
                return Ok();
            } catch (ArgumentException) {
                return NotFound();
            }
        }
    }
}
