using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Part1.Models
{
    public class Claim
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Hours Worked")]
        [Range(0.1, 100, ErrorMessage = "Hours must be between 0.1 and 100.")]
        public double HoursWorked { get; set; }

        [Required]
        [Display(Name = "Hourly Rate")]
        [DataType(DataType.Currency)]
        [Range(1, 1000, ErrorMessage = "Rate must be between 1 and 1000.")]
        public decimal HourlyRate { get; set; }

        // NEW: Field for the auto-calculated total
        [Display(Name = "Total Amount")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        public string? Notes { get; set; }

        [Display(Name = "Submitted Date")]
        public DateTime SubmittedDate { get; set; }

        public ClaimStatus Status { get; set; }

        public ClaimStatus Total { get; set; }

        public ClaimStatus LecturerName { get; set; }
        

        [Display(Name = "Supporting Document")]
        public string? DocumentName { get; set; }

        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
        public object SubmittedAt { get; internal set; }
    }
}

