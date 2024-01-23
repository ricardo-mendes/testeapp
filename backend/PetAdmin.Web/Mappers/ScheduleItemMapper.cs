using PetAdmin.Web.Dto.Schedule;
using PetAdmin.Web.Models.Domain;

namespace PetAdmin.Web.Mappers
{
    public static class ScheduleItemMapper
    {
        public static ScheduleItem DtoToEntity(ScheduleItemDto dto)
        {
            return new ScheduleItem
            {
                Name = dto.Name
            };
        }

        public static ScheduleItem DtoToEntity(ScheduleItemDto dto, ScheduleItem entity)
        {
            entity.Name = dto.Name; 
            return entity;
        }

        public static ScheduleItemDto EntityToDto(ScheduleItem entity)
        {
            return new ScheduleItemDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
