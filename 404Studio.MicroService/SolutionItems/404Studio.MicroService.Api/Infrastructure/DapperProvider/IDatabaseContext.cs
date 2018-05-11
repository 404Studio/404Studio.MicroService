using System;
using NPoco;

namespace YH.Etms.Settlement.Api.Infrastructure.DapperProvider
{
    public interface IDatabaseContext:IDisposable
    {
        Database Database { get; }
    }
}
