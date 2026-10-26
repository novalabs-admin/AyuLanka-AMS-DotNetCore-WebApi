namespace AyuLanka.AMS.AMSWeb.Models.RequestModels
{
    public class CityCreateRequestModel
    {
        public int DistrictId { get; set; }
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
