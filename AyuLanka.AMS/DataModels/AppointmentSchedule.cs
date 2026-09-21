using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AyuLanka.AMS.DataModels
{
    public class AppointmentSchedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime ScheduleDate { get; set; }

        public int? EmployeeId { get; set; }
        public int? SecondaryEmployeeId { get; set; }
        public int? DoctorEmployeeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CustomerName { get; set; }

        public int? CustomerId { get; set; }

        [Required]
        [MaxLength(15)]
        public string ContactNo { get; set; }

        [Required]
        public TimeSpan FromTime { get; set; }

        [Required]
        public TimeSpan ToTime { get; set; }
        
        public TimeSpan? ActualFromTime { get; set; }

        public TimeSpan? ActualToTime { get; set; }

        public TimeSpan? ActualFromTimeSecond { get; set; }

        public TimeSpan? ActualToTimeSecond { get; set; }

        [Required]
        public int EnteredBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }

        public DateTime EnteredDate { get; set; }
        public bool? IsDeleted { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public int? TokenNo { get; set; }
        public int? ChitNo { get; set; }
        public string? Remarks { get; set; }

        public DateTime TokenIssueTime { get; set; }
        public int? LocationId { get; set; }

        public int? MainTreatmentArea { get; set; }
        public bool? IsNeededToFollowUp { get; set; }
        public bool? IsPatientContacted { get; set; }

        [Required]
        public int CompanyId { get; set; }

        // Link to channeling session (Kottawa only; null for regular appointments)
        public int? DoctorSessionId { get; set; }

        // Self-referencing foreign key
        public int? ParentAppointmentScheduleId { get; set; }

        [ForeignKey(nameof(ParentAppointmentScheduleId))]
        public AppointmentSchedule? ParentAppointmentSchedule { get; set; }

        public ICollection<AppointmentSchedule>? ChildAppointments { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }

        [ForeignKey(nameof(SecondaryEmployeeId))]
        public Employee? SecondaryEmployee { get; set; }

        [ForeignKey(nameof(DoctorEmployeeId))]
        public Employee? DoctorEmployee { get; set; }

        [ForeignKey(nameof(EnteredBy))]
        public Employee? EnteredByEmployee { get; set; }

        [ForeignKey(nameof(UpdatedBy))]
        public Employee? UpdatedByEmployee { get; set; }

        [ForeignKey(nameof(DeletedBy))]
        public Employee? DeletedByEmployee { get; set; }

        [ForeignKey(nameof(LocationId))]
        public Location? Location { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }

        [ForeignKey(nameof(DoctorSessionId))]
        public DoctorSession? DoctorSession { get; set; }

        public ICollection<AppoinmentTreatment>? AppointmentTreatments { get; set; }
    }
}
