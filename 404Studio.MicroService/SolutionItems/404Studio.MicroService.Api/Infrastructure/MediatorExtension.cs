using System.Linq;
using System.Threading.Tasks;
using MediatR;
using YH.Etms.Settlement.Api.Domain.Entities;

namespace YH.Etms.Settlement.Api.Infrastructure
{
    static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, SettlementContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            if (domainEvents.Count == 0)
                return;

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.DomainEvents.Clear());


            //var tasks = domainEvents
            //    .Select(async (domainEvent) =>
            //    {
            //        await mediator.Publish(domainEvent);
            //    });
            domainEvents.ForEach(domainEvent =>
            {
                mediator.Publish(domainEvent).ConfigureAwait(true).GetAwaiter().GetResult();
            });
            //await Task.WhenAll(tasks);
            await Task.CompletedTask;
        }
    }
}
