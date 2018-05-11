using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YH.Etms.Settlement.Api.Application.Queries.Dtos;
using YH.Etms.Utility.Models.DataWraps;

namespace YH.Etms.Settlement.Api.Application.Queries
{
    public class PayableQuery: IPayableQuery
    {
        private readonly string _connectionString = string.Empty;

        public PayableQuery(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }

        public async Task<PagedResultOutputDto<PayableSearchResult>> SearchPayablesAsync() {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var multipleSql = Sql4SearchPayables() + ";" + Sql4SearchPayablesTotal();
                var gridReader = await connection.QueryMultipleAsync(multipleSql);
                if (gridReader.IsConsumed)
                {
                    var payResults = await gridReader.ReadAsync<PayableSearchResult>();
                    var total = await gridReader.ReadFirstAsync<long>();
                    return new PagedResultOutputDto<PayableSearchResult>
                    {
                        Items = payResults.ToList(),
                        TotalCount = (int)total
                    };
                }
                return default;
            }
        }

        private string Sql4SearchPayables() {
            var sql = @"select MAX(p.SettlementUnit) SettlementUnit,MAX(pi.Amount) Amount,MAX(pi.CreationTime) CreationTime,MAX(pi.Author) Author,
MAX(case pi.CostType WHEN 0 THEN '运费' ELSE '' END) 运费,
MAX(case pi.CostType WHEN 1 THEN '提货费' ELSE '' END) 提货费,
MAX(case pi.CostType WHEN 2 THEN '卸货费' ELSE '' END) 卸货费,
MAX(case pi.CostType WHEN 3 THEN '送货费' ELSE '' END) 送货费,
MAX(case pi.CostType WHEN 4 THEN '其他费' ELSE '' END) 其他费,
MAX(case pi.CostType WHEN 5 THEN '异常费' ELSE '' END) 异常费,
MAX(case pi.CostType WHEN 6 THEN '上楼费' ELSE '' END) 上楼费 
from payableitem pi inner join payable p on p.Id = pi.PayableId
GROUP BY 
pi.CostType";
            return sql;
        }

        private string Sql4SearchPayablesTotal()
        {
            return @"select count(1)
from payableitem pi inner join payable p on p.Id = pi.PayableId
GROUP BY 
pi.CostType";
        }
    }
}
