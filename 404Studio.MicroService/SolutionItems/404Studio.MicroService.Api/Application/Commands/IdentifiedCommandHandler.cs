using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YH.Etms.Settlement.Api.Infrastructure.Idempotency;

namespace YH.Etms.Settlement.Api.Application.Commands
{
    /// <summary>
    /// 处理重复请求确保幂等的基础实现
    /// 通过client发送的requestid来判断
    /// </summary>
    /// <typeparam name="T">如果请求不重复，则真正执行操作的命令处理程序的类型</typeparam>
    /// <typeparam name="R">内部命令处理程序的返回值</typeparam>
    public class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R>
        where T : IRequest<R>
    {
        private readonly IMediator _mediator;
        private readonly IRequestManager _requestManager;

        public IdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager)
        {
            _mediator = mediator;
            _requestManager = requestManager;
        }

        /// <summary>
        /// 如果找到前一个请求，则创建返回的结果值
        /// </summary>
        /// <returns></returns>
        protected virtual R CreateResultForDuplicateRequest()
        {
            return default(R);
        }

        /// <summary>
        /// 这个方法处理命令。 它只是确保没有其他请求存在相同的ID，如果是这种情况只是排队原始的内部命令。
        /// </summary>
        /// <param name="message">包含原始命令和请求ID的标识命令</param>
        /// <param name="cancellationToken">包含原始命令和请求ID的标识命令</param>
        /// <returns>内部命令的返回值或者当requestid相同时返回default</returns>
        public async Task<R> Handle(IdentifiedCommand<T, R> message, CancellationToken cancellationToken)
        {
            var alreadyExists = await _requestManager.ExistAsync(message.Id);
            if (alreadyExists)
            {
                //一般在实现类中重写CreateResultForDuplicateRequest方法
                return CreateResultForDuplicateRequest();
            }
            else
            {
                await _requestManager.CreateRequestForCommandAsync<T>(message.Id);
                // 发送给真正的业务Command
                var result = await _mediator.Send(message.Command);
                return result;
            }
        }
    }
}
