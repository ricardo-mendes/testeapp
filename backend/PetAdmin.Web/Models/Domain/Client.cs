using System;
using System.Collections.Generic;

namespace PetAdmin.Web.Models.Domain
{
    public class Client : PetAuditedEntityGuid
    {
        public Guid UserId { get; set; }
        public int ProfileTypeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public long LocationId { get; set; }
        public string DocumentInformation { get; set; }
        public int? DocumentTypeId { get; set; }
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

        public virtual Location Location { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Employee> EmployeeList { get; set; }
        public virtual ICollection<ScheduleItemClient> ScheduleItemClientList { get; set; }
        public virtual ICollection<PetLover> PetLoverList { get; set; }
        public virtual ICollection<PetLoverLocationClient> PetLoverLocationClientList { get; set; }

        public Client()
        {
            EmployeeList = new List<Employee>();
            ScheduleItemClientList = new List<ScheduleItemClient>();
        }
    }
}
