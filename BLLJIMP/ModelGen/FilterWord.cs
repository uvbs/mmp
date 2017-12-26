using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 过滤 关键字
    /// </summary>
   public class FilterWord: ZCBLLEngine.ModelTable
    {
       /// <summary>
       ///自动编号
       /// </summary>
       public int AutoID{get;set;}
       /// <summary>
       /// 关键字
       /// </summary>
       public string Word { get; set; }
       /// <summary>
       /// 类型 0代表文章
       /// </summary>
       public int FilterType { get; set; }
       /// <summary>
       /// 站点所有者
       /// </summary>
       public string WebsiteOwner { get; set; }


    }
}
