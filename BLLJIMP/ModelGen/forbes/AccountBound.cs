using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.Forbes
{
    /// <summary>
    /// 账号绑定
    /// </summary>
    public class AccountBound : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 微信openId
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 微信自动登录用户名
        /// </summary>
        public string WXUserId { get; set; }
        /// <summary>
        /// 要绑定的用户名
        /// </summary>
        public string BoundUserId { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebSiteOwner { get; set; }


    }
}
