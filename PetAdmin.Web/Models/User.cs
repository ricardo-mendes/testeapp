using Microsoft.AspNetCore.Identity;
using PetAdmin.Web.Models.Domain;
using System;
using System.Collections.Generic;

namespace PetAdmin.Web.Models
{
    public class User : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public int UserTypeId { get; set; }

        public bool IsActive { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public Guid? CreatedUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModificationUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public Guid? DeletedUserId { get; set; }

        public virtual ICollection<Client> ClientList { get; set; }
        public virtual ICollection<PetLover> PetLoverList { get; set; }

        public User()
        {
            IsActive = true;
        }
    }
}
