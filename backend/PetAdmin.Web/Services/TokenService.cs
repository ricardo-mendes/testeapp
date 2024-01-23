using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetAdmin.Web.Extensions;
using PetAdmin.Web.Models;
using PetAdmin.Web.Models.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PetAdmin.Web.Services
{
    public class TokenService
    {
        private readonly Extensions.JwtSettings _jwtSettings;

        public TokenService(
            IConfiguration configuration)
        {
            _jwtSettings = configuration.GetSection("JwtSettings").Get<Extensions.JwtSettings>(); //jwtSettings.Value;
        }

        public UserAuth BuildUserAuthObject(User user)
        {
            UserAuth userAuth = new UserAuth
            {
                IsAuthenticated = true
            };

            userAuth.BearerToken = BuildJwtToken(user);

            return userAuth;
        }

        private string BuildJwtToken(User user)
        {
            //var claims = await _userManager.GetClaimsAsync<UserClaim>(user);
            //var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64)
            };

            //foreach (var userRole in userRoles)
            //{
            //    claims.Add(new Claim("role", userRole));
            //}

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaims(claims);
            claimsIdentity.AddTokenInfo(user);

            // Manipulalador, para criar o token
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.MinutesToExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }

        public JwtSecurityToken VerifyToken(string token)
        {
            var provider = new RSACryptoServiceProvider(2048);

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            var validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidAudience = _jwtSettings.Audience,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true
            };

            var handler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken = null;
            try
            {
                handler.ValidateToken(token, validationParameters, out validatedToken);
                return (validatedToken as JwtSecurityToken);
            }
            catch (SecurityTokenException ex)
            {
                return null;
            }
            catch
            {
                return null;
            }
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        //private string BuildJwtToken(UserAuth userAuth)
        //{
        //    SymmetricSecurityKey key = new SymmetricSecurityKey(
        //      Encoding.UTF8.GetBytes(_settings.Key));

        //    var token = new JwtSecurityToken(
        //      issuer: _settings.Issuer,
        //      audience: _settings.Audience,
        //      notBefore: DateTime.UtcNow,
        //      expires: DateTime.UtcNow.AddMinutes(
        //          _settings.MinutesToExpiration),
        //      signingCredentials: new SigningCredentials(key,
        //                  SecurityAlgorithms.HmacSha256)
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}
