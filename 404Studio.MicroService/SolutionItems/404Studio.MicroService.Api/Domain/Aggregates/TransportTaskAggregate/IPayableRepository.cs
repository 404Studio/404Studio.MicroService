using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Swagger;
using YH.Etms.Utility.Models.ResponseModel;

namespace YH.Etms.Settlement.Api.Domain.Aggregates.TransportTaskAggregate
{
    public interface IPayableRepository
    {
        /// <summary>
        /// 初始化应付信息
        /// </summary>
        /// <param name="payable"></param>
        /// <returns></returns>
        Task<Response<int>> Init(Payable payable);
        /// <summary>
        /// 新建应付信息
        /// </summary>
        /// <param name="payable"></param>
        /// <returns></returns>
        Task<int> Create(Payable payable);
    }
}
