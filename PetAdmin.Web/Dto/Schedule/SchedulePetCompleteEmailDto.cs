namespace PetAdmin.Web.Dto.Schedule
{
    public class SchedulePetCompleteEmailDto
    {
        public string PetName { get; set; }
        public string PetLoverName { get; set; }
        public string PetLoverEmail { get; set; }
        public string ScheduleItemName { get; set; }
        public bool IsUser { get; set; }
    }
}
