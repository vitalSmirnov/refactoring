using CloneIntime.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CloneIntime.Services
{
    public class SupportService
    {

        private readonly Context _context;

        public SupportService (Context context)
        {
            _context = context;
        }   

        private ClaimsIdentity GetIdentity(string email, string id)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, email),
                new Claim(JwtRegisteredClaimNames.NameId, id),
            };
            ClaimsIdentity claimidentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimTypes.NameIdentifier);
            return claimidentity;
        }

        public JwtSecurityToken GenerateJWT(string email, string id)
        {
            var now = DateTime.UtcNow;
            var identity = GetIdentity(email, id);
            var jwt = new JwtSecurityToken(
                issuer: JWTConfiguration.Issuer,
                audience: JWTConfiguration.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.AddMinutes(JWTConfiguration.LifetimeDays),
                signingCredentials: new SigningCredentials(JWTConfiguration.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return jwt;
        }

        public async Task<string> GetToken(IHeaderDictionary headerDictionary) //Берется из HttpContext.Request.Headers
        {
            var requestHeaders = new Dictionary<string, string>();
            foreach (var header in headerDictionary)
            {
                requestHeaders.Add(header.Key, header.Value);
            }
            var autorizationSrting = requestHeaders["Authorization"];
            var token = autorizationSrting.Replace("Bearer ", "");
            return token;
        }

        public async Task<string> GetUserId(ClaimsPrincipal principal)//Берется из HttpContext.User
        {
            return principal.Claims.SingleOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value;
        }

        public async Task<bool> IsLogged(IHeaderDictionary headerDictionary)
        {
            var token = await GetToken(headerDictionary);
            var tokenEntity = _context.TokenEntity.FirstOrDefault(x => x.Token == token);
            return !(tokenEntity == null);
        }
    }
}
