namespace AyuLanka.AMS.AMSWeb.Models.RequestModels
{
    public class CreateCustomerRequest
    {
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string? CompanyCode { get; set; }
    }
}
