namespace Infrastructure
{
    public class ServiceConfiguration
    {
    }

    public class ConnectionStringOptions
    {
        public string DefaultConnection { get; set; }
    }

    public class SwaggerOptions
    {
        public string JsonRoute { get; set; }
        public string Description { get; set; }
        public string UiEndpoint { get; set; }
    }

    public class JwtOptions
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Key { get; set; }
        public int MaxAge { get; set; }
    }
}
