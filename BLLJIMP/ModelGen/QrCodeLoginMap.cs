using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 二维码登录凭证映射
    /// </summary>
    public class QrCodeLoginMap: ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 登录凭据
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime InsertDate { get; set; }

    }
}
