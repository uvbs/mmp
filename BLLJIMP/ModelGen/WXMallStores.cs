using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 门店管理
    /// </summary>
    public class WXMallStores : ZCBLLEngine.ModelTable
    {   
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 门店名称
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 门店地址
        /// </summary>
        public string StoreAddress { get; set; }
        /// <summary>
        /// 是否默认门店地址 1表示默认 
        /// </summary>
        public string IsDefaultStore { get; set; }
        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }



    }
}
