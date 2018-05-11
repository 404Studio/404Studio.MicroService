using System;

namespace YH.Etms.Settlement.Api.Infrastructure.Tools
{
    public static class CalculateTool
    {
        /// <summary>
        /// 用于将一个decimal值分为多份，合计结果与原decimal值相同
        /// </summary>
        /// <param name="sum">decimal值</param>
        /// <param name="count">份数</param>
        /// <param name="isLast">是否最后一份</param>
        /// <param name="digit">保留位数</param>
        /// <returns>返回每份的值</returns>
        public static decimal GetDecimalAverageVal(decimal sum, int count, bool isLast, int digit)
        {
            return Math.Round(isLast ? sum - Math.Round(sum / count, digit) * (count - 1) : sum / count, digit);
        }

        /// <summary>
        /// 用于将一个int值分为多份，int
        /// </summary>
        /// <param name="sum">int</param>
        /// <param name="count">份数</param>
        /// <param name="isLast">是否最后一份</param>
        /// <returns>返回每份的值</returns>
        public static int GetIntAverageVal(int sum, int count, bool isLast)
        {
            return isLast ? sum - sum / count * (count - 1) : sum / count;
        }
    }
}
