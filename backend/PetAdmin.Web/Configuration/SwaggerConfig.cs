//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.DependencyInjection;
//using Swashbuckle.AspNetCore.Swagger;
//using Swashbuckle.AspNetCore.SwaggerGen;
//using Swashbuckle.AspNetCore.SwaggerUI;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;

//namespace PetAdmin.Web.Configuration
//{
//    public static class SwaggerConfig
//    {
//        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
//        {
//            services.AddSwaggerGen(c =>
//            {
//                c.SwaggerDoc("v1", new Info { Title = "Addon API", Version = "v1" });
//                //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "PetAdmin.Web.xml"));

//                c.OperationFilter<FormFileOperationFilter>();

//                // Swagger 2.+ support
//                var security = new Dictionary<string, IEnumerable<string>>
//                {
//                    {"Bearer", new string[] { }},
//                };

//                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
//                {
//                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
//                    Name = "Authorization",
//                    In = "header",
//                    Type = "apiKey"
//                });
//                c.AddSecurityRequirement(security);
//            });

//            return services;
//        }

//        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
//        {
//            app.UseSwagger();
//            app.UseSwaggerUI(c =>
//            {
//                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Addon");

//                c.DocumentTitle = "Addon Documentation";
//                c.DocExpansion(DocExpansion.None);
//            });

//            return app;
//        }

//        public class FormFileOperationFilter : IOperationFilter
//        {
//            private const string FormDataMimeType = "multipart/form-data";
//            private static readonly string[] FormFilePropertyNames =
//                typeof(IFormFile).GetTypeInfo().DeclaredProperties.Select(x => x.Name).ToArray();

//            public void Apply(Operation operation, OperationFilterContext context)
//            {
//                if (context.ApiDescription.ParameterDescriptions.Any(x => x.ModelMetadata.ContainerType == typeof(IFormFile)))
//                {
//                    var formFileParameters = operation
//                        .Parameters
//                        .OfType<NonBodyParameter>()
//                        .Where(x => FormFilePropertyNames.Contains(x.Name))
//                        .ToArray();
//                    var index = operation.Parameters.IndexOf(formFileParameters.First());
//                    foreach (var formFileParameter in formFileParameters)
//                    {
//                        operation.Parameters.Remove(formFileParameter);
//                    }

//                    var formFileParameterName = context
//                        .ApiDescription
//                        .ActionDescriptor
//                        .Parameters
//                        .Where(x => x.ParameterType == typeof(IFormFile))
//                        .Select(x => x.Name)
//                        .First();
//                    var parameter = new NonBodyParameter()
//                    {
//                        Name = formFileParameterName,
//                        In = "formData",
//                        Description = "The file to upload.",
//                        Required = true,
//                        Type = "file"
//                    };
//                    operation.Parameters.Insert(index, parameter);

//                    if (!operation.Consumes.Contains(FormDataMimeType))
//                    {
//                        operation.Consumes.Add(FormDataMimeType);
//                    }
//                }
//            }
//        }
//    }
//}
