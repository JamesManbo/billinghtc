using System;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using ApplicationUserIdentity.API.Configs;
using ApplicationUserIdentity.API.Infrastructure;
using ApplicationUserIdentity.API.Infrastructure.AutofacModules;
using ApplicationUserIdentity.API.IntegrationEvents.EventHandling;
using ApplicationUserIdentity.API.IntegrationEvents.EventModels;
using ApplicationUserIdentity.API.Models.Configs;
using ApplicationUserIdentity.API.PolicyBasedAuthProvider;
using ApplicationUserIdentity.API.Services.GRPC.Servers;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using EventBus;
using EventBus.Abstractions;
using EventBusRabbitMQ;
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
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using RabbitMQ.Client;

namespace ApplicationUserIdentity.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc(config =>
            {
                config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
            });

            // Add framework services.
            services.AddCustomDbContext(Configuration);
            services.AddAutoMapper(typeof(Startup));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // configure jwt authentication
            var identityUrl = Configuration.GetValue<string>("IdentityUrl");
            var tokenOptionSettingsSection = Configuration.GetSection("TokenProvideOptions");
            var tokenOptionSettings = tokenOptionSettingsSection.Get<TokenProvideOptions>();
            services.Configure<TokenProvideOptions>(tokenOptionSettingsSection);
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

            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // If, for some reason, you need a reference to the built container, you
            // can use the convenience extension method GetAutofacRoot.
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { 
                endpoints.MapControllers();
                endpoints.MapGrpcService<AuthenticationGrpcService>();
                endpoints.MapGrpcService<UserGrpcService>();
                endpoints.MapGrpcService<ApplicationUsersGrpcService>();
                endpoints.MapGrpcService<CustomerGrpcService>();
                endpoints.MapGrpcService<OtpGrpcService>();
            });

            ConfigureEventBus();
        }

        public void ConfigureContainer(Autofac.ContainerBuilder container)
        {
            container.AddEventBus(Configuration)
                .AddCustomIntegrations(Configuration);

            container.RegisterModule<MediatorModule>();
            container.RegisterModule(new ApplicationModule(Configuration["ConnectionString"]));

        }

        private void ConfigureEventBus()
        {
            var eventBus = AutofacContainer.Resolve<IEventBus>();

            eventBus.Subscribe<NewContractorCreatedIntegrationEvent, NewContractorCreatedIntegrationEventHandler>();
            eventBus.Subscribe<UpdateContractorIntegrationEvent, UpdateContractorIntegrationEventHandler>();
            eventBus.Subscribe<InsertBulkApplicationUserFromContractorIntegrationEvent, InsertBulkContractorEventHandler>();
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

            containerBuilder.RegisterType<NewContractorCreatedIntegrationEventHandler>();

            return containerBuilder;
        }


        public static ContainerBuilder AddCustomIntegrations(this ContainerBuilder services,
            IConfiguration configuration)
        {
            services.Register<Func<DbConnection, IIntegrationEventLogService>>(sp =>
                (DbConnection c) => new IntegrationEventLogService(c));

            services.Register<IRabbitMQPersistentConnection>(sp =>
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

            services.AddDbContext<IntegrationEventLogDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"), x =>
                {
                    x.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql));
                    x.EnableRetryOnFailure(resilientRetryCount, TimeSpan.FromSeconds(30), null);
                    x.MigrationsAssembly(typeof(ApplicationUserDbContext).GetTypeInfo().Assembly.GetName().Name);
                });
            });

            services.AddDbContext<ApplicationUserDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"), x => x
                    .ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql))
                    .MigrationsAssembly(typeof(ApplicationUserDbContext).GetTypeInfo().Assembly.GetName().Name));
            }, ServiceLifetime.Scoped);

            return services;
        }
    }
}