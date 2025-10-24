using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part1.Data;
using Part1.Models;

namespace Part1.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminController(ApplicationDbContext db) { _db = db; }

        // GET: Admin/PendingClaims
        public async Task<IActionResult> PendingClaims()
        {
            var pending = await _db.Claims.Where(c => c.Status == ClaimStatus.Pending).OrderBy(c => c.SubmittedAt).ToListAsync();
            ViewBag.PendingCount = pending.Count;
            return View(pending);
        }

        // GET: Admin/Approve/{id}
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();
            return View(claim);
        }

        // POST: Admin/Approve/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveConfirmed(int id, string action)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            if (action == "approve")
                claim.Status = ClaimStatus.Approved;
            else if (action == "reject")
                claim.Status = ClaimStatus.Rejected;

            await _db.SaveChangesAsync();
            TempData["AdminMessage"] = $"Claim #{claim.Id} {claim.Status}";
            return RedirectToAction(nameof(PendingClaims));
        }

        // Optional: list all claims (for manager)
        public async Task<IActionResult> AllClaims()
        {
            var list = await _db.Claims.OrderByDescending(c => c.SubmittedAt).ToListAsync();
            return View("PendingClaims", list); // reuse view
        }
    }
}

