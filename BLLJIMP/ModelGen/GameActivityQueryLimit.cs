using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 限制只能查询的活动
    /// </summary>
   public class GameActivityQueryLimit : ZCBLLEngine.ModelTable
    {
       public int AutoID { get; set; }
       /// <summary>
       /// 活动ID
       /// </summary>
       public int ActivityID { get; set; }
       /// <summary>
       /// 活动名称
       /// </summary>
       public string ActivityName { get; set; }
    }
}
