using Microsoft.AspNetCore.Mvc;
using Part1.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Part1.Controllers
{
  
    public class ProgramController : Controller
    {       public IActionResult Index()
            {
                var claims = new List<Claim>
            {
                new Claim { ClaimID = 1,LecturerName = "messi Khumalo", HoursWorked = 5, HourlyRate = 50, Status = "pending"}
            };
                return View(claims);
        }

        private IActionResult View(List<Claim> claims)
        {
            throw new NotImplementedException();
        }
    }
}

