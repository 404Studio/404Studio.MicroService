using AutoMapper;
using DotNetCore.CAP;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YH.Etms.Settlement.Api.Application.Commands.Payable.Init;
using YH.Etms.Settlement.Api.Application.Commands.TransportTasks;
using YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate;
using YH.Etms.Settlement.Api.Domain.Events;
using YH.Etms.Settlement.Api.Domain.Exceptions;
using YH.Etms.Settlement.Api.Infrastructure;
using YH.Etms.Settlement.Api.Models.PriceCalculate;
using YH.Etms.Utility.Extensions;
using YH.Etms.Utility.IntegrationEvents.Events;
using YH.Etms.Utility.IntegrationEvents.Events.TransportTask.Create;
using YH.Etms.Utility.Models.PurchaseSettlement;
using YH.Framework.CAP;

namespace YH.Etms.Settlement.Api.IntegrationEvents.EventHandling
{
    public class SubscribeEventHandler : ISubscribeEventHandler
    {
        private readonly IMediator _mediator;
        private readonly ITransportTaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly IPublisher _publisher;
        private readonly SettlementContext _context;
        public SubscribeEventHandler(IMediator mediator, ITransportTaskRepository taskRepository, IMapper mapper, IPublisher publisher, SettlementContext context)
        {
            _mediator = mediator;
            _taskRepository = taskRepository;
            _mapper = mapper;
            _publisher = publisher;
            _context = context;
        }

        [SubscribeEvent(SettlementPriceMessageEvent.EVENT_NAME)]
        public async Task CalculatePrice(SettlementPriceMessageEvent evt)
        {
            if (evt == null) throw new ArgumentNullException(nameof(SettlementPriceMessageEvent));
            if (evt.Calculates.Count == 0)
                throw new ArgumentException("指派操作为：" + evt.OperationID + "的报价不存在");
            if (evt.Fees.Count == 0)
                throw new ArgumentException("指派操作为：" + evt.OperationID + "的费项不存在");
            //根据event内容分析出各种费项与计价单位
            //获取计价单位
            var priceUnit = GetPriceUnit(evt.Calculates.FirstOrDefault());
            //分析各种费项结果，并生成各种费项类
            var prices = AnalyzeFeesAndSetTransportFees(evt.Fees);
            //开始计算费用
            var priceHandler = new PriceHandler(prices, priceUnit, evt.Goods);
            priceHandler.Handle();
            if (priceHandler != null && priceHandler.PriceValue > 0)
            {
                //根据operationid取相关任务
                var task = await _taskRepository.FindByOperationAsync(evt.OperationID);
                if (task == null) throw new SettlementDomainException(evt.OperationID + "下的运输任务不存在");
                if (task.Payable == null) throw new SettlementDomainException(evt.OperationID + "运输下的应付没有被初始化");
                task.Payable.ExChangeStatus(PayableStatusEnum.Todo);
                task.Payable.SetPrice(priceHandler.PriceValue);
                //取送货类
                var sendPrice = prices.Where(p => p.GetType() == typeof(SendGoodsPrice)).Where(p => p.Value > 0).Cast<SendGoodsPrice>().FirstOrDefault();
                if (sendPrice != null)
                {
                    task.Payable.AddItem(new PayableItem
                    {
                        Amount = sendPrice.SumPrice,
                        UnitPrice = sendPrice.Value,
                        CostCode = sendPrice.Code,
                        CostName = sendPrice.Name,
                        CostType = sendPrice.Id,
                        LowestPrice = sendPrice.LowestPrice,
                        Unit = sendPrice.Unit,
                        Number = sendPrice.Number,
                        PayableBasis = PayableBasisEnum.Contract,
                        Note = sendPrice.Remark
                    });
                }
                prices.Where(p => p.GetType() != typeof(LowestPrice))
                    .Where(p => p.GetType() != typeof(SendGoodsPrice))
                    .Where(p => p.GetType() != typeof(FreePickWeightOrVolumePrice))
                    .Where(p => p.GetType() != typeof(FreeSendWeightOrVolumePrice))
                    .Where(p => p.Value > 0)
                    .ForEach(item =>
                    {
                        PayableItem pi = new PayableItem
                        {
                            Amount = item.SumPrice,
                            UnitPrice = item.Value,
                            CostCode = item.Code,
                            CostName = item.Name,
                            CostType = item.Id,
                            LowestPrice = item.LowestPrice,
                            Unit = item.Unit,
                            Number = item.Number,
                            PayableBasis = PayableBasisEnum.Contract
                        };
                        if (item.GetType() == typeof(PickGoodsPrice))
                            pi.Note = (item as PickGoodsPrice).Remark;
                        task.Payable.AddItem(pi);
                    });
                await _taskRepository.UpdateAsync(task);
                //这里可以继续发送FinishPriceEvent领域事件
                var finishSettlementEvent = new FinishSettlementEvent();
                var settlementPayable = new SettlementPayable
                {
                    Amount = task.Payable.Amount,
                    OperationId = evt.OperationID,
                    SettlementUnit = task.Payable.SettlementUnit,
                    SettlementUnitCode = task.Payable.SettlementUnitCode,
                    PayableItems = _mapper.Map<List<SettlementPayableItem>>(task.Payable.Details)
                };
                finishSettlementEvent.SetPayable(settlementPayable);
                task.AddDomainEvent(finishSettlementEvent);
                await _taskRepository.UnitOfWork.SaveEntitiesAsync();
            }
        }
        [SubscribeEvent(TransportTaskCreateEvent.EVENT_NAME)]
        public async Task CreateTransportTask(TransportTaskCreateEvent evt)
        {
            var createTransportTaskCommand = _mapper.Map<CreateTransportTaskCommand>(evt.TransportTask);
            var taskId = await _mediator.Send(createTransportTaskCommand);
            if (taskId > 0)
            {
                var createPayableCommand = _mapper.Map<InitPayableCommand>(evt);
                createPayableCommand.SetForignerKey(taskId);
                var obj = await _mediator.Send(createPayableCommand);//新建应付信息
                if (obj.Success)
                    using (var trans = _context.Database.BeginTransaction())
                    {
                        _publisher.Publish(CalculatePriceEvent.EVENT_NAME, evt.NextEvent);
                        trans.Commit();
                    }

            }
        }

        private List<IPrice> AnalyzeFeesAndSetTransportFees(List<Fee> fees)
        {
            List<IPrice> prices = new List<IPrice>(fees.Capacity);
            foreach (var fee in fees)
            {
                var price = PriceCalculateFactory.Creator(fee);
                if (price == null) continue;
                prices.Add(price);
            }
            return prices;
        }

        /// <summary>
        /// 获取计价单位
        /// </summary>
        /// <param name="calculateCondition"></param>
        /// <returns></returns>
        private PriceUnit GetPriceUnit(CalculateCondition calculateCondition)
        {
            return new PriceUnit(calculateCondition.Code);
        }
    }
}
