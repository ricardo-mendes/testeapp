using System;
using System.Collections.Generic;

namespace PetAdmin.Web.Models.Domain
{
    public class ScheduleItem : PetAuditedEntityGuid
    {
        public string Name { get; set; }

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

        public virtual IList<ScheduleItemClient> ScheduleItemClientList { get; set; }
        public virtual IList<ScheduleItemEmployee> ScheduleItemEmployeeList { get; set; }
    }
}
