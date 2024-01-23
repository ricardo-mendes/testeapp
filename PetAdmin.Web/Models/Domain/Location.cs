using System;
using System.Collections.Generic;

namespace PetAdmin.Web.Models.Domain
{
    public class Location : PetAuditedEntityGuid
    {
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string CityName { get; set; }
        public string Neighborhood { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string PostalCode { get; set; }
        public string Complement { get; set; }
        public double Latitude { get; set; }
        public double Longitue { get; set; }

        //--------------
        public bool IsActive { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public Guid? CreatedUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModificationUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public Guid? DeletedUserId { get; set; }
        //--------------

        public virtual ICollection<PetLover> PetLoverList { get; set; }
        public virtual ICollection<Client> ClientList { get; set; }
    }
}
