using CaptureMatchApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CaptureMatchApi.Controllers
{
    [Authorize(Policy = "RequirePhotographerRole")]
    public class PhotographerController : BaseApiController
    {
        private readonly UserManager<User> _userManager;

        public PhotographerController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("get-photographers")]
        public async Task<ActionResult> GetPhotographers()
        {
            var photographers = await _userManager.GetUsersInRoleAsync("Photographer");
            return Ok(photographers);
        }
    }
}
