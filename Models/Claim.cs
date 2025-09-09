using Microsoft.AspNetCore.Mvc;

namespace Part1.Models
{
    public class Claim : Controller
    {
        public int ClaimID { get; set; }
        public string LecturerName { get; set; }
        public double HoursWorked { get; set; }
        
        public double HourlyRate { get; set; }
        public string Status { get; set; }
    }
}
