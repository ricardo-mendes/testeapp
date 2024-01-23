using System;

namespace PetAdmin.Web.Dto
{
    public class PetDto
    {
        public long Id { get; set; }

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

        public PhotoDto Photo { get; set; }

        public PetLoverDto PetLover { get; set; }

        //Usado apenas num get para vacinas (pet/getallbypetloverid)
        public bool HasVaccine { get; set; }

        //Usado apenas para o get do pet
        public string PhotoUrl { get; set; }
    }
}
