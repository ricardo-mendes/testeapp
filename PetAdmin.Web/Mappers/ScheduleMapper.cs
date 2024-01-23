using PetAdmin.Web.Dto.Schedule;
using PetAdmin.Web.Enumerations;
using PetAdmin.Web.Models.Domain;

namespace PetAdmin.Web.Mappers
{
    public static class ScheduleMapper
    {
        public static ScheduleDto EntityToDto(Schedule entity)
        {
            var dto = new ScheduleDto
            {
                Id = entity.Id,
                Date = entity.Date,
                DateWithHour = entity.DateWithHour,
                Period = entity.Period,
                Status = (ScheduleStatusEnum)entity.Status,
                EmployeeId = entity.EmployeeId,
                QuantityOccupied = entity.QuantityOccupied,
                QuantityAllowed = entity.QuantityAllowed
            };

            if (entity.Employee != null)
                dto.Employee = EmployeeMapper.EntityToDto(entity.Employee);

            return dto;
        }

        //O Schedule Já vai ter o EmployeeId, Date e Hour
        public static Schedule DtoToRequestedStatus(ScheduleDto dto, Schedule schedule)
        {
            schedule.Status = (int)ScheduleStatusEnum.Requested;
            return schedule;
        }

        //O Schedule Já vai ter o PetId
        public static Schedule DtoToAvailableStatus(ScheduleDto dto, Schedule schedule)
        {
            schedule.Status = (int)ScheduleStatusEnum.Available;

            return schedule;
        }

        //O Schedule Já vai ter o PetId
        public static Schedule DtoToConfirmedStatus(ScheduleDto dto, Schedule schedule)
        {
            schedule.Status = (int)ScheduleStatusEnum.Confirmed;
            return schedule;
        }

        public static Schedule DtoToCompletedStatus(ScheduleDto dto, Schedule schedule)
        {
            schedule.Status = (int)ScheduleStatusEnum.Completed;
            return schedule;
        }

        public static Schedule DtoToBlockedStatus(ScheduleDto dto, Schedule schedule)
        {
            schedule.Status = (int)ScheduleStatusEnum.Blocked;

            return schedule;
        }

    }
}
