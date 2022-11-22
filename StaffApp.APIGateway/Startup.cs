using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Global.Configs.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using StaffApp.APIGateway.Configs;
using StaffApp.APIGateway.Services;
using System.Globalization;
using System.IO;

namespace StaffApp.APIGateway
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
                    Description = "Staff App API Gateway"
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
            services.AddAuthentication()
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

            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ITelcoService, TelcoService>();
            services.AddTransient<IPackageService, PackageService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IEquipmentTypeService, EquipmentTypeService>();
            services.AddTransient<IReceiptVoucherService, ReceiptVoucherService>();
            services.AddTransient<IOutContractService, OutContractService>();
            services.AddTransient<ISupportService, SupportService>();
            services.AddTransient<IContractorService, ContractorService>();
            services.AddTransient<IArticlesService, ArticlesService>();
            services.AddTransient<ITransactionsService, TransactionsService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IOrganizationUnitsService, OrganizationUnitsService>();
            services.AddTransient<IApplicationUsersService, ApplicationUsersService>();
            services.AddTransient<ITaxCategoryService, TaxCategoryService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ISupportLocationService, SupportLocationService>();
            services.AddTransient<IMarketAreaService, MarketAreaService>();
            services.AddTransient<IAcceptanceService, AcceptanceService>();
            services.AddTransient<IUnitOfMeasurementService, UnitOfMeasurementService>();
            services.AddTransient<IPromotionService, PromotionService>();
            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<ITransactionSupporterService, TransactionSupporterService>();
            services.AddTransient<IOtpService, OtpService>();
            services.AddTransient<IContractFormService, ContractFormService>();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            return services;
        }
    }
}
