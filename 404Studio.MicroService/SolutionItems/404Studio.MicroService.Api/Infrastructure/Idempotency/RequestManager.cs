using System;
using System.Threading.Tasks;
using YH.Etms.Settlement.Api.Domain.Exceptions;

namespace YH.Etms.Settlement.Api.Infrastructure.Idempotency
{
    public class RequestManager : IRequestManager
    {
        private readonly SettlementContext _context;

        public RequestManager(SettlementContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<bool> ExistAsync(Guid id)
        {
            var request = await _context.
                FindAsync<ClientRequest>(id);

            return request != null;
        }

        public async Task CreateRequestForCommandAsync<T>(Guid id)
        {
            var exists = await ExistAsync(id);

            var request = exists ?
                throw new SettlementDomainException($"Request Id {id} 已存在。") :
                new ClientRequest()
                {
                    Id = id,
                    Name = typeof(T).Name,
                    Time = DateTime.UtcNow
                };

            _context.Add(request);

            await _context.SaveChangesAsync();
        }
    }
}
