using DotNetCore.CAP;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate;
using YH.Etms.Settlement.Api.Domain.Exceptions;
using YH.Etms.Settlement.Api.Models.PriceCalculate;
using YH.Etms.Utility.IntegrationEvents.Events;
using YH.Etms.Utility.Models.PurchaseSettlement;
using YH.Framework.CAP;

namespace YH.Etms.Settlement.Api.IntegrationEvents.EventHandling
{
    public class SettlementPriceMessageEventHanlder : ISettlementPriceMessageEventHanlder
    {
        private readonly IMediator _mediator;
        private readonly ITransportTaskRepository _taskRepository;
        public SettlementPriceMessageEventHanlder(IMediator mediator, ITransportTaskRepository taskRepository)
        {
            _mediator = mediator;
            _taskRepository = taskRepository;
        }

        [SubscribeEvent(SettlementPriceMessageEvent.EVENT_NAME)]
        public async Task Handle(SettlementPriceMessageEvent @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(SettlementPriceMessageEvent));
            //根据event内容分析出各种费项与计价单位
            //获取计价单位
            var priceUnit = GetPriceUnit(@event.Calculates.FirstOrDefault());
            //分析各种费项结果，并生成各种费项类
            var prices = AnalyzeFeesAndSetTransportFees(@event.Fees);
            //开始计算费用
            var priceHandler = new PriceHandler(prices, priceUnit, @event.Goods);
            priceHandler.Handle();
            if (priceHandler != null && priceHandler.PriceValue > 0)
            {
                //根据operationid取相关任务
                var task = await _taskRepository.FindByOperationAsync(@event.OperationID);
                if (task == null) throw new SettlementDomainException(@event.OperationID + "下的运输任务不存在");
                if (task.Payable == null) throw new SettlementDomainException(@event.OperationID + "运输下的应付没有被初始化");
                task.Payable.ExChangeStatus(PayableStatusEnum.Todo);
                task.Payable.SetPrice(priceHandler.PriceValue);
                //应付子集
                foreach (var item in prices)
                {
                    PayableItem pi = new PayableItem
                    {
                        Amount = item.Value,
                        CostCode = item.Code,
                        CostName = item.Name,
                        CostType = item.Id,
                        PayableBasis = PayableBasisEnum.Contract
                    };
                    task.Payable.AddItem(pi);
                }
                await _taskRepository.UpdateAsync(task);
                //这里可以继续发送FinishPriceEvent事件
            }
            await _taskRepository.UnitOfWork.SaveChangesAsync();
        }

        private List<IPrice> AnalyzeFeesAndSetTransportFees(List<Fee> fees)
        {
            List<IPrice> prices = new List<IPrice>(fees.Capacity);
            foreach (var fee in fees)
            {
                prices.Add(PriceCalculateFactory.Creator(fee));
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
