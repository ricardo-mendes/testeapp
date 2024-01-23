namespace PetAdmin.Web.Dto
{
    public class UserDto
    {
        public long Id { get; set; }
        public string UserLogin { get; set; }
        public string Password { get; set; }
    }

    public class UserChangeDto
    {
        public long Id { get; set; }
        public string UserLogin { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
