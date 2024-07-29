using AutoMapper;
using CaptureMatchApi.DTOs;
using CaptureMatchApi.Entities;
using CaptureMatchApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaptureMatchApi.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<User> userManager, IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (await UserExists(registerDto.UserName)) return BadRequest("This username has been taken");

            var user = _mapper.Map<User>(registerDto);

            user.UserName = registerDto.UserName.ToLower();

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            IdentityResult roleAssignResult;

            if (registerDto.Role.ToLower() == "client")
            {
                roleAssignResult = await _userManager.AddToRoleAsync(user, "Client");
            }
            else if (registerDto.Role.ToLower() == "photographer")
            {
                roleAssignResult = await _userManager.AddToRoleAsync(user, "Photographer");
            }
            else
            {
                return BadRequest("Invalid role provided");
            }

            return new UserDto
            {
                Token = await _tokenService.CreateToken(user),
                UserName = user.UserName,
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.Username);

            if (user == null) return BadRequest("Invalid UserName");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if(!result) return Unauthorized("Invalid Password");

            return new UserDto
            {
                Token = await _tokenService.CreateToken(user),
                UserName = user.UserName,
            };
        }

        private async Task<bool> UserExists(string userName)
        {
            return await _userManager.Users.AnyAsync(u => u.UserName.ToLower() == userName.ToLower());
        }

    }
}
