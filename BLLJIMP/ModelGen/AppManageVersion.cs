using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// app版本管理
    /// </summary>
    [Serializable]
    public partial class AppManageVersion : ModelTable
    {
        /// <summary>
        /// AutoID
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// App管理Id
        /// </summary>
        public int ManageId { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 所属平台
        /// </summary>
        public string AppOS { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string AppVersion { get; set; }
        /// <summary>
        /// 版本信息
        /// </summary>
        public string AppVersionInfo { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? AppVersionPublishDate { get; set; }
        /// <summary>
        /// 发布平台
        /// </summary>
        public string AppVersionPublish { get; set; }
        /// <summary>
        /// 发布地址
        /// </summary>
        public string AppVersionPublishPath { get; set; }
        /// <summary>
        /// 安装地址
        /// </summary>
        public string AppVersionInstallPath { get; set; }
    }
}