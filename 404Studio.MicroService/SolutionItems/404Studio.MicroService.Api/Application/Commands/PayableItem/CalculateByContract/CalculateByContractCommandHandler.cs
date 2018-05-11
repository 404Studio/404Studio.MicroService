using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate;
using YH.Etms.Settlement.Api.Infrastructure.Idempotency;
using YH.Etms.Utility.Extensions;
using YH.Etms.Utility.Https;
using YH.Etms.Utility.Models.ResponseModel;

namespace YH.Etms.Settlement.Api.Application.Commands.PayableItem.CalculateByContract
{
    public class CalculateByContractCommandHandler : IRequestHandler<CalculateByContractCommand, Response<int>>
    {
        private readonly IPayableRepository _repository;
        private readonly IValidator<CalculateByContractCommand> _validator;
        private readonly IOptionsSnapshot<AppSettings> _settings;

        public CalculateByContractCommandHandler(IPayableRepository repository, IOptionsSnapshot<AppSettings> settings, IValidator<CalculateByContractCommand> validator)
        {
            _repository = repository;
            _settings = settings;
            _validator = validator;
        }

        public async Task<Response<int>> Handle(CalculateByContractCommand request, CancellationToken cancellationToken)
        {
            var validatorResult = _validator.Validate(request);

            var result = new Response<int>();

            if (validatorResult.IsValid)
            {
                try
                {
                    
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.MessageText = ex.Message;
                    if (ex.InnerException != null)
                    {
                        Message msg = new Message();
                        msg.MessageType = MessageTypeEnum.Error;
                        msg.Content = ex.InnerException.Message;
                        result.AttachedMessages.Add(msg);
                    }
                }
            }
            else
            {
                result.Success = false;
                result.MessageText = "未通过数据校验,请看详细信息!";
                validatorResult.Errors.ForEach(error =>
                {
                    Message msg = new Message();
                    msg.MessageType = MessageTypeEnum.Error;
                    msg.Content = error.ErrorMessage;
                    result.AttachedMessages.Add(msg);
                });
            }

            return result;
        }
    }

    public class CalculateByContractIdentifiedCommandHandler : IdentifiedCommandHandler<CalculateByContractCommand, Response<int>>
    {
        public CalculateByContractIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
        {
        }

        protected override Response<int> CreateResultForDuplicateRequest()
        {
            var result = new Response<int>();
            result.Success = false;
            return result;
        }
    }
}
