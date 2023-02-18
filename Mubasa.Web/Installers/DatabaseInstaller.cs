using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mubasa.DataAccess.Data;

namespace Mubasa.Web.Installers
{
    public class DatabaseInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                string serverName = configuration["ServerName"] ?? "DatabaseServer";
                string port = configuration["Port"] ?? "1433";
                string databaseName = configuration["DbName"] ?? "Mubasa";
                string userName = configuration["UserName"] ?? "SA";
                string password = configuration["Password"] ?? "I1yPa$$w0rd";
                string connectionString = $"Server={serverName},{port};Database={databaseName};User ID={userName};Password={password}";

                Console.WriteLine($"---{connectionString}---");

                options.UseSqlServer(connectionString);
            });

            //services.AddDbContext<ApplicationDbContext>(
            //    options => options.UseSqlServer(
            //        configuration.GetConnectionString("Default")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI();
        }
    }
}
