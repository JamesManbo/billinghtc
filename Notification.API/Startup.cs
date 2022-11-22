using AutoMapper;
using FirebaseAdmin;
using Global.Configs.Authentication;
using Global.Models.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Notification.API.Grpc;
using Notification.API.Grpc.Client;
using Notification.API.Repositories;
using Notification.API.Services;
using System;
using System.Net.Http;

namespace Notification.API
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

            services.AddGrpc(config =>
            {
                config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
            });

            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));

            services.AddHttpClient("fcmapi", c =>
            {
                c.BaseAddress = new Uri(@"https://fcm.googleapis.com");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"key={Configuration.GetValue<string>("FCMServerKey")}");
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                }
            });

            services.Configure<MongoDbSettings>(Configuration.GetSection("DatabaseConnections"));
            services.Configure<SMSOptions>(Configuration.GetSection("SMSOptions"));
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));
            services.AddSingleton<INotificationDbContext, NotificationDbContext>();
            services.AddSingleton<IPushNotification, PushNotification>();
            services.AddSingleton<ISendSMS, SendSMS>();
            services.AddSingleton<ISendMail, SendMail>();
            services.AddSingleton<IFcmService, FcmService>();

            services.AddTransient<IUserGrpcService, UserGrpcService>();
            services.AddTransient<IApplicationUserGrpcService, ApplicationUserGrpcService>();

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

            services.AddSingleton<INotificationRepository, NotificationRepository>();
            services.AddSingleton<ITopicRepository, TopicRepository>();

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("assets/billing-staff-adminsdk.json")
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<NotificationGrpcService>();
            });

        }
    }
}
