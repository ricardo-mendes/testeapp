using Microsoft.AspNetCore.Identity;
using System;

namespace PetAdmin.Web.Models
{
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        public DateTime CreationTime { get; set; }
    }
}
