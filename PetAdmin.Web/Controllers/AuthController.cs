//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using PetAdmin.Web.Extensions;
//using PetAdmin.Web.Services;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using static PetAdmin.Web.Dto.UserViewModel;

//namespace PetAdmin.Web.Controllers
//{
//    public class AuthController : BaseController
//    {
//        // Responsável por logar o usuário
//        private readonly SignInManager<IdentityUser> _signInManager;
//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly JwtSettings _jwtSettings;

//        //O IOptions é para pegar dados que servem de parâmetro, pois ele faz o .Value
//        public AuthController(
//            NotificationHandler notificationHandler,
//            SignInManager<IdentityUser> signInManager,
//                    UserManager<IdentityUser> userManager,
//                    IOptions<JwtSettings> jwtSettings,
//                    IUser user) : base(notificationHandler)
//        {
//            _signInManager = signInManager;
//            _userManager = userManager;
//            _jwtSettings = jwtSettings.Value;
//        }

//        [HttpPost("nova-conta")]
//        public async Task<IActionResult> Registrar(RegisterUserViewModel registerUser)
//        {
//            var user = new IdentityUser
//            {
//                UserName = registerUser.Email,
//                Email = registerUser.Email,
//                EmailConfirmed = true
//            };

//            var result = await _userManager.CreateAsync(user, registerUser.Password);

//            if (result.Succeeded)
//            {
//                await _signInManager.SignInAsync(user, false);
//                return await Response(await GerarJwt(user.Email));
//            }
//            else
//            {
//                foreach (var error in result.Errors)
//                {
//                    RaisError(error.Description);
//                }
//                return await Response(null);
//            }
//        }

//        [HttpPost("entrar")]
//        public async Task<IActionResult> Login(LoginUserViewModel loginUser)
//        {
//            //Eu poderia usar o _signInManager.SignInAsync(), mas ele pede o IdentityUser
//            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);//O ultimo parametro "true" vai travar o login
//            //depoois de 5 tentativas erradas (essas tentativas podem ser definidas) e ele define para só liberar daqui 5 minutos

//            if (result.Succeeded)
//            {
//                return await Response(await GerarJwt(loginUser.Email));
//            }
//            if (result.IsLockedOut)
//            {
//                RaisError("Usuário temporariamente bloqueado por tentativas inválidas");
//                return await Response(null);
//            }

//            RaisError("Usuário ou Senha incorretos");
//            return await Response(null);
//        }

//        private async Task<LoginResponseViewModel> GerarJwt(string email)
//        {
//            var user = await _userManager.FindByEmailAsync(email);
//            var claims = await _userManager.GetClaimsAsync(user);
//            var userRoles = await _userManager.GetRolesAsync(user);

//            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
//            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
//            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
//            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
//            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
//            foreach (var userRole in userRoles)
//            {
//                claims.Add(new Claim("role", userRole));
//            }

//            var identityClaims = new ClaimsIdentity();
//            identityClaims.AddClaims(claims);

//            // Manipulalador, para criar o token
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
//            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
//            {
//                Issuer = _jwtSettings.Issuer,
//                Audience = _jwtSettings.Audience,
//                Subject = identityClaims,
//                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.MinutesToExpiration),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//            });

//            var encodedToken = tokenHandler.WriteToken(token);

//            var response = new LoginResponseViewModel
//            {
//                AccessToken = encodedToken,
//                ExpiresIn = TimeSpan.FromMinutes(_jwtSettings.MinutesToExpiration).TotalSeconds,
//                UserToken = new UserTokenViewModel
//                {
//                    Id = user.Id,
//                    Email = user.Email,
//                    Claims = claims.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value })
//                }
//            };

//            return response;
//        }

//        private static long ToUnixEpochDate(DateTime date)
//            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
//    }
//}
