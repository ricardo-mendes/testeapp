using System.Collections.Generic;

namespace PetAdmin.Web.Dto.Schedule
{
    public class ScheduleItemClientEmployeeDto
    {
        public List<ScheduleItemPrice> ScheduleItemPriceList { get; set; }
        public long ClientId { get; set; }
        public long EmployeeId { get; set; }

        public ScheduleItemClientEmployeeDto()
        {
            ScheduleItemPriceList = new List<ScheduleItemPrice>();
        }
    }

    public class ScheduleItemPrice
    {
        public long ScheduleItemId { get; set; }
        public decimal? Price { get; set; }
    }

    public class ScheduleItemClientResponseDto
    {
        public long ScheduleItemId { get; set; }
        public string ScheduleItemName { get; set; }
        public decimal? Price { get; set; }
        public bool IsActive { get; set; }
    }
}
