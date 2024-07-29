using CaptureMatchApi.DTOs;
using CaptureMatchApi.Entities;
using CaptureMatchApi.Helpers;

namespace CaptureMatchApi.Interfaces
{
    public interface IUserRepository
    {
        void Update(User user);

        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserByIdAsync(string id);

        Task<User> GetUserByUserNameAsync(string userName);

        Task<MemberDto> GetMemberAsync(string userName);

        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);

        Task<PagedList<MemberDto>> GetPhotographersAsync(UserParams userParams);
    }
}
