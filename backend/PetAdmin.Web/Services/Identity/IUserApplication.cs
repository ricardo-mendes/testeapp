using System;
using System.Collections.Generic;

namespace PetAdmin.Web.Services.Identity
{
    public interface IUserApplication
    {
        string Name { get; }
        Guid GetUserId();
        string GetUserEmail();
        long GetPetLoverId();
        long GetClientId();
        bool IsAuthenticated();
        bool IsInRole(string role);
        Dictionary<string, string> GetClaimsIdentity();
    }
}
