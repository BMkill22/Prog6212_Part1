using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Part1.Models;
using System.Collections.Generic;

namespace Part1.Controllers
{
    public class LecturerController : Controller
    {
        public IActionResult Index()
        {
            var claims = new List<Claim>
            {
                new Claim { ClaimID = 1,LecturerName = "Siya Khumalo", HoursWorked = 10, HourlyRate = 200, Status = "pending"}
            };
            return View(claims);
        }
    }
}
