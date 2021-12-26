using Encryption.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenApi;
using TokenApi.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection RegisterPermissionServices(this IServiceCollection services)
        {
            return services.AddScoped<IPermissionService, PermissionService>();
        }
        public static IServiceCollection RegisterEncryptionServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthorizationService>();
            return services;
        }
    }
}
