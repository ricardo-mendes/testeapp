using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PetAdmin.Web.Models.Domain
{
    public class PetLover : PetAuditedEntityGuid
    {
        public Guid? UserId { get; set; }
        public long? ClientId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        private string _phoneNumber;

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(value))
                {
                    value = CleanPhone(value);
                }
                if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrEmpty(value)
                    && value.Length <= 9)
                {
                    _phoneNumber = "5521" + value;
                }
                else if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrEmpty(value)
                    && value.Length >= 10 && value[0] != '5')
                {
                    _phoneNumber = "55" + value;
                }
                else
                {
                    _phoneNumber = value;
                }
            }
        }

        public long LocationId { get; set; }
        public int Gender { get; set; }
        public bool IsClub { get; set; }

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
        public virtual Client Client { get; set; }
        public virtual ICollection<Pet> PetList { get; set; }
        public virtual ICollection<SchedulePet> SchedulePetList { get; set; }
        public virtual ICollection<PetLoverLocationClient> PetLoverLocationClientList { get; set; }

        public PetLover()
        {
            PetList = new List<Pet>();
        }

        private string CleanPhone(string phone)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(phone, "");
        }
    }
}
