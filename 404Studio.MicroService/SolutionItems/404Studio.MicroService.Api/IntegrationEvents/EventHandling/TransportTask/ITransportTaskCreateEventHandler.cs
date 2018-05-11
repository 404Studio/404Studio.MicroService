using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YH.Etms.Utility.IntegrationEvents.Events.TransportTask.Create;
using YH.Framework.CAP;

namespace YH.Etms.Settlement.Api.IntegrationEvents.EventHandling.TransportTask
{
    public interface ITransportTaskCreateEventHandler : IEventHandler
    {
        Task Handle(TransportTaskCreateEvent evt);
    }
}
