using AyuLanka.AMS.AMSWeb.Models.ResponseModels;
using AyuLanka.AMS.BusinessSevices.Contracts;
using System.Text.Json;

namespace AyuLanka.AMS.BusinessSevices
{
    public class GeographyService : IGeographyService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GeographyService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<object>> SearchCitiesAsync(string keyword, int limit = 20)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return Enumerable.Empty<object>();

            var client = _httpClientFactory.CreateClient("CustomerApi");

            var response = await client.GetAsync(
                $"api/userManagement/geography/cities/search?term={Uri.EscapeDataString(keyword)}&limit={limit}"
            );

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<object>();

            var json = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<GeographySearchApiResponse>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return apiResponse?.Data?.Result
                ?.Select(c => new
                {
                    Id = c.Id,
                    DistrictId = c.DistrictId,
                    DistrictName = c.DistrictName,
                    ProvinceId = c.ProvinceId,
                    ProvinceName = c.ProvinceName,
                    NameEn = c.NameEn,
                    NameSi = c.NameSi,
                    NameTa = c.NameTa,
                    SubNameEn = c.SubNameEn,
                    SubNameSi = c.SubNameSi,
                    SubNameTa = c.SubNameTa,
                    PostCode = c.PostCode,
                    Latitude = c.Latitude,
                    Longitude = c.Longitude
                })
                ?? Enumerable.Empty<object>();
        }
    }
}
