namespace Mubasa.Web.Installers
{
    public static class InstallerExtensions
    {
        public static void InstallServicesInAssembly(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var installer = typeof(Program).Assembly.ExportedTypes
                .Where(x => 
                    typeof(IInstaller).IsAssignableFrom(x) && 
                    !x.IsAbstract && 
                    !x.IsInterface)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();

            installer.ForEach(x => x.InstallService(services, configuration));
        }
    }
}
