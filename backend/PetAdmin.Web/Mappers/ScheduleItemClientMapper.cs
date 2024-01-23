using PetAdmin.Web.Dto.Schedule;
using PetAdmin.Web.Models.Domain;

namespace PetAdmin.Web.Mappers
{
    public static class ScheduleItemClientMapper
    {
        public static ScheduleItemClient ScheduleItemAndClientToEntity(ScheduleItem scheduleItem, Client client)
        {
            return new ScheduleItemClient
            {
                ClientId = client.Id,
                ScheduleItemId = scheduleItem.Id,
                IsActive = false
            };
        }
    }
}
