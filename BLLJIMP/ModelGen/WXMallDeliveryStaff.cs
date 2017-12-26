using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微商城 配送员管理
    /// </summary>
    public class WXMallDeliveryStaff : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 配送员管理
        /// </summary>
        public string StaffName { get; set; }

        /// <summary>
        /// 配送员手机号
        /// </summary>
        public string StaffPhone { get; set; }

        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }


    }
}
