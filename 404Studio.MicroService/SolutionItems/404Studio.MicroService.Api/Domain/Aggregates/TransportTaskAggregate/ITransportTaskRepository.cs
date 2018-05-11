using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YH.Etms.Settlement.Api.Domain.Entities;
using YH.Etms.Utility.Models.ResponseModel;

namespace YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate
{
    public interface ITransportTaskRepository: IRepository<TransportTask>
    {
        /// <summary>
        /// 新增任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        Task<bool> Create(TransportTask task);
        Task<TransportTask> FindByOperationAsync(Guid operationID);
        Task<TransportTask> UpdateAsync(TransportTask task);
    }
}
