using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YH.Etms.Settlement.Api
{
    public class AppSettings
    {
        /// <summary>
        /// 是否启用异常情况的深度跟踪
        /// </summary>
        public bool EnableTrace { get; set; }

        public string TmsPurchasingApi { get; set; }
    }
}
