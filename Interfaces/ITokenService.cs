using CaptureMatchApi.Entities;

namespace CaptureMatchApi.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(User user);
    }
}
