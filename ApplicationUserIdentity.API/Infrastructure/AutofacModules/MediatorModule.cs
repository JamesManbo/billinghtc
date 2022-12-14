using System.Reflection;
using ApplicationUserIdentity.API.Application.Behaviors;
using ApplicationUserIdentity.API.Application.Commands.UserGroup;
using ApplicationUserIdentity.API.Application.DomainEventHandlers.Contractor;
using Autofac;
using FluentValidation;
using MediatR;

namespace ApplicationUserIdentity.API.Infrastructure.AutofacModules
{ 
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            // Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
            builder.RegisterAssemblyTypes(typeof(CUUserGroupCommandHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            // Register the DomainEventHandler classes (they implement INotificationHandler<>) in assembly holding the Domain Events
            builder.RegisterAssemblyTypes(typeof(UpdateContractorPropertyDomainEventHandler).GetTypeInfo().Assembly)
               .AsClosedTypesOf(typeof(INotificationHandler<>));

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

            builder
               .RegisterAssemblyTypes(typeof(CreateUserGroupValidator).GetTypeInfo().Assembly)
               .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
               .AsImplementedInterfaces();

            //builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(TransactionBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}
