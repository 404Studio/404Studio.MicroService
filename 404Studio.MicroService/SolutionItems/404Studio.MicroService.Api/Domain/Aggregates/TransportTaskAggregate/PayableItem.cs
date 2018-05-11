using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YH.Etms.Settlement.Api.Domain.Entities;

namespace YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate
{
    /// <summary>
    /// 应付的明细项
    /// </summary>
    public class PayableItem: Entity
    {
        public int PayableId { get; set; }
        /// <summary>
        /// 应付依据
        /// </summary>
        public PayableBasisEnum PayableBasis { get; set; }
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
        /// 最低收费
        /// </summary>
        public decimal LowestPrice { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 记录创建人
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifiedAt { get; set; }
    }
}
