using AyuLanka.AMS.AMSWeb.Models.ResponseModels;
using AyuLanka.AMS.DataModels;

namespace AyuLanka.AMS.Repositories.Contracts
{
    public interface IAppointmentScheduleRepository
    {
        Task<IEnumerable<AppointmentSchedule>> GetAllAppointmentSchedulesAsync(int? companyId = null);
        Task<AppointmentSchedule?> GetAppointmentScheduleByIdAsync(int id);
        Task<IEnumerable<AppointmentSchedule?>> GetAppointmentScheduleByDateAsync(DateTime date, int? companyId = null);
        Task<IEnumerable<AppointmentSchedule?>> GetPrimeCareAppointmentScheduleByDateAsync(DateTime date, int? companyId = null);
        Task<IEnumerable<AppointmentSchedule?>> GetTokensByDateAsync(DateTime date, int? companyId = null);
        Task<IEnumerable<AppointmentSchedule?>> GetTokensByDateAndCompanyCodeAsync(DateTime date, string? companyCode = null);
        Task<IEnumerable<AppointmentSchedule?>> GetIssuedTokensByDateAsync(int? companyId = null);
        Task<IEnumerable<AppointmentSchedule?>> GetDeletedAppoitmentByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null);
        Task<IEnumerable<AppointmentSchedule?>> GetAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null);
        Task<IEnumerable<AppointmentSchedule?>> GetAllAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null);
        Task<IEnumerable<DashboardDateChartDto?>> GetAllDashboardChartsDatabyDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DashboardSummaryDto>> GetDashboardSummaryByDateRangeAsync(DateTime startDate, DateTime endDate, string category);
        Task<IEnumerable<DashboardDetailsDto>> GetDashboardDetailsByDateAsync(DateTime date, string category, string type);
        Task<IEnumerable<AppointmentSchedule?>> GetPrimeCareAppointmentScheduleByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null);
        Task<IEnumerable<AppointmentSchedule?>> GetCompletedPreScheduledAppointmentAsync(DateTime startDate, DateTime endDate, int? companyId = null);
        Task<IEnumerable<AppointmentSchedule?>> GetAllPreScheduledAppointmentAsync(DateTime startDate, DateTime endDate, int? companyId = null);
        Task<AppointmentSchedule> AddAppointmentScheduleAsync(AppointmentSchedule appointmentSchedule);
        Task<AppointmentSchedule> UpdateAppointmentScheduleAsync(AppointmentSchedule appointmentSchedule);
        Task DeleteAppointmentScheduleAsync(int id, int deletedByUserId, string remark);
        Task<int> GetMaxChitNoAsync(DateTime scheduleDate, int? companyId = null);
        Task<IEnumerable<AppointmentSchedule>> GetCustomerDetailsByIdAsync(int customerId);
        Task<bool> IsTokenExistsAsync(int tokenNo, DateTime scheduleDate, int? excludeAppointmentId = null);
        Task<IEnumerable<AppointmentSchedule>> GetByDoctorSessionIdAsync(int sessionId, int? companyId = null);
        Task<IEnumerable<AppointmentSchedule>> GetDoctorChannelingAppointmentsByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null);
    }
}
