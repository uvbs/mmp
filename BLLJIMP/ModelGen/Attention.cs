using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 关注表
    /// </summary>
    public class Attention : ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 编号
       /// </summary>
       public int AutoID { get; set; }
       /// <summary>
       /// 关注人用户名
       /// </summary>
       public string FromUserID { get; set; }
       /// <summary>
       /// 关注人姓名
       /// </summary>
       public string FromTrueName { get; set; }
       /// <summary>
       /// 被关注人用户名
       /// </summary>
       public string ToUserID { get; set; }
       /// <summary>
       /// 被关注人姓名
       /// </summary>
       public string ToTrueName { get; set; }
       /// <summary>
       /// 提交时间
       /// </summary>
       public DateTime InsertDate { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
       public string WebsiteOwner { get; set; }
    }
}
