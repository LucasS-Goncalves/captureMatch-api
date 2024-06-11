using System.ComponentModel.DataAnnotations;

namespace CaptureMatchApi.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public DateOnly? DateOfBirth { get; set; }
    }
}
