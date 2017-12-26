using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 短信验证码 记录
    /// </summary>
   public class SmsVerificationCode : ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 自动编号
       /// </summary>
       public int AutoID { get; set; }
       /// <summary>
       /// 手机号
       /// </summary>
       public string Phone { get; set; }
       /// <summary>
       /// 验证码
       /// </summary>
       public string VerificationCode { get; set; }
       /// <summary>
       /// 日期
       /// </summary>
       public DateTime InsertDate { get; set; }
       /// <summary>
       /// 网站所有者
       /// </summary>
       public string WebsiteOwner { get; set; }


    }
}
