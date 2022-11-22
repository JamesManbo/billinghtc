using AutoMapper;
using Global.Configs.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using CustomerApp.APIGateway.Configs;
using CustomerApp.APIGateway.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Newtonsoft.Json;

namespace CustomerApp.APIGateway
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
                    Description = "Customer API Gateway"
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

            services.AddApplicationServices();
            services.AddAutoMapper(typeof(Startup));
            services.AddOcelot(Configuration);
            services.AddCustomAuthentication(Configuration);
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
            app.UseSwagger()
                .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", Program.AppName); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });
            app.UseOcelot();
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services,
            IConfiguration configuration)
        {
            //var identityUrl = configuration.GetValue<string>("IdentityUrl");
            //var authenticationProviderKey = "SystemUserIdentityApiKey";
            //services.AddAuthentication()
            //    .AddJwtBearer(authenticationProviderKey, x =>
            //    {
            //        x.Authority = identityUrl;
            //        x.RequireHttpsMetadata = false;
            //        x.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidAudiences = new[] { "contract" }
            //        };
            //    });

            // configure jwt authentication
            var tokenOptionSettingsSection = configuration.GetSection("TokenProvideOptions");
            var tokenOptionSettings = tokenOptionSettingsSection.Get<TokenProvideOption>();
            services.AddAuthentication(AuthenticationSchemes.CmsApiIdentityKey)
                .AddJwtBearer(AuthenticationSchemes.CmsApiIdentityKey, x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = tokenOptionSettings.SigningCredentials.Key,
                        ValidateIssuer = true,
                        ValidIssuer = tokenOptionSettings.JWTIssuer,
                        ValidateAudience = false
                    };
                });

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //register delegating handlers
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //register http services
            services.AddTransient<ITelService, TelService>();
            services.AddTransient<IContractService, ContractService>();
            services.AddTransient<IPackageService, PackageService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IArticlesService, ArticlesService>();
            services.AddTransient<ISupportService, SupportService>();
            services.AddTransient<IReceiptVoucherService, ReceiptVoucherService>();
            services.AddTransient<ITransactionsService, TransactionsService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<ISupportLocationService, SupportLocationService>();
            services.AddTransient<IContractorService, ContractorService>();
            services.AddTransient<IOtpService, OtpService>();
            services.AddTransient<IApplicationUsersService, ApplicationUsersService>();

            return services;
        }
    }
}
