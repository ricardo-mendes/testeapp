namespace PetAdmin.Web.Dto.PetLover
{
    public class PetLoverLocationDto
    {
        public long PetLoverId { get; set; }

        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string CityName { get; set; }
        public string Neighborhood { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string PostalCode { get; set; }
        public string Complement { get; set; }
        public double Latitude { get; set; }
        public double Longitue { get; set; }
    }
}
