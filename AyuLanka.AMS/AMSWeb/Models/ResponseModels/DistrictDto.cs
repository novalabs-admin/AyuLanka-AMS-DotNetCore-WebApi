namespace AyuLanka.AMS.AMSWeb.Models.ResponseModels
{
    public class DistrictDto
    {
        public int Id { get; set; }
        public int ProvinceId { get; set; }
        public string? ProvinceName { get; set; }
        public string NameEn { get; set; }
        public string? NameSi { get; set; }
        public string? NameTa { get; set; }
    }
}
