using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CounterManagerDb.Data;

namespace CounterManagerDb.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CounterController : ControllerBase {
        private readonly CounterManagerDbContext _context;

        public CounterController(CounterManagerDbContext context)
        {
            _context = context;
        }

        // GET: api/Counters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Counter>>> GetCounter()
        {
            if (_context.Counter == null) {
                return NotFound();
            }
            return await _context.Counter.ToListAsync();
        }

        // GET: api/Counters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Counter>> GetCounter(long id)
        {
            if (_context.Counter == null) {
                return NotFound();
            }
            var counter = await _context.Counter.FindAsync(id);

            if (counter == null) {
                return NotFound();
            }

            return counter;
        }

        // POST: api/Counters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Counter>> CreateCounter(CounterModel counter)
        {
            if (_context.Counter == null) {
                return Problem("Entity set 'CounterManagerDbContext.Counter'  is null.");
            }
            Counter dbCounter = new (counter.Name, counter.Count);
            _context.Counter.Add(dbCounter);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCounter", new { id = dbCounter.Id }, dbCounter);
        }

        // PUT: api/Counters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCounter(long id, CounterModel counter)
        {
            var dbCounter = await _context.Counter.FindAsync(id);
            if (dbCounter != null) {
                var dbEntry = _context.Entry(dbCounter);
                dbEntry.State = EntityState.Modified;
                dbEntry.CurrentValues.SetValues(counter);
            } else {
                return NotFound();
            }

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!CounterExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return AcceptedAtAction("GetCounter", new { id = dbCounter.Id }, dbCounter);
        }


        // DELETE: api/Counters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCounter(long id)
        {
            if (_context.Counter == null) {
                return NotFound();
            }
            var counter = await _context.Counter.FindAsync(id);
            if (counter == null) {
                return NotFound();
            }

            _context.Counter.Remove(counter);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CounterExists(long id)
        {
            return (_context.Counter?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
