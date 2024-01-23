using PetAdmin.Web.Dto;
using PetAdmin.Web.Models.Domain;
using System;

namespace PetAdmin.Web.Mappers
{
    public static class LocationMapper
    {
        public static Location DtoToEntity(ClientDto dto)
        {
            return new Location
            {
                Uid = Guid.NewGuid(),
                CountryCode = "BR",
                StateCode = "RJ",
                CityName = "Rio de Janeiro",
                Neighborhood = dto.Neighborhood,
                Complement = dto.Complement,
                PostalCode = null,
                StreetName = dto.StreetName,
                StreetNumber = dto.StreetNumber,
                IsActive = true
            };
        }

        public static Location DtoToEntity(ClientDto dto, Location entity)
        {
            entity.Neighborhood = dto.Neighborhood;
            entity.Complement = dto.Complement;
            entity.StreetName = dto.StreetName;
            entity.StreetNumber = dto.StreetNumber;

            return entity;
        }

        //---------------

        public static Location DtoToEntity(PetLoverDto dto)
        {
            return new Location
            {
                Uid = Guid.NewGuid(),
                CountryCode = "BR",
                StateCode = "RJ",
                CityName = "Rio de Janeiro",
                Neighborhood = dto.Neighborhood,
                Complement = dto.Complement,
                PostalCode = null,
                StreetName = dto.StreetName,
                StreetNumber = dto.StreetNumber,
                IsActive = true
            };
        }

        public static Location DtoToEntity(PetLoverDto dto, Location entity)
        {
            entity.Neighborhood = dto.Neighborhood;
            entity.Complement = dto.Complement;
            entity.StreetName = dto.StreetName;
            entity.StreetNumber = dto.StreetNumber;

            return entity;
        }
    }
}
