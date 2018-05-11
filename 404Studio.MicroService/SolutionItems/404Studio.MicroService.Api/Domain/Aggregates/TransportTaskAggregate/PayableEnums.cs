using System.ComponentModel;

namespace YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate
{
    //todo:各项目相同的枚举提炼成公有
    /// <summary>
    /// 应付状态
    /// </summary>
    public enum PayableStatusEnum
    {
        [Description("原始")]
        Init =0,
        [Description("待审")]
        Todo = 1,
        [Description("通过")]
        Passed = 2
    }
    /// <summary>
    /// 应付费用依据
    /// </summary>
    public enum PayableBasisEnum
    {
        [Description("合同")]
        Contract = 0,
        [Description("其他")]
        Others = 1
    }

    /// <summary>
    /// 费项
    /// </summary>
    public enum CostTypeEnum
    { }

    
}
