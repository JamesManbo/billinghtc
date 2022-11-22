using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
//using News.API.Infrastructure.Queries;
//using ApplicationUserIdentity.API.Infrastructure.Repositories;
using News.API.IntegrationEvents.EventHandling;
using Autofac;
using EventBus.Abstractions;
using GenericRepository.Setups;
using Microsoft.Extensions.DependencyInjection;
using News.API.Infrastructure.Queries;
using News.API.Infrastructure.Repositories.ArticleRepository;
using News.API.Infrastructure.Repositories;
using News.API.Services.StaticResource;
using News.API.Infrastructure.Repositories.ArticleCategoryRepository;
using News.API.Infrastructure.Repositories.ArticleArticleCategoryRepository;
using News.API.Infrastructure.Repositories.FileAttachmentRepository;

namespace News.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        public string ConnectionString { get; set; }

        public ApplicationModule(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.GenericRepositorySetupByAutofac(typeof(ApplicationModule).Assembly);
            builder.RegisterRepository();
            //builder.RegisterAssemblyTypes(typeof(NewContractorCreatedIntegrationEventHandler).GetTypeInfo().Assembly)
            //    .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }
    }
    public static class CustomApplicationModuleExtension
    {
        public static void RegisterRepository(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ArticlesQueries>()
                .As<IArticlesQueries>()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<ArticleRepository>()
                .As<IArticleRepository>()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<PictureRepository>()
                .As<IPictureRepository>()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<PictureQueries>()
                .As<IPictureQueries>()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<StaticResourceService>()
                .As<IStaticResourceService>()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<ArticleCategoryQueries>()
                .As<IArticleCategoryQueries>()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<ArticleCategoryRepository>()
                .As<IArticleCategoryRepository>()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<ArticleArticleCategoryRepository>()
                .As<IArticleArticleCategoryRepository>()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<FileAttachmentRepository>()
                .As<IFileAttachmentRepository>()
                .InstancePerLifetimeScope();
        }
    }
}