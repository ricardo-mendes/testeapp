using System;

namespace PetAdmin.Web.Models.Domain
{
    public class Vaccine : PetAuditedEntityGuid
    {
        public long PetId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime? RevaccineDate { get; set; }
        public string ClinicName { get; set; }
        public string Note { get; set; }
        public string PhotoUrl { get; set; }
        public string PhotoName { get; set; }

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

        public Pet Pet { get; set; }
    }
}
