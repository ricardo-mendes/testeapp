using System;
using System.Collections.Generic;

namespace PetAdmin.Web.Dto.Schedule
{
    public class SchedulePostDto
    {
        public long EmployeeId { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime EndDate { get; set; }
        public int InitialHour { get; set; }
        public int EndHour { get; set; }
        public ICollection<int> DaysOfWeek { get; set; }
        public int QuantityAllowed { get; set; }
    }
}
