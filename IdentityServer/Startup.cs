using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using IdentityServer.Data;

namespace IdentityServer
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(config => {
                config.UseInMemoryDatabase("Memory");
            });

            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;

                //config.Lockout.MaxFailedAccessAttempts = 4;
                //config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                //config.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config => {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
                config.AccessDeniedPath = "/Auth/UserAccessDenied";
            });

            services.AddIdentityServer()
              //to paketo identity server 4 asp 
              .AddAspNetIdentity<IdentityUser>()
              .AddInMemoryApiResources(Configuration.GetApis())
              .AddInMemoryIdentityResources(Configuration.GetIdentityResources())
              .AddInMemoryClients(Configuration.GetClients())
              .AddDeveloperSigningCredential();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddControllersWithViews();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("MyPolicy");

            app.UseRouting();
            app.UseStaticFiles();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

        }
    }
}
