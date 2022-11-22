using Autofac;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Infrastructure.Middlewares
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseResolveUserIdentity(
            this IApplicationBuilder app, ContainerBuilder builder)
        {
            return app.UseMiddleware<ResolveUserIdentityMiddleware>(builder);
        }
    }
}
