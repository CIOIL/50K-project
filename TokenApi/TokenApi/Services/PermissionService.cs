using Encryption.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TokenApi.Common.DTO;
using TokenApi.Entities;
using TokenApi.Services;

namespace TokenApi
{
    public class PermissionService : IPermissionService
    {
        public IConfiguration Configuration { get; }
        private readonly ILogger<PermissionService> _logger;
        private readonly IAuthService _authService;

        public PermissionService(IConfiguration configuration, ILogger<PermissionService> logger, IAuthService authService)
        {
            Configuration = configuration;
            _logger = logger;
            _authService = authService;
        }
        async Task<string> IPermissionService.Authenticate(string userID, string key, string encryptionKey)
        {

            var Perm = new List<UserRole>();
            Perm.Add(new UserRole() { Id = 901, RoleId = 2, UnitId = 340, UserId = 1257 });
            Perm.Add(new UserRole() { Id = 1136, RoleId = 3, UnitId = 340, UserId = 1257 });
            if (Perm.FindAll(i => i.RoleId == 2).Count > 1)
            {
                var TLRole = Perm.FindAll(i => i.RoleId == 2);
                var remove = TLRole.Max(i => i.UnitId);
                Perm.Remove(Perm.Find(i => i.UnitId == remove && i.RoleId == 2));
            }

            var Roles = new List<Role>();
            Roles.Add(new Role() { Id = 2, Name = "MG" });
            Roles.Add(new Role() { Id = 3, Name = "OM" });

            int id = 1257;
            string unit = "ממשל זמין";
            List<ActualRole> actualRoles = new List<ActualRole>();

            foreach (var userRole in Perm)
            {
                ActualRole ar = new ActualRole();
                ar.roleName = Roles.Where(i => i.Id == userRole.RoleId).FirstOrDefault().Name;
                ar.unitId = userRole.UnitId;
                actualRoles.Add(ar);
            }

            List<Claim> li = new List<Claim>();
            string ts2 = JsonConvert.SerializeObject(actualRoles);
            string ids = JsonConvert.SerializeObject(id);

            li.Add(new Claim("UnitName", unit));

            li.Add(new Claim("Permissions2", ts2));
            li.Add(new Claim("HebName", "אנצו"));
            li.Add(new Claim("HebLastName", "פרארי"));

            var appSettingsSection = Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            var ticketExpiration = 5;
            return await Task.Run(() => Authenticate(userID, key, encryptionKey, li, ticketExpiration));
        }

        public string Authenticate(string userID, string secret, string encryptionKey, List<Claim> listToken, int ticketExpiration)
        {
            return _authService.Authenticate(userID, listToken, ticketExpiration, "ta");
        }
        public bool Verify(string authToken, string secret, string userID)
        {
            throw new NotImplementedException();
        }
    }
}
