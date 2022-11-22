using System;
using System.Reflection;
using SystemUserIdentity.API.Infrastructure.Helpers.PasswordVerification;
using SystemUserIdentity.API.Infrastructure.Services;
using GenericRepository.Setups;
using Global.Configs.Authentication;
using Global.Models.Auth;
using IntegrationEventLogEF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Infrastructure;
using OrganizationUnit.Infrastructure.Queries;
using OrganizationUnit.Infrastructure.Repositories;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SystemUserIdentity.API.Grpc;
using AutoMapper;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Newtonsoft.Json;
using OrganizationUnit.Infrastructure.Repositories.FCMRepository;
using SystemUserIdentity.API.Grpc.ServerServices;
using SystemUserIdentity.API.Grpc.ClientServices;
using SystemUserIdentity.API.Grpc.Clients;
using OrganizationUnit.Infrastructure.Repositories.OtpRepository;

namespace SystemUserIdentity.API
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
            var supportedCultures = new[] { new CultureInfo("vi") };
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("vi");
                options.SupportedCultures = supportedCultures;
            });

            services.AddGrpc(config =>
            {
                config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
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
            // Add framework services.
            services.AddCustomDbContext(Configuration);

            services.AddAutoMapper(typeof(Startup));

            services.GenericRepositorySetup(typeof(Startup).Assembly);

            services.Configure<PasswordOptions>(Configuration.GetSection("PasswordOptions"));
            services.Configure<TokenProvideOptions>(Configuration.GetSection("TokenProvideOptions"));

            services.AddCustomServices();

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

            // Microsoft.AspNetCore.Mvc.NewtonsoftJson            
            services.AddControllers()
                .AddNewtonsoftJson(
                    opt => opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<AuthenticationGrpcService>();
                endpoints.MapGrpcService<OtpService>();
            });
        }
    }
    public static class ServiceCollectionExtenstion
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IOrganizationUnitQueryRepository, OrganizationUnitQueryRepository>();
            services.AddTransient<IUserQueries, UserQueries>();
            services.AddTransient<IRoleQueries, RoleQueries>();
            services.AddTransient<IFCMTokenRepository, FCMTokenRepository>();
            services.AddTransient<IFCMTokenQueries, FCMTokenQueries>();
            services.AddTransient<IOtpRepository, OtpRepository>();
            services.AddTransient<IOtpQueries, OtpQueries>();

            services.AddTransient<IAuthenticationService<User>, EFAuthenticationService>();

            services.AddTransient<IProjectGrpcService, ProjectGrpcService>();
            services.AddTransient<INotificationGrpcService, NotificationGrpcService>();
            return services;
        }

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services,
            IConfiguration configuration)
        {

            var resilientRetryCount = 5;
            if (!string.IsNullOrEmpty(configuration["ResilientMySqlCount"]))
            {
                resilientRetryCount = int.Parse(configuration["ResilientMySqlCount"]);
            }

            services.AddDbContext<OrganizationUnitDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"), x =>
                {
                    x.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql));
                    x.EnableRetryOnFailure(resilientRetryCount, TimeSpan.FromSeconds(30), null);
                    x.MigrationsAssembly(typeof(OrganizationUnitDbContext).GetTypeInfo().Assembly.GetName().Name);
                });
            });

            services.AddDbContext<IntegrationEventLogDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"), x =>
                {
                    x.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql));
                    x.EnableRetryOnFailure(resilientRetryCount, TimeSpan.FromSeconds(30), null);
                    x.MigrationsAssembly(typeof(OrganizationUnitDbContext).GetTypeInfo().Assembly.GetName().Name);
                });
            });

            return services;
        }
    }
}
