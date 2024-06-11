using Microsoft.AspNetCore.Identity;

namespace CaptureMatchApi.Entities
{
    public class AppUserRole : IdentityUserRole<string>
    {
        public User User { get; set; }

        public AppRole Role { get; set; }
    }
}
