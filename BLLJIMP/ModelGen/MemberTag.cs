using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 系统标签
    /// </summary>
   public class MemberTag:ZCBLLEngine.ModelTable
    {
       public MemberTag() 
       { }
       /// <summary>
       /// 自动编号
       /// </summary>
       public int AutoId { get; set; }
       /// <summary>
       /// 标签名称
       /// </summary>
       public string TagName { get; set; }
       /// <summary>
       /// 站点所有者
       /// </summary>
       public string WebsiteOwner { get; set; }
       /// <summary>
       /// 创建者
       /// </summary>
       public string Creator { get; set; }
       /// <summary>
       /// 创建时间
       /// </summary>
       public DateTime? CreateTime { get; set; }

       /// <summary>
       /// 标签类型
       /// </summary>
       public string TagType { get; set; }

       /// <summary>
       /// 访问级别
       /// </summary>
       public int AccessLevel { get; set; }
    }
}
