using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using YH.Etms.Settlement.Api.Infrastructure;
using YH.Etms.Utility.IntegrationEvents.Events;
using YH.Etms.Utility.Tools.FluentQuery;
using YH.Framework.CAP;

namespace YH.Etms.Settlement.Api.Controllers
{
    [Route("api/v1/[controller]")]
    public class TestController : ControllerBase
    {
        private IFluentQuery _fluentQuery;
        private IPublisher _publisher;
        private readonly SettlementContext _context;

        public TestController(IFluentQuery fluentQuery, IPublisher publisher, SettlementContext context)
        {
            _fluentQuery = fluentQuery;
            _publisher = publisher;
            _context = context;
        }

        /// <summary>
        /// 用于测试
        /// </summary>
        /// <param name="input"></param>
        [Route("MyTest")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public void MyTest(string input)
        {

            _fluentQuery.Select("")
                 .From("sfds")
                 .LeftJoin("sdfs").On("111")
                 .LeftJoin("sdagdf").On("222")
                 .Where("dfgdfg").EqualTo("dfgdg")
                //.Skip(1).Take(10)
                ;
            var obj = _fluentQuery;
        }
        [Route("CAP")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public void CAP()
        {
            var calculateEvent = new CalculatePriceEvent(
                orgid: 36,
                carrierId: 75235,
                startRegion: 402,
                endRegion: 1884,
                calculates: new System.Collections.Generic.List<string> { "5" },
                scheme: "送电商货物",
                operationId: new Guid(),
                new Utility.Models.PurchaseSettlement.Goods());
            using (var trans = _context.Database.BeginTransaction())
            {
                _publisher.Publish("yh.tms.purchasing.price", calculateEvent);
                trans.Commit();
            }

        }
    }
}
