using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 用户统计（访客）
    /// </summary>
   [Serializable]
   public class UserStatistics:ZentCloud.ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 自增id 主键
       /// </summary>
       public int AutoId { get; set; }

       /// <summary>
       /// 用户id
       /// </summary>
       public string UserId { get; set; }

       /// <summary>
       /// 微信昵称
       /// </summary>
       public string WXNickName { get; set; }

       /// <summary>
       /// 微信头像
       /// </summary>
       public string WXHeadimgurl { get; set; }

       /// <summary>
       /// 真实姓名
       /// </summary>
       public string TrueName { get; set; }

       /// <summary>
       /// 时间类型(昨天[day]、近7天[week]、近30天[month])
       /// </summary>
       public string DateType { get; set; }

       /// <summary>
       /// 更新时间
       /// </summary>
       public DateTime UpdateDate { get; set; }

       /// <summary>
       /// 站点所有者
       /// </summary>
       public string WebsiteOwner { get; set; }

       /// <summary>
       /// 访问次数
       /// </summary>
       public int VisitCount { get; set; }

       /// <summary>
       /// 文章访问次数
       /// </summary>
       public int ArticleBrowseCount { get; set; }

       /// <summary>
       /// 活动访问次数
       /// </summary>
       public int ActivityBrowseCount { get; set; }

       /// <summary>
       /// 活动报名访问次数
       /// </summary>
       public int ActivitySignUpCount { get; set; }

       /// <summary>
       /// 下单次数
       /// </summary>
       public int OrderCount { get; set; }

       /// <summary>
       /// 积分
       /// </summary>
       public int Score { get; set; }

       /// <summary>
       /// 其他访问次数
       /// </summary>
       public int OtherBrowseCount { get; set; }

    }
}
