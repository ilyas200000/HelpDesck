
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Auth.Data;
using Service_Auth.Models.Identity;

namespace Service_Auth.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntiteSiegesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EntiteSiegesController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntiteSiege>>> GetENTITE_SIEGE()
        {
          var entiteSiege = await _context.ENTITE_SIEGE.ToListAsync();

          if (entiteSiege == null)
          {
            return NotFound();
          }
          return Ok(entiteSiege);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EntiteSiege>> GetEntiteSiege(int id)
        {
            var entiteSiege = await _context.ENTITE_SIEGE.FindAsync(id);
          
            if (entiteSiege == null)
            {
                return NotFound();
            }

            return entiteSiege;
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntiteSiege(int id, EntiteSiege entiteSiege)
        {
            if (id != entiteSiege.ENTT_SG_ID)
            {
                return BadRequest();
            }

            _context.Entry(entiteSiege).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntiteSiegeExists(id))
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

       
        [HttpPost]
        public async Task<ActionResult<EntiteSiege>> PostEntiteSiege(EntiteSiege entiteSiege)
        {
          if (_context.ENTITE_SIEGE == null)
          {
              return Problem("Entity set 'ApplicationDbContext.ENTITE_SIEGE'  is null.");
          }
            _context.ENTITE_SIEGE.Add(entiteSiege);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntiteSiege", new { id = entiteSiege.ENTT_SG_ID }, entiteSiege);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntiteSiege(int id)
        {
            if (_context.ENTITE_SIEGE == null)
            {
                return NotFound();
            }
            var entiteSiege = await _context.ENTITE_SIEGE.FindAsync(id);
            if (entiteSiege == null)
            {
                return NotFound();
            }

            _context.ENTITE_SIEGE.Remove(entiteSiege);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EntiteSiegeExists(int id)
        {
            return (_context.ENTITE_SIEGE?.Any(e => e.ENTT_SG_ID == id)).GetValueOrDefault();
        }
    }
}
