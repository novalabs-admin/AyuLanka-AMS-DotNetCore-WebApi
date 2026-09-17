using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.DataModels;

namespace AyuLanka.AMS.BusinessSevices.Contracts
{
    public interface IStaffLeaveService
    {
        Task<IEnumerable<StaffLeave>> GetStaffLeavesByDateAsync(DateTime date);
        Task<IEnumerable<StaffLeave>> GetAllStaffLeavesAsync(int? companyId = null);
        Task<StaffLeave> GetStaffLeaveByIdAsync(int id);
        Task<StaffLeave> AddStaffLeaveAsync(StaffLeaveRequestModel staffLeaveRequestModel);
        Task<StaffLeave> UpdateStaffLeaveAsync(StaffLeaveRequestModel staffLeaveRequestModel);
        Task DeleteStaffLeaveAsync(int id);
        Task<IEnumerable<StaffLeave>> GetStaffLeavesByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null);
    }
}
