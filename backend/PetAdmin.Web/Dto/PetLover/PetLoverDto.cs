using System;
using System.Collections.Generic;

namespace PetAdmin.Web.Dto
{
    public class PetLoverDto
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? ClientId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Neighborhood { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string Complement { get; set; }
        public int Gender { get; set; }
        public bool IsClub { get; set; }

        //---------------------
        public UserDto User { get; set; }
        public virtual ICollection<PetDto> PetList { get; set; }

        //--------------------
        public bool IsClientOfPetShop { get; set; }

        public PetLoverDto()
        {
            PetList = new List<PetDto>();
        }
    }
}
