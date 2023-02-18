using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Mubasa.Web.Installers
{
    public class CultureInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("vi-VN");

                var cultures = new CultureInfo[]
                {
                    new CultureInfo("vi-VN"),
                    new CultureInfo("en-US"),
                };

                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });
        }
    }
}
