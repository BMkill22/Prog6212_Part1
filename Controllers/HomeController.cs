using Microsoft.AspNetCore.Mvc;
using Part1.Data;
using Part1.Models;
using Microsoft.EntityFrameworkCore;

namespace Part1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db) { _db = db; }

        public async Task<IActionResult> Index()
        {
            // Show latest 5 claims
            var latest = await _db.Claims.OrderByDescending(c => c.SubmittedAt).Take(5).ToListAsync();
            ViewBag.LatestClaims = latest; // using ViewBag as requested
            return View();
        }
    }
}
