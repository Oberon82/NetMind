using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetMind.Data;
using NetMind.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace NetMind
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options => options.UseFirebird(connection));
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Login");
                    options.ExpireTimeSpan = TimeSpan.FromDays(30); // time for the cookei to last in the browser
                    options.SlidingExpiration = true; // the cookie would be re-issued on any request half way through the ExpireTimeSpan
                });

            services.AddControllersWithViews();
            services.AddSingleton<DummyUserCountService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<RegisterMiddleWare>();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); 
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "login",
                    pattern: "{action=Login}");
            });
        }
    }

    public class RegisterMiddleWare
    {
        private readonly RequestDelegate _next;
        public RegisterMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationContext dbcontext, DummyUserCountService userCount)
        {
            if  (context.Request.Path.Value.ToLower() != "/register")
            {
                if (!userCount.HasChecked())
                {
                    userCount.SetChecked(dbcontext.Users.Count() != 0);
                }
                

                if (userCount.HasUser())
                {
                    await _next.Invoke(context);
                }
                else
                    context.Response.Redirect("/Register");
            }
            else
                await _next.Invoke(context);
        }
    }

    public class DummyUserCountService
    {
        private bool _hasUser;

        private bool _hasChecked;
        public bool HasUser()
        {
            return _hasUser;
        }

        public bool HasChecked()
        {
            return _hasChecked;
        }

        public void SetChecked(bool hasUser)
        {
            _hasChecked = true;
            _hasUser = hasUser;
        }
    }
}
