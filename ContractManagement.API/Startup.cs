using System;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using Autofac;
using AutoMapper;
using ContractManagement.API.Application.IntegrationEvents;
using ContractManagement.API.Grpc.Servers;
using ContractManagement.API.Infrastructure.AutofacModules;
using ContractManagement.API.PolicyBasedAuthProvider;
using ContractManagement.Infrastructure;
using ContractManagement.Infrastructure.Repositories.ContractHistoryRepository;
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
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using RabbitMQ.Client;
using Serilog;
using Newtonsoft.Json;
using ContractManagement.API.Application.IntegrationEvents.EventHandling;
using ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents;
using ContractManagement.API.Application.IntegrationEvents.Events.ApplicationUserEvents;
using ContractManagement.RadiusDomain.Models;
using ContractManagement.API.Application.IntegrationEvents.Events;
using ContractManagement.API.Application.IntegrationEvents.EventHandling.DebtEventHandlers;
using ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents.Payments;
using ContractManagement.API.Application.IntegrationEvents.EventHandling.DebtEventHandlers.Payments;
using ContractManagement.API.Application.IntegrationEvents.EventHandling.ContractorEventHandlers;
using ContractManagement.API.Application.IntegrationEvents.Events.ContractorEvents;
using ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents.Receipts;

namespace ContractManagement.API
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
            IdentityModelEventSource.ShowPII = true;

            var supportedCultures = new[] { new CultureInfo("vi") };
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("vi");
                options.SupportedCultures = supportedCultures;
            });

            services.AddControllers()
                .AddNewtonsoftJson(
                    opt =>
                    {
                        opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                        opt.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ";
                    }
                );

            services.AddGrpc(config =>
            {
                config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
            });

            // Add framework services.
            services.AddCustomDbContext(Configuration);
            services.AddAutoMapper(typeof(Startup));

            services.Configure<ContractMongoDbSettings>(
              Configuration.GetSection("ContractHistoryMongoDb"));
            services.AddSingleton<IContractMongoDbContext, ContractMongoDbContext>();
            services.Configure<TikConnectionSettings>(
              Configuration.GetSection("TikConnectionSettings"));
            services.AddSingleton<IContractMongoDbContext, ContractMongoDbContext>();

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
                        ValidIssuers = tokenOptionSettings.JWTIssuers,
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

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", Program.AppName); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<OutContractService>();
                endpoints.MapGrpcService<TelcoService>();
                endpoints.MapGrpcService<PackageService>();
                endpoints.MapGrpcService<ProjectService>();
                endpoints.MapGrpcService<MarketAreaService>();
                endpoints.MapGrpcService<EquipmentTypeService>();
                endpoints.MapGrpcService<ContractorService>();
                endpoints.MapGrpcService<TransactionsService>();
                endpoints.MapGrpcService<TaxCategoryService>();
                endpoints.MapGrpcService<AcceptanceService>();
                endpoints.MapGrpcService<UnitOfMeasurementService>();
                endpoints.MapGrpcService<PromotionService>();
                endpoints.MapGrpcService<TransactionSupporterService>();
                endpoints.MapGrpcService<ContractFormService>();
                endpoints.MapGrpcService<ExchangeRateGrpcService>();
            });

            ConfigureEventBus(app);
        }

        public void ConfigureContainer(ContainerBuilder container)
        {
            container.AddEventBus(Configuration)
                .AddCustomIntegrations(Configuration);

            container.RegisterModule<MediatorModule>();
            container.RegisterModule(new ApplicationModule(Configuration));
        }

        public void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<PaidRealtimeReceiptVoucherIntegrationEvent, PaidRealtimeRcptVchrIntegrationEventHandler>();
            eventBus.Subscribe<UpdateContractorIntegrationEvent, UpdateContractorIntegrationEventHandler>();
            eventBus.Subscribe<UpdateServicePackageSuspensionTimesIntegrationEvent, UpdateServicePackageSuspensionTimesIntegrationEventHandler>();
            eventBus.Subscribe<BillingPaymentSuccessIntegrationEvent, BillingPaymentSuccessIntegrationEventHandler>();
            eventBus.Subscribe<UpdateOutContractServicePackageClearingIntegrationEvent, UpdateOutContractServicePackageClearingIntegrationEventHandler>();
            eventBus.Subscribe<BillingPaymentCanceledIntegrationEvent, BillingPaymentCanceledIntegrationEventHandler>();
            eventBus.Subscribe<BillingPaymentPendingIntegrationEvent, BillingPaymentPendingIntegrationEventHandler>();

            eventBus.Subscribe<PaymentVoucherCanceledIntegrationEvent, PaymentVoucherCanceledIntegrationEventHandler>();
            eventBus.Subscribe<PaymentVoucherCreatedIntegrationEvent, PaymentVoucherCreatedIntegrationEventHandler>();
            eventBus.Subscribe<PaymentVoucherSuccessIntegrationEvent, PaymentVoucherSuccessIntegrationEventHandler>();

            eventBus.Subscribe<UpdateContractorPropertyIntegrationEvent, UpdateContractorPropertyIntegrationEventHandler>();
            eventBus.Subscribe<RemoveContractorPropertyIntegrationEvent, RemoveContractorPropertyIntegrationEventHandler>();
            eventBus.Subscribe<VoucherBillingDateChangeIntegrationEvent, VoucherBillingDateChangeIntegrationEventHandler>();
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

            containerBuilder.RegisterType<ContractIntegrationEventService>().As<IContractIntegrationEventService>();

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
                    x.MigrationsAssembly(typeof(ContractDbContext).GetTypeInfo().Assembly.GetName().Name);
                });
            });

            services.AddDbContext<ContractDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"), x =>
                {
                    x.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql));
                    x.EnableRetryOnFailure(resilientRetryCount, TimeSpan.FromSeconds(30), null);
                    x.MigrationsAssembly(typeof(ContractDbContext).GetTypeInfo().Assembly.GetName().Name);
                });
            });

            services.AddDbContext<RadiusContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("RadiusDbConnection"));
            });

            return services;
        }
    }
}