using AutoMapper;
using Encryption.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenApi.Services;

namespace TokenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : TControllerBase
    {
        private readonly IPermissionService _permissionService;
        readonly string secret = "";
        readonly string encryptionKey = "";
        public AuthController(IPermissionService permissionService
                                , IHttpContextAccessor httpContextAccessor
                                , IConfiguration configuration
                                , ILogger<AuthController> logger
                                , IMapper mapper
                                , IAuthService authorizationService)
                                : base(httpContextAccessor, configuration, logger, mapper, authorizationService)
        {
            _permissionService = permissionService;
            secret = this.Configuration["AppSettings:Secret"];
            encryptionKey = this.Configuration["AppSettings:EncryptionKey"];
            httpContextAccessor.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "*");
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            string auth = "";
            if (_httpContextAccessor.HttpContext.Request.Headers["TokenJWT"].Count == 0 && UserIdNum != null && UserIdNum.Length > 0)
            {
                auth = await _permissionService.Authenticate(UserIdNum, secret, encryptionKey);
                //Added here instead of client intersceptor
                _httpContextAccessor.HttpContext.Request.Headers.Add("Authorization", $"Bearer {auth}");
                //
                _httpContextAccessor.HttpContext.Response.Headers.Add("Authorization", auth);

            }
            else
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                _httpContextAccessor.HttpContext.Response.Headers.Add("Content-Length", "0");
                _httpContextAccessor.HttpContext.Response.Body.Flush();
                _httpContextAccessor.HttpContext.Abort();
                return Forbid();
            }
            return Ok($"Hello {HebName} {HebLastName},  Id: {UserIdNum}, Unit: {UnitName}");
        }
    }
}
