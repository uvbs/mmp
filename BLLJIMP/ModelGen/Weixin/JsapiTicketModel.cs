using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.Weixin
{
    /// <summary>
    /// 获取jsapi_ticket 模型
    /// </summary>
   [Serializable]
   public class JsapiTicketModel
   {
       /// <summary>
       /// 错误码
       /// </summary>
       public int errcode { get; set; }
       /// <summary>
       /// 错误信息
       /// </summary>
       public string errmsg { get; set; }
       /// <summary>
       /// ticket
       /// </summary>
       public string ticket { get; set; }
       /// <summary>
       /// 过期秒数
       /// </summary>
       public int expires_in { get; set; }
   }
}
