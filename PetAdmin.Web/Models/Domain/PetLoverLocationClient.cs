namespace PetAdmin.Web.Models.Domain
{
    public class PetLoverLocationClient
    {
        public long Id { get; set; }
        public long PetLoverId { get; set; }
        public long ClientId { get; set; }
        public double Distance { get; set; }

        public virtual PetLover PetLover { get; set; }
        public virtual Client Client { get; set; }
    }
}
