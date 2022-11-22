using Autofac;
using AutoMapper;
using Feedback.API.Grpc;
using Feedback.API.Grpc.Client;
using Feedback.API.Models;
using Feedback.API.PolicyBasedAuthProvider;
using Feedback.API.Queries;
using Feedback.API.Repository;
using Global.Configs.Authentication;
using Global.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net.Http;

namespace Feedback.API
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
            services.Configure<FeedbackMongoDbSettings>(
              Configuration.GetSection(nameof(FeedbackMongoDbSettings)));

            services.AddGrpc(config =>
            {
                config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
            });

            services.AddCustomDbContext(Configuration);
            services.AddAutoMapper(typeof(Startup));

            services.AddHttpClient("htcticket", c =>
            {
                c.BaseAddress = new Uri("https://inc.htc-itc.vn");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                }
            });

            services.AddSingleton<IFeedbackMongoDbContext, FeedbackMongoDbContext>();

            services.AddTransient<INotificationGrpcService, NotificationServiceGrpc>();

            services.AddTransient<IOutContractServiceGrpc, OutContractServiceGrpc>();

            services.AddTransient<IFeedbackAndRequestQueries, FeedbackAndRequestQueries>();

            services.AddTransient<IFeedbackAndRequestRepository, FeedbackAndRequestRepository>();

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

            // Replace the default authorization policy provider with our own
            // custom provider which can return authorization policies for given
            // policy names (instead of using the default policy provider)
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            // As always, handlers must be provided for the requirements of the authorization policies
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            //app.UseSwagger();

            //app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", Program.AppName); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<FeedbackAndRequestServiceGrpc>();
            });
        }


    }
    public static class CustomExtensionMethods
    {
        public static ContainerBuilder AddEventBus(this ContainerBuilder containerBuilder, IConfiguration configuration)
        {
            var subscriptionClientName = configuration["SubscriptionClientName"];

            return containerBuilder;
        }

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services,
           IConfiguration configuration)
        {
            return services;
        }
    }
}
