using YH.Etms.Utility.Enums;

namespace YH.Etms.Settlement.Api.Application.Commands.Payable.Init.Dto
{
    /// <summary>
    /// 运输任务创建
    /// </summary>
    public class TransportTaskCreateDto
    {
        public string OperationID { get; set; }
        public decimal Weight { get; set; }
        public decimal Volume { get; set; }
        public string Line { get; set; }
        public string LinCode { get; set; }
        public CargoTypeEnum CargoType { get; set; }
        public PackageTypeEnum PackageType { get; set; }
        public TransportModeEnum TransportMode { get; set; }
    }
}
