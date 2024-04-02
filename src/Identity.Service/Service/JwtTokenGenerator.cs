using AuthApi.Models;
using AuthAPI.Service.IService;
using Identity.Service.DTO;
using Identity.Service.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthAPI.Service
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions, IConfiguration configuration)
        {
            _jwtOptions = jwtOptions.Value;
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a JWT token.
        /// </summary>
        /// <param name="applicationUser">Application User</param>
        /// <param name="roles">Roles</param>
        /// <returns></returns>
        public LoginResponseDto GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            var claimList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email,applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.Sub,applicationUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name,applicationUser.UserName),
                new Claim(JwtRegisteredClaimNames.Name, applicationUser.Email)
            };

            DateTime expiration = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryInMinutes);

            claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            UserDto userDTO = new()
            {
                Email = applicationUser.Email,
                ID = applicationUser.Id,
                PersonName = applicationUser.PersonName,
                PhoneNumber = applicationUser.PhoneNumber
            };

            return new LoginResponseDto()
            {
                User = userDTO,
                Token = tokenHandler.WriteToken(token),
                TokenExpirationDateTime = expiration,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpirationDateTime = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["RefreshToken:EXPIRATION_MINUTES"]))
            };
        }

        /// <summary>
        /// Creates a refresh token (base 64 string of random numbers).
        /// </summary>
        /// <returns>Refresh token in format of base 64 string of random numbers.</returns>
        private string GenerateRefreshToken()
        {
            byte[] bytes = new byte[64];
            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Gets the principal from a JWT token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
        public ClaimsPrincipal? GetPrincipalFromJwtToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = _jwtOptions.Audience,
                ValidateIssuer = true,
                ValidIssuer = _jwtOptions.Issuer,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)),

                ValidateLifetime = false //Should be false!
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
