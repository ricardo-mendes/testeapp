using Microsoft.AspNetCore.Identity;
using System;

namespace PetAdmin.Web.Models
{
    public class UserLogin : IdentityUserLogin<Guid>
    {
        public DateTime CreationTime { get; set; }
    }
}
