using System;
using System.Collections.Generic;

namespace PetAdmin.Web.Models.Domain
{
    public class Employee : PetAuditedEntityGuid
    {
        public long ClientId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

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

        public virtual ICollection<ScheduleItemEmployee> ScheduleItemEmployeeList { get; set; }
        public virtual ICollection<Schedule> ScheduleList { get; set; }
        public virtual Client Client { get; set; }
    }
}
