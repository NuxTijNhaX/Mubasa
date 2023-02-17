namespace Mubasa.Models.ConfigModels
{
    public class DatabaseConfig
    {
        public string ServerName { get; set; } = "Mubasa-Server";
        public int Port { get; set; } = 1433;
        public string DbName { get; set; } = "Mubasa";
        public string UserName { get; set; } = "SA";
        public string Password { get; set; } = "I1yPa$$w0rd";
    }
}
