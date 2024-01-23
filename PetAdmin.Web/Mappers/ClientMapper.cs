using GeoCoordinatePortable;
using PetAdmin.Web.Dto;
using PetAdmin.Web.Enumerations;
using PetAdmin.Web.Models.Domain;
using System;

namespace PetAdmin.Web.Mappers
{
    public static class ClientMapper
    {
        public static Client DtoToEntity(ClientDto dto, Guid userId)
        {
            var client = new Client
            {
                ProfileTypeId = (int)ProfileTypeEnum.PetShop,
                PhoneNumber = dto.PhoneNumber,
                Name = dto.Name,
                Email = dto.Email,
                DocumentTypeId = (int)DocumentTypeEnum.BrLegalPersonCNPJ,
                DocumentInformation = dto.DocumentInformation,
                IsActive = true,
                UserId = userId,
                Location = LocationMapper.DtoToEntity(dto)
            };

            return client;
        }

        public static Client DtoToEntity(ClientDto dto, Client entity)
        {
            entity.PhoneNumber = dto.PhoneNumber;
            entity.Name = dto.Name;
            entity.Location.Neighborhood = dto.Neighborhood;
            entity.Location.Complement = dto.Complement;
            entity.Location.StreetName = dto.StreetName;
            entity.Location.StreetNumber = dto.StreetNumber;

            return entity;
        }

        //----------------------------

        public static ClientDto EntityToDto(Client entity, double petLoverLatiude = default, double petLoverLongitude = default)
        {
            var dto =  new ClientDto
            {
                Id = entity.Id,
                ProfileTypeId = entity.ProfileTypeId,
                PhoneNumber = entity.PhoneNumber,
                Name = entity.Name,
                Email = entity.Email,
                Neighborhood = entity.Location?.Neighborhood,
                StreetName = entity.Location?.StreetName,
                StreetNumber = entity.Location?.StreetNumber,
                Complement = entity.Location?.Complement,
                PhotoUrl = entity.PhotoUrl,
                Latitude = entity.Location?.Latitude,
                Longitue = entity.Location?.Longitue
            };

            if (petLoverLatiude != default && petLoverLongitude != default 
                && dto.Latitude.HasValue && dto.Latitude.Value != default
                && dto.Longitue.HasValue && dto.Longitue.Value != default)
            {
                var locA = new GeoCoordinate(petLoverLatiude, petLoverLongitude);
                var locB = new GeoCoordinate(dto.Latitude.Value, dto.Longitue.Value);

                dto.Distance = locA.GetDistanceTo(locB);
            }

            if (!string.IsNullOrWhiteSpace(entity.PhotoUrl))
            {
                dto.Photo = new PhotoDto
                {
                    File = entity.PhotoUrl,
                    Name = entity.PhotoName
                };
            }

            foreach (var employee in entity.EmployeeList)
            {
                dto.EmployeeList.Add(EmployeeMapper.EntityToDto(employee));
            }

            return dto;
        }
    }
}
