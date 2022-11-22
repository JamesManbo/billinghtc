using System;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using DebtManagement.API.Application.DomainEventHandlers.ReceiptVoucherHandlers;
using DebtManagement.API.Application.IntegrationEvents;
using DebtManagement.API.Application.IntegrationEvents.EventHandling;
using DebtManagement.API.Application.IntegrationEvents.EventHandling.VoucherTargets;
using DebtManagement.API.Application.IntegrationEvents.Events;
using DebtManagement.API.Application.IntegrationEvents.Events.VoucherTargetEvents;
using DebtManagement.API.Controllers;
using DebtManagement.API.Grpc;
using DebtManagement.API.Grpc.Clients;
using DebtManagement.API.Grpc.Servers;
using DebtManagement.API.Infrastructure.AutofacModules;
using DebtManagement.API.Infrastructure.Middlewares;
using DebtManagement.API.PolicyBasedAuthProvider;
using DebtManagement.Domain.Events.ReceiptVoucherEvents;
using DebtManagement.Infrastructure;
using EventBus;
using EventBus.Abstractions;
using EventBusRabbitMQ;
using Global.Configs.Authentication;
using Global.Models.Auth;
using IntegrationEventLogEF;
using IntegrationEventLogEF.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using RabbitMQ.Client;
using Serilog;

namespace DebtManagement.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ContainerBuilder AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;

            services.AddGrpc(config =>
            {
                config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
            });

            // Add framework services.
            services.AddCustomDbContext(Configuration);
            services.AddAutoMapper(typeof(Startup));

            // configure jwt authentication
            var identityUrl = Configuration.GetValue<string>("IdentityUrl");
            var tokenOptionSettingsSection = Configuration.GetSection("TokenProvideOptions");
            var tokenOptionSettings = tokenOptionSettingsSection.Get<TokenProvideOptions>();

            services.Configure<ConnectionSettings>(
              Configuration.GetSection("ConnectionStrings"));

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

            // Replace the default authorization policy provider with our own
            // custom provider which can return authorization policies for given
            // policy names (instead of using the default policy provider)
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            // As always, handlers must be provided for the requirements of the authorization policies
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            // Microsoft.AspNetCore.Mvc.NewtonsoftJson
            var supportedCultures = new[] { new CultureInfo("vi") };
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("vi");
                options.SupportedCultures = supportedCultures;
            });

            services.AddControllers()
                .AddNewtonsoftJson(
                    opt => opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc
                );

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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddSerilog();

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthorization();

            app.UseResolveUserIdentity(this.AutofacContainer);

            app.UseSwagger();

            app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", Program.AppName);
                });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<ReceiptVoucherGrpcService>();
                endpoints.MapGrpcService<CollectedVoucherGrpcService>();
            });

            ConfigureEventBus(app);
        }

        public void ConfigureContainer(ContainerBuilder container)
        {
            this.AutofacContainer = container;
            container.AddEventBus(Configuration)
                .AddCustomIntegrations(Configuration);

            container.RegisterModule<MediatorModule>();
            container.RegisterModule(new ApplicationModule(Configuration));
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<CreateFirstBillingReceiptIntegrationEvent, CreateFirstBillingReceiptIntegrationEventHandler>();
            eventBus.Subscribe<AddedNewServicePackageIntegrationEvent, AddedNewServicePackageIntegrationEventHandler>();
            eventBus.Subscribe<UpdateTimelineReceiptVoucherIntegrationEvent, UpdateTimelineReceiptVoucherIntegrationEventHandler>();
            eventBus.Subscribe<UpdateContractorIntegrationEvent, UpdateContractorIntegrationEventHandler>();
            eventBus.Subscribe<RestoreOrSuspendServicesIntegrationEvent, RestoreOrSuspendServicesIntegrationEventHandler>();
            //eventBus.Subscribe<NextBillingToReceiptVoucherIntegrationEvent, NextBillingToReceiptVoucherIntegrationEventHandler>();
            //eventBus.Subscribe<ChangeServicePackageIntegrationEvent, ChangeServicePackageIntegrationEventHandler>();
            //eventBus.Subscribe<UpgradeServicePackageIntegrationEvent, UpgradeServicePackageIntegrationEventHandler>();
            eventBus.Subscribe<TerminateServicePackagesIntegrationEvent, TerminateServicePackagesIntegrationEventHandler>();
            eventBus.Subscribe<TransactionIntegrationEvent, TransactionIntergrationEventHandler>();

            eventBus.Subscribe<UpdateContractorPropertyIntegrationEvent, UpdateContractorPropertyIntegrationEventHandler>();
            eventBus.Subscribe<RemoveContractorPropertyIntegrationEvent, RemoveContractorPropertyIntegrationEventHandler>();
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
            containerBuilder.RegisterType<OutContractService>().As<IOutContractService>();

            containerBuilder.Register<Func<DbConnection, IIntegrationEventLogService>>(sp =>
                (DbConnection c) => new IntegrationEventLogService(c));

            containerBuilder.RegisterType<DebtIntegrationEventService>().As<IDebtIntegrationEventService>();

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

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            var resilientRetryCount = 5;
            if (!string.IsNullOrEmpty(configuration["ResilientMySqlCount"]))
            {
                resilientRetryCount = int.Parse(configuration["ResilientMySqlCount"]);
            }

            services.AddDbContext<IntegrationEventLogDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"), x =>
                {
                    x.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql));
                    x.EnableRetryOnFailure(resilientRetryCount, TimeSpan.FromSeconds(30), null);
                    x.MigrationsAssembly(typeof(DebtDbContext).GetTypeInfo().Assembly.GetName().Name);
                });
            });

            services.AddDbContext<DebtDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"), x =>
                {
                    x.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql));
                    x.EnableRetryOnFailure(resilientRetryCount, TimeSpan.FromSeconds(30), null);
                    x.MigrationsAssembly(typeof(DebtDbContext).GetTypeInfo().Assembly.GetName().Name);
                });
                options.EnableSensitiveDataLogging(true);
            });

            return services;
        }
    }
}
