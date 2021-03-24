using Alura.ListaLeitura.Seguranca;
using Alura.ListaLeitura.HttpClients;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Alura.WebAPI.WebApp.Formatters;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Alura.ListaLeitura.WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options  => {
                    options.LoginPath = "/Usuario/Login";
            });

            services.AddHttpClient<LivroApiClient>(client => {
                client.BaseAddress = new Uri(Configuration["ApiURIs:LivrosApi"]);
            });

            services.AddHttpClient<AuthApiClient>(client =>
            {
                client.BaseAddress = new Uri(Configuration["ApiURIs:AuthApi"]);
            });

            services.AddMvc(options => {
                options.OutputFormatters.Add(new LivroCsvFormatter());
            }).AddXmlSerializerFormatters();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
