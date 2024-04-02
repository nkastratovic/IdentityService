using Identity.Service.DTO;

namespace Identity.Service.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<LoginResponseDto> GenerateNewAccessToken(TokenModel tokenModel);
    }
}
