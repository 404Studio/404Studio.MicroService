using System.Collections.Generic;
using System.Reflection;
using Autofac;
using FluentValidation;
using MediatR;
using YH.Etms.Settlement.Api.Application.Behaviors;
using YH.Etms.Settlement.Api.Application.Commands;
using YH.Etms.Settlement.Api.Application.Commands.Payable.Init;
using YH.Etms.Settlement.Api.Application.Validations;
using YH.Etms.Settlement.Api.Application.Validations.Payable;

namespace YH.Etms.Settlement.Api.Infrastructure.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            // 注册所有 实现 IRequestHandler<,>接口的 CommandHandler
            builder.RegisterAssemblyTypes(typeof(InitPayableCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            // 注册所有 实现 INotificationHandler<,>接口的 CommandHandler
            builder.RegisterAssemblyTypes(typeof(InitPayableCommandHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));

            // 注册所有 实现 IValidator<>接口的
            builder.RegisterAssemblyTypes(typeof(CreatePayableCommandValidatorcs).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            builder.Register<SingleInstanceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

            builder.Register<MultiInstanceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();

                return t =>
                {
                    var resolved = (IEnumerable<object>)componentContext.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
                    return resolved;
                };
            });

            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}
