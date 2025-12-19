using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CloneIntime
{
    public class JWTConfiguration
    {
        public const string Issuer = "JwtIssuer";
        public const string Audience = "JwtClient";
        private static readonly string Key = Environment.GetEnvironmentVariable("JWT_SECRET") 
            ?? throw new InvalidOperationException("JWT_SECRET environment variable is not set.");
        public const int LifetimeDays = 180;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
