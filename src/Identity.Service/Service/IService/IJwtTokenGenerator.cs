using Identity.Service.DTO;
using Identity.Service.Entities;
using System.Security.Claims;

namespace AuthAPI.Service.IService
{
    public interface IJwtTokenGenerator
    {
        LoginResponseDto GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
    }
}
