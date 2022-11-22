using System;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using Autofac;
using AutoMapper;
using EventBus;
using EventBus.Abstractions;
using EventBusRabbitMQ;
using FluentValidation.AspNetCore;
using Global.Configs.Authentication;
using IntegrationEventLogEF;
using IntegrationEventLogEF.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using OrganizationUnit.API.Application.IntegrationEvents;
using OrganizationUnit.API.Application.IntegrationEvents.EventHandling;
using OrganizationUnit.API.Application.IntegrationEvents.Events.ApplicationUserEvents;
using OrganizationUnit.API.Grpc.Servers;
using OrganizationUnit.API.Infrastructure.AutofacModules;
using OrganizationUnit.API.Infrastructure.Helpers.PasswordVerification;
using OrganizationUnit.API.PolicyBasedAuthProvider;
using OrganizationUnit.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using RabbitMQ.Client;
using Serilog;

namespace OrganizationUnit.API
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

            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            var resilientRetryCount = 5;
            if (!string.IsNullOrEmpty(Configuration["ResilientMySqlCount"]))
            {
                resilientRetryCount = int.Parse(Configuration["ResilientMySqlCount"]);
            }

            // Add framework services.
            services.AddDbContext<OrganizationUnitDbContext>(options =>
            {
                options.UseMySql(connectionString, x => { 
                    x.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql));
                    x.EnableRetryOnFailure(resilientRetryCount, TimeSpan.FromSeconds(30), null);
                    x.MigrationsAssembly(typeof(OrganizationUnitDbContext).GetTypeInfo().Assembly.GetName().Name);
                });
                
            }, ServiceLifetime.Scoped);
            services.AddDbContext<IntegrationEventLogDbContext>(options =>
            {
                options.UseMySql(connectionString, x =>
                {
                    x.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql));
                    x.EnableRetryOnFailure(resilientRetryCount, TimeSpan.FromSeconds(30), null);
                    x.MigrationsAssembly(typeof(OrganizationUnitDbContext).GetTypeInfo().Assembly.GetName().Name);
                });
            });


            services.AddSingleton(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
            // Replace the default authorization policy provider with our own
            // custom provider which can return authorization policies for given
            // policy names (instead of using the default policy provider)
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            // As always, handlers must be provided for the requirements of the authorization policies
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddGrpc(config =>
            {
                config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
            });

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

            services.AddAutoMapper(typeof(Startup));

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

            loggerFactory.AddSerilog();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", Program.AppName); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<UsersService>();
                endpoints.MapGrpcService<OrganizationUnitsService>();
                endpoints.MapGrpcService<ConfigurationSystemParameterService>();
            });

            ConfigureEventBus(app);
        }
        public void ConfigureContainer(ContainerBuilder container)
        {
            container.AddEventBus(Configuration)
                .AddCustomIntegrations(Configuration);

            container.RegisterModule<MediatorModule>();
            container.RegisterModule(new ApplicationModule(Configuration.GetConnectionString("DefaultConnection")));
        }

        public void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<UpdateContractorIntegrationEvent, UpdateContractorIntegrationEventHandler>();
        }
    }
    public static class CustomExtensionMethods
    {
        public static ContainerBuilder AddEventBus(this ContainerBuilder containerBuilder, IConfiguration configuration)
        {
            var subscriptionClientName = configuration["SubscriptionClientName"];

            containerBuilder.Register(sp =>
            {
                var rabbitMqPersistentConnection = sp.Resolve<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.Resolve<ILifetimeScope>();
                var logger = sp.Resolve<ILogger<EventBusRabbitMQ.EventBusRabbitMQ>>();
                var eventBusSubscriptionsManager = sp.Resolve<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBusRetryCount"]);
                }

                return new EventBusRabbitMQ.EventBusRabbitMQ(rabbitMqPersistentConnection,
                    logger, iLifetimeScope, eventBusSubscriptionsManager, subscriptionClientName, retryCount);
            }).As<IEventBus>().SingleInstance();

            containerBuilder.RegisterType<InMemoryEventBusSubscriptionsManager>().As<IEventBusSubscriptionsManager>()
                .SingleInstance();
            return containerBuilder;
        }

        public static ContainerBuilder AddCustomIntegrations(this ContainerBuilder containerBuilder,
            IConfiguration configuration)
        {
            containerBuilder.Register<Func<DbConnection, IIntegrationEventLogService>>(sp =>
                (DbConnection c) => new IntegrationEventLogService(c));

            containerBuilder.RegisterType<UserIntegrationEventService>().As<IUserIntegrationEventService>();

            containerBuilder.Register<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.Resolve<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = configuration["EventBusConnection"],
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
                {
                    factory.UserName = configuration["EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
                {
                    factory.Password = configuration["EventBusPassword"];
                }

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBusRetryCount"]);
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            }).SingleInstance();

            return containerBuilder;
        }
    }
}
