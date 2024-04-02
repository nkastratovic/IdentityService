using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Identity.Service.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [MaxLength(50)]
        public string? PersonName { get; set; }

        [MaxLength(90)]
        public string? RefreshToken { get; set; }
        [MaxLength(30)]
        public DateTime RefreshTokenExpirationDateTime { get; set; }
    }
}
