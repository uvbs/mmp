using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.HaiMa
{
    /// <summary>
    /// 海马门店
    /// </summary>
    public class HaiMaStore : ZCBLLEngine.ModelTable
    {

        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 省份区域
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 门店代码
        /// </summary>
        public string StoreCode { get; set; }
        /// <summary>
        /// 门店名称
        /// </summary>
        public string StoreName { get; set; }
    }
}
