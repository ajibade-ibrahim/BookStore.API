using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BookStore.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string VersionOne = "v1";
        private const string BookStoreApi = "Book Store API";

        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(
                setup =>
                {
                    setup.SwaggerDoc(
                        VersionOne,
                        new OpenApiInfo
                        {
                            Title = BookStoreApi,
                            Version = VersionOne
                        });

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    setup.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
                });
        }

        public static void AddSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                setup =>
                {
                    setup.SwaggerEndpoint($"/swagger/{VersionOne}/swagger.json", BookStoreApi);
                    setup.RoutePrefix = string.Empty;
                });
        }
    }
}