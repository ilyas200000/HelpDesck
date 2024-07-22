using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Incidents.Data;
using Service_Incidents.Models;

namespace Service_Incidents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrioritesController : ControllerBase
    {
        private readonly IncidentsDbContext _context;

        public PrioritesController(IncidentsDbContext context)
        {
            _context = context;
        }

        // GET: api/Priorites
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Priorite>>> GetPriorites()
        {
          if (_context.Priorites == null)
          {
              return NotFound();
          }
            return await _context.Priorites.ToListAsync();
        }

        // GET: api/Priorites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Priorite>> GetPriorite(int id)
        {
          if (_context.Priorites == null)
          {
              return NotFound();
          }
            var priorite = await _context.Priorites.FindAsync(id);

            if (priorite == null)
            {
                return NotFound();
            }

            return priorite;
        }

        // PUT: api/Priorites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPriorite(int id, Priorite priorite)
        {
            if (id != priorite.INCD_PRIO_ID)
            {
                return BadRequest();
            }

            _context.Entry(priorite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrioriteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Priorites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add")]
        public async Task<ActionResult<Priorite>> PostPriorite(Priorite priorite)
        {
          if (_context.Priorites == null)
          {
              return Problem("Entity set 'IncidentsDbContext.Priorites'  is null.");
          }
            _context.Priorites.Add(priorite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPriorite", new { id = priorite.INCD_PRIO_ID }, priorite);
        }

        // DELETE: api/Priorites/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePriorite(int id)
        {
            if (_context.Priorites == null)
            {
                return NotFound();
            }
            var priorite = await _context.Priorites.FindAsync(id);
            if (priorite == null)
            {
                return NotFound();
            }

            _context.Priorites.Remove(priorite);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrioriteExists(int id)
        {
            return (_context.Priorites?.Any(e => e.INCD_PRIO_ID == id)).GetValueOrDefault();
        }
    }
}
