using System;

namespace PetAdmin.Web.Dto.Schedule
{
    public class ScheduleGetAllEmployee : BaseGetAll
    {
        public long EmployeeId { get; set; }
        public DateTime? InitialDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool OnlyScheduleOccupied { get; set; }
    }
}
