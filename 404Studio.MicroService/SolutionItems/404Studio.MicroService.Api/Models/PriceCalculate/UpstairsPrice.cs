using System;

namespace YH.Etms.Settlement.Api.Models.PriceCalculate
{
    internal class UpstairsPrice : IPrice
    {
        private string _name;
        private string _code;
        private int _id;
        private decimal _value;
        private decimal _number;
        private decimal _sumPrice;
        private decimal _lowestPrice;
        private string _unit;

        public UpstairsPrice(decimal value, string code, int id) : this(value, "上楼费", code, id) { }
        public UpstairsPrice(decimal value, string name, string code, int id)
        {
            _value = value;
            _name = name;
            _code = code;
            _id = id;
            _lowestPrice = 0;
            _number = 1;
        }
        public decimal Value => _value;
        public decimal SumPrice => _sumPrice;
        public decimal Number => _number;

        public string Name => _name;

        public string Code => _code;

        public int Id => _id;

        public string Unit => _unit;

        public decimal LowestPrice => _lowestPrice;

        internal void Calculate(decimal weightOrvolume)
        {
            _number = weightOrvolume;
            _sumPrice = _value * _number;
        }
        public UpstairsPrice SetNumber(decimal number) {
            _number = number;
            return this;
        }
        public UpstairsPrice SetUnit(PriceUnit priceUnit)
        {
            _unit = priceUnit.ToString();
            return this;
        }

        public UpstairsPrice SetLowestPrice(decimal price)
        {
            _lowestPrice = price;
            return this;
        }
    }
}