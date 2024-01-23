using System;

namespace PetAdmin.Web.Dto.Schedule
{
    public class SchedulePetCancelEmailDto
    {
        public string PetName { get; set; }
        public string PetLoverName { get; set; }
        public string PetLoverEmail { get; set; }
        public DateTime DateWithHour { get; set; }
        public string EmployeeEmail { get; set; }
    }
}
