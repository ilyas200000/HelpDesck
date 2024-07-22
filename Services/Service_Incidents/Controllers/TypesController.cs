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
    public class TypesController : ControllerBase
    {
        private readonly IncidentsDbContext _context;

        public TypesController(IncidentsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: api/Types/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Types>>> GetTypes()
        {
            return await _context.Types.ToListAsync();
        }

        // GET: api/Types/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Types>> GetTypes(int id)
        {
            var types = await _context.Types.FindAsync(id);

            if (types == null)
            {
                return NotFound();
            }

            return Ok(types);
        }

        // PUT: api/Types/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypes(int id, Types types)
        {
            if (id != types.INCD_TYPE_ID)
            {
                return BadRequest("Type ID mismatch");
            }

            _context.Entry(types).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypesExists(id))
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

        // GET: api/Types/byCategory/5
        [HttpGet("byCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Types>>> GetTypesByCategory(int categoryId)
        {
            var types = await _context.Types.Where(t => t.CategoryID == categoryId).ToListAsync();

            if (types == null || !types.Any())
            {
                return NotFound("No types found for the given category ID.");
            }

            return types;
        }


        // POST: api/Types
        [HttpPost]
        public async Task<ActionResult<Types>> PostTypes(Types types)
        {
            if (types == null)
            {
                return BadRequest("Type is null");
            }

            _context.Types.Add(types);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTypes), new { id = types.INCD_TYPE_ID }, types);
        }

        // DELETE: api/Types/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypes(int id)
        {
            var types = await _context.Types.FindAsync(id);
            if (types == null)
            {
                return NotFound();
            }

            _context.Types.Remove(types);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TypesExists(int id)
        {
            return _context.Types.Any(e => e.INCD_TYPE_ID == id);
        }
    }
}
