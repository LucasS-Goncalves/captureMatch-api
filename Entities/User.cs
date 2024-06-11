using Microsoft.AspNetCore.Identity;

namespace CaptureMatchApi.Entities
{
    public class User : IdentityUser
    {       
        public string FirstName { get; set; }

        public string LastName { get; set; }
            
        public string City {  get; set; }

        public string State { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime LastActive { get; set; } = DateTime.UtcNow;

        public ICollection<AppUserRole> UserRoles { get; set; }

        public List<Photo> Photos { get; set; } = [];
        public string PhotographerAboutMe { get; set; }
    }
}
