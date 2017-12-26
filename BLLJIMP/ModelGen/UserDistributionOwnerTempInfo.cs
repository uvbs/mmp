using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 
    /// 用户分销上级临时存储：
    /// 如果站点不允许自动注册，而又关注了推广二维码，则记录临时关系到该表
    /// 用户绑定手机的时候，判断是否是有临时关系，有则把临时关系记录到正式用户数据上
    /// 
    /// </summary>
    [Serializable]
    public class UserDistributionOwnerTempInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        public int AutoId { get; set; }

        public string DistributionOwner { get; set; }

        public string OpenId { get; set; }

        public DateTime InsertTime { get; set; }

        /// <summary>
        /// 状态：0， 1已绑定且关系为当前临时关系
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 来源：qrcode 微信推广二维码扫描
        /// </summary>
        public string FromSource { get; set; }
    }
}
