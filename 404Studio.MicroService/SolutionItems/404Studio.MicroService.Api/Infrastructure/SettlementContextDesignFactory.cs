using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using YH.Etms.Settlement.Api.Infrastructure.Configuration;

namespace YH.Etms.Settlement.Api.Infrastructure
{
    /// <summary>
    /// 此类只在执行EF 命令时有效
    /// </summary>
    public class SettlementContextDesignFactory : IDesignTimeDbContextFactory<SettlementContext>
    {
        public SettlementContext CreateDbContext(string[] args)
        {
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(),
                addUserSecrets: true);
            var builder = new DbContextOptionsBuilder<SettlementContext>()
                .UseMySql(configuration.GetConnectionString("Master"));
            return new SettlementContext(builder.Options, new NullMediator());
        }

        class NullMediator : IMediator
        {
            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
            {
                return Task.CompletedTask;
            }

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<TResponse>(default(TResponse));
            }

            public Task Send(IRequest request, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.CompletedTask;
            }
        }
    }
}
