using AyuLanka.AMS.AMSWeb.Models.ResponseModels;
using AyuLanka.AMS.DataModels;

namespace AyuLanka.AMS.Repositories.Contracts
{
    public interface IAppointmentScheduleRepository
    {
        Task<IEnumerable<AppointmentSchedule>> GetAllAppointmentSchedulesAsync();
        Task<AppointmentSchedule?> GetAppointmentScheduleByIdAsync(int id);
        Task<IEnumerable<AppointmentSchedule?>> GetAppointmentScheduleByDateAsync(DateTime date);
        Task<IEnumerable<AppointmentSchedule?>> GetPrimeCareAppointmentScheduleByDateAsync(DateTime date);
        Task<IEnumerable<AppointmentSchedule?>> GetTokensByDateAsync(DateTime date);
        Task<IEnumerable<AppointmentSchedule?>> GetIssuedTokensByDateAsync();
        Task<IEnumerable<AppointmentSchedule?>> GetDeletedAppoitmentByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<AppointmentSchedule?>> GetAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<AppointmentSchedule?>> GetAllAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DashboardDateChartDto?>> GetAllDashboardChartsDatabyDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DashboardSummaryDto>> GetDashboardSummaryByDateRangeAsync(DateTime startDate, DateTime endDate, string category);
        Task<IEnumerable<DashboardDetailsDto>> GetDashboardDetailsByDateAsync(DateTime date, string category, string type);
        Task<IEnumerable<AppointmentSchedule?>> GetPrimeCareAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<AppointmentSchedule?>> GetCompletedPreScheduledAppointmentAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<AppointmentSchedule?>> GetAllPreScheduledAppointmentAsync(DateTime startDate, DateTime endDate);
        Task<AppointmentSchedule> AddAppointmentScheduleAsync(AppointmentSchedule appointmentSchedule);
        Task<AppointmentSchedule> UpdateAppointmentScheduleAsync(AppointmentSchedule appointmentSchedule);
        Task DeleteAppointmentScheduleAsync(int id, int deletedByUserId, string remark);
        Task<int> GetMaxChitNoAsync(DateTime scheduleDate);
        Task<IEnumerable<AppointmentSchedule>> GetCustomerDetailsByIdAsync(int customerId);
        Task<bool> IsTokenExistsAsync(int tokenNo, DateTime scheduleDate, int? excludeAppointmentId = null);
        Task<List<EmployeeForAppointmentDto>> GetEmployeesByAppointmentIdsAsync(List<int> appointmentIds);
    }
}
