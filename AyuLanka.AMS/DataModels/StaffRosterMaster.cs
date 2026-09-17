using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AyuLanka.AMS.DataModels
{
    public class StaffRosterMaster
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime Todate { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }
    }
}
