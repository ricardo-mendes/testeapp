using System;

namespace PetAdmin.Web.Dto.PetLover
{
    public class MyPetLoverScheduleResultDto
    {
        public string PetName { get; set; }
        public DateTime DateWithHour { get; set; }
        public int Status { get; set; }
    }
}
