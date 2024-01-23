using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PetAdmin.Web.Configuration;
using PetAdmin.Web.Infra;
using PetAdmin.Web.Infra.Repositories;
using PetAdmin.Web.Localization;
using PetAdmin.Web.Models.Domain;
using PetAdmin.Web.Services;
using PetAdmin.Web.Services.Identity;
using System;
using System.Linq;
using System.Net.Mime;

namespace PetAdmin.Web
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });

            services.AddDbContext<PetContext>(options =>
                options.UseSqlServer(Configuration[$"ConnectionStrings:{Configuration["DefaultConnectionString"]}"]));

            AddServiceDependencies(services);

            services
                .AddIdentityConfiguration(Configuration);

            services.AddCors();

            services.AddMvc()
            .AddJsonOptions(options =>
              options.SerializerSettings.ContractResolver =
            new CamelCasePropertyNamesContractResolver());

            //HealthChecks
            services.AddHealthChecks()
                .AddSqlServer(Configuration[$"ConnectionStrings:{Configuration["DefaultConnectionString"]}"], name: "BancoSQL");

            services.AddHealthChecksUI();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseSwaggerDocumentation();
            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();

            //HealthChecks
            app.UseHealthChecks("/status", new HealthCheckOptions()
            {
                ResponseWriter = async (context, report) =>
                {
                    var result = JsonConvert.SerializeObject(
                        new
                        {
                            statusApplication = report.Status.ToString(),
                            healthChecks = report.Entries.Select(e => new
                            {
                                check = e.Key,
                                ErrorMessage = e.Value.Exception?.Message,
                                status = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                            })
                        });
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(result);
                }
            });

            // Gera o endpoint que retornará os dados utilizados no dashboard
            app.UseHealthChecks("/healthchecks-data-ui", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI();
            //app.UseHealthChecksUI(options => { options.UIPath = "/api/hc-ui"; });
        }

        private void AddServiceDependencies(IServiceCollection services)
        {
            services.AddScoped<UnitOfWork, UnitOfWork>();
            services.AddScoped<NotificationHandler, NotificationHandler>();

            //--------------------
            services.AddScoped<UserService, UserService>();
            services.AddScoped<TokenService, TokenService>();
            services.AddScoped<ScheduleService, ScheduleService>();
            services.AddScoped<SchedulePetService, SchedulePetService>();
            services.AddScoped<EmailService, EmailService>();
            services.AddScoped<MyPetLoverService, MyPetLoverService>();
            services.AddScoped<PetLoverService, PetLoverService>();
            services.AddScoped<SchedulePetRangeService, SchedulePetRangeService>();
            services.AddScoped<AttachmentService, AttachmentService>();
            services.AddScoped<PetService, PetService>();
            services.AddScoped<VaccineService, VaccineService>();
            services.AddScoped<ClientService, ClientService>();
            services.AddScoped<PetLoverLocationClientService, PetLoverLocationClientService>();
            services.AddScoped<LocationGoogleService, LocationGoogleService>();
            
            //Mesmo sendo AddSingleton, ele não vai confundir os usuários mesmo sendo pra toda aplicação
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserApplication, UserApplication>();

            //--------------------
            services.AddScoped<RepositoryBase<Pet>, RepositoryBase<Pet>>();
            services.AddScoped<RepositoryBase<Client>, RepositoryBase<Client>>();
            services.AddScoped<RepositoryBase<Employee>, RepositoryBase<Employee>>();
            services.AddScoped<RepositoryBase<PetLover>, RepositoryBase<PetLover>>();
            services.AddScoped<RepositoryBase<Schedule>, RepositoryBase<Schedule>>();
            services.AddScoped<RepositoryBase<SchedulePet>, RepositoryBase<SchedulePet>>();
            services.AddScoped<RepositoryBase<ScheduleItem>, RepositoryBase<ScheduleItem>>();
            services.AddScoped<RepositoryBase<ScheduleItemClient>, RepositoryBase<ScheduleItemClient>>();
            services.AddScoped<RepositoryBase<ScheduleItemEmployee>, RepositoryBase<ScheduleItemEmployee>>();
            services.AddScoped<RepositoryBase<Vaccine>, RepositoryBase<Vaccine>>();
            services.AddScoped<ScheduleItemClientRepository, ScheduleItemClientRepository>();
            services.AddScoped<SchedulePetRepository, SchedulePetRepository>();
            services.AddScoped<PetLoverRepository, PetLoverRepository>();
            services.AddScoped<PetLoverLocationClientRepository, PetLoverLocationClientRepository>();
            services.AddScoped<ClientRepository, ClientRepository>();
        }
    }
}
