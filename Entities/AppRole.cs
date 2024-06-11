using Microsoft.AspNetCore.Identity;

namespace CaptureMatchApi.Entities
{
    public class AppRole : IdentityRole
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
