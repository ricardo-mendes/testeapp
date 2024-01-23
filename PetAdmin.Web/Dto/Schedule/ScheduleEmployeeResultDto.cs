using System;

namespace PetAdmin.Web.Dto.Schedule
{
    public class ScheduleEmployeeResultDto
    {
        public long ScheduleId { get; set; }
        public long? SchedulePetId { get; set; }
        public string PetName { get; set; }
        public string PetBreedName { get; set; }
        public string PetLoverName { get; set; }
        public string PetLoverPhoneNumber { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
        public long ScheduleItemId { get; set; }
        public string ScheduleItemName { get; set; }
        public decimal? ScheduleItemPrice { get; set; }
        public string PetNote { get; set; }
        public int? QuantityOccupied { get; set; }
        public int? QuantityAllowed { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
