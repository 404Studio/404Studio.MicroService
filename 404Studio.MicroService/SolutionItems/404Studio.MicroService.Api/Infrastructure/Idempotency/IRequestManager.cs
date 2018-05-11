using System;
using System.Threading.Tasks;

namespace YH.Etms.Settlement.Api.Infrastructure.Idempotency
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(Guid id);

        Task CreateRequestForCommandAsync<T>(Guid id);
    }
}
