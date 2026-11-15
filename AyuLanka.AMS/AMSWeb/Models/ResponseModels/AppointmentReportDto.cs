using System.Text.Json.Serialization;

namespace AyuLanka.AMS.AMSWeb.Models.ResponseModels
{
    // Slim row shared by the Appointment Report and the Appointments Missing Next Visit report
    public class AppointmentReportDto
    {
        public int Id { get; set; }
        public int? TokenNo { get; set; }
        public int? ChitNo { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ContactNo { get; set; }
        public string? EmployeeName { get; set; }
        public string? SecondaryEmployeeName { get; set; }
        public string? LocationName { get; set; }
        public List<string> TreatmentTypes { get; set; } = new();

        // Distinct item mode names of this appointment's treatment types (filled for the Appointment Report only)
        public List<string> ItemModes { get; set; } = new();

        // Item codes of the treatment types, used to look up the item modes; not part of the response
        [JsonIgnore]
        public List<string> TreatmentShortCodes { get; set; } = new();
        public DateTime ScheduleDate { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public TimeSpan? ActualFromTime { get; set; }
        public TimeSpan? ActualToTime { get; set; }
        public string? Remarks { get; set; }
        public DateTime TokenIssueTime { get; set; }
        public string? EnteredByName { get; set; }
        public DateTime EnteredDate { get; set; }
    }
}
