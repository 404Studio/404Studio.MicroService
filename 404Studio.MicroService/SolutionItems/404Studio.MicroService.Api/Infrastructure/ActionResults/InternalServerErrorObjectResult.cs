using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YH.Etms.Settlement.Api.Infrastructure.ActionResults
{
    /// <summary>
    /// 服务器内部错误返回对象
    /// </summary>
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
