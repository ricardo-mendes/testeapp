using Microsoft.AspNetCore.Identity;
using System;

namespace PetAdmin.Web.Models
{
    public class UserToken : IdentityUserToken<Guid>
    {
        public DateTime CreationTime { get; set; }
    }
}
