namespace AyuLanka.AMS.AMSWeb.Models.ResponseModels
{
    public class EmployeeForAppointmentDto
    {
        public int AppointmentId { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeFullName { get; set; }
        public string? EmployeeCallingName { get; set; }
        public string? EmployeeNumber { get; set; }
    }
}
