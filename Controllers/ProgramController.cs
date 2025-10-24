using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Part1.Data; 
using Part1.Models; 
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Authorization; // Needed for [Authorize]

namespace Part1.Controllers
{
    // Restrict this entire controller to users in the "Program" role
    [Authorize(Roles = "Program")]
    public class ProgramController : Controller
    {
        // This is the database context, which you need to get the data
        private readonly ApplicationDbContext _context;

        // The context is "injected" by the system when the controller is created
        public ProgramController(ApplicationDbContext context)
        {
            _context = context;
        }

        //
        // GET: /Program/Index
        // This action fetches and displays the list of all claims
        //
        public async Task<IActionResult> Index()
        {
            // Get all claims from the database as a list
            var claims = await _context.Claims.ToListAsync();

            // Send the list of claims to the view
            return View(claims);
        }

        //
        // POST: /Program/Approve/5
        // This action handles the "Approve" button click
        //
        [HttpPost]
        [ValidateAntiForgeryToken] // Security check
        public async Task<IActionResult> Approve(int id)
        {
            // Find the specific claim in the database using its ID
            var claim = await _context.Claims.FindAsync(id);

            if (claim != null)
            {
                // Change the status to Approved
                claim.Status = ClaimStatus.Approved;

                // Save the change back to the database
                _context.Update(claim);
                await _context.SaveChangesAsync();
            }

            // Send the user back to the Index page to see the updated list
            return RedirectToAction(nameof(Index));
        }

        //
        // POST: /Program/Reject/5
        // This action handles the "Reject" button click
        //
        [HttpPost]
        [ValidateAntiForgeryToken] // Security check
        public async Task<IActionResult> Reject(int id)
        {
            // Find the specific claim in the database using its ID
            var claim = await _context.Claims.FindAsync(id);

            if (claim != null)
            {
                // Change the status to Rejected
                claim.Status = ClaimStatus.Rejected;

                // Save the change back to the database
                _context.Update(claim);
                await _context.SaveChangesAsync();
            }

            // Send the user back to the Index page
            return RedirectToAction(nameof(Index));
        }
    }
}

