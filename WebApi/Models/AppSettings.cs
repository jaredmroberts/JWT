namespace WebApi.Models.Configuration
{
    public class AppSettings
    {
        public Security Security { get; set; }
    }

    public class Security
    {
        public string Issuer { get; set; }
        public string SecurityKey { get; set; }
    }
}

