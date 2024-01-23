using Microsoft.AspNetCore.Http;
using System;

namespace Microsoft.AspNetCore.Authentication
{
    public static class AuthenticationHttpContextExtensions
    {
        public static string GetBearerToken(this HttpContext context)
        {
            string token = null;

            string authorization = context.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorization))
                token = null;

            if (authorization != null && authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                token = authorization.Substring("Bearer ".Length).Trim();

            return token;
        }
    }
}
