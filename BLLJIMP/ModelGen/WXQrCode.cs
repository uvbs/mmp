using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微信二维码
    /// </summary>
   public class WXQrCode: ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 自动编号
       /// </summary>
       public int AutoId { get; set; }
       /// <summary>
       /// 二维码地址
       /// </summary>
       public string QrCodeUrl { get; set; }
       /// <summary>
       /// 二维码类型
       /// WeiXinRecommendFans
       /// WeiXinLimitScene
       /// </summary>
       public string QrCodeType { get; set; }
       /// <summary>
       /// 用户id
       /// </summary>
       public string UserId { get; set; }
       /// <summary>
       /// 站点所有者
       /// </summary>
       public string WebsiteOwner { get; set; }
       /// <summary>
       /// id
       /// </summary>
       public string Id { get; set; }
       /// <summary>
       /// 二维码内容
       /// </summary>
       public string Scene { get; set; }
    }
}
