using AyuLanka.AMS.DataModels;

namespace AyuLanka.AMS.Repositories.Contracts
{
    public interface IStaffLeaveRepository
    {
        Task<IEnumerable<StaffLeave>> GetStaffLeavesByDateAsync(DateTime date);
        Task<IEnumerable<StaffLeave>> GetAllStaffLeavesAsync(int? companyId = null);
        Task<StaffLeave> GetStaffLeaveByIdAsync(int id);
        Task<StaffLeave> GetEmployeeScheduleAsync(int employeeId, string scheduleDate);
        Task<StaffLeave> AddStaffLeaveAsync(StaffLeave StaffLeave);
        Task<StaffLeave> UpdateStaffLeaveAsync(StaffLeave StaffLeave);
        Task DeleteStaffLeaveAsync(int id);
        Task<IEnumerable<StaffLeave>> GetStaffLeavesByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null);
    }
}
