using System;
using System.Collections.Generic;

namespace PetAdmin.Web.Models.Domain
{
    public class Pet : PetAuditedEntityGuid
    {
        public string Name { get; set; }
        public long PetLoverId { get; set; }
        public int PetTypeId { get; set; }

        // Raça
        public string Breed { get; set; }

        public DateTime? BirthDate { get; set; }

        public int Gender { get; set; }

        // Porte(Tamanho)
        public int Size { get; set; }

        // Pelagem
        public int Coat { get; set; }

        public string Note { get; set; }

        public bool IsClub { get; set; }

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

        public virtual PetLover PetLover { get; set; }
        public virtual ICollection<SchedulePet> SchedulePetList { get; set; }
        public virtual ICollection<Vaccine> VaccineList { get; set; }
    }
}
