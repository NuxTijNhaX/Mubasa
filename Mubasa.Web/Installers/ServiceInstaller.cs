using Mubasa.Models.ConfigModels;

namespace Mubasa.Web.Installers
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GiaoHangNhanhConfig>(
                configuration.GetSection("GiaoHangNhanhConfig"));
            services.Configure<ZaloPayConfig>(
                configuration.GetSection("ZaloPayConfig"));
            services.Configure<MoMoConfig>(
                configuration.GetSection("MoMoConfig"));
            services.Configure<MailGunConfig>(
                configuration.GetSection("MailGunConfig"));

            services.AddAuthentication()
                .AddFacebook(options =>
                {
                    options.AppId = configuration.GetSection("Facebook")["AppId"];
                    options.AppSecret = configuration.GetSection("Facebook")["AppSecret"];
                })
                .AddGoogle(options =>
                {
                    options.ClientId = configuration.GetSection("Google")["ClientId"]; ;
                    options.ClientSecret = configuration.GetSection("Google")["ClientSecret"]; ;
                });
        }
    }
}
