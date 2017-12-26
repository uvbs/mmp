using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 我的卡券
    /// </summary>
    public class MyCardCoupons : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 卡券主表编号
        /// </summary>
        public int CardId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 卡券类型
        /// </summary>
        public string CardCouponType { get; set; }
        /// <summary>
        /// 卡券编号
        /// </summary>
        public string CardCouponNumber { get; set; }
        /// <summary>
        /// 插入日期
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 使用日期
        /// </summary>
        public DateTime? UseDate { get; set; }
        /// <summary>
        /// 状态 0未使用 1已使用 2已转赠
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebSiteOwner { get; set; }
        /// <summary>
        /// 赠送用户Id
        /// </summary>
        public string FromUserId { get; set; }
        /// <summary>
        /// 接收赠送用户Id
        /// </summary>
        public string ToUserId { get; set; }
        /// <summary>
        /// 接收赠送用户openid
        /// </summary>
        public string ToOpenId { get; set; }
        /// <summary>
        /// 核销码
        /// </summary>
        public string HexiaoCode { get; set; }
        /// <summary>
        /// 核销渠道
        /// </summary>
        public string HexiaoChannel { get; set; }
        /// <summary>
        /// 微信核销码
        /// </summary>
        public string WeixinHexiaoCode { get; set; }

        /// <summary>
        /// 翼码卡号
        /// </summary>
        public string YimaCardCode { get; set; }

    }
}
