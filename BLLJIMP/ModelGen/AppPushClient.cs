using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// ZCJ_AppPushClient
    /// </summary>
    [Serializable]
    public partial class AppPushClient : ModelTable
    {
        /// <summary>
        /// AutoID
        /// </summary>
        public long AutoID { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 手机唯一码id
        /// </summary>
        public string UUId { get; set; }
        /// <summary>
        /// 推送AppId
        /// </summary>
        public string PushAppId { get; set; }
        /// <summary>
        /// 推送客户端Id
        /// </summary>
        public string PushClientId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 首次启动时间
        /// </summary>
        public DateTime InsertDate { get; set; }
    }
}