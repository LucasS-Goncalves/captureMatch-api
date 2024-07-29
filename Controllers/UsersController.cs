using AutoMapper;
using CaptureMatchApi.DTOs;
using CaptureMatchApi.Entities;
using CaptureMatchApi.Extensions;
using CaptureMatchApi.Helpers;
using CaptureMatchApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaptureMatchApi.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;

        public UsersController(IMapper mapper, IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<PagedList<MemberDto>> GetUsers([FromQuery] UserParams userParams)
        {
            var users = await _unitOfWork.UserRepository.GetMembersAsync(userParams);
            return users;
        }

        [HttpGet("photographers")]
        public async Task<PagedList<MemberDto>> GetPhotographers([FromQuery] UserParams userParams)
        {
            userParams.CurrentUserName = User.GetUsername();
            var photographers = await _unitOfWork.UserRepository.GetPhotographersAsync(userParams);
            Response.AddPaginationHeader(new PaginationHeader(photographers.CurrentPage, photographers.PageSize, photographers.TotalCount, photographers.TotalPages));
            return photographers;
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<MemberDto>> GetUser(string userName)
        {
            return await _unitOfWork.UserRepository.GetMemberAsync(userName);
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUserNameAsync(User.GetUsername());

            if (user == null) return NotFound("user is null");

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };

            if (user.Photos.Count == 0) photo.IsProfilePicture = true;

            user.Photos.Add(photo);

            if (await _unitOfWork.Complete())
            {
                return CreatedAtAction(nameof(GetUser), new { userName = user.UserName }, _mapper.Map<PhotoDto>(photo));
                //return _mapper.Map<PhotoDto>(photo);
            }

            return BadRequest("Problem adding photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUserNameAsync(User.GetUsername());

            if (user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsProfilePicture) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("There was a problem deleting the photo");

        }
    }
}
