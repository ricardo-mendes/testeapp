using PetAdmin.Web.Dto;
using PetAdmin.Web.Models.Domain;
using System;

namespace PetAdmin.Web.Mappers
{
    public static class PetMapper
    {
        public static Pet DtoToEntity(PetDto dto)
        {
            return new Pet
            {
                Uid = Guid.NewGuid(),
                Name = dto.Name,
                PetLoverId = dto.PetLoverId,
                PetTypeId = dto.PetTypeId,
                Breed = string.IsNullOrEmpty(dto.Breed) ? "Não Informado" : dto.Breed,
                BirthDate = dto.BirthDate,
                Gender = dto.Gender,
                Size = dto.Size,
                Coat = dto.Coat,
                Note = dto.Note,
                IsActive = true,
                IsClub = dto.IsClub
            };
        }

        public static Pet DtoToEntity(PetDto dto, Pet entity)
        {
            entity.Name = dto.Name;
            entity.PetTypeId = dto.PetTypeId;
            entity.Breed = dto.Breed;
            entity.BirthDate = dto.BirthDate;
            entity.Gender = dto.Gender;
            entity.Size = dto.Size;
            entity.Coat = dto.Coat;
            entity.Note = dto.Note;
            entity.IsClub = dto.IsClub;

            return entity;
        }

        //-----------------------

        public static PetDto EntityToDto(Pet entity)
        {
            var dto = new PetDto
            {
                Id = entity.Id,
                Name = entity.Name,
                PetLoverId = entity.PetLoverId,
                PetTypeId = entity.PetTypeId,
                Breed = entity.Breed,
                BirthDate = entity.BirthDate,
                Gender = entity.Gender,
                Size = entity.Size,
                Coat = entity.Coat,
                Note = entity.Note,
                IsClub = entity.IsClub,
                PhotoUrl = entity.PhotoUrl
            };

            return dto;
        }
    }
}
