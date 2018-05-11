using System;
using System.Collections.Generic;
using System.Linq;
using YH.Etms.Settlement.Api.Domain.Entities;

namespace YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate
{
    /// <summary>
    /// 应付费用
    /// </summary>
    public class Payable: Entity
    {
        public int TransportTaskId { get; set; }
        /// <summary>
        /// 结算单位
        /// </summary>
        public string SettlementUnit { get; set; }
        /// <summary>
        /// 结算单位编码
        /// </summary>
        public string SettlementUnitCode { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public PayableStatusEnum Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 应付明细集合
        /// </summary>
        public List<PayableItem> Details { get; set; }

        /// <summary>
        /// 应付的运单任务
        /// </summary>
        public TransportTask TransportTask { get; set; }

        public void ExChangeStatus(PayableStatusEnum status)
        {
            Status = status;
        }

        public void SetPrice(decimal priceValue)
        {
            Amount = priceValue;
        }
        public void AddItem(PayableItem item)
        {
            if (Details == null)
                Details = new List<PayableItem>();
            if (Details.Any(p => p.CostType == item.CostType)) return;
            Details.Add(item);
        }
    }
}
