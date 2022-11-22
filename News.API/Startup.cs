using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Global.Models.Auth;
using IntegrationEventLogEF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using News.API.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Global.Configs.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Logging;
using Autofac;
using News.API.Infrastructure.AutofacModules;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.OpenApi.Models;
using News.API.Common;
using AutoMapper;
using News.API.Grpc;

namespace News.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            //services.AddControllers();
            var resilientRetryCount = 5;
            if (!string.IsNullOrEmpty(Configuration["ResilientMySqlCount"]))
            {
                resilientRetryCount = int.Parse(Configuration["ResilientMySqlCount"]);
            }
            services.AddAutoMapper(typeof(Startup));
            services.AddGrpc(config =>
            {
                config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
            });

            services.AddDbContext<IntegrationEventLogDbContext>(options =>
            {
                options.UseMySql(connectionString, x =>
                {
                    x.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql));
                    x.EnableRetryOnFailure(resilientRetryCount, TimeSpan.FromSeconds(30), null);
                    x.MigrationsAssembly(typeof(NewsDbContext).GetTypeInfo().Assembly.GetName().Name);
                });
            });

            // Add framework services.
            services.AddDbContext<NewsDbContext>(options =>
            {
                options.UseMySql(connectionString, x => {
                    x.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql));
                    x.EnableRetryOnFailure(resilientRetryCount, TimeSpan.FromSeconds(30), null);
                    x.MigrationsAssembly(typeof(NewsDbContext).GetTypeInfo().Assembly.GetName().Name);
                });

            }, ServiceLifetime.Scoped);

            //services.AddSingleton(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
            //// Replace the default authorization policy provider with our own
            //// custom provider which can return authorization policies for given
            //// policy names (instead of using the default policy provider)
            //services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            //// As always, handlers must be provided for the requirements of the authorization policies
            //services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            // configure jwt authentication
            var identityUrl = Configuration.GetValue<string>("IdentityUrl");
            var tokenOptionSettingsSection = Configuration.GetSection("TokenProvideOptions");
            var tokenOptionSettings = tokenOptionSettingsSection.Get<TokenProvideOptions>();

            services.AddAuthentication(AuthenticationSchemes.CmsApiIdentityKey)
                .AddJwtBearer(AuthenticationSchemes.CmsApiIdentityKey, x =>
                {
                    x.Authority = identityUrl;
                    x.RequireHttpsMetadata = false;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = tokenOptionSettings.SigningCredentials.Key,
                        ValidateIssuer = true,
                        ValidIssuer = tokenOptionSettings.JWTIssuer,
                        ValidateAudience = false
                    };
                    // Read more: https://stackoverflow.com/a/46179977/6344177
                    x.Configuration = new OpenIdConnectConfiguration();
                });

            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = $"{Program.AppName}",
                    Version = "v1",
                    Description = "The User Identity HTTP API"
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.Configure<PasswordOptions>(Configuration.GetSection("PasswordOptions"));

            IdentityModelEventSource.ShowPII = true;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // this will do the initial DB population

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            //loggerFactory.AddSerilog();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", Program.AppName); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<ArticleService>();
            });
        }
        public void ConfigureContainer(ContainerBuilder container)
        {
            //container.AddEventBus(Configuration)
            //    .AddCustomIntegrations(Configuration);

            container.RegisterModule<MediatorModule>();
            container.RegisterModule(new ApplicationModule(Configuration.GetConnectionString("DefaultConnection")));
        }

    }
}
