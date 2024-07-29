using System.Security.Claims;

namespace CaptureMatchApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {   
        public static string GetUsername(this ClaimsPrincipal user)
        {
            
            var userName = user.FindFirstValue(ClaimTypes.Name) 
                ?? throw new Exception("Cannot get username from token");

            return userName;
        }
    }
}
