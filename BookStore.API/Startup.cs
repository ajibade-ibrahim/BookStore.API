using System;
using System.Reflection;
using AutoMapper;
using BookStore.API.Contracts;
using BookStore.API.Extensions;
using BookStore.API.Services;
using BookStore.Data;
using BookStore.Data.Repositories;
using BookStore.Data.Repositories.Contracts;
using BookStore.Services;
using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookStore.API
{
    public class Startup
    {
        private const string ApplicationProblemJson = "application/problem+json";

        private static Action<ApiBehaviorOptions> ApiBehaviorOptionsSetupAction()
        {
            return setupAction => setupAction.InvalidModelStateResponseFactory = context =>
            {
                var httpContext = context.HttpContext;
                var problemDetailsFactory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                    httpContext,
                    context.ModelState);
                problemDetails.Detail = "See errors section for details";
                problemDetails.Instance = httpContext.Request.Path;

                var containsUnparsedArguments = ((ActionExecutingContext)context).ActionArguments.Count
                    != context.ActionDescriptor.Parameters.Count;

                if (!containsUnparsedArguments && !context.ModelState.IsValid)
                {
                    problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                    problemDetails.Title = "One or more validation errors occurred.";
                    problemDetails.Type = "https://tools.ietf.org/html/rfc7807";
                }

                return new UnprocessableEntityObjectResult(problemDetails)
                {
                    ContentTypes =
                    {
                        ApplicationProblemJson
                    }
                };
            };
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/{shortdate}_logfile.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.AddSwaggerConfiguration();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BookStoreDbContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction => sqlServerOptionsAction.MigrationsAssembly("BookStore.Data")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<BookStoreDbContext>();
            services.AddCors(
                setup => setup.AddPolicy(
                    "BlazorAppPolicy",
                    policy =>
                    {
                        policy.AllowAnyOrigin();
                        policy.AllowAnyHeader();
                        policy.AllowAnyMethod();
                    }));

            services.AddSwaggerConfiguration();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();
            services.AddAutoMapper(Assembly.Load("BookStore.Services"));
            services.AddControllers().ConfigureApiBehaviorOptions(ApiBehaviorOptionsSetupAction()).AddNewtonsoftJson();
        }
    }
}