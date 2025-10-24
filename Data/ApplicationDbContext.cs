using Microsoft.EntityFrameworkCore;
using Part1.Models;

namespace Part1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Claim> Claims { get; set; } = null!;
        public DbSet<ApplicationUser> Users { get; set; } = null!;
    }
}

