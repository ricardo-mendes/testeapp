using System.Collections.Generic;

namespace PetAdmin.Web.Dto
{
    public class ClientDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int ProfileTypeId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Neighborhood { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string Complement { get; set; }
        public string DocumentInformation { get; set; }
        public double? Latitude { get; set; }
        public double? Longitue { get; set; }

        public double Distance { get; set; }

        public ICollection<EmployeeDto> EmployeeList { get; set; }
        public UserDto User { get; set; }

        public PhotoDto Photo { get; set; }
        public string PhotoUrl { get; set; }

        public ClientDto()
        {
            EmployeeList = new List<EmployeeDto>();
        }
    }
}
