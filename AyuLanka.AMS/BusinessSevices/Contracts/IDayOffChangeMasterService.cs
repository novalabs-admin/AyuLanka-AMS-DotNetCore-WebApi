using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.DataModels;

namespace AyuLanka.AMS.BusinessSevices.Contracts
{
    public interface IDayOffChangeMasterService
    {
        Task<IEnumerable<DayOffChangeDetail>> GetAllDayOffChangeDetailsAsync(int? companyId = null);
        Task<DayOffChangeMaster> GetDayOffChangeMasterByIdAsync(int id);
        Task AddDayOffChangesAsync(DayOffChangeRequestModel dayOffChangeRequestModel);
        Task<DayOffChangeMaster> UpdateDayOffChangeMasterAsync(int id, DayOffChangeMasterRequestModel DayOffChangeMasterRequestModel);
        Task DeleteDayOffChangeMasterAsync(int id);
    }
}
