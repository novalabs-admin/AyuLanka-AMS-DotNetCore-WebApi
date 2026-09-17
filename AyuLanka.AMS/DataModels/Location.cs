using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AyuLanka.AMS.DataModels
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        // Foreign key
        public int? LocationTypeId { get; set; }

        public bool? IsTreatmentLocation { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [ForeignKey("LocationTypeId")]
        public LocationType? LocationType { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }
    }
}
