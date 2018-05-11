using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YH.Etms.Settlement.Api.Models.PriceCalculate
{
    public interface IPrice
    {
        string Name { get; }
        string Code { get; }
        int Id { get; }
        /// <summary>
        /// 单价
        /// </summary>
        decimal Value { get;}
        /// <summary>
        /// 总价
        /// </summary>
        decimal SumPrice { get; }
        /// <summary>
        /// 单位
        /// </summary>
        string Unit { get; }
        /// <summary>
        /// 数量
        /// </summary>
        decimal Number { get;}
        /// <summary>
        /// 最低收费单价标准
        /// </summary>
        decimal LowestPrice { get; }
    }
}
