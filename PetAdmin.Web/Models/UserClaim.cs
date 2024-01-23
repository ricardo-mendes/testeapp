using Microsoft.AspNetCore.Identity;
using System;

namespace PetAdmin.Web.Models
{
    public class UserClaim : IdentityUserClaim<Guid>
    {
        public DateTime CreationTime { get; set; }
    }
}
