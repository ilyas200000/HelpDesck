
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Incidents.Data;
using Service_Incidents.Models;

namespace Service_Incidents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatutsController : ControllerBase
    {
        private readonly IncidentsDbContext _context;

        public StatutsController(IncidentsDbContext context)
        {
            _context = context;
        }

        // GET: api/Statuts
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Statut>>> GetStatuts()
        {
          if (_context.Statuts == null)
          {
              return NotFound();
          }
            return await _context.Statuts.ToListAsync();
        }

        // GET: api/Statuts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Statut>> GetStatut(int id)
        {
          if (_context.Statuts == null)
          {
              return NotFound();
          }
            var statut = await _context.Statuts.FindAsync(id);

            if (statut == null)
            {
                return NotFound();
            }

            return statut;
        }

        // PUT: api/Statuts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatut(int id, Statut statut)
        {
            if (id != statut.INCD_STAT_ID)
            {
                return BadRequest();
            }

            _context.Entry(statut).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatutExists(id))
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

        // POST: api/Statuts
        [HttpPost("add")]
        public async Task<ActionResult<Statut>> PostStatut(Statut statut)
        {
          if (_context.Statuts == null)
          {
              return Problem("Entity set 'IncidentsDbContext.Statuts'  is null.");
          }
            _context.Statuts.Add(statut);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStatut", new { id = statut.INCD_STAT_ID }, statut);
        }

        // DELETE: api/Statuts/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteStatut(int id)
        {
            if (_context.Statuts == null)
            {
                return NotFound();
            }
            var statut = await _context.Statuts.FindAsync(id);
            if (statut == null)
            {
                return NotFound();
            }

            _context.Statuts.Remove(statut);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StatutExists(int id)
        {
            return (_context.Statuts?.Any(e => e.INCD_STAT_ID == id)).GetValueOrDefault();
        }
    }
}
