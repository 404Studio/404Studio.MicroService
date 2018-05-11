using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YH.Etms.Settlement.Api.Models.PriceCalculate
{
    /// <summary>
    /// 最低收费(元/票)
    /// </summary>
    public class LowestPrice : IPrice
    {
        private string _name;
        private string _code;
        private int _id;
        private decimal _value;
        private decimal _sumPrice;
        private string _unit;
        private int _number;
        private decimal _lowestPrice;

        //private decimal _sumValue;

        public LowestPrice(decimal value, string name, string code, int id)
        {
            _value = value;
            _name = name;
            _code = code;
            _id = id;
        }
        public decimal Value => _value;

        public string Name => _name;

        public string Code => _code;

        public int Id => _id;

        public decimal SumPrice => _sumPrice;

        public string Unit => _unit;

        public decimal Number => _number;

        decimal IPrice.LowestPrice => _lowestPrice;

        //public decimal SumValue => _sumValue;

        /// <summary>
        /// 是否大于value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool IsGreaterThan(decimal value)
        {
            return Value > value;
        }

        internal void SetZero()
        {
            _value = 0;
        }
    }
}
