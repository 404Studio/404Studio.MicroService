using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate;
using YH.Etms.Settlement.Api.Domain.Entities;
using YH.Etms.Settlement.Api.Infrastructure.EntityConfigurations;

namespace YH.Etms.Settlement.Api.Infrastructure
{
    public class SettlementContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        private SettlementContext(DbContextOptions<SettlementContext> options) : base(options)
        {

        }

        public SettlementContext(DbContextOptions<SettlementContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            System.Diagnostics.Debug.WriteLine("SettlementContext::ctor ->" + this.GetHashCode());
        }

        // 定义实体
        public DbSet<Payable> Payables { get; set; }//应付信息
        public DbSet<PayableItem> PayableItems { get; set; }//应付明细
        public DbSet<TransportTask> TransportTasks { get; set; }//运输任务

        // 配置实体
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // 配置实体
            builder.ApplyConfiguration(new ClientRequestEntityTypeConfiguration());
            builder.ApplyConfiguration(new PayableEntityTypeConfiguration());
            builder.ApplyConfiguration(new PayableItemEntityTypeConfiguration());
            builder.ApplyConfiguration(new TransportTaskEntityTypeConfiguration());

#if DEBUG
            //release 版本应该让cap自己建表
            builder.ApplyConfiguration(new PublishEntityTypeConfiguration());//todo
            builder.ApplyConfiguration(new ReceiveEntityTypeConfiguration());//todo
#endif
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEventsAsync(this);
            var result = await base.SaveChangesAsync();
            return true;
        }
    }
}
