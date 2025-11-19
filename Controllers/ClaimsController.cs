using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part1.Data;
using Part1.Models;

namespace Part1.Controllers
{
    [Authorize]
    public class ClaimController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClaimController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // LECTURER: Submit Claim (Default Page)
        [Authorize(Roles = "Lecturer")]
        public IActionResult Create()
        {
            ViewBag.Title = "Submit New Claim";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> Create(Claim claim, IFormFile? document)
        {
            ViewBag.Title = "Submit New Claim";

            // AUTOMATION: Auto-calculate on the server side to ensure accuracy
            claim.TotalAmount = (decimal)claim.HoursWorked * claim.HourlyRate;

            if (ModelState.IsValid)
            {
                if (document != null && document.Length > 0)
                {
                    var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
                    var ext = Path.GetExtension(document.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(ext))
                    {
                        ViewBag.Message = "Error: Invalid file type. Only PDF, DOCX, and XLSX are allowed.";
                        return View(claim);
                    }

                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + document.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await document.CopyToAsync(fileStream);
                    }
                    claim.DocumentName = uniqueFileName;
                }

                claim.SubmittedDate = DateTime.Now;
                claim.Status = ClaimStatus.Pending;
                claim.UserId = _userManager.GetUserId(User);

                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                ViewBag.Message = "Claim submitted successfully! Total calculated: " + claim.TotalAmount.ToString("C");
                return View("Create");
            }
            return View(claim);
        }

        // LECTURER: My Claims
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var claims = await _context.Claims.Where(c => c.UserId == userId).OrderByDescending(c => c.SubmittedDate).ToListAsync();
            return View(claims);
        }

        // MANAGER: Dashboard
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ManagerView()
        {
            // Include User data to see who submitted the claim
            var pendingClaims = await _context.Claims.Include(c => c.User).Where(c => c.Status == ClaimStatus.Pending).ToListAsync();
            return View(pendingClaims);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null) { claim.Status = ClaimStatus.Approved; await _context.SaveChangesAsync(); }
            return RedirectToAction("ManagerView");
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null) { claim.Status = ClaimStatus.Rejected; await _context.SaveChangesAsync(); }
            return RedirectToAction("ManagerView");
        }

        // --- PART 3 NEW FEATURES: HR VIEW ---

        // HR: Reporting Dashboard (Invoices)
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> HRDashboard()
        {
            // Fetch only APPROVED claims for payment processing
            var approvedClaims = await _context.Claims
                .Include(c => c.User)
                .Where(c => c.Status == ClaimStatus.Approved)
                .ToListAsync();

            return View(approvedClaims);
        }

        // HR: Manage Lecturer Data (Directory)
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> LecturerDirectory()
        {
            // Get all users in the "Lecturer" role (Requires Identity logic)
            // For simplicity in this POE, we list all users so HR can update details
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // HR: Update Lecturer Details (Simple GET/POST)
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> EditLecturer(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> EditLecturer(ApplicationUser updatedUser)
        {
            var user = await _userManager.FindByIdAsync(updatedUser.Id);
            if (user != null)
            {
                user.FullName = updatedUser.FullName;
                user.Email = updatedUser.Email;
                user.PhoneNumber = updatedUser.PhoneNumber;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("LecturerDirectory");
            }
            return View(updatedUser);
        }
    }
}

