using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate;
using YH.Etms.Settlement.Api.Domain.Dtos;
using YH.Etms.Settlement.Api.Domain.Entities;
using YH.Etms.Utility.IntegrationEvents.Events;
using YH.Etms.Utility.Models.ResponseModel;
using YH.Framework.CAP;

namespace YH.Etms.Settlement.Api.Infrastructure.Repositories
{
    public class PayableRepository : IPayableRepository
    {
        private readonly SettlementContext _context;
        private IOptionsSnapshot<AppSettings> _settings;
        private readonly IPublisher _publisher;
        private readonly IMediator _mediator;
        private readonly ICodeService _codeService;
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public PayableRepository(IOptionsSnapshot<AppSettings> settings, SettlementContext context, IPublisher publisher,IMediator mediator, ICodeService codeService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _settings = settings;
            _publisher = publisher;
            _mediator = mediator;
            _codeService = codeService;
        }

        public async Task<Response<int>> Init(Payable payable)
        {
            Response<int> result=new Response<int>();
            try
            {
                if (payable.IsTransient())
                {
                    var track = await _context.Payables.AddAsync(payable);
                    await _context.SaveEntitiesAsync();
                    //await _mediator.Publish(new OrderCreatedDomainEvent(order.Id));
                    result.Item= track.Entity.Id;
                    result.Success = true;
                    result.MessageText = "创建初始应付信息成功.";

                    using (var trans = _context.Database.BeginTransaction())
                    {
                        CalculatePriceEvent myEvent = new CalculatePriceEvent();
                        myEvent.Id=Guid.NewGuid();
                        _publisher.Publish<CalculatePriceEvent>(CalculatePriceEvent.EVENT_NAME, myEvent);
                        trans.Commit();
                    }
                }
                else
                {
                    result.Success = false;
                    result.MessageText = $"已存在改编号'#{payable.Id}'的应付信息,执行创建不成功.";
                    result.Item= await Task.FromResult(payable.Id);
                }
            }
            catch (Exception ex)
            {
                result.Item = -1;
                result.Success = false;
                result.MessageText = ex.Message;
                if (_settings.Value.EnableTrace && ex.InnerException != null)
                {
                    Message msg=new Message();
                    msg.MessageType = MessageTypeEnum.Error;
                    msg.Content = ex.InnerException.Message;
                    result.AttachedMessages.Add(msg);
                }
            }
            return result;
        }

        public async Task<int> Create(Payable payable) {
            if (payable.IsTransient())
            {
                if (_context.Payables.Any(p => p.TransportTaskId == payable.TransportTaskId))
                    return 0;
                var track = await _context.Payables.AddAsync(payable);
                return await _context.SaveChangesAsync();            
            }
            return await Task.FromResult(0);
        }
    }
}
