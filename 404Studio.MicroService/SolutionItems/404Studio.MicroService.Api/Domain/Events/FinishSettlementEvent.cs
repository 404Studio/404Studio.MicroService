using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YH.Etms.Settlement.Api.Domain.Events
{
    public class FinishSettlementEvent : INotification
    {
        public SettlementPayable Payable { get; set; }

        public void SetPayable(SettlementPayable payable) {
            Payable = payable;
        }
    }

    public class SettlementPayable
    {
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
        /// 运输任务操作Id
        /// </summary>
        public Guid OperationId { get; set; }
        /// <summary>
        /// 应付明细集合
        /// </summary>
        public List<SettlementPayableItem> PayableItems { get; set; }
    }

    public class SettlementPayableItem
    {
        /// <summary>
        /// 费项Id
        /// </summary>
        public int CostType { get; set; }
        /// <summary>
        /// 费项名称
        /// </summary>
        public string CostName { get; set; }
        /// <summary>
        /// 费项Code
        /// </summary>
        public string CostCode { get; set; }
        /// <summary>
        /// 费用单价
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Number { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
