using Autofac;
using YH.Etms.Settlement.Api.Application.Queries;
using YH.Etms.Settlement.Api.Controllers;
using YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate;
using YH.Etms.Settlement.Api.Infrastructure.Idempotency;
using YH.Etms.Settlement.Api.Infrastructure.Repositories;
using YH.Etms.Utility.Tools.FluentQuery;

namespace YH.Etms.Settlement.Api.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        public string QueriesConnectionString { get; }

        public ApplicationModule(string qconstr)
        {
            QueriesConnectionString = qconstr;
        }

        protected override void Load(ContainerBuilder builder)
        {


            builder.RegisterType<RequestManager>()
                .As<IRequestManager>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CodeController>()
                   .As<ICodeService>()
                   .InstancePerLifetimeScope();

            builder.Register(c => new FluentQuery(QueriesConnectionString))
                   .As<IFluentQuery>()
                   .InstancePerLifetimeScope();

            builder.Register(d => new PayableQuery(QueriesConnectionString))
                .As<IPayableQuery>()
                .InstancePerLifetimeScope();
            builder.RegisterType<PayableRepository>()
                   .As<IPayableRepository>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<TransportTaskRepository>()
                .As<ITransportTaskRepository>()
                .InstancePerLifetimeScope();

        }
    }
}
