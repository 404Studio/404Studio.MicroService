using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate;
using YH.Etms.Settlement.Api.Domain.Events.Payable;

namespace YH.Etms.Settlement.Api.Domain.EventHandlers.Payable
{
    /// <summary>
    /// 应付信息被初始化后发出的事件处理
    /// </summary>
    public class InitedPayableDomainEventHandler : INotificationHandler<PayableInitedDomainEvent>
    {
        private readonly IMediator _mediator;
        private readonly IPayableRepository _payableRepository;

        public InitedPayableDomainEventHandler(IMediator mediator, IPayableRepository payableRepository)
        {
            _mediator = mediator;
            _payableRepository = payableRepository;
        }
        public async Task Handle(PayableInitedDomainEvent notification, CancellationToken cancellationToken)
        {
            var obj = notification.Id;
            //订单被创建后生成默认运单
            //await _mediator.Send(new CreateDefaultTransportInfoCommand(notification.Id), cancellationToken);

            //订单创建成功后获取相关单号
            //await _mediator.Send(new SetOrderCodeCommand(notification.Id), cancellationToken);
        }
    }
}
