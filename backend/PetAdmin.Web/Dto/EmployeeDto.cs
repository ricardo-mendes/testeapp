namespace PetAdmin.Web.Dto
{
    public class EmployeeDto
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ClientDto Client { get; set; }
    }
}
