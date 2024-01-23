using PetAdmin.Web.Dto;
using PetAdmin.Web.Models.Domain;

namespace PetAdmin.Web.Mappers
{
    public class EmployeeMapper
    {
        public static Employee DtoToEntity(EmployeeDto dto)
        {
            return new Employee
            {
                ClientId = dto.ClientId,
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsActive = true
            };
        }

        public static Employee ClientDtoToEntity(ClientDto dto)
        {
            return new Employee
            {
                ClientId = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsActive = true
            };
        }

        public static Employee ClientDtoToEntity(ClientDto dto, Employee entity)
        {
            entity.Name = dto.Name;
            entity.Email = dto.Email;
            entity.PhoneNumber = dto.PhoneNumber;

            return entity;
        }

        //----------------------------

        public static EmployeeDto EntityToDto(Employee entity, Client client = null)
        {
            var dto =  new EmployeeDto
            {
                Id = entity.Id,
                ClientId = entity.ClientId,
                Name = entity.Name,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber
            };

            if (client != null)
            {
                dto.Client = new ClientDto
                {
                    StreetName = client.Location?.StreetName,
                    StreetNumber = client.Location?.StreetNumber,
                    Complement = client.Location?.Complement,
                    Neighborhood = client.Location?.Neighborhood,
                    PhoneNumber = client.PhoneNumber
                };
            }

            return dto;
        }
    }
}
