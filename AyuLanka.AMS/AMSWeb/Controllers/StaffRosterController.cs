using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.BusinessSevices;
using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace AyuLanka.AMS.AMSWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffRosterController : ControllerBase
    {
        private readonly IStaffRosterService _staffRosterService;

        public StaffRosterController(IStaffRosterService staffRosterService)
        {
            _staffRosterService = staffRosterService;
        }

        [HttpGet("getdayoffsbydate/{date}")]
        public async Task<ActionResult<IEnumerable<StaffRoster>>> GetDayOffsByDate(DateTime date)
        {
            var dayOffs = await _staffRosterService.GetDayOffsByDateAsync(date);
            return Ok(dayOffs);
        }

        [HttpGet("getworkingshiftssbydate/{date}")]
        public async Task<ActionResult<IEnumerable<StaffRoster>>> GetWorkingShiftsByDate(DateTime date)
        {
            var dayOffs = await _staffRosterService.GetWorkingShiftsByDate(date);
            return Ok(dayOffs);
        }

        [HttpGet("dates")]
        public async Task<IActionResult> GetRosterDates([FromQuery] int? companyId = null)
        {
            try
            {
                var dates = await _staffRosterService.GetRosterDateRangesAsync(companyId);
                return Ok(dates.Select(d => new
                {
                    Id = d.Id,
                    StartDate = d.FromDate,
                    EndDate = d.Todate,
                    IsApproved = d.IsApproved,
                    ApprovedBy = d.ApprovedBy,
                    ApprovedDate = d.ApprovedDate
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving roster dates.");
            }
        }

        [HttpGet("approvedDates/{employeeId}/{rosterMasterId}")]
        public async Task<IActionResult> GetApprovedRosterDates(int employeeId, int rosterMasterId)
        {
            try
            {
                var dates = await _staffRosterService.GetApprovedRosterDatesAsync(employeeId, rosterMasterId);
                if (dates == null)
                {
                    return NotFound($"No roster found with ID {dates}");
                }
                return Ok(dates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving roster dates.");
            }
        }

        [HttpGet("employeeschedule/{employeeId}")]
        public async Task<IActionResult> GetApprovedRosterDates(int employeeId, [FromQuery] string scheduledate)
        {
            try
            {
                var employeeRoster = await _staffRosterService.GetEmployeeScheduleAsync(employeeId, scheduledate);
                    if (employeeRoster == null)
                {
                    return Ok(employeeRoster);
                }
                return Ok(employeeRoster);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving roster dates.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<StaffRoster>> SaveRoster(StaffRosterRequestModel staffRosterRequestModel)
        {
            try
            {
                var result = await _staffRosterService.SaveRoster(staffRosterRequestModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while saving roster dates.");
            }
        }

        [HttpGet("{rosterMasterId}")]
        public async Task<IActionResult> GetRosterDetails(int rosterMasterId)
        {
            try
            {
                var rosterDetails = await _staffRosterService.GetRosterByRosterMasterIdAsync(rosterMasterId);
                if (rosterDetails == null)
                {
                    return NotFound($"No roster found with ID {rosterMasterId}");
                }
                return Ok(rosterDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving roster details: {ex.Message}");
            }
        }

        [HttpPut("{rosterId}")]
        public async Task<IActionResult> UpdateRoster(int rosterId, StaffRosterRequestModel staffRosterRequestModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedRoster = await _staffRosterService.UpdateRoster(rosterId, staffRosterRequestModel);
                if (updatedRoster == null)
                {
                    return NotFound($"No roster found with ID {rosterId}");
                }

                return Ok(updatedRoster);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the roster: {ex.Message}");
            }
        }
    }
}
