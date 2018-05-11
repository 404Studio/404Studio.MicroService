using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace YH.Etms.Settlement.Api.Application.Behaviors
{
    // 
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest>[] _validators;
        public ValidatorBehavior(IValidator<TRequest>[] validators) => _validators = validators;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //var failures = _validators
            //    .Select(v => v.Validate(request))
            //    .SelectMany(result => result.Errors)
            //    .Where(error => error != null)
            //    .ToList();
            //if (failures.Any())
            //{
            //    throw new TransportationDomainException(
            //        $"类型 {typeof(TRequest).Name} 对象的属性验证时发生错误。", new ValidationException("验证时发生异常", failures));
            //}

            var response = await next();
            return response;
        }
    }
}
