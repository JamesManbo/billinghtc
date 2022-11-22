using Autofac;
using GenericRepository.Setups;
using System.Reflection;

namespace Location.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        public string QueriesConnectionString { get; }

        public ApplicationModule(string queriesConnectionString)
        {
            QueriesConnectionString = queriesConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.GenericRepositorySetupByAutofac(typeof(ApplicationModule).Assembly);

            //Repository

            //GRPC

            //MongoDb

            //Radius management repos
        }
    }
}