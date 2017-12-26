using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.Weixin
{
    /// <summary>
    /// 微信二维码Tiket 返回实体
    /// </summary>
    public class WeixinQrcodeTicketResp
    {
        public string ticket { get; set; }

        public string expire_seconds { get; set; }

        public string url { get; set; }
    }
}
