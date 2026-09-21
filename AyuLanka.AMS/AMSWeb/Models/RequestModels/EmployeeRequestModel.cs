using System.ComponentModel.DataAnnotations;

namespace AyuLanka.AMS.AMSWeb.Models.RequestModels
{
    public class EmployeeRequestModel
    {
        public int Id { get; set; }
        public int ShiftMasterId { get; set; }
        public int EmploymentTypeId { get; set; }
        public int DesignationId { get; set; }
        public string FullName { get; set; }
        public string CallingName { get; set; }
        public string EmployeeNumber { get; set; }
        public string Address { get; set; }
        public string NIC { get; set; }
        public DateTime JoinedDate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int CompanyId { get; set; }
    }
}
