using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace AyuLanka.AMS.AMSWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftChangeMasterController : ControllerBase
    {
        private readonly IShiftChangeMasterService _ShiftChangeMasterService;

        public ShiftChangeMasterController(IShiftChangeMasterService ShiftChangeMasterService)
        {
            _ShiftChangeMasterService = ShiftChangeMasterService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShiftChangeDetail>>> GetAllShiftChangeDeatils([FromQuery] int? companyId = null)
        {
            var ShiftChangeMasters = await _ShiftChangeMasterService.GetAllShiftChangeDetailsAsync(companyId);
            return Ok(ShiftChangeMasters);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShiftChangeMaster>> GetShiftChangeMasterById(int id)
        {
            var ShiftChangeMaster = await _ShiftChangeMasterService.GetShiftChangeMasterByIdAsync(id);
            if (ShiftChangeMaster == null)
            {
                return NotFound();
            }
            return Ok(ShiftChangeMaster);
        }

        [HttpPost]
        public async Task<ActionResult<ShiftChangeMaster>> AddShiftChangeMasters(ShiftChangeRequestModel ShiftChangeRequestModel)
        {
            await _ShiftChangeMasterService.AddShiftChangesAsync(ShiftChangeRequestModel);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateShiftChangeMaster(int id, ShiftChangeMasterRequestModel ShiftChangeMasterRequestModel)
        {
            if (id != ShiftChangeMasterRequestModel.Id)
            {
                return BadRequest();
            }

            var updatedShiftChangeMaster = await _ShiftChangeMasterService.UpdateShiftChangeMasterAsync(id, ShiftChangeMasterRequestModel);
            if (updatedShiftChangeMaster == null)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteShiftChangeMaster(int id)
        {
            await _ShiftChangeMasterService.DeleteShiftChangeMasterAsync(id);
            return NoContent();
        }
    }
}
