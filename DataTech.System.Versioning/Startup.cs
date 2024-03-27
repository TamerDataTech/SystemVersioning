using DataTech.System.Versioning.Data;
using DataTech.System.Versioning.Extensions;
using DataTech.System.Versioning.Middleware;
using DataTech.System.Versioning.Models.Domain;
using DataTech.System.Versioning.Repositories;
using DataTech.System.Versioning.Service.Db;
using DataTech.System.Versioning.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataTech.System.Versioning
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
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

            });

            services.AddMemoryCache();
            services.AddDistributedMemoryCache();

            services.AddControllersWithViews();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddEFSqlContext<DataContext>("db-conn"); 


            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddTransient<ISystemService, SystemService>();
            services.AddTransient<ISystemRepository, SystemRepository>();


            services.AddTransient<IModuleService, ModuleService>();
            services.AddTransient<IModuleRepository, ModuleRepository>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(config =>
             {
                 config.Cookie.Name = "_Management_Login_";
                 config.LoginPath = "/user/login";
             });

            services.AddTransient<DatabaseService>(); 

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            UpdateDatabase(app);

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
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization(); 

            app.UseUserHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=Login}/{id?}");
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<DataContext>())
                {
                    context.Database.Migrate();

                    var adminUser = context.AppUsers.FirstOrDefault(u => u.Username == "admin");
                    if (adminUser == null)
                    {
                        adminUser = new AppUser
                        {
                            Username = "admin", 
                            Fullname = "Admin User",
                            Email = "info@datatech.com", 
                            Password = "9bFFOGv/x2MS94QkSVoySg==",
                        };
                         
                        context.AppUsers.Add(adminUser);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
