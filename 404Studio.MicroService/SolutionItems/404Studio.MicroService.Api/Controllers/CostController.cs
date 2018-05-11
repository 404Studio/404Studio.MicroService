using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YH.Etms.Settlement.Api.Application.Queries;
using YH.Etms.Settlement.Api.Domain.Dtos.Payable;

namespace YH.Etms.Settlement.Api.Controllers
{
    [Route("api/v1/[Controller]")]
    public class CostController : Controller
    {
        private readonly IPayableQuery _query;
        public CostController(IPayableQuery query)
        {
            _query = query;
        }
        /// <summary>
        /// 应付运费查询
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [Route("Payable/Query")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PayableQuery([FromBody]PayableSearch search)
        {
            return Ok(await _query.SearchPayablesAsync());
        }
    }
}