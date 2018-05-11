using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YH.Etms.Settlement.Api.Application.Queries.Dtos
{
    public class PayableSearchResult
    {
        public string SettlementUnit { get; set; }
        public string Author { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreationTime { get; set; }
        public decimal TransportFee { get; set; }
        public decimal PickGoodsFee { get; set; }
        public decimal UnloadGoodsFee { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal SomeElseFee { get; set; }
        public decimal ExceptionFee { get; set; }
        public decimal UpFloorFee { get; set; }
    }
}
