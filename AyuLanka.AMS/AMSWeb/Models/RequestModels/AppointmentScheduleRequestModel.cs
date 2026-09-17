using System.ComponentModel.DataAnnotations;

namespace AyuLanka.AMS.AMSWeb.Models.RequestModels
{
    public class AppointmentScheduleRequestModel
    {
        public int Id { get; set; }
        public DateTime ScheduleDate { get; set; }
        public int? EmployeeId { get; set; }
        public int? SecondaryEmployeeId { get; set; }
        public int? DoctorEmployeeId { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string ContactNo { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public TimeSpan? ActualFromTime { get; set; }
        public TimeSpan? ActualToTime { get; set; }
        public TimeSpan? ActualFromTimeSecond { get; set; }
        public TimeSpan? ActualToTimeSecond { get; set; }
        public int EnteredBy { get; set; }
        public DateTime EnteredDate { get; set; }
        public int? TokenNo { get; set; }
        public bool IsTokenIssued { get; set; }
        public DateTime TokenIssueTime { get; set; }
        public string? Remarks { get; set; }
        public int? LocationId { get; set; }
        public int? MainTreatmentArea { get; set; }
        public bool? IsNeededToFollowUp { get; set; }
        public bool? IsPatientContacted { get; set; }
        public int? ParentAppointmentScheduleId { get; set; }
        public int CompanyId { get; set; }
        public int? DoctorSessionId { get; set; }

        public IEnumerable<AppoinmentTreatmentRequestModel> appoinmentTreatments { get; set; }
    }
}
