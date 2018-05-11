using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YH.Etms.Settlement.Api.Models.PriceCalculate
{
    /// <summary>
    /// 计价单位
    /// </summary>
    public class PriceUnit
    {
        private const char SPLIT_CHAR = '/';
        public PriceUnit(string code)
        {
            Code = code;
            var infos = code.Split(SPLIT_CHAR, StringSplitOptions.RemoveEmptyEntries);
            if (infos.Length == 1) throw new InvalidOperationException("计价单位无效");

            var priceTypeString = infos[0];
            PriceType = Enum.Parse<PriceTypeEnum>(priceTypeString, ignoreCase: true);
            var unitTypeString = infos[1];
            UnitType = Enum.Parse<UnitTypeEnum>(unitTypeString, ignoreCase: true);
        }

        public PriceTypeEnum PriceType { get; }
        public UnitTypeEnum UnitType { get; }
        public string Code { get; }

        public override string ToString()
        {
            return PriceType.ToString() + "/" + UnitType.ToString();
        }
    }
}
