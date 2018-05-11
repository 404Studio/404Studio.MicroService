using System;

namespace YH.Etms.Settlement.Api.Domain.Exceptions
{
    public class SettlementDomainException : Exception
    {
        public SettlementDomainException()
        { }

        public SettlementDomainException(string message)
            : base(message)
        { }

        public SettlementDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
