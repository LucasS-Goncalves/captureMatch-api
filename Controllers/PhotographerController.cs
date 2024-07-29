using CaptureMatchApi.Entities;
using CaptureMatchApi.Helpers;
using CaptureMatchApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CaptureMatchApi.Controllers
{
    [Authorize(Policy = "RequirePhotographerRole")]
    public class PhotographerController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public PhotographerController(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
    }
}
