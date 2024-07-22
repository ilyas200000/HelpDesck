using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Auth.Data;
using Service_Auth.Models.Identity;

namespace Service_Auth.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DroitsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DroitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Droits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Droit>>> GetDROITS()
        {
          if (_context.DROITS == null)
          {
              return NotFound();
          }
            var result = await _context.DROITS.ToListAsync();
            return Ok(result);
        }

        // GET: api/Droits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Droit>> GetDroit(int id)
        {
          if (_context.DROITS == null)
          {
              return NotFound();
          }
            var droit = await _context.DROITS.FindAsync(id);

            if (droit == null)
            {
                return NotFound();
            }

            return droit;
        }

        // PUT: api/Droits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDroit(int id, Droit droit)
        {
            if (id != droit.DRT_ID)
            {
                return BadRequest();
            }

            _context.Entry(droit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DroitExists(id))
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

        // POST: api/Droits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Droit>> PostDroit(Droit droit)
        {
          if (_context.DROITS == null)
          {
              return Problem("Entity set 'ApplicationDbContext.DROITS'  is null.");
          }
            _context.DROITS.Add(droit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDroit", new { id = droit.DRT_ID }, droit);
        }

        // DELETE: api/Droits/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDroit(int id)
        {
            if (_context.DROITS == null)
            {
                return NotFound();
            }
            var droit = await _context.DROITS.FindAsync(id);
            if (droit == null)
            {
                return NotFound();
            }

            _context.DROITS.Remove(droit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DroitExists(int id)
        {
            return (_context.DROITS?.Any(e => e.DRT_ID == id)).GetValueOrDefault();
        }
    }
}
