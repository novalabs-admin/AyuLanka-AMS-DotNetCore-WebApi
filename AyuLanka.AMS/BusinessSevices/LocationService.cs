using AyuLanka.AMS.BusinessSevices.Contracts;
using AyuLanka.AMS.DataModels;
using AyuLanka.AMS.Repositories.Contracts;

namespace AyuLanka.AMS.BusinessSevices
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<IEnumerable<Location>> GetAllLocationAsync()
        {
            return await _locationRepository.GetAllLocationAsync();
        }

        public async Task<IEnumerable<Location>> GetPrimeCareLocationAsync(int? companyId = null)
        {
            return await _locationRepository.GetPrimeCareLocationAsync(companyId);
        }

        public async Task<IEnumerable<Location>> GetEliteCareLocationAsync(int? companyId = null)
        {
            return await _locationRepository.GetEliteCareLocationAsync(companyId);
        }

        public async Task<IEnumerable<Location>> GetDoctorChannelingLocationAsync(int? companyId = null)
        {
            return await _locationRepository.GetDoctorChannelingLocationAsync(companyId);
        }
    }
}
