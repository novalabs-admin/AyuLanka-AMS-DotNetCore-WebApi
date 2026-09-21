using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.DataModels;

namespace AyuLanka.AMS.BusinessSevices.Contracts
{
    public interface IShiftChangeMasterService
    {
        Task<IEnumerable<ShiftChangeDetail>> GetAllShiftChangeDetailsAsync(int? companyId = null);
        Task<ShiftChangeMaster> GetShiftChangeMasterByIdAsync(int id);
        Task AddShiftChangesAsync(ShiftChangeRequestModel ShiftChangeRequestModel);
        Task<ShiftChangeMaster> UpdateShiftChangeMasterAsync(int id, ShiftChangeMasterRequestModel ShiftChangeMasterRequestModel);
        Task DeleteShiftChangeMasterAsync(int id);
    }
}
