using System;

namespace PetAdmin.Web.Dto
{
    public class VaccineDto
    {
        public long Id { get; set; }
        public long PetId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime? RevaccineDate { get; set; }
        public string ClinicName { get; set; }
        public string Note { get; set; }
        public PhotoDto Photo { get; set; }

        //Usado apenas no get da vaccine
        public string PhotoUrl { get; set; }
    }
}
