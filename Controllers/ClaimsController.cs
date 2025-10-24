using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part1.Data;
using Part1.Models;

namespace Part1.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ClaimsController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // GET: Claims
        public async Task<IActionResult> Index()
        {
            var claims = await _db.Claims.OrderByDescending(c => c.SubmittedAt).ToListAsync();
            return View(claims);
        }

        // GET: Claims/Create
        public IActionResult Create()
        {
            // Provide some ViewBag data for the form
            ViewBag.Roles = Enum.GetNames(typeof(UserRole));
            ViewBag.Statuses = Enum.GetNames(typeof(ClaimStatus));
            return View();
        }

        // POST: Claims/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LecturerName,LecturerEmail,HoursWorked,HourlyRate,Notes")] Claim claim, IFormFile? document)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = Enum.GetNames(typeof(UserRole));
                return View(claim);
            }

            // File upload handling
            if (document != null && document.Length > 0)
            {
                var allowed = new[] { ".pdf", ".docx", ".xlsx", ".doc" };
                var ext = Path.GetExtension(document.FileName).ToLowerInvariant();
                if (!allowed.Contains(ext))
                {
                    ModelState.AddModelError("Document", "Only PDF/DOCX/XLSX allowed");
                    ViewBag.Roles = Enum.GetNames(typeof(UserRole));
                    return View(claim);
                }

                var uploads = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
                Directory.CreateDirectory(uploads);
                var unique = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploads, unique);
                using var stream = System.IO.File.Create(filePath);
                await document.CopyToAsync(stream);

                claim.DocumentFileName = unique;
            }

            // Basic validation on numeric fields
            if (claim.HoursWorked <= 0 || claim.HourlyRate <= 0)
            {
                ModelState.AddModelError("", "Hours and hourly rate must be greater than zero.");
                ViewBag.Roles = Enum.GetNames(typeof(UserRole));
                return View(claim);
            }

            claim.Status = ClaimStatus.Pending;
            claim.SubmittedAt = DateTime.UtcNow;

            _db.Claims.Add(claim);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Claim submitted successfully.";
            return RedirectToAction(nameof(LecturerClaims));
        }

        // GET: Claims/LecturerClaims
        public async Task<IActionResult> LecturerClaims(string? email)
        {
            ViewBag.FilterEmail = email ?? string.Empty;
            IQueryable<Claim> query = _db.Claims;
            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(c => c.LecturerEmail.ToLower() == email.ToLower());
            }
            var list = await query.OrderByDescending(c => c.SubmittedAt).ToListAsync();
            return View(list);
        }

        // GET: Claims/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();
            return View(claim);
        }

        // helper to serve file download
        public IActionResult DownloadDocument(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return NotFound();
            var path = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", fileName);
            if (!System.IO.File.Exists(path)) return NotFound();
            var mime = "application/octet-stream";
            return PhysicalFile(path, mime, fileName);
        }
    }
}

