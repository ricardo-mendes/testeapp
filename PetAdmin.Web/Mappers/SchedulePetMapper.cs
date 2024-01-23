using PetAdmin.Web.Dto.Schedule;
using PetAdmin.Web.Enumerations;
using PetAdmin.Web.Extensions;
using PetAdmin.Web.Models.Domain;
using System;
using System.Collections.Generic;

namespace PetAdmin.Web.Mappers
{
    public static class SchedulePetMapper
    {
        public static SchedulePet DtoToEntity(SchedulePetDto dto)
        {
            return new SchedulePet
            {
                Status = dto.Status == ScheduleStatusEnum.Confirmed ? (int)ScheduleStatusEnum.Confirmed : (int)ScheduleStatusEnum.Requested,
                ScheduleId = dto.ScheduleId,
                PetLoverId = dto.PetLoverId,
                PetId = dto.PetId,
                ScheduleItemEmployeeId = dto.ScheduleItemEmployeeId,
                Note = dto.Note
            };
        }

        public static SchedulePetDto EntityToDto(SchedulePet entity)
        {
            var dto = new SchedulePetDto
            {
                Id = entity.Id,
                Status = (ScheduleStatusEnum)entity.Status,
                Note = entity.Note,
                PetLoverId = entity.PetLoverId,
                PetId = entity.PetId,
                ScheduleId = entity.ScheduleId,
                ScheduleItemEmployeeId = entity.ScheduleItemEmployeeId
            };

            if (entity.PetLover != null)
                dto.PetLover = PetLoverMapper.EntityToDto(entity.PetLover);
            if (entity.Pet != null)
                dto.Pet = PetMapper.EntityToDto(entity.Pet);
            if (entity.Schedule != null)
                dto.Schedule = ScheduleMapper.EntityToDto(entity.Schedule);
            if (entity.ScheduleItemEmployee != null)
                dto.ScheduleItemEmployee = ScheduleItemEmployeeMapper.EntityToDto(entity.ScheduleItemEmployee);

            return dto;
        }

        //O Schedule Já vai ter o EmployeeId, Date e Hour
        public static SchedulePet DtoToRequestedStatus(SchedulePetDto dto, SchedulePet schedulePet)
        {
            schedulePet.Note = dto.Note;
            schedulePet.Status = (int)ScheduleStatusEnum.Requested;
            schedulePet.PetLoverId = dto.PetLoverId;
            schedulePet.PetId = dto.PetId;
            schedulePet.ScheduleId = dto.ScheduleId;
            schedulePet.ScheduleItemEmployeeId = dto.ScheduleItemEmployeeId;
            schedulePet.LastModificationTime = DateTime.UtcNow.ToBrazilTimeZoneDateTime();

            return schedulePet;
        }

        //O Schedule Já vai ter o PetId
        public static SchedulePet DtoToAvailableStatus(SchedulePet schedulePet)
        {
            schedulePet.Note = null;
            schedulePet.Status = (int)ScheduleStatusEnum.Available;
            schedulePet.PetId = null;
            schedulePet.PetLoverId = null;
            schedulePet.ScheduleItemEmployeeId = null;
            schedulePet.ScheduleId = null;
            schedulePet.LastModificationTime = DateTime.UtcNow.ToBrazilTimeZoneDateTime();

            return schedulePet;
        }

        public static SchedulePet DtoToCanceledStatus(SchedulePet schedulePet)
        {
            schedulePet.Status = (int)ScheduleStatusEnum.Canceled;
            schedulePet.LastModificationTime = DateTime.UtcNow.ToBrazilTimeZoneDateTime();

            return schedulePet;
        }

        //O Schedule Já vai ter o PetId
        public static SchedulePet DtoToConfirmedStatus(SchedulePetDto dto, SchedulePet schedulePet)
        {
            schedulePet.Status = (int)ScheduleStatusEnum.Confirmed;
            schedulePet.LastModificationTime = DateTime.UtcNow.ToBrazilTimeZoneDateTime();

            return schedulePet;
        }

        public static SchedulePet DtoToCompletedStatus(SchedulePetDto dto, SchedulePet schedule)
        {
            schedule.Status = (int)ScheduleStatusEnum.Completed;
            schedule.LastModificationTime = DateTime.UtcNow.ToBrazilTimeZoneDateTime();

            return schedule;
        }

        public static SchedulePet DtoToBlockedStatus(SchedulePetDto dto, SchedulePet schedulePet)
        {
            schedulePet.Note = dto.Note;
            schedulePet.Status = (int)ScheduleStatusEnum.Blocked;
            schedulePet.PetId = null;
            schedulePet.PetLoverId = null;
            schedulePet.ScheduleItemEmployeeId = null;
            schedulePet.LastModificationTime = DateTime.UtcNow.ToBrazilTimeZoneDateTime();

            return schedulePet;
        }

        public static List<SchedulePet> ScheduleListToEntityList(List<long> scheduleIdList, long petLoverId, long petId, long scheduleItemEmployeeId)
        {
            var schedulePetList = new List<SchedulePet>();

            foreach (var scheduleId in scheduleIdList)
            {
                var schedulePet = new SchedulePet
                {
                    ScheduleId = scheduleId,
                    PetId = petId,
                    PetLoverId = petLoverId,
                    ScheduleItemEmployeeId = scheduleItemEmployeeId,
                    Status = (int)ScheduleStatusEnum.Confirmed
                };
                schedulePetList.Add(schedulePet);
            }

            return schedulePetList;
        }
    }
}
