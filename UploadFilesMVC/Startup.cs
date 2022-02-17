using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RickApps.UploadFilesMVC.Data;
using RickApps.UploadFilesMVC.Interfaces;
using RickApps.UploadFilesMVC.Models;

namespace RickApps.UploadFilesMVC
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
            // We will use cookie authentication for our admin and photo controllers.
            // You can add options to customize the cookie. Like how long to expire,
            // and other security settings.
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Authorize/Login";
                options.Cookie.Name = ".RickApps.MVCUpload";
            });

            services.AddControllersWithViews();

            // Set up our database. 
            services.AddDbContext<EFContext>(options =>
            {
                // Below works for SQL Server Express, SQL Server and even Azure. Just add/change your connection
                // string in appsettings.json. 
                options.UseSqlServer(Configuration.GetConnectionString("SQLServerMVCContext"));
                // Not using the logger in this project, but you could add if you wanted.
                options.UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }));
            });

            // Add our unit of work to manage all our repositories. Use same instance for all.
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Injection for our AuthorizeController. We are using the Options pattern here.
            // Consider using the database instead of appsettings for credentials.
            // We only have one login for the administrator and one role, so it seems overkill to use Identity.
            // But know that Visual Studio can add all tables and code automatically if you decide to use it.
            services.Configure<Credentials>(
                Configuration.GetSection(Credentials.Administrator));
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
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // We added the two statements below for our login stuff
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
