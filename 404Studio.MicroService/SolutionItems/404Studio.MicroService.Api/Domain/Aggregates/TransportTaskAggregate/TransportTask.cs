using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate;
using YH.Etms.Settlement.Api.Domain.Entities;
using YH.Etms.Utility.Enums;

namespace YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate
{
    /// <summary>
    /// 运输任务
    /// </summary>
    public class TransportTask: Entity, IAggregateRoot
    {
        /// <summary>
        /// 任务标记
        /// </summary>
        public string OperationID { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// 体积
        /// </summary>
        public decimal Volume { get; set; }
        /// <summary>
        /// 线路
        /// </summary>
        public string Line { get; set; }
        public string LineCode { get; set; }
        /// <summary>
        /// 货物类别
        /// </summary>
        public CargoTypeEnum CargoType { get; set; }
        /// <summary>
        /// 包装形式
        /// </summary>
        public PackageTypeEnum PackageType { get; set; }
        /// <summary>
        /// 运输方式
        /// </summary>
        public TransportModeEnum TransportMode { get; set; }
        /// <summary>
        /// 应付费用信息
        /// </summary>
        public Payable Payable { get; set; }
    }
}
