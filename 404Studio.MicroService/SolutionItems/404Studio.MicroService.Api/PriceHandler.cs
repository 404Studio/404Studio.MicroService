using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YH.Etms.Settlement.Api.Models.PriceCalculate;
using YH.Etms.Utility.Models.PurchaseSettlement;

namespace YH.Etms.Settlement.Api
{
    /// <summary>
    /// 价格计算Handler
    /// </summary>
    public class PriceHandler
    {
        private readonly List<IPrice> _prices;
        private readonly PriceUnit _priceUnit;
        private readonly Goods _goods;
        private readonly bool _isWeight;
        private readonly bool _isVolume;
        private decimal _priceValue;
        public PriceHandler(List<IPrice> prices, PriceUnit priceUnit, Goods goods)
        {
            _prices = prices;
            _priceUnit = priceUnit;
            _goods = goods;
            _isVolume = priceUnit.UnitType == UnitTypeEnum.LF;
            _isWeight = !_isVolume;
        }

        public void Handle()
        {
            if (_prices.Count == 0) throw new ArgumentException("价格信息不能为空");
            // 取得运单单价
            var transportPrice = GetPrice<TransportUnitPrice>();
            // 取最低费用 根据计价单位算先决费用
            var lowestPrice = GetPrice<LowestPrice>();
            transportPrice.SetLowestPrice(lowestPrice)
                .SetUnit(_priceUnit)
                .CalculateTransportPrice(_isVolume ? _goods.Volume : _goods.Weight);
            AppandPrice(transportPrice.SumPrice);
            // 卸货费
            var unloadPrice = GetPrice<UnloadGoodsPrice>();
            unloadPrice.SetPriceUnit(_priceUnit)
                .Calculate(_isVolume ? _goods.Volume : _goods.Weight);
            AppandPrice(unloadPrice.SumPrice);
            // 取提货费类
            var pickGoodsPrice = GetPrice<PickGoodsPrice>();
            // 获取免提货费体积/重量
            var freePickGoodsVolumeOrWeight = GetPrice<FreePickWeightOrVolumePrice>();
            // 比较根据计价单位来比较其重量或者体积是否到达面提货费的重量或体积的临界点
            // 如果到达临界点，则无需取添加提货费，否则加上提货费
            pickGoodsPrice.SetUnit(_priceUnit.ToString())
                .SetNumber(_isVolume ? _goods.Volume : _goods.Weight)
                    .SetFreePickCondition(freePickGoodsVolumeOrWeight)
                    .SetUnit(_priceUnit);
            pickGoodsPrice.Calculate();
            //AppandPrice(pickGoodsPrice, freePickGoodsVolumeOrWeight);
            AppandPrice(pickGoodsPrice.SumPrice);
            // 取送货费，如果有[直送客户，京东送费，百安居送费，沃尔玛送费]任意一个，则取代送货费参与运算
            var sendGoodsPrices = GetPrices<SendGoodsPrice>();
            if(sendGoodsPrices.Any(p=>p.Value > 0))
            {
                SendGoodsPrice sendGoodsPrice = sendGoodsPrices.Where(p => p.Code != SendGoodsPrice.DefaultCode).FirstOrDefault(p => p.Value > 0);
                if (sendGoodsPrice == null)
                    sendGoodsPrice = sendGoodsPrices.FirstOrDefault(p => p.Value > 0);
                // 获取免送货费体积/重量
                var freeSendGoodsVolumeOrWeight = GetPrice<FreeSendWeightOrVolumePrice>();
                sendGoodsPrice.SetFreeSend(freeSendGoodsVolumeOrWeight)
                    .SetUnit(_priceUnit);
                sendGoodsPrice.Calculate();
                //10 比较根据计价单位来比较其重量或者体积是否到达面送货费的重量或体积的临界点
                //如果到达临界点，则无需取添加送货费，否则加上送货费
                //AppandPrice(sendGoodsPrice, freeSendGoodsVolumeOrWeight);
                AppandPrice(sendGoodsPrice.SumPrice);
            } 
            //11 上楼费 是否根据重量/体积有关 待确认
            var upstairs = GetPrice<UpstairsPrice>();
            upstairs.SetUnit(_priceUnit)
                .SetLowestPrice(0)
                .SetNumber(_isVolume ? _goods.Volume : _goods.Weight)
                .Calculate(_isVolume ? _goods.Volume : _goods.Weight);
            AppandPrice(upstairs.SumPrice);
        }
        ////追加送货费
        //private void AppandPrice(SendGoodsPrice sendGoodsPrice, FreeSendWeightOrVolumePrice freeSendGoodsVolumeOrWeight)
        //{
        //    if (freeSendGoodsVolumeOrWeight.IsLessThan(_goods, _priceUnit))
        //        sendGoodsPrice.SetZero();
        //    _priceValue += sendGoodsPrice.Value;
        //}

        ////添加提送货费
        //private void AppandPrice(PickGoodsPrice pickGoodsPrice, FreePickWeightOrVolumePrice freePickGoodsVolumeOrWeight)
        //{
        //    if (freePickGoodsVolumeOrWeight.IsLessThan(_goods, _priceUnit))
        //    {
        //        pickGoodsPrice.SetZero();
        //    }
        //    _priceValue += pickGoodsPrice.Value;
        //}

        public decimal PriceValue { get => _priceValue; }

        private T GetPrice<T>() where T : IPrice
        {
            return (T)_prices.FirstOrDefault(p => p.GetType() == typeof(T));
        }
        private List<T> GetPrices<T>() where T : IPrice
        {
            var filters = _prices.Where(p => p.GetType() == typeof(T));
            var typeFilters = filters.Cast<T>();
            return typeFilters.ToList();
            //return _prices.Where(p => p.GetType() == typeof(T)).Cast<T>().ToList();
        }

        private void AppandPrice(decimal value)
        {
            _priceValue += value;
        }

        private void SetPrice(decimal value)
        {
            _priceValue = value;
        }
    }
}
