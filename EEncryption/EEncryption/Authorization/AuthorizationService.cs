using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace Encryption.Authorization
{
    public class AuthorizationService : IAuthService
    {
        private static string secret;
        private static string encryptionKey;
        private byte[] key;
        private byte[] encSecret;
        private SymmetricSecurityKey encKey;
        private IHttpContextAccessor _httpContextAccessor;

        public IConfiguration Configuration { get; }
        public AuthorizationService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            Configuration = configuration;
            string sectionName = "AppSettings";
           
            secret = Configuration.GetSection(sectionName)["Secret"];
            key = Encoding.ASCII.GetBytes(secret);
            encryptionKey = Configuration.GetSection(sectionName)["EncryptionKey"];
            encSecret = Encoding.ASCII.GetBytes(encryptionKey);
            encKey = new SymmetricSecurityKey(encSecret);
        }

        public string Authenticate(string userID, List<Claim> claims, int ticketExpiration, string program)
        {
            var key = Encoding.ASCII.GetBytes(secret);
            var signinKey = new SymmetricSecurityKey(key);

            var key1 = Encoding.ASCII.GetBytes(encryptionKey);
            var signinKey2 = new SymmetricSecurityKey(key1);

            List<Claim> li = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userID),
                new Claim("IDNum", userID),
            };
            li.AddRange(claims);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(li),
                Issuer = program,
                Audience = program,
                Expires = DateTime.UtcNow.AddMinutes(ticketExpiration),
                SigningCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256Signature),
                EncryptingCredentials = new EncryptingCredentials(signinKey2, SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256)
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var token = jwtTokenHandler.WriteToken(jwtToken);
            return token;
        }
        public JwtSecurityToken GetJwtSecurityToken(string token)
        {
            SymmetricSecurityKey signinKey = new SymmetricSecurityKey(key);

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = signinKey,
                TokenDecryptionKey = encKey
            };

            SecurityToken validatedToken;
            var handler = new JwtSecurityTokenHandler();

            handler.ValidateToken(token, tokenValidationParameters, out validatedToken);
            foreach (var claim in ((JwtSecurityToken)validatedToken).Claims)
            {
                if (claim.Type == "IDNum")
                {
                    if (claim.Value != _httpContextAccessor.HttpContext.Request.Headers["IDNum"])
                        return null;
                }

            }
            return (JwtSecurityToken)validatedToken;
        }
        public string GetPropertyFromToken(string propety)
        {
            string prop = string.Empty;
            try
            {
                string tokenStr = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                tokenStr = tokenStr.Split(' ')[1];
                JwtSecurityToken token = GetJwtSecurityToken(tokenStr);

                foreach (var cl in token.Claims)
                {
                    if (cl.Type == propety)
                    {
                        prop = cl.Value;
                    }

                }
                if (prop.Length > 0)
                {
                    return prop;
                }
                else
                {
                    return $"{propety} length error";
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

    }
}
