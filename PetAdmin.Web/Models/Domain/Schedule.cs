using System;
using System.Collections.Generic;

namespace PetAdmin.Web.Models.Domain
{
    public class Schedule : PetAuditedEntityGuid
    {
        public int? Period { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateWithHour { get; set; }
        public int Status { get; set; }
        public long EmployeeId { get; set; }
        public int QuantityAllowed { get; set; }
        public int QuantityOccupied { get; set; }

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

        public virtual Employee Employee { get; set; }
        public virtual ICollection<SchedulePet> SchedulePetList { get; set; }
    }
}
