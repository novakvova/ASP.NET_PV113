using WebRozetka.Data.Entities.Identity;

namespace WebRozetka.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> CreateTokenAsync(UserEntity user);
    }
}
