using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Part1.Models
{
    public class Claim
    {
        public int Id { get; set; }

        [Required] public string LecturerName { get; set; } = string.Empty;
        [Required, EmailAddress] public string LecturerEmail { get; set; } = string.Empty;

        [Required] public double HoursWorked { get; set; }
        [Required] public decimal HourlyRate { get; set; }

        [NotMapped]
        public decimal Total => (decimal)HoursWorked * HourlyRate; // calculated on the fly

        public string? Notes { get; set; }

        // stored filename relative to wwwroot/uploads
        public string? DocumentFileName { get; set; }

        public ClaimStatus Status { get; set; } = ClaimStatus.Pending;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        // public for EF and view binding
        public int SubmittedByUserId { get; set; }
        public int ClaimID { get; internal set; }
    }
}

