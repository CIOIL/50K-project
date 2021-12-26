using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenApi.Services
{
    public interface IPermissionService
    {
        Task<string> Authenticate(string userID, string key, string encryptionKey);
        bool Verify(string authToken, string secret, string userID);
    }
}
