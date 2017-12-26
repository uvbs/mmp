using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微信加V任务表
    /// </summary>
    public class WXAddVTask : ZentCloud.ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动增长标识
        /// </summary>
        public long AutoID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 微信OpenID
        /// </summary>
        public string WeixinOpenID { get; set; }

        /// <summary>
        /// 加V图标路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 状态0:未推送，1已推送.
        /// </summary>
        public string Statu { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }



    }
}
