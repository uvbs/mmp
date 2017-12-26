using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.step5
{
    /// <summary>
    /// 微信登录二维码凭据
    /// </summary>
   public class QrCode
    {
        /// <summary>
        /// 登录凭据
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 二维码登录图片地址
        /// </summary>
        public string imgurl { get; set; }
    }
}
