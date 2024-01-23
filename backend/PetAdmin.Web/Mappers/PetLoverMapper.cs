using PetAdmin.Web.Dto;
using PetAdmin.Web.Models.Domain;
using System;

namespace PetAdmin.Web.Mappers
{
    public static class PetLoverMapper
    {
        public static PetLover DtoToEntity(PetLoverDto dto, Guid? userId = null, bool withUser = true)
        {
            var petLover =  new PetLover
            {
                PhoneNumber = dto.PhoneNumber,
                Name = dto.Name,
                Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email,
                IsActive = true,
                ClientId = dto.ClientId,
                Gender = dto.Gender,
                IsClub = dto.IsClub,
                Location = LocationMapper.DtoToEntity(dto)
            };

            if (withUser && userId.HasValue)
            {
                petLover.UserId = userId.Value;
            }

            if (dto.PetList != null && dto.PetList.Count > 0)
            {
                foreach (var petDto in dto.PetList)
                    petLover.PetList.Add(PetMapper.DtoToEntity(petDto));
            }
                
            return petLover;
        }

        public static PetLover DtoToEntity(PetLoverDto dto, PetLover entity)
        {
            entity.PhoneNumber = dto.PhoneNumber;
            entity.Name = dto.Name;
            entity.Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email;
            entity.Gender = dto.Gender;
            entity.IsClub = dto.IsClub;
            entity.Location.Neighborhood = dto.Neighborhood;
            entity.Location.Complement = dto.Complement;
            entity.Location.StreetName = dto.StreetName;
            entity.Location.StreetNumber = dto.StreetNumber;

            return entity;
        }

        public static PetLover DtoToEntityWithoutEmail(PetLoverDto dto, PetLover entity)
        {
            entity.PhoneNumber = dto.PhoneNumber;
            entity.Name = dto.Name;
            entity.Gender = dto.Gender;
            entity.IsClub = dto.IsClub;
            entity.Location.Neighborhood = dto.Neighborhood;
            entity.Location.Complement = dto.Complement;
            entity.Location.StreetName = dto.StreetName;
            entity.Location.StreetNumber = dto.StreetNumber;

            return entity;
        }

        //----------------------

        public static PetLoverDto EntityToDto(PetLover entity)
        {
            var dto = new PetLoverDto
            {
                Id = entity.Id,
                Neighborhood = entity.Location?.Neighborhood,
                StreetName = entity.Location?.StreetName,
                StreetNumber = entity.Location?.StreetNumber,
                Complement = entity.Location?.Complement,
                PhoneNumber = entity.PhoneNumber,
                Name = entity.Name,
                Email = entity.Email,
                ClientId = entity.ClientId,
                Gender = entity.Gender,
                IsClub = entity.IsClub
            };

            foreach (var pet in entity.PetList)
            {
                dto.PetList.Add(PetMapper.EntityToDto(pet));
            }

            return dto;
        }
    }
}
