using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ReactSupply.Models.Entity
{
    public class ApplicationUser: IdentityUser
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
