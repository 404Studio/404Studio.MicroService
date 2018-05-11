using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YH.Etms.Utility.Models.PurchaseSettlement;

namespace YH.Etms.Settlement.Api.Models.PriceCalculate
{
    public class PriceCalculateFactory
    {

        public static IPrice Creator(Fee fee)
        {
            var price = Convert(fee.Value);
            switch (fee.Code)
            {
                case "YFDJ":
                    return new TransportUnitPrice(price,fee.Code, fee.Id);
                case "ZDSF(Y/P)":
                    return new LowestPrice(price, fee.Name, fee.Code, fee.Id);
                case "THF(Y/C)":
                    return new PickGoodsPrice(price, fee.Code, fee.Id);
                case "SHF(Y/P)":
                    return new SendGoodsPrice(price, fee.Code, fee.Id);
                case "MTFS/ZL":
                    return new FreePickWeightOrVolumePrice(price, fee.Name, fee.Code, fee.Id);
                case "MSFS/ZL":
                    return new FreeSendWeightOrVolumePrice(price, fee.Name, fee.Code, fee.Id);
                case "XHF":
                    return new UnloadGoodsPrice(price, fee.Name, fee.Code, fee.Id);
                case "SLF":
                    return new UpstairsPrice(price, fee.Name, fee.Code, fee.Id);
                case "JDSHF":
                case "WEMSHF":
                case "ZSKH":
                case "BAJSHF":
                    return new SendGoodsPrice(price, fee.Name, fee.Code, fee.Id);
                default:
                    return null;
            }
        }

        private static decimal Convert(string price)
        {
            decimal.TryParse(price, out decimal standardPrice);
            return standardPrice;
        }
    }
}
