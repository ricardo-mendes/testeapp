using Microsoft.AspNetCore.Identity;
using System;

namespace PetAdmin.Web.Models
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public DateTime CreationTime { get; set; }
    }
}
