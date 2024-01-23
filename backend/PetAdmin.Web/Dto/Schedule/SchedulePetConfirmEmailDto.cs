using System;

namespace PetAdmin.Web.Dto.Schedule
{
    public class SchedulePetConfirmEmailDto
    {
        public string PetName { get; set; }
        public string PetLoverName { get; set; }
        public string PetLoverEmail { get; set; }
        public DateTime DateWithHour { get; set; }
        public bool IsUser { get; set; }
    }
}
