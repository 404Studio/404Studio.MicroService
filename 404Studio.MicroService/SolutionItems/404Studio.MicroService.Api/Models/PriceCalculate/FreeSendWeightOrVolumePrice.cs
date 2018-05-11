using System;
using YH.Etms.Utility.Models.PurchaseSettlement;

namespace YH.Etms.Settlement.Api.Models.PriceCalculate
{
    public class FreeSendWeightOrVolumePrice : IPrice
    {
        private string _name;
        private string _code;
        private int _id;
        private decimal _value;
        private decimal _sumPrice;
        private string _unit;
        private decimal _number;
        private decimal _lowestPrice;

        public FreeSendWeightOrVolumePrice(decimal value, string name, string code, int id)
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
            _unit = priceUnit.ToString();
        }
    }
}