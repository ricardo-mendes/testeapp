using PetAdmin.Web.Enumerations;

namespace PetAdmin.Web.Dto.Schedule
{
    public class SchedulePetDto
    {
        public long Id { get; set; }
        public ScheduleStatusEnum Status { get; set; }
        public string Note { get; set; }
        public long? PetLoverId { get; set; }
        public long? PetId { get; set; }
        public long? ScheduleId { get; set; }
        public long? ScheduleItemEmployeeId { get; set; }
        public ScheduleItemEmployeeDto ScheduleItemEmployee { get; set; }
        public PetDto Pet { get; set; }
        public PetLoverDto PetLover { get; set; }
        public ScheduleDto Schedule { get; set; }
    }
}
