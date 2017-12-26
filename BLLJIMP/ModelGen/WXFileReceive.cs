using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微信公众平台接收文件
    /// </summary>
    public class WXFileReceive : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 微信OpenID
        /// </summary>
        public string  WeixinOpenID { get; set; }
        /// <summary>
        ///本地路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 文件类型 图片：image
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// 媒体ID
        /// </summary>
        public string MediaID { get; set; }
        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

    }
}
