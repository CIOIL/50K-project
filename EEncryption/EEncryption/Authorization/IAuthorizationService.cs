using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace Encryption.Authorization
{
    public interface IAuthService
    {
        JwtSecurityToken GetJwtSecurityToken(string token);
        string Authenticate(string userID, List<Claim> claims, int ticketExpiration, string program);
        string GetPropertyFromToken(string propety);
    }
}
