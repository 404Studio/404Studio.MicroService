using MediatR;
using YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate;
using YH.Etms.Utility.Models.ResponseModel;
using YH.Etms.Settlement.Api.Application.Commands.Payable.Init.Dto;
using YH.Etms.Utility.IntegrationEvents.Events;

namespace YH.Etms.Settlement.Api.Application.Commands.Payable.Init
{
    public class InitPayableCommand: IRequest<Response<int>>
    {
        public string SettlementUnit { get; set; }
        public string SettlementUnitCode { get; set; }
        public decimal Amount { get; set; }
        public PayableStatusEnum Status { get; set; }
        public string Note { get; set; }
        public int TransportTaskId { get; set; }
        public void SetForignerKey(int taskId)
        {
            TransportTaskId = taskId;
        }
    }
}
