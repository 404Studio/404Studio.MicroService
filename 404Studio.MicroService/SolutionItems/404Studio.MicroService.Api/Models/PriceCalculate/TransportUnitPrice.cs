using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YH.Etms.Utility.Models.PurchaseSettlement;

namespace YH.Etms.Settlement.Api.Models.PriceCalculate
{
    /// <summary>
    /// 运单单价
    /// 与最低报价比较，如果大于最低报价，Value不变；否则Value = lowestPrice
    /// </summary>
    public class TransportUnitPrice : IPrice
    {
        private string _name;
        private string _code;
        private int _id;
        /// <summary>
        /// 单价
        /// </summary>
        private decimal _value;
        private LowestPrice _lowestPrice;
        private decimal _lowerPrice;
        private string _unit;
        private decimal _sumPrice;
        private decimal _number;

        public TransportUnitPrice(decimal value, string code, int id) : this(value, "运费", code, id)
        {

        }

        public TransportUnitPrice(decimal value, string name, string code, int id)
        {
            _value = value;
            _name = name;
            _code = code;
            _id = id;
            _number = 1;
        }
        public decimal Value => _value;

        public string Name => _name;

        public string Code => _code;

        public int Id => _id;

        public LowestPrice LowestPrice => _lowestPrice;

        public decimal SumPrice => _sumPrice;

        public string Unit => _unit;

        public decimal Number => _number;

        decimal IPrice.LowestPrice => _lowerPrice;

        public decimal CompareLowest()
        {
            if (LowestPrice == null) return _value;
            return _value > LowestPrice.Value ? _value : LowestPrice.Value;
        }
        /// <summary>
        /// 体积/重量 * 单价value
        /// 并设置数量number
        /// </summary>
        /// <param name="weightOrheightValue"></param>
        public void CalculateTransportPrice(decimal weightOrheightValue)
        {
            _number = weightOrheightValue;
            _sumPrice = CompareLowest() * weightOrheightValue;
        }
        /// <summary>
        /// 设置最低价格
        /// </summary>
        /// <param name="lowestPrice"></param>
        /// <returns></returns>
        public TransportUnitPrice SetLowestPrice(LowestPrice lowestPrice)
        {
            _lowestPrice = lowestPrice;
            _lowerPrice = lowestPrice.Value;
            return this;
        }
        /// <summary>
        /// 设置单位
        /// </summary>
        /// <returns></returns>
        public TransportUnitPrice SetUnit(PriceUnit priceUnit)
        {
            _unit = priceUnit.ToString();
            return this;
        }


    }
}
