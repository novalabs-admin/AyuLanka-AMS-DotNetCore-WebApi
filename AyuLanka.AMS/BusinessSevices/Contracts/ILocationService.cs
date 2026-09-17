using AyuLanka.AMS.DataModels;

namespace AyuLanka.AMS.BusinessSevices.Contracts
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetAllLocationAsync();
        Task<IEnumerable<Location>> GetPrimeCareLocationAsync(int? companyId = null);
        Task<IEnumerable<Location>> GetEliteCareLocationAsync(int? companyId = null);
        Task<IEnumerable<Location>> GetDoctorChannelingLocationAsync(int? companyId = null);
    }
}
