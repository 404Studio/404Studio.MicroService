using System;
using YH.Etms.Utility.Models.PurchaseSettlement;

namespace YH.Etms.Settlement.Api.Models.PriceCalculate
{
    /// <summary>
    /// 免提方数/重量
    /// 如果该次报价所包含的总体积或总重量（根据计价单位而定）> Value 则无需计算（加）提货费<see cref="PickGoodsPrice"/>，否则要加上提货费
    /// </summary>
    public class FreePickWeightOrVolumePrice : IPrice
    {
        private string _name;
        private string _code;
        private int _id;
        private decimal _value;
        private decimal _number;
        private string _unit;
        private decimal _sumPrice;
        private decimal _lowestPrice;

        public FreePickWeightOrVolumePrice(decimal value, string name, string code, int id)
        {
            _value = value;
            _name = name;
            _code = code;
            _id = id;
            _number = 0;
            _sumPrice = 0;
            _lowestPrice = 0;
        }
        public decimal Value => _value;

        public string Name => _name;

        public string Code => _code;

        public int Id => _id;

        public decimal SumPrice => _sumPrice;

        public string Unit => _unit;

        public decimal Number => _number;

        public decimal LowestPrice => _lowestPrice;

        internal bool IsLessThan(Goods goods, PriceUnit priceUnit)
        {
            if (Value <= 0) return false;
            if (priceUnit.UnitType == UnitTypeEnum.GJ)
                return _value < goods.Weight;
            else
                return _value < goods.Volume;
        }

        public void SetUnit(PriceUnit priceUnit)
        {
            _unit = priceUnit.UnitType.ToString();
        }
    }
}