using PetAdmin.Web.Enumerations;
using System;
using System.Collections.Generic;

namespace PetAdmin.Web.Dto.Schedule
{
    public class ScheduleDto
    {
        public long Id { get; set; }
        public int? Period { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateWithHour { get; set; }
        public ScheduleStatusEnum Status { get; set; }
        public long EmployeeId { get; set; }
        public int QuantityOccupied { get; set; }
        public int QuantityAllowed { get; set; }

        public EmployeeDto Employee { get; set; }
        public ICollection<SchedulePetDto> SchedulePetList { get; set; }
    }
}
