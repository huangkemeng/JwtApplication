using Microsoft.IdentityModel.Tokens;

namespace JwtUtils
{
    public class JWTTokenOptions
    {
        public string Audience { get; set; }
        public RsaSecurityKey Key { get; set; }
        public SigningCredentials Credentials { get; set; }
        public string Issuer { get; set; }
    }
}
