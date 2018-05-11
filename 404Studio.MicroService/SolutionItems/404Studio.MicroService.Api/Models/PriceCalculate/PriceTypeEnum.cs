using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace YH.Etms.Settlement.Api.Models.PriceCalculate
{
    /// <summary>
    /// 价格类型
    /// Y:元
    /// </summary>
    public enum PriceTypeEnum
    {
        [Description("元")]
        Y = 1
    }
    /// <summary>
    /// 单位类型
    /// </summary>
    public enum UnitTypeEnum
    {
        [Description("立方")]
        LF = 1,
        [Description("公斤")]
        GJ = 2
    }
}
