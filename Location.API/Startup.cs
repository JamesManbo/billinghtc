using Autofac;
using AutoMapper;
using Location.API.AutofacModules;

using Location.API.Grpc;
using Location.API.Infrastructure.AutofacModules;
using Location.API.Location;
using Location.API.Reponsitory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Location.API
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
            services.Configure<LocationMongoDbSettings>( Configuration.GetSection(nameof(LocationMongoDbSettings)));
            services.AddGrpc(config =>
            {
                config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
            });

            services.AddSingleton<ILocationkMongoDbContext, LocationMongoDbContext>();
            services.AddSingleton<ISupportLocationQueries, SupportLocationQueries>();
            services.AddSingleton<ILocationQueries, LocationQueries>();

            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<LocationService>();
                endpoints.MapGrpcService<SupportLocationService>();
            });
        }

        public void ConfigureContainer(ContainerBuilder container)
        {
            container.RegisterModule<MediatorModule>();
            container.RegisterModule(new ApplicationModule(Configuration["ConnectionString"]));
        }
    }
}
