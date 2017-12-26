using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.Weixin
{
    /// <summary>
    /// 微信AccessToken实体
    /// </summary>
   public class WeixinAccessToken
    {
       /// <summary>
       /// 微信AccessToken
       /// </summary>
       public string access_token{get;set;}
       /// <summary>
       /// 过期时间 单位:秒
       /// </summary>
       public int expires_in{get;set;}

       /// <summary>
       /// 错误码
       /// </summary>
       public int errcode { get; set; }

       /// <summary>
       /// 错误信息
       /// </summary>
       public string errmsg { get; set; }
    }
}
