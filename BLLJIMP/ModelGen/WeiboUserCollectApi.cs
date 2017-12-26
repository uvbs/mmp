using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    [Serializable]
   public class WeiboUserCollectApi
    {
       /// <summary>
       /// 分组ID
       /// </summary>
       public string groupId { get; set; }

       /// <summary>
       /// 分组名称
       /// </summary>
       public string groupName { get; set; }

       /// <summary>
       /// 微博用户列表
       /// </summary>
       public List<WeiboUserCollect> items{get;set;}
    }
}
