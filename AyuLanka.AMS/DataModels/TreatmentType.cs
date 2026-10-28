using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AyuLanka.AMS.DataModels
{
    public class TreatmentType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]  // Assuming a maximum length of 100 characters for the name
        public string Name { get; set; }

        [Required]
        public int DurationHours { get; set; }

        public int? DurationMinutes { get; set; }
        public bool IsActive { get; set; } = true;
        public string? TreatmentShortCode { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal? UnitPrice { get; set; }
    }
}
