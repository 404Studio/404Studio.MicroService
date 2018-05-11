using System;

namespace YH.Etms.Settlement.Api.Models.PriceCalculate
{
    /// <summary>
    /// 卸货费，根据单次指派的总体积或者总重量（根据计价单位而定） * price
    /// </summary>
    internal class UnloadGoodsPrice : IPrice
    {
        private const int number = 1;
        private PriceUnit _priceUnit;
        private string _name;
        private string _code;
        private int _id;
        private decimal _value;
        private decimal _sumValue;
        private decimal _number;
        private decimal _lowestPrice;
        private string _unit;

        public UnloadGoodsPrice(decimal value, string code, int id) : this(value, "卸货费", code, id) { }
        public UnloadGoodsPrice(decimal value, string name, string code, int id)
        {
            _value = value;
            _name = name;
            _code = code;
            _id = id;
            _lowestPrice = 0;
        }
        public decimal Value => _value;

        public string Name => _name;

        public string Code => _code;

        public int Id => _id;

        public PriceUnit PriceUnit => _priceUnit;

        public decimal SumPrice => _sumValue;

        public decimal Number => _number;

        public decimal LowestPrice => _lowestPrice;

        public string Unit => _unit;
        /// <summary>
        /// 设置计价单位
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public UnloadGoodsPrice SetPriceUnit(PriceUnit unit)
        {
            _priceUnit = unit;
            _unit = unit.ToString();
            return this;
        }
        /// <summary>
        /// 设置单位
        /// </summary>
        /// <returns></returns>
        public UnloadGoodsPrice SetUnit()
        {
            _unit = _priceUnit.ToString();
            return this;
        }
        /// <summary>
        /// 计算价格并设置数量
        /// </summary>
        /// <param name="weightOrvolume"></param>
        internal void Calculate(decimal weightOrvolume)
        {
            _number = weightOrvolume;
            _sumValue = _value * weightOrvolume;
        }
    }
}