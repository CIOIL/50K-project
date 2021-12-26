using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Encryption.Authorization;
using TokenApi.Common.DTO;

namespace TokenApi.Controllers
{    
    public class TControllerBase : ControllerBase
    {
        public readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IAuthService _authorizationService;


        public TControllerBase(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ILogger logger, IMapper mapper, IAuthService authorizationService)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        protected IHttpContextAccessor HttpContextAccessor => _httpContextAccessor;
        protected IConfiguration Configuration => _configuration;
        protected ILogger Logger => _logger;
        protected IMapper Mapper => _mapper;
        protected List<ActualRole> Permissions2
        {

            get
            {
                List<ActualRole> result = new List<ActualRole>();               
                var per =  _authorizationService.GetPropertyFromToken(nameof(Permissions2));                 
                result = JsonConvert.DeserializeObject<List<ActualRole>>(per);
                return result;
            }



        }
        protected string UnitName
        {
            get
            {             
                return _authorizationService.GetPropertyFromToken(nameof(UnitName));
            }
        }

        protected string SupplierId
        {
            get
            {               
                return _authorizationService.GetPropertyFromToken(nameof(SupplierId));
            }
        }

        protected string IsMain
        {
            get
            {               
                return _authorizationService.GetPropertyFromToken(nameof(IsMain));
            }
        }

        protected string AgentID
        {
            get
            {
                return _authorizationService.GetPropertyFromToken(nameof(AgentID));
            }
        }


        protected List<int> Offices
        {
            get
            {
                List<int> result = new List<int>();
                var offices = _authorizationService.GetPropertyFromToken(nameof(Offices));
                result = JsonConvert.DeserializeObject<List<int>>(offices);             
                return result;
            }
        }

        protected JwtSecurityToken ValidateToken(string tokenStr)
        {


            _logger.LogInformation("Validate Token!!!!!!");        
            return (JwtSecurityToken)_authorizationService.GetJwtSecurityToken(tokenStr);


        }       
        protected string UserIdNum
        {
            get
            {
                try
                {
                    string idNum = _httpContextAccessor.HttpContext.Request.Headers["IDNum"];

                    if (idNum.Length==9)
                    {
                        _logger.LogInformation($"IDNum is log in  {idNum}");

                        return idNum; 
                    }
                    else
                    {
                        throw new Exception("length not valid");
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }
                return "Headers[IDNum] not found";
            }
        }
        protected string HebName
        {
            get
            {
                return _authorizationService.GetPropertyFromToken(nameof(HebName));
            }
        }

        protected string HebLastName
        {
            get
            {
                return _authorizationService.GetPropertyFromToken(nameof(HebLastName));
            }
        }


        protected string CardType
        {
            get
            {
                return _authorizationService.GetPropertyFromToken(nameof(CardType));
            }
        }
    }
}
