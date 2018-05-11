using MediatR;

namespace YH.Etms.Settlement.Api.Domain.Events.Payable
{
    /// <summary>
    /// 应付信息初始化后发出的事件
    /// </summary>
    public class PayableInitedDomainEvent : INotification
    {
        public int Id;

        public PayableInitedDomainEvent(int id) { Id = id; }
    }
}
