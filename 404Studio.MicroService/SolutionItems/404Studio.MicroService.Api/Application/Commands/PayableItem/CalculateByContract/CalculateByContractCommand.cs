using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using YH.Etms.Utility.Models.ResponseModel;

namespace YH.Etms.Settlement.Api.Application.Commands.PayableItem.CalculateByContract
{
    public class CalculateByContractCommand: IRequest<Response<int>>
    {
        public int PayableId { get; set; }

        public CalculateByContractCommand(int payableId) { PayableId = payableId; }
    }
}
