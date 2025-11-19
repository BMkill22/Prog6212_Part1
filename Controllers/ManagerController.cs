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
                
            };
            return View(claims);
        }
    }
}
