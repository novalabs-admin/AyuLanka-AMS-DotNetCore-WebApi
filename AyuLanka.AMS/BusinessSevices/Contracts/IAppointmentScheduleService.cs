using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.AMSWeb.Models.ResponseModels;
using AyuLanka.AMS.DataModels;

namespace AyuLanka.AMS.BusinessSevices.Contracts
{
    public interface IAppointmentScheduleService
    {
        Task<IEnumerable<AppointmentSchedule>> GetAllAppointmentSchedulesAsync();
        Task<AppointmentSchedule> GetAppointmentScheduleByIdAsync(int id);
        Task<IEnumerable<AppointmentSchedule>> GetAppointmentScheduleByDateAsync(DateTime date);
        Task<IEnumerable<AppointmentSchedule?>> GetPrimeCareAppointmentScheduleByDateAsync(DateTime date);
        Task<IEnumerable<AppointmentSchedule?>> GetTokensByDateAsync(DateTime date);
        Task<IEnumerable<AppointmentSchedule?>> GetIssuedTokensByDateAsync();
        Task<IEnumerable<AppointmentSchedule>> GetDeletedAppoitmentByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<AppointmentSchedule>> GetAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<AppointmentSchedule?>> GetAllAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DashboardDateChartDto?>> GetAllDashboardChartsDatabyDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DashboardSummaryDto>> GetDashboardSummaryByDateRangeAsync(DateTime startDate, DateTime endDate, string category);
        Task<IEnumerable<DashboardDetailsDto>> GetDashboardDetailsByDateAsync(DateTime date, string category, string type);
        Task<IEnumerable<AppointmentSchedule?>> GetPrimeCareAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<AppointmentSchedule?>> GetCompletedPreScheduledAppointmentAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<AppointmentSchedule?>> GetAllPreScheduledAppointmentAsync(DateTime startDate, DateTime endDate);
        Task<AppointmentSchedule> AddAppointmentScheduleAsync(AppointmentScheduleRequestModel appointmentScheduleRequestModel);
        Task<AppointmentSchedule> UpdateAppointmentScheduleAsync(AppointmentSchedule appointmentScheduleRequestModel);
        Task DeleteAppointmentScheduleAsync(int id, int deletedByUserId, string remark);
        Task<IEnumerable<object>> SearchPatientsAsync(string keyword);
        Task<IEnumerable<AppointmentSchedule?>> GetCustomerDetailsByIdAsync(int customerId);
        Task<object> CreateCustomerAsync(CreateCustomerRequest request);
        Task<List<EmployeeForAppointmentDto>> GetEmployeesByAppointmentIdsAsync(List<int> appointmentIds);
    }
}
