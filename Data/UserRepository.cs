using AutoMapper;
using AutoMapper.QueryableExtensions;
using CaptureMatchApi.DTOs;
using CaptureMatchApi.Entities;
using CaptureMatchApi.Helpers;
using CaptureMatchApi.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CaptureMatchApi.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserRepository(DataContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<MemberDto> GetMemberAsync(string userName)
        {
            return await _context.Users
                .Where(u => u.UserName == userName)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();

            query = query.Where(u => u.UserName != userParams.CurrentUserName);

            query = query.OrderByDescending(u => u.LastActive);

            return await PagedList<MemberDto>.CreateAsync(query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
        }

        public async Task<PagedList<MemberDto>> GetPhotographersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();

            query = query.Where(u => u.UserName != userParams.CurrentUserName);

            query = query.Where(u => u.Role == "photographer");

            if (!string.IsNullOrEmpty(userParams.State))
            {                
                query = query.Where(u => u.State.ToLower() == userParams.State.ToLower());

                if (!string.IsNullOrEmpty(userParams.City))
                {
                    query = query.Where(u => u.City.ToLower() == userParams.City.ToLower());
                }
            }                        

            if (!string.IsNullOrWhiteSpace(userParams.PhotographersSearchKey))
            {
                query = query.Where(
                u => u.UserName.StartsWith(userParams.PhotographersSearchKey) ||
                u.FirstName.StartsWith(userParams.PhotographersSearchKey) ||
                u.LastName.StartsWith(userParams.PhotographersSearchKey)
            );
            }            
            
            query = query.OrderByDescending(u => u.LastActive);

            return await PagedList<MemberDto>.CreateAsync(query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
