using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Blazored.LocalStorage;
using BookStore.BlazorServer.HttpHandlers;
using BookStore.BlazorServer.Providers;
using BookStore.BlazorServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;

namespace BookStore.BlazorServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapBlazorHub();
                    endpoints.MapFallbackToPage("/_Host");
                });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddTransient<BearerTokenHandler>();
            services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            services.AddHttpClient<AccountService>(HttpClientSetup).AddHttpMessageHandler<BearerTokenHandler>();
            services.AddHttpClient<BookStoreService>(HttpClientSetup).AddHttpMessageHandler<BearerTokenHandler>();
            services.AddBlazoredLocalStorage();
            services.AddScoped<JwtSecurityTokenHandler>();
        }

        private void HttpClientSetup(HttpClient client)
        {
            client.BaseAddress = new Uri(Configuration["ApiBaseUrl"]);
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }
    }
}