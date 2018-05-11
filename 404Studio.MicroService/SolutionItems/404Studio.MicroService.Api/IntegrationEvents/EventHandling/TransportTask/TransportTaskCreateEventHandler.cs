using System.Threading.Tasks;
using AutoMapper;
using DotNetCore.CAP;
using MediatR;
using YH.Etms.Settlement.Api.Application.Commands.Payable.Init;
using YH.Etms.Settlement.Api.Application.Commands.TransportTasks;
using YH.Etms.Utility.IntegrationEvents.Events;
using YH.Etms.Utility.IntegrationEvents.Events.TransportTask.Create;
using YH.Framework.CAP;

namespace YH.Etms.Settlement.Api.IntegrationEvents.EventHandling.TransportTask
{
    /// <summary>
    /// 处理分配运输任务给承运商后发生的事件
    /// </summary>
    public class TransportTaskCreateEventHandler : ITransportTaskCreateEventHandler
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ICapPublisher _publisher;

        public TransportTaskCreateEventHandler(IMapper mapper, IMediator mediator, ICapPublisher publisher)
        {
            _mapper = mapper;
            _mediator = mediator;
            _publisher = publisher;
        }

        [SubscribeEvent(TransportTaskCreateEvent.EVENT_NAME)]
        public async Task Handle(TransportTaskCreateEvent evt)
        {
            var createTransportTaskCommand = _mapper.Map<CreateTransportTaskCommand>(evt.TransportTask);
            var taskId = await _mediator.Send(createTransportTaskCommand);
            if (taskId > 0)
            {
                var createPayableCommand = _mapper.Map<InitPayableCommand>(evt);
                createPayableCommand.SetForignerKey(taskId);
                var obj = await _mediator.Send(createPayableCommand);//新建应付信息
                if (obj.Success)
                    _publisher.Publish(CalculatePriceEvent.EVENT_NAME, evt.NextEvent);
            }
        }
    }
}
