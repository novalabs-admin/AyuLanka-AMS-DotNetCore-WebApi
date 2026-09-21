using AyuLanka.AMS.DataModels;

namespace AyuLanka.AMS.Repositories.Contracts
{
    public interface IStaffRosterMasterRepository
    {
        Task<List<StaffRosterMaster>> GetRosterDateRangesAsync(int? companyId = null);
        Task<StaffRosterMaster> CreateRosterMasterAsync(StaffRosterMaster rosterMaster);
        Task<StaffRosterMaster> GetRosterMasterByIdAsync(int rosterMasterId);
        Task UpdateRosterMasterAsync(StaffRosterMaster rosterMaster);
    }
}
