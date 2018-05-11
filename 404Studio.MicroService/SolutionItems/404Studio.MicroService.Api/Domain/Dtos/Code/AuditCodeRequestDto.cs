namespace YH.Etms.Settlement.Api.Domain.Dtos.Code
{
    public class AuditCodeRequestDto
    {
        /// <summary>
        /// 业务单号(例如订单号)
        /// </summary>
        public string BizCode { get; set; }
        /// <summary>
        /// 约定为"TMS"
        /// </summary>
        public string SystemCode { get { return "TMS"; } }
        /// <summary>
        /// 运输订单=OR,运单=WB,派车单=PA,派车作业单=DD,应付账单=AP,应收账单=AR
        /// </summary>
        public string OrderType { get; set; }
        public string CompanyCode { get; set; }
        public string CustomFlag { get; set; }
    }
}
