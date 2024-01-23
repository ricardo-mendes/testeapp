using PetAdmin.Web.Dto.Schedule;
using PetAdmin.Web.Models.Domain;
using System;
using System.Globalization;

namespace PetAdmin.Web.Mappers
{
    public static class ScheduleItemEmployeeMapper
    {
        public static ScheduleItemEmployee DtoToEntity(ScheduleItemEmployeeDto dto, ScheduleItemEmployee entity)
        {
            entity.Name = dto.Name;
            entity.Price = decimal.Parse(dto.Price.Trim().Replace(",", "."), CultureInfo.InvariantCulture);
            entity.IsActive = dto.IsActive;

            return entity;
        }

        public static ScheduleItemEmployee DtoToEntity(ScheduleItemEmployeeDto dto)
        {
            return new ScheduleItemEmployee
            {
                Name = dto.Name,
                Price = decimal.Parse(dto.Price.Trim().Replace(",", "."), CultureInfo.InvariantCulture),
                IsActive = dto.IsActive,
                EmployeeId = dto.EmployeeId,
                ScheduleItemId = dto.ScheduleItemId
            };
        }

        //------------------

        public static ScheduleItemEmployeeDto EntityToDto(ScheduleItemEmployee entity)
        {
            return new ScheduleItemEmployeeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price.ToString(),
                Hour = entity.Hour,
                Minutes = entity.Minutes,
                IsActive = entity.IsActive,
                EmployeeId = entity.EmployeeId,
                ScheduleItemId = entity.ScheduleItemId
            };
        }

        //----------------

        public static ScheduleItemEmployee ScheduleItemAndEmployeeToEntity(ScheduleItem scheduleItem, Employee employee)
        {
            return new ScheduleItemEmployee
            {
                EmployeeId = employee.Id,
                ScheduleItemId = scheduleItem.Id,
                Name = scheduleItem.Name,
                IsActive = false
            };
        }
    }
}
