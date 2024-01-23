namespace PetAdmin.Web.Dto.Response
{
    public class PhotoResponse
    {
        public PhotoResponse(string photoName, string photoUrl)
        {
            PhotoName = photoName;
            PhotoUrl = photoUrl;
        }

        public string PhotoName { get; set; }
        public string PhotoUrl { get; set; }
    }
}
