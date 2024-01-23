namespace PetAdmin.Web.Dto.Schedule
{
    public class ScheduleItemEmployeeDto
    {
        public long Id { get; set; }
        public long ScheduleItemId { get; set; }
        public long EmployeeId { get; set; }

        public string Name { get; set; }
        public string Price { get; set; }
        public int? Hour { get; set; }
        public int? Minutes { get; set; }
        public bool IsActive { get; set; }
    }
}
