using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.AMSWeb.Models.ResponseModels;
using AyuLanka.AMS.BusinessSevices;
using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace AyuLanka.AMS.AMSWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentScheduleController : ControllerBase
    {
        private readonly IAppointmentScheduleService _appointmentScheduleService;
        private readonly IAppoinmentTreatmentService _appoinmentTreatmentService;

        public AppointmentScheduleController(IAppointmentScheduleService appointmentScheduleService, IAppoinmentTreatmentService appoinmentTreatmentService)
        {
            _appointmentScheduleService = appointmentScheduleService;
            _appoinmentTreatmentService = appoinmentTreatmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentSchedule>>> GetAllAppointmentSchedules([FromQuery] int? companyId = null)
        {
            var AppointmentSchedules = await _appointmentScheduleService.GetAllAppointmentSchedulesAsync(companyId);
            return Ok(AppointmentSchedules);
        }

        [HttpGet("DeletedAppointmentsByDateRange")]
        public async Task<ActionResult<AppointmentSchedule>> GetDeletedAppoitmentByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int? companyId = null)
        {
            var AppointmentSchedule = await _appointmentScheduleService.GetDeletedAppoitmentByDateRangeAsync(startDate, endDate, companyId);
            if (AppointmentSchedule == null)
            {
                return NotFound();
            }
            return Ok(AppointmentSchedule);
        }

        [HttpGet("ByDate/{date}")]
        public async Task<ActionResult<AppointmentSchedule>> GetAppointmentScheduleByDate(DateTime date, [FromQuery] int? companyId = null)
        {
            var AppointmentSchedule = await _appointmentScheduleService.GetAppointmentScheduleByDateAsync(date, companyId);
            if (AppointmentSchedule == null)
            {
                return NotFound();
            }
            return Ok(AppointmentSchedule);
        }

        [HttpGet("PrimeCareByDate/{date}")]
        public async Task<ActionResult<AppointmentSchedule>> GetPrimeCareAppointmentScheduleByDate(DateTime date, [FromQuery] int? companyId = null)
        {
            var AppointmentSchedule = await _appointmentScheduleService.GetPrimeCareAppointmentScheduleByDateAsync(date, companyId);
            if (AppointmentSchedule == null)
            {
                return NotFound();
            }
            return Ok(AppointmentSchedule);
        }

        [HttpGet("tokensbydate")]
        public async Task<ActionResult<AppointmentSchedule>> GetTokensByDate([FromQuery] DateTime date, [FromQuery] int? companyId = null)
        {
            var AppointmentSchedule = await _appointmentScheduleService.GetTokensByDateAsync(date, companyId);
            if (AppointmentSchedule == null)
            {
                return NotFound();
            }
            return Ok(AppointmentSchedule);
        }

        [HttpGet("tokensbydateandcompanycode")]
        public async Task<ActionResult<AppointmentSchedule>> GetTokensByDateAndCompanyCode([FromQuery] DateTime date, [FromQuery] string? companyCode = null)
        {
            var AppointmentSchedule = await _appointmentScheduleService.GetTokensByDateAndCompanyCodeAsync(date, companyCode);
            if (AppointmentSchedule == null)
            {
                return NotFound();
            }
            return Ok(AppointmentSchedule);
        }

        [HttpGet("issuedtokens")]
        public async Task<ActionResult<AppointmentSchedule>> GetIssuedTokensByDate([FromQuery] int? companyId = null)
        {
            var AppointmentSchedule = await _appointmentScheduleService.GetIssuedTokensByDateAsync(companyId);
            if (AppointmentSchedule == null)
            {
                return NotFound();
            }
            return Ok(AppointmentSchedule);
        }

        [HttpGet("bydaterange")]
        public async Task<ActionResult<IEnumerable<AppointmentSchedule>>> GetAppointmentScheduleByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int? companyId = null)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be before end date.");
            }

            var result = await _appointmentScheduleService.GetAppointmentScheduleByDateRangeAsync(startDate, endDate, companyId);
            return Ok(result);
        }

        [HttpGet("allbydaterange")]
        public async Task<ActionResult<IEnumerable<AppointmentSchedule>>> GetAllAppointmentScheduleByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int? companyId = null)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be before end date.");
            }

            var result = await _appointmentScheduleService.GetAllAppointmentScheduleByDateRangeAsync(startDate, endDate, companyId);
            return Ok(result);
        }

        [HttpGet("dashboardchartsdatabydaterange")]
        public async Task<ActionResult<IEnumerable<DashboardDateChartDto>>> GetAllDashboardChartsDatabyDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be before end date.");
            }

            var result = await _appointmentScheduleService.GetAllDashboardChartsDatabyDateRangeAsync(startDate, endDate);
            return Ok(result);
        }

        [HttpGet("dashboardSummaryByDateRange")]
        public async Task<ActionResult<IEnumerable<DashboardSummaryDto>>> GetDashboardSummaryByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string category)
        {
            if (startDate > endDate)
                return BadRequest("Start date must be before end date.");

            var result = await _appointmentScheduleService.GetDashboardSummaryByDateRangeAsync(startDate, endDate, category);
            return Ok(result);
        }


        [HttpGet("dashboarddetailsbydate")]
        public async Task<ActionResult<IEnumerable<DashboardDetailsDto>>> GetDashboardDetailsByDate([FromQuery] DateTime date, [FromQuery] string category, [FromQuery] string type)
        {
            if (date == DateTime.MinValue)
                return BadRequest("Invalid date");

            var result = await _appointmentScheduleService.GetDashboardDetailsByDateAsync(date, category, type);
            return Ok(result);
        }


        [HttpGet("patientsearch")]
        public async Task<ActionResult<IEnumerable<AppointmentSchedule>>> SearchPatients(
            string keyword, [FromQuery] string? companyCode = null)
        {
            var result = await _appointmentScheduleService.SearchPatientsAsync(keyword, companyCode);
            return Ok(result);
        }

        [HttpGet("customerprofile")]
        public async Task<ActionResult<IEnumerable<AppointmentSchedule>>> GetCustomerDetailsById(int customerId)
        {
            var result = await _appointmentScheduleService.GetCustomerDetailsByIdAsync(customerId);
            return Ok(result);
        }

        [HttpPost("create-customer")]
        public async Task<IActionResult> CreateCustomer(CreateCustomerRequest request)
        {
            var result = await _appointmentScheduleService.CreateCustomerAsync(request);
            return Ok(result);
        }

        [HttpGet("primecarebydaterange")]
        public async Task<ActionResult<IEnumerable<AppointmentSchedule>>> GetPrimeCareAppointmentScheduleByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int? companyId = null)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be before end date.");
            }

            var result = await _appointmentScheduleService.GetPrimeCareAppointmentScheduleByDateRangeAsync(startDate, endDate, companyId);
            return Ok(result);
        }

        [HttpGet("getAllPreScheduledScheduledAppointments")]
        public async Task<ActionResult<IEnumerable<AppointmentSchedule>>> GetAllPreScheduledAppointment([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int? companyId = null)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be before end date.");
            }

            var result = await _appointmentScheduleService.GetAllPreScheduledAppointmentAsync(startDate, endDate, companyId);
            return Ok(result);
        }

        [HttpGet("getCompletedPreScheduledScheduledAppointments")]
        public async Task<ActionResult<IEnumerable<AppointmentSchedule>>> GetCompletedPreScheduledAppointment([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int? companyId = null)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be before end date.");
            }

            var result = await _appointmentScheduleService.GetCompletedPreScheduledAppointmentAsync(startDate, endDate, companyId);
            return Ok(result);
        }

        [HttpGet("doctorechannelingbydaterange")]
        public async Task<ActionResult<IEnumerable<AppointmentSchedule>>> GetDoctorChannelingAppointmentsByDateRange(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int? companyId = null)
        {
            if (startDate > endDate)
                return BadRequest("Start date must be before end date.");
            var result = await _appointmentScheduleService.GetDoctorChannelingAppointmentsByDateRangeAsync(startDate, endDate, companyId);
            return Ok(result);
        }

        [HttpGet("ByDoctorSession/{sessionId}")]
        public async Task<ActionResult<IEnumerable<AppointmentSchedule>>> GetByDoctorSession(int sessionId, [FromQuery] int? companyId = null)
        {
            var result = await _appointmentScheduleService.GetByDoctorSessionIdAsync(sessionId, companyId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentSchedule>> GetAppointmentScheduleById(int id)
        {
            var AppointmentSchedule = await _appointmentScheduleService.GetAppointmentScheduleByIdAsync(id);
            if (AppointmentSchedule == null)
            {
                return NotFound();
            }
            return Ok(AppointmentSchedule);
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentSchedule>> AddAppointmentSchedule(AppointmentScheduleRequestModel appointmentScheduleRequestModel)
        {
            try
            {
                var createdAppointmentSchedule = await _appointmentScheduleService.AddAppointmentScheduleAsync(appointmentScheduleRequestModel);

                await _appoinmentTreatmentService.DeleteAppointmentTreatmentAsync(createdAppointmentSchedule.Id);
                foreach (var val in appointmentScheduleRequestModel.appoinmentTreatments)
                {
                    val.AppoinmentId = createdAppointmentSchedule.Id;
                    await _appoinmentTreatmentService.AddAppointmentTreatmentAsync(val);
                }

                return CreatedAtAction(nameof(GetAppointmentScheduleById), new { id = createdAppointmentSchedule.Id }, createdAppointmentSchedule);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    field = "tokenNo"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAppointmentSchedule(int id, AppointmentSchedule appointmentSchedule)
        {
            if (id != appointmentSchedule.Id)
            {
                return BadRequest();
            }

            await _appointmentScheduleService.UpdateAppointmentScheduleAsync(appointmentSchedule);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAppointmentSchedule(int id, int deletedByUserId, string remark)
        {
            await _appointmentScheduleService.DeleteAppointmentScheduleAsync(id, deletedByUserId, remark);
            return NoContent();
        }
    }
}
