using AyuLanka.AMS.DataModels;

namespace AyuLanka.AMS.Repositories.Contracts
{
    public interface IShiftChangeDetailRepository
    {
        Task<IEnumerable<ShiftChangeDetail>> GetAllShiftChangeDetailsAsync(int? companyId = null);
        Task<ShiftChangeDetail> GetShiftChangeDetailByDatePreAndRosterAsync(int ShiftPre, int staffRosterId);
        Task<ShiftChangeDetail> GetShiftChangeDetailByIdAsync(int Id);
        Task<ShiftChangeDetail> CreateShiftChangeDetailAsync(ShiftChangeDetail ShiftChangeDetail);
        Task<ShiftChangeDetail> UpdateShiftChangeDetailAsync(ShiftChangeDetail ShiftChangeDetail);
    }
}
