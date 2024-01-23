using PetAdmin.Web.Dto;
using System;

namespace PetAdmin.Web.Models.Security
{
    public class UserAuth
    {
        public UserAuth() : base()
        {
            UserLogin = "Não Autorizado";
            BearerToken = string.Empty;
        }

        public Guid UserId { get; set; }
        public ClientDto Client { get; set; }
        public PetLoverDto PetLover { get; set; }
        public string UserLogin { get; set; }
        public string BearerToken { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
