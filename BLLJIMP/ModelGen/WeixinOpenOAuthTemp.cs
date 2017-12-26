using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微信开放平台临时表
    /// </summary>
    public class WeixinOpenOAuthTemp : ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// Token
       /// </summary>
       public string Token { get; set; }
       /// <summary>
       /// 跳转Url
       /// </summary>
       public string Url { get; set; }

    }
}
