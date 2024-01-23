using PetAdmin.Web.Dto;
using PetAdmin.Web.Models.Domain;
using System;

namespace PetAdmin.Web.Mappers
{
    public class VaccineMapper
    {
        public static Vaccine DtoToEntity(VaccineDto dto)
        {
            return new Vaccine
            {
                Uid = Guid.NewGuid(),
                PetId = dto.PetId,
                Name = dto.Name,
                Date = dto.Date,
                RevaccineDate = dto.RevaccineDate,
                ClinicName = dto.ClinicName,
                IsActive =  true,
                Note = string.IsNullOrEmpty(dto.Note) ? null : dto.Note
            };
        }

        //For update
        public static Vaccine DtoToEntity(VaccineDto dto, Vaccine entity)
        {
            entity.Name = dto.Name;
            entity.Date = dto.Date;
            entity.RevaccineDate = dto.RevaccineDate;
            entity.ClinicName = dto.ClinicName;
            entity.Note = dto.Note;

            return entity;
        }

        //-----------------------

        public static VaccineDto EntityToDto(Vaccine entity)
        {
            return new VaccineDto
            {
                Id = entity.Id,
                PetId = entity.PetId,
                Name = entity.Name,
                Date = entity.Date,
                RevaccineDate = entity.RevaccineDate,
                ClinicName = entity.ClinicName,
                Note = entity.Note,
                PhotoUrl = entity.PhotoUrl
            };
        }
    }
}
