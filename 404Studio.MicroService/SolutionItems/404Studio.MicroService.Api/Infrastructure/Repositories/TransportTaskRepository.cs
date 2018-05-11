using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate;
using YH.Etms.Settlement.Api.Domain.Entities;
using YH.Etms.Utility.Extensions;
using YH.Etms.Utility.Models.ResponseModel;

namespace YH.Etms.Settlement.Api.Infrastructure.Repositories
{
    public class TransportTaskRepository : ITransportTaskRepository
    {
        private readonly SettlementContext _context;
        private IOptionsSnapshot<AppSettings> _settings;

        public TransportTaskRepository(SettlementContext context, IOptionsSnapshot<AppSettings> settings)
        {
            _context = context;
            _settings = settings;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<bool> Create(TransportTask task)
        {
            if (_context.TransportTasks.Any(t => t.OperationID == task.OperationID))
            {
                return await Task.FromResult(false);
            }
            await _context.TransportTasks.AddAsync(task);
            return true;
        }

        public async Task<TransportTask> FindByOperationAsync(Guid operationID)
        {
            if (Guid.Empty == operationID) return await Task.FromResult(default(TransportTask));
            return _context.TransportTasks
                .Include(p=>p.Payable)
                .FirstOrDefault(p => p.OperationID == operationID.ToString());
        }

        public async Task<TransportTask> UpdateAsync(TransportTask task)
        {
            if (!task.IsTransient())
            {
                var taskFromDb = await _context.TransportTasks
                    .Include(p => p.Payable)
                    .Where(p => p.Payable.Status != PayableStatusEnum.Todo)
                    .FirstOrDefaultAsync(p => p.Id == task.Id);
                taskFromDb.AssignFrom(task);
                return _context.TransportTasks.Update(task).Entity;
            }
            return task;
        }
    }
}
