using System;

namespace PetAdmin.Web.Dto
{
    public class MyPetLoverResultDto
    {
        public long PetLoverId { get; set; }
        public long PetId { get; set; }
        public string PetName { get; set; }
        public string PetLoverName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Neighborhood { get; set; }
        public string Complement { get; set; }
        public bool? IsClub { get; set; }
    }
}
