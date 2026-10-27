using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.AMSWeb.Models.ResponseModels;

namespace AyuLanka.AMS.BusinessSevices.Contracts
{
    public interface IGeographyService
    {
        Task<IEnumerable<object>> SearchCitiesAsync(string keyword, int limit = 20);

        Task<IEnumerable<ProvinceDto>> GetProvincesAsync();

        Task<IEnumerable<DistrictDto>> GetDistrictsAsync(int? provinceId = null);

        Task<object> CreateCityAsync(CityCreateRequestModel request);

        Task<IEnumerable<CountryDto>> SearchCountriesAsync(string keyword, int limit = 20);
    }
}
