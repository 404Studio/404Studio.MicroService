using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YH.Etms.Utility.IntegrationEvents.Events;
using YH.Framework.CAP;

namespace YH.Etms.Settlement.Api.IntegrationEvents.EventHandling
{
    public interface ISettlementPriceMessageEventHanlder : IEventHandler
    {
        Task Handle(SettlementPriceMessageEvent @event);
    }
}
