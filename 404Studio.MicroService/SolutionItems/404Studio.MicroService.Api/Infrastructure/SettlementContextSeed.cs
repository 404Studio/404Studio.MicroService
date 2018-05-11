using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Polly;

namespace YH.Etms.Settlement.Api.Infrastructure
{
    public class SettlementContextSeed
    {
        public async Task SeedAsync(SettlementContext context, IHostingEnvironment env, ILogger<SettlementContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(SettlementContext));

            await policy.ExecuteAsync(async () =>
            {

                if (env.IsDevelopment())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                    //if (!context.AssVehJobRelas.Any())
                    //{
                    //    context.AssVehJobRelas.AddRange(GetAssVehJobRelas());
                    //    await context.SaveChangesAsync();
                    //}
                }
                else
                {
                    context.Database.Migrate();
                }
            });
        }




        private Policy CreatePolicy(ILogger<SettlementContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy
                .Handle<SqlException>().Or<MySqlException>()
                .WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogTrace($"[{prefix}] Exception {exception.GetType().Name} with message ${exception.Message} detected on attempt {retry} of {retries}");
                    }
                );
        }
    }
}
