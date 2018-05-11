using System;

namespace YH.Etms.Settlement.Api.Infrastructure
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 获取当天最大的时间
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ToDayMax(this DateTime date)
        {
            if (date.Hour > 0 || date.Minute > 0 || date.Second > 0)
            {
                return date;
            }
            return Convert.ToDateTime(date.ToString("yyyy-MM-dd 23:59:59"));
        }
    }
}
