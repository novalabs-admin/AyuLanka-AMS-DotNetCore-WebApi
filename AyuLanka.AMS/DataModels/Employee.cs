using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AyuLanka.AMS.DataModels
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ShiftMasterId { get; set; }

        [Required]
        public int EmploymentTypeId { get; set; }

        [Required]
        public int DesignationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(100)]
        public string CallingName { get; set; }

        [Required]
        [MaxLength(50)]
        public string EmployeeNumber { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required]
        [MaxLength(15)]
        public string NIC { get; set; }

        [Required]
        public DateTime JoinedDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(DesignationId))]
        public Designation? Designation { get; set; }

        [ForeignKey(nameof(EmploymentTypeId))]
        public EmploymentType? EmploymentType { get; set; }

        [ForeignKey(nameof(ShiftMasterId))]
        public ShiftMaster? ShiftMaster { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }
    }
}
