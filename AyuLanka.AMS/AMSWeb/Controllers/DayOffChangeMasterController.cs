using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace AyuLanka.AMS.AMSWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DayOffChangeMasterController : ControllerBase
    {
        private readonly IDayOffChangeMasterService _dayOffChangeMasterService;

        public DayOffChangeMasterController(IDayOffChangeMasterService dayOffChangeMasterService)
        {
            _dayOffChangeMasterService = dayOffChangeMasterService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DayOffChangeDetail>>> GetAllDayOffChangeDeatils([FromQuery] int? companyId = null)
        {
            var DayOffChangeMasters = await _dayOffChangeMasterService.GetAllDayOffChangeDetailsAsync(companyId);
            return Ok(DayOffChangeMasters);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DayOffChangeMaster>> GetDayOffChangeMasterById(int id)
        {
            var DayOffChangeMaster = await _dayOffChangeMasterService.GetDayOffChangeMasterByIdAsync(id);
            if (DayOffChangeMaster == null)
            {
                return NotFound();
            }
            return Ok(DayOffChangeMaster);
        }

        [HttpPost]
        public async Task<ActionResult<DayOffChangeMaster>> AddDayOffChangeMasters(DayOffChangeRequestModel dayOffChangeRequestModel)
        {
            await _dayOffChangeMasterService.AddDayOffChangesAsync(dayOffChangeRequestModel);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDayOffChangeMaster(int id, DayOffChangeMasterRequestModel dayOffChangeMasterRequestModel)
        {
            if (id != dayOffChangeMasterRequestModel.Id)
            {
                return BadRequest();
            }

            var updatedDayOffChangeMaster = await _dayOffChangeMasterService.UpdateDayOffChangeMasterAsync(id, dayOffChangeMasterRequestModel);
            if (updatedDayOffChangeMaster == null)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDayOffChangeMaster(int id)
        {
            await _dayOffChangeMasterService.DeleteDayOffChangeMasterAsync(id);
            return NoContent();
        }
    }
}
