using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace AyuLanka.AMS.AMSWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffLeaveController : ControllerBase
    {
        private readonly IStaffLeaveService _staffLeaveService;

        public StaffLeaveController(IStaffLeaveService staffLeaveService)
        {
            _staffLeaveService = staffLeaveService;
        }

        [HttpGet("getleavesbydate/{date}")]
        public async Task<ActionResult<IEnumerable<StaffLeave>>> GetStaffLeavesByDate(DateTime date)
        {
            var staffLeaves = await _staffLeaveService.GetStaffLeavesByDateAsync(date);
            return Ok(staffLeaves);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffLeave>>> GetAllStaffLeaves([FromQuery] int? companyId = null)
        {
            var StaffLeaves = await _staffLeaveService.GetAllStaffLeavesAsync(companyId);
            return Ok(StaffLeaves);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StaffLeave>> GetStaffLeaveById(int id)
        {
            var StaffLeave = await _staffLeaveService.GetStaffLeaveByIdAsync(id);
            if (StaffLeave == null)
            {
                return NotFound();
            }
            return Ok(StaffLeave);
        }

        [HttpPost]
        public async Task<ActionResult<StaffLeave>> AddStaffLeave(StaffLeaveRequestModel staffLeaveRequestModel)
        {
            var createdStaffLeave = await _staffLeaveService.AddStaffLeaveAsync(staffLeaveRequestModel);
            return CreatedAtAction(nameof(GetStaffLeaveById), new { id = createdStaffLeave.Id }, createdStaffLeave);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStaffLeave(int id, StaffLeaveRequestModel staffLeaveRequestModel)
        {
            if (id != staffLeaveRequestModel.Id)
            {
                return BadRequest();
            }

            await _staffLeaveService.UpdateStaffLeaveAsync(staffLeaveRequestModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStaffLeave(int id)
        {
            await _staffLeaveService.DeleteStaffLeaveAsync(id);
            return NoContent();
        }

        [HttpGet("bydaterange")]
        public async Task<ActionResult<IEnumerable<StaffLeave>>> GetStaffLeavesByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int? companyId = null)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be before end date.");
            }

            var staffLeaves = await _staffLeaveService.GetStaffLeavesByDateRangeAsync(startDate, endDate, companyId);
            return Ok(staffLeaves);
        }

    }
}
