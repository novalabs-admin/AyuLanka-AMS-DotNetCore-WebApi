using AyuLanka.AMS.AMSWeb.Models.RequestModels;
using AyuLanka.AMS.AMSWeb.Models.ResponseModels;
using AyuLanka.AMS.BusinessSevices.Contracts;
using System.Text.Json;

namespace AyuLanka.AMS.BusinessSevices
{
    public class GeographyService : IGeographyService
    {
        private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

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

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<GeographyDto>>(json, JsonOptions);

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

        public Task<IEnumerable<ProvinceDto>> GetProvincesAsync() =>
            GetResultAsync<ProvinceDto>("api/userManagement/geography/provinces");

        public Task<IEnumerable<DistrictDto>> GetDistrictsAsync(int? provinceId = null) =>
            GetResultAsync<DistrictDto>(provinceId.HasValue
                ? $"api/userManagement/geography/districts?provinceId={provinceId}"
                : "api/userManagement/geography/districts");

        public async Task<object> CreateCityAsync(CityCreateRequestModel request)
        {
            var client = _httpClientFactory.CreateClient("CustomerApi");

            using var response = await client.PostAsJsonAsync("api/userManagement/geography/cities", request);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(
                    $"Customer API returned {(int)response.StatusCode}: {body}", null, response.StatusCode);

            return JsonSerializer.Deserialize<JsonElement>(body);
        }

        private async Task<IEnumerable<T>> GetResultAsync<T>(string url)
        {
            var client = _httpClientFactory.CreateClient("CustomerApi");

            using var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(
                    $"Customer API returned {(int)response.StatusCode}: {body}", null, response.StatusCode);

            return JsonSerializer.Deserialize<ApiResponse<T>>(body, JsonOptions)?.Data?.Result
                ?? Enumerable.Empty<T>();
        }
    }
}
