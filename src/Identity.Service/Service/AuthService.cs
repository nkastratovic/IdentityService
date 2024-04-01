using Identity.Service.Data;
using Identity.Service.DTO;
using Identity.Service.Entities;
using Identity.Service.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Identity.Service.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == loginRequestDto.Email.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            //if user was found , Generate JWT Token
            var roles = await _userManager.GetRolesAsync(user);

            UserDto userDTO = new()
            {
                Email = user.Email,
                ID = user.Id,
                PersonName = user.PersonName,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDTO,
                Token = ""
            };

            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                PersonName = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    //AssignRole(registrationRequestDto.Email, registrationRequestDto.Role.ToUpper());

                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);

                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        PersonName = userToReturn.PersonName,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    //if (!_roleManager.RoleExistsAsync(registrationRequestDto.Role.ToUpper()).GetAwaiter().GetResult())
                    //{
                    //    //Create role if it does not exist.
                    //    _roleManager.CreateAsync(new IdentityRole(registrationRequestDto.Role.ToUpper())).GetAwaiter().GetResult();
                    //}

                    ////Add the specified user to the named role.
                    //await _userManager.AddToRoleAsync(user, registrationRequestDto.Role.ToUpper());

                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {

            }
            return "Error Encountered";
        }
    }
}
