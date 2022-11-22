using Autofac;
using Global.Configs.Authentication;
using Global.Models.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DebtManagement.API.Infrastructure.Middlewares
{
    public class ResolveUserIdentityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ContainerBuilder _builder;
        public ResolveUserIdentityMiddleware(RequestDelegate next, ContainerBuilder builder)
        {
            this._next = next;
            this._builder = builder;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            this._builder.RegisterInstance(this.ReadFromContext(context)).As<UserIdentity>();
            await this._next(context);
        }

        public UserIdentity ReadFromContext(HttpContext context)
        {
            return new UserIdentity(context);
        }
    }
}
