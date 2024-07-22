using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Incidents.Data;
using Service_Incidents.Models;


namespace Service_Incidents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        private readonly IncidentsDbContext _context;
       

        public IncidentController(IncidentsDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        [Authorize(Policy = "RequireDroitDeclaration")]
        public async Task<IActionResult> CreateIncident([FromBody] IncidentReq request)
        {
            if (request == null)
            {
                return BadRequest("Request is null.");
            }

            try
            {
                var priority = await _context.Priorites.FindAsync(request.INCD_PRIO_ID);
                var type = await _context.Types.FindAsync(request.INCD_TYPE_ID);
                var status = await _context.Statuts.FindAsync(request.INCD_STAT_ID);

                if (priority == null || type == null || status == null)
                {
                    return BadRequest("Invalid priority, type or status ID.");
                }

                var incident = new Incident
                {
                    INCD_DESC = request.INCD_DESC,
                    INCD_PRIO_ID = request.INCD_PRIO_ID,
                    INCD_TYPE_ID = request.INCD_TYPE_ID,
                    agn_code = request.agn_code,
                    INCD_STAT_ID = request.INCD_STAT_ID,
                    INCD_UTIL_ID = request.INCD_UTIL_ID,
                    incd_date_creation = DateTime.UtcNow,
                    INCD_NUM_TICK = GenerateUniqueIdentifier(),
                    INCD_ENTT_SG_ID = request.INCD_ENTT_SG_ID,
                    createdBy = request.createdBy
                };

                _context.Incidents.Add(incident);
                await _context.SaveChangesAsync();

                var history = new IncidentHistory
                {
                    IncidentId = incident.INCD_ID,
                    NewStatusId = incident.INCD_STAT_ID,
                    OldStatusId = incident.INCD_STAT_ID,
                    ChangeDate = DateTime.UtcNow,
                    ChangedBy = incident.INCD_UTIL_ID
                };

                _context.IncidentHistories.Add(history);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetIncident), new { id = incident.INCD_ID }, incident);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIncident(int id)
        {
            try
            {
                var incident = await _context.Incidents.FindAsync(id);
                if (incident == null)
                {
                    return NotFound();
                }

                return Ok(incident);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("ByNumTicket")]
        [Authorize(Policy = "RequireDroitConsIncidentsDeclarés")]
        public async Task<IActionResult> GetIncidentByTicket([FromQuery] string ticketNumber)
        {
            try
            {
                var incident = await _context.Incidents.FirstOrDefaultAsync(inc => inc.INCD_NUM_TICK == ticketNumber);

                if (incident == null)
                {
                    return NotFound();
                }

                return Ok(incident);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Incident>>> GetIncidents()
        {
            try
            {
                var incidents = await _context.Incidents.ToListAsync();
                return Ok(incidents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Traiter")]
        //[Authorize(Policy = "RequireDroitTraitement")]
        public async Task<IActionResult> ProcessIncident(int id, int idStatut, int idUser)
        {
            try
            {
                var incident = await _context.Incidents.FindAsync(id);
                if (incident == null)
                {
                    return NotFound("Incident not found.");
                }

                var statut = await _context.Statuts.FindAsync(idStatut);
                if (statut == null)
                {
                    return NotFound("Statut not found.");
                }

                var history = new IncidentHistory
                {
                    IncidentId = incident.INCD_ID,
                    OldStatusId = incident.INCD_STAT_ID,
                    NewStatusId = statut.INCD_STAT_ID,
                    ChangeDate = DateTime.UtcNow,
                    ChangedBy = idUser
                };

                incident.INCD_STAT_ID = statut.INCD_STAT_ID;
                incident.INCD_UTIL_ID = idUser;

                _context.IncidentHistories.Add(history);
                _context.Entry(incident).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(incident);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("cancel/{id}/{idUser}")]
        [Authorize(Policy = "RequireDroitAnnulerIncident")]
        public async Task<IActionResult> CancelIncident(int id ,int idUser)
        {
            try
            {
                var incident = await _context.Incidents.FindAsync(id);
                if (incident == null)
                {
                    return NotFound("Incident not found.");
                }

                if (incident.INCD_STAT_ID == 4)
                {
                    return BadRequest("Incident already cancelled.");
                }

                var annuleStatus = await _context.Statuts.SingleOrDefaultAsync(s => s.STAT_DESC == "Annulé");
                if (annuleStatus == null)
                {
                    return BadRequest("Status 'annulé' not found.");
                }

                var oldStatusId = incident.INCD_STAT_ID;
                incident.INCD_STAT_ID = annuleStatus.INCD_STAT_ID;
                incident.incd_date_cloture = DateTime.UtcNow;
                incident.INCD_UTIL_ID = idUser;

                var history = new IncidentHistory
                {
                    IncidentId = incident.INCD_ID,
                    OldStatusId = oldStatusId,
                    NewStatusId = annuleStatus.INCD_STAT_ID,
                    ChangeDate = DateTime.UtcNow,
                    ChangedBy = idUser
                };

                _context.IncidentHistories.Add(history);
                await _context.SaveChangesAsync();

                return Ok(incident);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize(Policy = "RequireDroitSuivi")]
        public async Task<ActionResult<IEnumerable<IncidentHistory>>> GetIncidentsByUser(int userId)
        {
            try
            {
                // user has the "CHEF ENTITE DEMANDEUR" role ?
                var isAdmin = User.IsInRole("CHEF ENTITE DEMANDEUR");

                //  incidents based on the user's role
                var historyQuery = _context.IncidentHistories
                    .Include(h => h.OldStatus)
                    .Include(h => h.NewStatus)
                    .Include(h => h.Incident)
                    .AsQueryable();

                if (!isAdmin)
                {
                    historyQuery = historyQuery.Where(h => h.ChangedBy == userId);
                }

                var history = await historyQuery
                    .GroupBy(h => h.IncidentId) 
                    .Select(g => g.OrderBy(h => h.ChangeDate).First()) 
                    .ToListAsync();

                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("{id}/history")]
        [Authorize(Policy = "RequireDroitConsIncidentsDeclarés")]
        public async Task<ActionResult<IEnumerable<IncidentHistory>>> GetIncidentHistory(int id)
        {
            try
            {
                var incident = await _context.Incidents.FindAsync(id);
                if (incident == null)
                {
                    return NotFound("Incident not found.");
                }

                var history = await _context.IncidentHistories
                    .Where(h => h.IncidentId == id)
                    .Include(h => h.OldStatus)
                    .Include(h => h.NewStatus)
                    .ToListAsync();
                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string GenerateUniqueIdentifier()
        {
            string datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            string randomPart = new Random().Next(1000, 9999).ToString();
            return $"{datePart}-{randomPart}";
        }
    }
}
