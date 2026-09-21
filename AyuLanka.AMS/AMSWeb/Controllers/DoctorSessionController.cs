using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace AyuLanka.AMS.AMSWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorSessionController : ControllerBase
    {
        private readonly IDoctorSessionService _doctorSessionService;

        public DoctorSessionController(IDoctorSessionService doctorSessionService)
        {
            _doctorSessionService = doctorSessionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorSession>>> GetAll(
            [FromQuery] int? companyId, [FromQuery] DateTime? date)
        {
            var sessions = await _doctorSessionService.GetAllAsync(companyId, date);
            return Ok(sessions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorSession>> GetById(int id)
        {
            var session = await _doctorSessionService.GetByIdAsync(id);
            if (session == null) return NotFound();
            return Ok(session);
        }

        [HttpGet("{id}/Availability")]
        public async Task<ActionResult<object>> GetAvailability(int id)
        {
            var availability = await _doctorSessionService.GetAvailabilityAsync(id);
            if (availability == null) return NotFound();
            return Ok(availability);
        }

        [HttpGet("ByDoctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<DoctorSession>>> GetByDoctor(
            int doctorId, [FromQuery] DateTime? date)
        {
            var sessions = await _doctorSessionService.GetByDoctorAsync(doctorId, date);
            return Ok(sessions);
        }

        [HttpPost]
        public async Task<ActionResult<DoctorSession>> Add(DoctorSession session)
        {
            try
            {
                var created = await _doctorSessionService.AddAsync(session);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DoctorSession>> Update(int id, DoctorSession session)
        {
            try
            {
                var updated = await _doctorSessionService.UpdateAsync(id, session);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
