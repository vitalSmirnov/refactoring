using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CloneIntime
{
    public class JWTConfiguration
    {
        public const string Issuer = "JwtIssuer";
        public const string Audience = "JwtClient";
        private const string Key = "awxrvtbyildgashdgakh";
        public const int Lifetime = 180;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
