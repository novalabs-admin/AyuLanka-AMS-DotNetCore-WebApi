namespace AyuLanka.AMS.AMSWeb.Models.ResponseModels
{
    public class CountryDto
    {
        public int Id { get; set; }
        public string Alpha2 { get; set; } = null!;
        public string Alpha3 { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
