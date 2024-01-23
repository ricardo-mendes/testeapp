using System;

namespace PetAdmin.Web.Models.Domain
{
    public class ScheduleItemClient : PetAuditedEntityGuid
    {
        public long ScheduleItemId { get; set; }
        public long ClientId { get; set; }

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
        public virtual Client Client { get; set; }
    }
}
