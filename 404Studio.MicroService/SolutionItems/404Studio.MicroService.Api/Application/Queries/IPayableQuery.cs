using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YH.Etms.Settlement.Api.Application.Queries.Dtos;
using YH.Etms.Utility.Models.DataWraps;

namespace YH.Etms.Settlement.Api.Application.Queries
{
    public interface IPayableQuery
    {
        Task<PagedResultOutputDto<PayableSearchResult>> SearchPayablesAsync();
    }
}
