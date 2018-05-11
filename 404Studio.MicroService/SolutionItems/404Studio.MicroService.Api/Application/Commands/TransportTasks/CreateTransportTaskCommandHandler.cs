using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate;

namespace YH.Etms.Settlement.Api.Application.Commands.TransportTasks
{
    public class CreateTransportTaskCommandHandler : IRequestHandler<CreateTransportTaskCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly ITransportTaskRepository _taskRepository;
        public CreateTransportTaskCommandHandler(IMapper mapper, ITransportTaskRepository taskRepository) {
            _mapper = mapper;
            _taskRepository = taskRepository;
        }
        public async Task<int> Handle(CreateTransportTaskCommand request, CancellationToken cancellationToken)
        {
            var task = _mapper.Map<TransportTask>(request);
            await _taskRepository.Create(task);
            await _taskRepository.UnitOfWork.SaveChangesAsync();
            return task.Id;
        }
    }
}
