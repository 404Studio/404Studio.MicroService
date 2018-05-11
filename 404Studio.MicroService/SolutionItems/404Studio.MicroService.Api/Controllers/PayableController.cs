using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using YH.Etms.Settlement.Api.Application.Commands;
using YH.Etms.Settlement.Api.Application.Commands.Payable.Init;
using YH.Etms.Settlement.Api.Infrastructure;
using YH.Etms.Utility.IntegrationEvents.Events;
using YH.Etms.Utility.Models.ResponseModel;
using YH.Etms.Utility.Tools.FluentQuery;
using YH.Framework.CAP;

namespace YH.Etms.Settlement.Api.Controllers
{
    [Route("api/v1/[controller]")]
    public class PayableController: ControllerBase
    {
        private IOptionsSnapshot<AppSettings> _settings;
        private readonly IMediator _mediator;
        private readonly SettlementContext _context;//todo
        private readonly IPublisher _publisher;//tod

        public PayableController(IOptionsSnapshot<AppSettings> settings, IMediator mediator, SettlementContext context, IPublisher publisher)
        {
            _settings = settings;
            _mediator = mediator;
            _context = context;
            _publisher = publisher;
        }

        /// <summary>
        /// 生成应付的原始信息
        /// </summary>
        /// <param name="createCommand"></param>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [Route("Create")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreatePayable([FromBody]InitPayableCommand createCommand, [FromHeader(Name = "x-requestid")] string requestId)
        {
            Response<int> commandResult = new Response<int>();
            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var requestCreateCommand = new IdentifiedCommand<InitPayableCommand, Response<int>>(createCommand, guid);
                commandResult = await _mediator.Send(requestCreateCommand);
            }
            var result = commandResult.Success ? (IActionResult)Ok(commandResult) : BadRequest(commandResult);
            return result;
        }

        /// <summary>
        /// 集成事件测试
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("EventTest")]
        [HttpPost]
        public IActionResult EventTest(string input)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                CalculatePriceEvent myEvent = new CalculatePriceEvent();
                myEvent.Id = Guid.NewGuid();
                myEvent.Scheme = input;
                _publisher.Publish<CalculatePriceEvent>(CalculatePriceEvent.EVENT_NAME, myEvent);
                trans.Commit();
            }
            return Ok();
        }

        /// <summary>
        /// 字符串测试
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("StringTest")]
        [HttpPost]
        public IActionResult StringTest(string input)
        {
            input = input.Replace(Environment.NewLine, "");
            var selectedColumnItems = input.Split(',').ToList();
            var pattern = " as ";
            foreach (var selectedColumnItem in selectedColumnItems)
            {
                
            }
            return Ok();
        }
    }
}
