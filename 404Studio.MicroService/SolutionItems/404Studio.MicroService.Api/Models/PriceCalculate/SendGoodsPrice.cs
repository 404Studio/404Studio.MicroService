using System;

namespace YH.Etms.Settlement.Api.Models.PriceCalculate
{
    /// <summary>
    ///  送货费(元/票)
    ///  如果[直送客户，京东送费，百安居送货费，沃尔玛送货费]中有任意的费存在，则取代标准的送货费
    /// </summary>
    public class SendGoodsPrice : IPrice
    {
        public const string DefaultCode = "SHF(Y/P)";
        private decimal _number = 1;
        private string _name;
        private string _code;
        private int _id;
        private decimal _value;
        private decimal _sumPrice;
        private FreeSendWeightOrVolumePrice _freeSend;
        private string _unit;
        private decimal _lowestPrice;
        private string _remark;

        public SendGoodsPrice(decimal value, string code, int id) : this(value, "送货费", code, id)
        {

        }
        public SendGoodsPrice(decimal value, string name, string code, int id)
        {
            _value = value;
            _name = name;
            _code = code;
            _id = id;
            _unit = "元/次";
            _lowestPrice = 0;
            _number = 1;
        }
        public decimal Value => _value;
        public decimal SumPrice => _sumPrice;
        public decimal Number => _number;

        public string Name => _name;

        public string Code => _code;

        public int Id => _id;

        public FreeSendWeightOrVolumePrice FreeSend => _freeSend;

        public string Unit => _unit;

        public decimal LowestPrice => _lowestPrice;
        public string Remark => _remark;

        internal void SetZero()
        {
            _value = 0;
        }

        public FreeSendWeightOrVolumePrice SetFreeSend(FreeSendWeightOrVolumePrice freeSend)
        {
            _freeSend = freeSend;
            return _freeSend;
        }

        public void Calculate()
        {
            if (_freeSend?.Value <= 0)
            {
                _sumPrice = _value * Number;
            }
            else if (_value > _freeSend.Value)
            {
                _sumPrice = _value * Number;
            }
            else
            {
                _sumPrice = _freeSend.Value * Number;
            }
            _remark = "送货满足免送货要求不收送货费;免送货条件：总量 >= " + _freeSend.Value + _freeSend.Unit;
        }
    }
}