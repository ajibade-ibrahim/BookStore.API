namespace BookStore.API.Models
{
    public interface IJwtConfiguration
    {
        string Issuer { get; set; }
        string Key { get; set; }
    }

    public class JwtConfiguration : IJwtConfiguration
    {
        public string Issuer { get; set; }
        public string Key { get; set; }
    }
}