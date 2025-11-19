using System.ComponentModel.DataAnnotations;

namespace Part1.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }

        [Required] public string FullName { get; set; } = string.Empty;
        [Required] public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Lecturer;
        public object PhoneNumber { get; internal set; }
    }
}

