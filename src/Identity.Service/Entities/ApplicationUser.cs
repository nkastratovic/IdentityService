using Microsoft.AspNetCore.Identity;

namespace Identity.Service.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }
    }
}
