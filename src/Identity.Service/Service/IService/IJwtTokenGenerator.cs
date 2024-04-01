using Identity.Service.DTO;
using Identity.Service.Entities;

namespace AuthAPI.Service.IService
{
    public interface IJwtTokenGenerator
    {
        LoginResponseDto GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}
