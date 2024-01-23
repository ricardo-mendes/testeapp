using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetAdmin.Web.Extensions;
using PetAdmin.Web.Infra;
using PetAdmin.Web.Models;
using System;
using System.Text;

namespace PetAdmin.Web.Configuration
{
    public static class IdentityConfig 
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            //services.AddDbContext<UserContext>(options =>
            //    options.UseSqlServer(configuration[$"ConnectionStrings:{configuration["DefaultConnectionString"]}"]));

            services.AddDefaultIdentity<User>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddRoles<Roles>()
            .AddEntityFrameworkStores<PetContext>()
            .AddErrorDescriber<IdentityMensagensPortugues>()
            .AddDefaultTokenProviders();

            var jwtSettingsSection = configuration.GetSection("JwtSettings");

            services.Configure<JwtSettings>(jwtSettingsSection);

            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
            // Está fazendo um encoding no secret com base no ASCII
            var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

            /*
             Explicaçõa sobre essas duas opções:
             x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            
             R: Toda vez que for autenticar alguem, o padrão de autenticação é para gerar um token.
             E toda vez que for validar o token, que é Challenge, também é com base no token
            */

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true; //Se for trabalhar apenas com https
                x.SaveToken = true; //Fica mais fácil da aplicação validar o usuário logado após a apresentação do tokens
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, //Validar se quem está emitindo é o mesmo qdo vc recebeu o token(validar com base na chave de quem passou)
                    IssuerSigningKey = new SymmetricSecurityKey(key), //Transforma a chave q está codificada em ASCII para uma chave criptografada

                    ValidateIssuer = true, //Validar se quem está emitindo conforme o nome
                    ValidIssuer = jwtSettings.Issuer, // Valor do Issuer // ValidIssuers para vários
                    
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience, // Valor do Audience // ValidAudiences pra vários
                    
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(
                         jwtSettings.MinutesToExpiration)
                };
            });

            return services;
        }
    }
}
