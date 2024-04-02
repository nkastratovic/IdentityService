using Identity.Service.DTO;
using Identity.Service.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authenticationService;
        protected AuthenticationResponseDto _response;

        public AccountController(IAuthService authenticationService)
        {
            _authenticationService = authenticationService;
            _response = new AuthenticationResponseDto();
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="model">New user.</param>
        /// <returns></returns>
        [HttpPost("register")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {

            var errorMessage = await _authenticationService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }

            return Ok(_response);
        }

        /// <summary>
        /// Login a user.
        /// </summary>
        /// <param name="model">Object contains userName and password.</param>
        /// <returns>LoginResponse object with JWT and refresh token.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authenticationService.Login(model);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);

        }

        /// <summary>
        /// Generate a new JWT token using old expired JWT token and refresh token.
        /// </summary>
        /// <param name="tokenModel">Object that contains old expired JWT token and refresh token.</param>
        /// <returns>LoginResponse object with new JWT and refresh token.</returns>
        [HttpPost("generate-new-jwt-token")]
        public async Task<IActionResult> GenerateNewAccessToken(TokenModel tokenModel)
        {
            var loginResponse = await _authenticationService.GenerateNewAccessToken(tokenModel);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }
    }
}
