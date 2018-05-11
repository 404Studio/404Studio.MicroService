using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DotNetCore.CAP;
using FluentValidation;
using MediatR;
using YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate;
using YH.Etms.Settlement.Api.Domain.Events.Payable;
using YH.Etms.Settlement.Api.Infrastructure.Idempotency;
using YH.Etms.Utility.Extensions;
using YH.Etms.Utility.IntegrationEvents.Events;
using YH.Etms.Utility.Models.ResponseModel;
using YH.Framework.CAP;

namespace YH.Etms.Settlement.Api.Application.Commands.Payable.Init
{
    public class InitPayableCommandHandler: IRequestHandler<InitPayableCommand, Response<int>>
    {

        private readonly IPayableRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IValidator<InitPayableCommand> _validator;
        private readonly IPublisher _publisher;

        public InitPayableCommandHandler(IPayableRepository repository, IMapper mapper, IMediator mediator, IValidator<InitPayableCommand> validator,IPublisher publisher)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
            _validator = validator;
            _publisher = publisher;
        }

        public async Task<Response<int>> Handle(InitPayableCommand request, CancellationToken cancellationToken)
        {
            var validatorResult = _validator.Validate(request);

            var result = new Response<int>();

            if (validatorResult.IsValid)
            {
                try
                {
                    var payable = _mapper.Map<Domain.Aggregates.TransportTaskAggregate.Payable>(request);
                    //result = await _repository.Init(payable);
                    var payableId = await _repository.Create(payable);
                    result.Success = payableId > 0;
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
                    //errorMessage += error.ErrorMessage + Environment.NewLine;
                    Message msg=new Message();
                    msg.MessageType = MessageTypeEnum.Error;
                    msg.Content = error.ErrorMessage;
                    result.AttachedMessages.Add(msg);
                });
            }

            return result;
        }
    }

    public class InitPayableIdentifiedCommandHandler : IdentifiedCommandHandler<InitPayableCommand, Response<int>>
    {
        public InitPayableIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
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
