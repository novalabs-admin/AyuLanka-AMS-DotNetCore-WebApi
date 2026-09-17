using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories.Contracts;

namespace AyuLanka.AMS.BusinessSevices
{
    public class StaffLeaveService : IStaffLeaveService
    {
        private readonly IStaffLeaveRepository _staffLeaveRepository;

        public StaffLeaveService(IStaffLeaveRepository staffLeaveRepository)
        {
            _staffLeaveRepository = staffLeaveRepository;
        }

        public async Task<IEnumerable<StaffLeave>> GetStaffLeavesByDateAsync(DateTime date)
        {
            return await _staffLeaveRepository.GetStaffLeavesByDateAsync(date);
        }

        public async Task<IEnumerable<StaffLeave>> GetAllStaffLeavesAsync(int? companyId = null)
        {
            return await _staffLeaveRepository.GetAllStaffLeavesAsync(companyId);
        }

        public async Task<StaffLeave> GetStaffLeaveByIdAsync(int id)
        {
            return await _staffLeaveRepository.GetStaffLeaveByIdAsync(id);
        }

        public async Task<StaffLeave> AddStaffLeaveAsync(StaffLeaveRequestModel staffLeaveRequestModel)
        {
            var StaffLeave = new StaffLeave()
            {
                EmployeeId = staffLeaveRequestModel.EmployeeId,
                LeaveTypeId = staffLeaveRequestModel.LeaveTypeId,
                NoOfDays = staffLeaveRequestModel.NoOfDays,
                HalfDay = staffLeaveRequestModel.HalfDay,
                FromDate = staffLeaveRequestModel.FromDate,
                ToDate = staffLeaveRequestModel.ToDate
            };
            return await _staffLeaveRepository.AddStaffLeaveAsync(StaffLeave);
        }

        public async Task<StaffLeave> UpdateStaffLeaveAsync(StaffLeaveRequestModel staffLeaveRequestModel)
        {
            // Retrieve the existing entity from the database
            var staffLeave = await _staffLeaveRepository.GetStaffLeaveByIdAsync(staffLeaveRequestModel.Id);
            if (staffLeave == null)
            {
                throw new Exception("Staff leave not found");
            }

            // Update properties
            staffLeave.EmployeeId = staffLeaveRequestModel.EmployeeId;
            staffLeave.LeaveTypeId = staffLeaveRequestModel.LeaveTypeId;
            staffLeave.NoOfDays = staffLeaveRequestModel.NoOfDays;
            staffLeave.HalfDay = staffLeaveRequestModel.HalfDay;
            staffLeave.FromDate = staffLeaveRequestModel.FromDate;
            staffLeave.ToDate = staffLeaveRequestModel.ToDate;

            return await _staffLeaveRepository.UpdateStaffLeaveAsync(staffLeave);
        }

        public async Task DeleteStaffLeaveAsync(int id)
        {
            await _staffLeaveRepository.DeleteStaffLeaveAsync(id);
        }

        public async Task<IEnumerable<StaffLeave>> GetStaffLeavesByDateRangeAsync(DateTime startDate, DateTime endDate, int? companyId = null)
        {
            return await _staffLeaveRepository.GetStaffLeavesByDateRangeAsync(startDate, endDate, companyId);
        }
    }
}
