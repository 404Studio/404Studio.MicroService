using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YH.Etms.Utility.IntegrationEvents.Events;
using YH.Framework.CAP;

namespace YH.Etms.Settlement.Api.IntegrationEvents.EventHandling
{
    public class TestEventHandler : IEventHandler
    {
        [SubscribeEvent(TestEvent.EVENT_NAME)]
        public void Event1Handle1(TestEvent evt)
        {
            var obj = evt.Content;
        }
    }
}
