using AyuLanka.AMS.BusinessSevices.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AyuLanka.AMS.AMSWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeographyController : ControllerBase
    {
        private readonly IGeographyService _geographyService;

        public GeographyController(IGeographyService geographyService)
        {
            _geographyService = geographyService;
        }

        [HttpGet("search-cities")]
        public async Task<IActionResult> SearchCities([FromQuery] string keyword, [FromQuery] int limit = 20)
        {
            var result = await _geographyService.SearchCitiesAsync(keyword, limit);
            return Ok(result);
        }
    }
}
