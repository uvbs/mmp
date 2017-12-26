using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.Weixin
{
    /// <summary>
    /// jsapi配置 模型
    /// </summary>
    [Serializable]
    public class JsapiConfigModel
    {
        /// <summary>
        /// 必填，公众号的唯一标识
        /// </summary>
        public string appId { get; set; }
        /// <summary>
        /// 必填，生成签名的时间戳
        /// </summary>
        public long timestamp { get; set; }
        /// <summary>
        ///必填，生成签名的随机串
        /// </summary>
        public string nonceStr { get; set; }
        /// <summary>
        ///必填，签名
        /// </summary>
        public string signature { get; set; }
        /// <summary>
        /// 卡券的签名
        /// </summary>
        public string cardSign { get; set; }

        public string cardSN { get; set; }
        public string cardCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cardTicket { get; set; }

    }
}
