using Microsoft.AspNetCore.Identity;
using System;

namespace PetAdmin.Web.Models
{
    public class Roles : IdentityRole<Guid>
    {
        public DateTime CreationTime { get; set; }
    }
}
