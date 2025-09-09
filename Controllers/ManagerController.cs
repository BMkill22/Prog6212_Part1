using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Part1.Models;

namespace Part1.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            var claims = new List<Claim>
            {
                new Claim { ClaimID = 1, LecturerName = "Siya Khumalo", HoursWorked = 10, HourlyRate = 200, Status = "Pending"}
            };
            return View(claims);
        }
    }
}
