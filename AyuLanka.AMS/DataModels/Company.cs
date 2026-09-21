using System.ComponentModel.DataAnnotations;

namespace AyuLanka.AMS.DataModels
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Short unique identifier sent to the external customer API (e.g. "WATTALA", "KOTTAWA").
        /// </summary>
        [MaxLength(20)]
        public string? CompanyCode { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }

        [MaxLength(20)]
        public string? PhoneNo { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
