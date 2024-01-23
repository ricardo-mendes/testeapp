using PetAdmin.Web.Models;
using PetAdmin.Web.Security;
using System.Security.Claims;
using System.Linq;

namespace PetAdmin.Web.Extensions
{
    public static class TokenExtensions
    {

        public static ClaimsIdentity AddTokenInfo(this ClaimsIdentity identity, User user)
        {
            identity.AddClaims(
                new[] {
                        new Claim(TokenInfoClaims.ClientId, user.ClientList != null && user.ClientList.Any() ? user.ClientList.FirstOrDefault().Id.ToString() : string.Empty),
                        new Claim(TokenInfoClaims.PetLoverId, user.PetLoverList != null && user.PetLoverList.Any() ? user.PetLoverList.FirstOrDefault().Id.ToString() : string.Empty)
                }
            );
            return identity;
        }
    }
}
