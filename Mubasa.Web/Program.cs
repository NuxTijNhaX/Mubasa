using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Mubasa.DataAccess.Data;
using Mubasa.DataAccess.Repository;
using Mubasa.DataAccess.Repository.IRepository;
using System.Globalization;

namespace Mubasa.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            builder.Services.AddControllersWithViews().AddViewLocalization();

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("vi-VN");

                var cultures = new CultureInfo[]
                {
                    new CultureInfo("vi-VN"),
                    new CultureInfo("en-US"),
                };

                options.SupportedCultures = cultures;
                options.SupportedUICultures= cultures;
            });

            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRequestLocalization();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}