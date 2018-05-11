using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YH.Etms.Settlement.Api.Domain.Dtos.Code;
using YH.Etms.Settlement.Api.Domain.Dtos.Mdm;
using YH.Etms.Settlement.Api.Infrastructure;
using YH.Etms.Utility.Tools.FluentQuery;
using YH.Framework.ServiceAgent;

namespace YH.Etms.Settlement.Api.Controllers
{
    /// <summary>
    /// 获取单号服务
    /// </summary>
    [Route("api/v1/[controller]")]
    public class CodeController : ControllerBase, ICodeService
    {
        private static ServiceAgent _agent;
        private const string MDM_ORDER_CODE_ROUTE = "/api/v1/SequenceCode/GenerateCode";//定单号生成服务
        private const string MDM_AUDIT_CODE_ROUTE = "/api/v1/SequenceCode/GenerateCodeForAudit";//审计单号生成服务

        public CodeController(ServiceAgent agent) { _agent = agent; }
        /// <summary>
        /// 获取订单号
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("GetTmsOrderCode")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public string GetTmsOrderCode(CodeRequestDto dto)
        {
            if (string.IsNullOrEmpty(dto.SystemCode) || string.IsNullOrEmpty(dto.OrderType) || string.IsNullOrEmpty(dto.CompanyCode))
            {
                throw new Exception("缺失必要信息.");
            }
            var orgCodePrefix = dto.CompanyCode.Substring(0, 2);//取前两位
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("SystemCode", dto.SystemCode);
            dic.Add("OrderType", CodeType.OR);
            dic.Add("CompanyCode", orgCodePrefix);
            dic.Add("CustomFlag", dto.CustomFlag);
            var remoteResult = _agent.Get(MDM_ORDER_CODE_ROUTE, dic);
            var mdmApiResult = JsonConvert.DeserializeObject<MdmResultDto>(remoteResult);
            if (mdmApiResult.Successful)
            {
                if (mdmApiResult.Data != null)
                {
                    return mdmApiResult.Data.ToString();
                }
                throw new Exception(string.Format("获取'{0}'单号失败", dto.SystemCode));
            }
            else
            {
                throw new Exception(mdmApiResult.Exception.Message);
            }
        }

        /// <summary>
        /// 获取审计单号
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("GetTmsAuditCode")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public string GetTmsAuditCode(AuditCodeRequestDto dto)
        {
            if (string.IsNullOrEmpty(dto.BizCode) || string.IsNullOrEmpty(dto.SystemCode) || string.IsNullOrEmpty(dto.OrderType) || string.IsNullOrEmpty(dto.CompanyCode))
            {
                throw new Exception("缺失必要信息.");
            }
            var orgCodePrefix = dto.CompanyCode.Substring(0, 2);//取前两位

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("BizCode", dto.BizCode);
            dic.Add("SystemCode", dto.SystemCode);
            dic.Add("OrderType", dto.OrderType);
            dic.Add("CompanyCode", orgCodePrefix);
            dic.Add("CustomFlag", dto.CustomFlag);
            var remoteResult = _agent.Get(MDM_AUDIT_CODE_ROUTE, dic);
            var mdmApiResult = JsonConvert.DeserializeObject<MdmResultDto>(remoteResult);
            if (mdmApiResult.Successful)
            {
                if (mdmApiResult.Data != null)
                {
                    return mdmApiResult.Data.ToString();
                }
                throw new Exception("获取审计单号失败");
            }
            else
            {
                throw new Exception(mdmApiResult.Exception.Message);
            }
        }

        /// <summary>
        /// 获取运单号
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("GetTmsTransportCode")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public string GetTmsTransportCode(CodeRequestDto dto)
        {
            if (string.IsNullOrEmpty(dto.SystemCode) || string.IsNullOrEmpty(dto.OrderType) || string.IsNullOrEmpty(dto.CompanyCode))
            {
                throw new Exception("缺失必要信息.");
            }
            var orgCodePrefix = dto.CompanyCode.Substring(0, 2);//取前两位
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("SystemCode", dto.SystemCode);
            dic.Add("OrderType", CodeType.WB);
            dic.Add("CompanyCode", orgCodePrefix);
            dic.Add("CustomFlag", dto.CustomFlag);
            var remoteResult = _agent.Get(MDM_ORDER_CODE_ROUTE, dic);
            var mdmApiResult = JsonConvert.DeserializeObject<MdmResultDto>(remoteResult);
            if (mdmApiResult.Successful)
            {
                if (mdmApiResult.Data != null)
                {
                    return mdmApiResult.Data.ToString();
                }
                throw new Exception(string.Format("获取'{0}'单号失败", dto.SystemCode));
            }
            else
            {
                throw new Exception(mdmApiResult.Exception.Message);
            }
        }

        
    }
}