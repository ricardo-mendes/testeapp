using PetAdmin.Web.Infra;
using System;

namespace PetAdmin.Web.Models.Domain
{
    public class SchedulePet : EntityGuid
    {
        public string Note { get; set; }
        public int Status { get; set; }
        public long? PetLoverId { get; set; }
        public long? PetId { get; set; }
        public long? ScheduleId { get; set; }
        public long? ScheduleItemEmployeeId { get; set; }

        public virtual PetLover PetLover { get; set; }
        public virtual Pet Pet { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual ScheduleItemEmployee ScheduleItemEmployee { get; set; }

        public virtual DateTime CreationTime { get; set; }
        public Guid? CreatedUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModificationUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public Guid? DeletedUserId { get; set; }
    }
}
