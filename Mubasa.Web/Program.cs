using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Mubasa.DataAccess.Data;
using Mubasa.DataAccess.Repository;
using Mubasa.DataAccess.Repository.IRepository;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Mubasa.Utility;
using Mubasa.Utility.ThirdParties.Carrier;
using Mubasa.Utility.ThirdParties.PaymentGateway;
using Mubasa.Web.Functionality.EmailSender;

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

            builder.Services.Configure<GiaoHangNhanh>(
                builder.Configuration.GetSection("GiaoHangNhanh"));
            builder.Services.Configure<ZaloPay>(
                builder.Configuration.GetSection("ZaloPay"));
            builder.Services.Configure<MoMo>(
                builder.Configuration.GetSection("MoMo"));
            builder.Services.Configure<MailGun>(
                builder.Configuration.GetSection("MailGun"));

            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(1200);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddSingleton<IEmailSender, EmailSender>();
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}