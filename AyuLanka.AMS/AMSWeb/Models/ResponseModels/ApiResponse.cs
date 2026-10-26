namespace AyuLanka.AMS.AMSWeb.Models.ResponseModels
{
    // Envelope returned by the user-management (customer) API
    public class ApiResponse<T>
    {
        public ApiData<T> Data { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }

    public class ApiData<T>
    {
        public List<T> Result { get; set; }
        public bool HandShake { get; set; }
    }
}
