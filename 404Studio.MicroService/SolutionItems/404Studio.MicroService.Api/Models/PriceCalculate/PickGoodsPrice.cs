using System;

namespace YH.Etms.Settlement.Api.Models.PriceCalculate
{
    /// <summary>
    /// 提货费(元/次)
    /// </summary>
    public class PickGoodsPrice : IPrice
    {
        private decimal sumPrice;
        private string _name;
        private string _code;
        private int _id;
        private decimal _value;
        private FreePickWeightOrVolumePrice _freePick;
        private string _unit;
        private decimal _number;
        private decimal _LowestPrice;
        private string _remark;

        public PickGoodsPrice(decimal value, string code, int id) : this(value, "提货费", code, id)
        {

        }
        public PickGoodsPrice(decimal value, string name, string code, int id)
        {
            _value = value;
            _name = name;
            _code = code;
            _id = id;
            _unit = "元/次";
            _number = 1;
            _LowestPrice = 0;
        }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Value => _value;

        public string Name => _name;

        public string Code => _code;

        public int Id => _id;
        public decimal SumPrice => sumPrice;
        public FreePickWeightOrVolumePrice FreePick => _freePick;

        public string Unit => _unit;

        public decimal Number => _number;

        public decimal LowestPrice => _LowestPrice;

        public string Remark => _remark;

        internal void SetZero()
        {
            _value = 0;
        }

        public FreePickWeightOrVolumePrice SetFreePickCondition(FreePickWeightOrVolumePrice condition)
        {
            _freePick = condition;

            return _freePick;
        }
        /// <summary>
        /// 根据计价单位设置提送货单位
        /// </summary>
        /// <param name="unitString"></param>
        /// <returns></returns>
        public PickGoodsPrice SetUnit(string unitString)
        {
            _unit = unitString;
            return this;
        }
        /// <summary>
        /// 设置数量
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public PickGoodsPrice SetNumber(decimal number)
        {
            _number = number;
            return this;
        }

        internal void Calculate()
        {
            if (_freePick?.Value <= 0)
            {
                sumPrice = _value * _number;
            }
            else if (_value > _freePick.Value)
            {
                sumPrice = _value * _number;
            }
            else
            {
                sumPrice = _freePick.Value * _number;
            }
            _remark = "提送货满足免提要求不收提货费;免提货条件：总量 >= " + _freePick.Value + _freePick.Unit;
        }
    }
}