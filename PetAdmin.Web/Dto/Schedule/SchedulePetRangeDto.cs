using System;

namespace PetAdmin.Web.Dto.Schedule
{
    public class SchedulePetRangeDto
    {
        public long ScheduleId { get; set; }
        public int QuantityOfWeeks { get; set; }
        public long PetLoverId { get; set; }
        public long PetId { get; set; }
        public long ScheduleItemEmployeeId { get; set; }
    }
}
