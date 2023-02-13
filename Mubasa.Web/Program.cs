using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Mubasa.DataAccess.Data;
using Mubasa.DataAccess.Repository;
using Mubasa.DataAccess.Repository.IRepository;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Mubasa.Web.Services.ThirdParties.PaymentGateway.MoMo;
using Mubasa.Web.Services.ThirdParties.Carrier.GiaoHangNhanh;
using Mubasa.Web.Services.ThirdParties.EmailSender;
using Mubasa.Web.Services.ThirdParties.PaymentGateway.ZaloPay;

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

            builder.Services.Configure<GiaoHangNhanhConfig>(
                builder.Configuration.GetSection("GiaoHangNhanhConfig"));
            builder.Services.Configure<ZaloPayConfig>(
                builder.Configuration.GetSection("ZaloPayConfig"));
            builder.Services.Configure<MoMoConfig>(
                builder.Configuration.GetSection("MoMoConfig"));
            builder.Services.Configure<MailGun>(
                builder.Configuration.GetSection("MailGun"));

            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI();

            builder.Services.AddAuthentication()
                .AddFacebook(options =>
                {
                    options.AppId = builder.Configuration.GetSection("Facebook")["AppId"];
                    options.AppSecret = builder.Configuration.GetSection("Facebook")["AppSecret"];
                })
                .AddGoogle(options =>
                {
                    options.ClientId = builder.Configuration.GetSection("Google")["ClientId"]; ;
                    options.ClientSecret = builder.Configuration.GetSection("Google")["ClientSecret"]; ;
                });

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