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
        public async Task<ActionResult<Counter>> GetCounter(long id)
        {
            try {
                return await counterApi.GetCounter(id);
            } catch (ArgumentException) {
                return NotFound();
            }
        }

        // POST api/<CounterController>
        [HttpPost]
        public async Task<IActionResult> CreateCounter(CounterModel newCounter)
        {
            var created = await counterApi.CreateCounter(newCounter);
            return CreatedAtAction("GetCounter", new { id = created.Id }, created);
        }

        // PUT api/<CounterController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCounter(long id, CounterModel counter)
        {
            try {
                var updated = await counterApi.UpdateCounter(id, counter);
                return AcceptedAtAction("GetCounter", new { id = updated.Id }, updated);
            } catch (ArgumentException) {
                return NotFound();
            }
        }

        // DELETE api/<CounterController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCounter(long id)
        {
            try {
                await counterApi.DeleteCounter(id);
                return NoContent();
            } catch (ArgumentException) {
                return NotFound();
            }
        }
    }
}
