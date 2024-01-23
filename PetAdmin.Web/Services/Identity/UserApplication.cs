using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PetAdmin.Web.Services.Identity
{
    public class UserApplication : IUserApplication
    {
        private readonly IHttpContextAccessor _accessor;
        private Dictionary<string, string> _claims;
        private readonly string _petLoverIdField = "PetLoverId";
        private readonly string _clientIdField = "ClientId";

        public UserApplication(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
            _claims = null;
        }

        public string Name => _accessor.HttpContext.User.Identity.Name;

        public Guid GetUserId()
        {
            return IsAuthenticated() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
        }

        public string GetUserEmail()
        {
            return IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : "";
        }

        public long GetPetLoverId()
        {
            if (_claims == null)
                _claims = GetClaimsIdentity();

            if (_claims.ContainsKey(_petLoverIdField) && !string.IsNullOrEmpty(GetClaimValue(_petLoverIdField)))
                return Convert.ToInt64(GetClaimValue(_petLoverIdField));
            else return 0;
        }

        public long GetClientId()
        {
            if (_claims == null)
                _claims = GetClaimsIdentity(); 

            if (_claims.ContainsKey(_clientIdField) && !string.IsNullOrEmpty(GetClaimValue(_clientIdField)))
                return Convert.ToInt64(GetClaimValue(_clientIdField));
            else return 0;
        }

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool IsInRole(string role)
        {
            return _accessor.HttpContext.User.IsInRole(role);
        }

        //public IEnumerable<Claim> GetClaimsIdentity()
        //{
        //    return _accessor.HttpContext.User.Claims;
        //}

        public Dictionary<string, string> GetClaimsIdentity()
        {
            var result = new Dictionary<string, string>();

            foreach (var claim in _accessor.HttpContext.User.Claims)
                if (!result.ContainsKey(claim.Type))
                    result.Add(claim.Type, claim.Value);

            return result;
        }


        private string GetClaimValue(string key)
            => _claims.FirstOrDefault(e => e.Key == key).Value;
    }

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
    }
}
