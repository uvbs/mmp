using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// app管理
    /// </summary>
    [Serializable]
    public partial class AppManage : ModelTable
    {
        /// <summary>
        /// app管理Id
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// 应用编码
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 应用信息
        /// </summary>
        public string AppInfo { get; set; }
        /// <summary>
        /// 苹果AppId
        /// </summary>
        public string IosAppId { get; set; }
        /// <summary>
        /// 苹果私钥密码
        /// </summary>
        public string IosAppPrivate { get; set; }
        /// <summary>
        /// 苹果Profile文件
        /// </summary>
        public string IosAppProfile { get; set; }
        /// <summary>
        /// 苹果私钥证书
        /// </summary>
        public string IosAppPrivateFile { get; set; }
        /// <summary>
        /// 安卓包名
        /// </summary>
        public string AndroidAppId { get; set; }
        /// <summary>
        /// 安卓证书别名
        /// </summary>
        public string AndroidAppCertificateName { get; set; }
        /// <summary>
        /// 安卓私钥文件
        /// </summary>
        public string AndroidAppPrivate { get; set; }
        /// <summary>
        /// 安卓证书文件
        /// </summary>
        public string AndroidAppCertificateFile { get; set; }

        public string AlipayAppId { get; set; }
        public string AlipayPrivatekey { get; set; }
        public string AlipayPublickey { get; set; }
        public string AlipaySignType { get; set; }
        public string WxAppId { get; set; }
        public string WxAppSecret { get; set; }

        /// <summary>
        /// 启动广告页
        /// </summary>
        public string StartAdHref { get; set; }

    }
}