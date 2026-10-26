namespace AyuLanka.AMS.AMSWeb.Models.ResponseModels
{
    public class GeographyDto
    {
        public int Id { get; set; }
        public int DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public int ProvinceId { get; set; }
        public string? ProvinceName { get; set; }
        public string NameEn { get; set; }
        public string? NameSi { get; set; }
        public string? NameTa { get; set; }
        public string? SubNameEn { get; set; }
        public string? SubNameSi { get; set; }
        public string? SubNameTa { get; set; }
        public string? PostCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
