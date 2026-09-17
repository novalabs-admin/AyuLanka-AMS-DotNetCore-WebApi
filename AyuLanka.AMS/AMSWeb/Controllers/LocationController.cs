using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace AyuLanka.AMS.AMSWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetAllLocationAsync()
        {
            var Locations = await _locationService.GetAllLocationAsync();
            return Ok(Locations);
        }

        [HttpGet("primecare")]
        public async Task<ActionResult<IEnumerable<Location>>> GetPrimeCareLocationAsync([FromQuery] int? companyId = null)
        {
            var Locations = await _locationService.GetPrimeCareLocationAsync(companyId);
            return Ok(Locations);
        }

        [HttpGet("elitecare")]
        public async Task<ActionResult<IEnumerable<Location>>> GetEliteCareLocationAsync([FromQuery] int? companyId = null)
        {
            var Locations = await _locationService.GetEliteCareLocationAsync(companyId);
            return Ok(Locations);
        }

        [HttpGet("doctorechanneling")]
        public async Task<ActionResult<IEnumerable<Location>>> GetDoctorChannelingLocationAsync([FromQuery] int? companyId = null)
        {
            var Locations = await _locationService.GetDoctorChannelingLocationAsync(companyId);
            return Ok(Locations);
        }

    }
}
