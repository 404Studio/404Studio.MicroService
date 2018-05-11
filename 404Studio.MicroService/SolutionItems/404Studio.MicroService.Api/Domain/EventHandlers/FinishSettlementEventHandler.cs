using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YH.Etms.Settlement.Api.Domain.Events;
using YH.Etms.Settlement.Api.Infrastructure;
using YH.Etms.Utility.IntegrationEvents.Events;
using YH.Framework.CAP;

namespace YH.Etms.Settlement.Api.Domain.EventHandlers
{
    public class FinishSettlementEventHandler : INotificationHandler<FinishSettlementEvent>
    {
        private readonly IPublisher _publisher;
        private readonly IMapper _mapper;
        private readonly SettlementContext _context;
        public FinishSettlementEventHandler(IPublisher publisher,IMapper mapper, SettlementContext context)
        {
            _publisher = publisher;
            _mapper = mapper;
            _context = context;
        }

        public async Task Handle(FinishSettlementEvent finishSettlementEvent, CancellationToken cancellationToken)
        {
            //这里可以发送费用均摊事件/命令
            var finishEvent = _mapper.Map<FinishPriceEvent>(finishSettlementEvent.Payable);
            if (finishEvent == null) return;
            using (var trans = await _context.Database.BeginTransactionAsync())
            {
                _publisher.Publish(FinishPriceEvent.EVENT_NAME, finishEvent);
                trans.Commit();
            }
        }
    }
}
