using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YH.Etms.Utility.Enums;

namespace YH.Etms.Settlement.Api.Application.Commands.TransportTasks
{
    public class CreateTransportTaskCommand : IRequest<int>
    {
        public CargoTypeEnum CargoType { get; set; }
        public string LineCode { get; set; }
        public string Line { get; set; }
        public Guid OperationID { get; set; }
        public PackageTypeEnum PackageType { get; set; }
        public TransportModeEnum TransportMode { get; set; }
        public decimal Volume { get; set; }
        public decimal Weight { get; set; }
    }
}
