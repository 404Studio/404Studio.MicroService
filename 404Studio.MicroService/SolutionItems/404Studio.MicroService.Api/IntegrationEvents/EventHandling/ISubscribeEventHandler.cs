using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YH.Etms.Utility.IntegrationEvents.Events;
using YH.Etms.Utility.IntegrationEvents.Events.TransportTask.Create;
using YH.Framework.CAP;

namespace YH.Etms.Settlement.Api.IntegrationEvents.EventHandling
{
    public interface ISubscribeEventHandler : IEventHandler
    {
        /// <summary>
        /// 新建运输任务订阅事件
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        Task CreateTransportTask(TransportTaskCreateEvent evt);
        /// <summary>
        /// 计算报价订阅事件
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        Task CalculatePrice(SettlementPriceMessageEvent evt);
    }
}
