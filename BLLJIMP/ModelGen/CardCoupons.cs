using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 卡券主表
    /// </summary>
    public partial class CardCoupons : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 卡券编号
        /// </summary>
        public int CardId { get; set; }
       /// <summary>
        /// 卡券类型
        ///MallCardCoupon_Discount    折扣券：凭折扣券对指定商品（全场）打折
        ///MallCardCoupon_Deductible  抵扣券：支付时可以抵扣现金
        ///MallCardCoupon_FreeFreight 免邮券：满一定金额包邮
        ///MallCardCoupon_Buckle      满扣券：消费满一定金额减去一定金额
        ///MallCardCoupon_BuckleGive  满送券：消费满一定金额减去一定金额
        /// </summary>
        public string CardCouponType { get; set; }
        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime? ValidFrom { get; set; }
        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? ValidTo { get; set; }
        /// <summary>
        /// 卡券名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 卡券LOGO
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 最多发放数量0不限制 总数量
        /// </summary>
        public int MaxCount { get; set; }

        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebSiteOwner { get; set; }
        /// <summary>
        /// 创建者用户名
        /// </summary>
        public string CreateUserId { get; set; }
        /// <summary>
        ///创建日期
        /// </summary>
        public DateTime InsertDate { get; set; }
        /// <summary>
        /// 扩展字段1 折扣券 折扣1-10
        /// </summary>
        public string Ex1 { get; set; }
        /// <summary>
        /// 扩展字段2 商品ID (商品ID跟商品标签二选一)
        /// </summary>
        public string Ex2 { get; set; }
        /// <summary>
        /// 扩展字段3 抵扣券 抵扣金额
        /// </summary>
        public string Ex3 { get; set; }
        /// <summary>
        /// 扩展字段4 免邮券 满多少元包邮
        /// </summary>
        public string Ex4 { get; set; }
        /// <summary>
        /// 扩展字段5 满扣券 满多少金额
        /// </summary>
        public string Ex5 { get; set; }
        /// <summary>
        /// 扩展字段6 满扣券 满送券 可以减多少金额
        /// </summary>
        public string Ex6 { get; set; }
        /// <summary>
        /// 扩展字段7  限制类型 (空不限制 0 商品id 1商品标签)
        /// </summary>
        public string Ex7 { get; set; }
        /// <summary>
        /// 扩展字段8 商品标签 (商品ID跟商品标签二选一)
        /// </summary>
        public string Ex8 { get; set; }

        public string Ex9 { get; set; }
        /// <summary>
        /// 扩展字段10
        /// </summary>
        public string Ex10 { get; set; }
        /// <summary>
        /// 获取限制类型：空不限制、1限制为分销会员（有消费历史）才能领取、2限制为非分销会员
        /// </summary>
        public string GetLimitType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsCanUseShop { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsCanUseGroupbuy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsCanUseGroupbuyMember { get; set; }
        /// <summary>
        /// 绑定渠道UserID
        /// </summary>
        public string BindChannelUserId { get; set; }
        /// <summary>
        /// 过期类型
        /// 空 主卡券填的日期
        /// 1 从领取之日算的过期天数
        /// </summary>
        public string ExpireTimeType { get; set; }
        /// <summary>
        /// 过期类型
        /// 从领取之日算的过期天数 ExpireTimeType为1时有值
        /// </summary>
        public string ExpireDay { get; set; }
        /// <summary>
        /// 是否关注自动赠送
        /// True
        /// </summary>
        public string IsSubscribeGive { get; set; }

        /// <summary>
        /// 订单金额满多少元自动赠送此券
        /// </summary>
        public string FullGive { get; set; }

        /// <summary>
        /// 微信卡券Id
        /// </summary>
        public string WeixinCardId { get; set; }
        /// <summary>
        /// 微信二维码地址
        /// </summary>
        public string WeixinQrCodeUrl { get; set; }
        /// <summary>
        /// 门店id
        /// </summary>
        public string StoreIds { get; set; }
        /// <summary>
        /// 限制金额 最小金额
        /// </summary>
        public decimal LimitAmount { get; set; }
        /// <summary>
        /// 限制数量 最小购买数量
        /// </summary>
        public int LimitCount { get; set; }
        /// <summary>
        /// 限制分类
        /// </summary>
        public string Categorys { get; set; }
        /// <summary>
        /// 是否正价 商品才能使用
        /// </summary>
        public int IsPrePrice { get; set; }

        /// <summary>
        /// 翼码活动Id
        /// </summary>
        public string YimaActivityId { get; set; }
        /// <summary>
        /// 翼码制卡流水号
        /// </summary>
        public string YimaMakeCardTransId { get; set; }

    }
}
