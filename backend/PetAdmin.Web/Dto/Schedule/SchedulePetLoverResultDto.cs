using System;

namespace PetAdmin.Web.Dto.Schedule
{
    public class SchedulePetLoverResultDto
    {
        public long SchedulePetId { get; set; }
        public string PetName { get; set; }
        public DateTime DateWithHour { get; set; }
        public long EmployeeId { get; set; }
        public int ScheduleStatus { get; set; }
        public string ClientName { get; set; }
        public string ClientAddress { get; set; }
        public string ClientNeighborhood { get; set; }
        public string ClientComplement { get; set; }
        public string EmployeePhoneNumber { get; set; }
        public string ScheduleItemEmployeeName { get; set; }
        public decimal ScheduleItemEmployeePrice { get; set; }

        //Quando o funcionário fizer alguma ação
        public string Note { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
