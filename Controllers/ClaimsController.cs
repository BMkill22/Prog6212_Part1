using Microsoft.AspNetCore.Mvc;
using Part1.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Part1.Controllers
{
    public class ClaimsController : Controller
    {
        public IActionResult Index()
        {
            var claims = new List<Claim>
            {
                new Claim { ClaimID = 2,LecturerName = "messi Khumalo", HoursWorked = 5, HourlyRate = 50, Status = "pending"}
            };
            return View(claims);
        }
    }
}
