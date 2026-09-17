using AyuLanka.AMS.DataModels;

namespace AyuLanka.AMS.Repositories.Contracts
{
    public interface ILocationRepository
    {
        Task<Location> GetLocationByLocationIdAsync(int locationId);
        Task<IEnumerable<Location>> GetAllLocationAsync(int? companyId = null);
        Task<IEnumerable<Location>> GetPrimeCareLocationAsync(int? companyId = null);
        Task<IEnumerable<Location>> GetEliteCareLocationAsync(int? companyId = null);
        Task<IEnumerable<Location>> GetDoctorChannelingLocationAsync(int? companyId = null);
        Task<Location> GetTreatmentLocationByNameAsync(string locationName, int? companyId = null);
    }
}
