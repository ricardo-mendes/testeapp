using System;
using System.Collections.Generic;

namespace PetAdmin.Web.Models.Domain
{
    public class ScheduleItemEmployee : PetAuditedEntityGuid
    {
        public long ScheduleItemId { get; set; }
        public long EmployeeId { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public int? Hour { get; set; }
        public int? Minutes { get; set; }

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

        public virtual ScheduleItem ScheduleItem { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<SchedulePet> SchedulePetList { get; set; }
    }
}
