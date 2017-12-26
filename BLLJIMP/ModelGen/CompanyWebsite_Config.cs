using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    /// 企业微网站 网站配置
    /// </summary>
    public class CompanyWebsite_Config : ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 网站标题
        /// </summary>
        public string WebsiteTitle { get; set; }
        /// <summary>
        /// 版权
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        /// 网站所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 网站图片
        /// </summary>
        public string WebsiteImage { get; set; }

        /// <summary>
        /// 网站描述
        /// </summary>
        public string WebsiteDescription { get; set; }

        /// <summary>
        /// 导航分组名称	
        /// </summary>
        public string ShopNavGroupName { get; set; }

        /// <summary>
        /// 首页广告设置
        /// </summary>
        public string ShopAdType { get; set; }

        /// <summary>
        /// 底部工具栏
        /// </summary>
        public string BottomToolbars { get; set; }

        /// <summary>
        /// 会员标准 1需验证手机(仅验证手机) 2需完善资料(含验证手机) 3需提交申请(含验证手机，且需待审核)
        /// </summary>
        public int MemberStandard { get; set; }
        /// <summary>
        /// 会员标准说明
        /// </summary>
        public string MemberStandardDescription { get; set; }
        /// <summary>
        /// 是否有文章评论0无评论 1有评论
        /// </summary>
        public int HaveComment { get; set; }
        /// <summary>
        /// 我的卡券列表title
        /// </summary>
        public string MyCardCouponsTitle { get; set; }
        /// <summary>
        /// 微信公众号名称
        /// </summary>
        public string WeixinAccountNickName { get; set; }
        /// <summary>
        /// 分销二维码图标
        /// </summary>
        public string DistributionQRCodeIcon { get; set; }
        /// <summary>
        /// 文章底部导航组
        /// </summary>
        public string ArticleToolBarGrous { get; set; }
        /// <summary>
        /// 活动底部导航组
        /// </summary>
        public string ActivityToolBarGrous { get; set; }
        /// <summary>
        /// 拼团首页链接
        /// </summary>
        public string GroupBuyIndexUrl { get; set; }
        /// <summary>
        /// 文章活动无权限的跳转方式
        /// </summary>
        public int NoPermissionsPage { get; set; }
        /// <summary>
        /// 个人中心链接
        /// </summary>
        public string PersonalCenterLink { get; set; }

        /// <summary>
        /// 最低限额
        /// </summary>
        public decimal LowestAmount { get; set; }

        public string WeiXinBindDomain { get; set; }

        /// <summary>
        /// 电话咨询（电话）
        /// </summary>
        public string Tel { get; set; }

       /// <summary>
       /// 在线咨询（QQ）
       /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 是否显示头像 1隐藏  0显示
        /// </summary>
        public int IsHideHeadImg { get; set; }

        /// <summary>
        /// 二维码使用指南
        /// </summary>
        public string QRCodeUseGuide { get; set; }

        /// <summary>
        /// 微信昵称显示位置：0显示在头像底部，1显示在二维码底部
        /// </summary>
        public int WXNickShowPosition { get; set; }

        /// <summary>
        /// 是否显示微信昵称  0显示 1隐藏
        /// </summary>
        public int IsShowWXNickName { get; set; }

        /// <summary>
        /// 微信昵称字体颜色
        /// </summary>
        public string WXNickNameFontColor { get; set; }
        /// <summary>
        /// 是否禁用客服
        /// </summary>
        public int IsDisableKefu { get; set; }
        /// <summary>
        /// 客服链接
        /// </summary>
        public string KefuUrl { get; set; }
        /// <summary>
        /// 客服图标
        /// </summary>
        public string KefuImage { get; set; }
        /// <summary>
        /// 客服在线自动回复
        /// </summary>
        public string KefuOnLineReply { get; set; }
        /// <summary>
        /// 客服离线自动回复
        /// </summary>
        public string KefuOffLineReply { get; set; }

        /// <summary>
        /// 是否开启自定义登录页
        /// </summary>
        public int IsEnableCustomizeLoginPage { get; set; }
        /// <summary>
        /// 是否自动关闭退款
        /// </summary>
        public int IsAutoCloseRefund { get; set; }
        /// <summary>
        /// 自动关闭退款时间(天)
        /// </summary>
        public int AutoCloseRefundDay { get; set; }
        /// <summary>
        /// 登录配置
        /// </summary>
        public string LoginConfigJson { get; set; }
        /// <summary>
        /// 门店搜索范围(千米)
        /// </summary>
        public string OutletsSearchRange { get; set; }

        /// <summary>
        /// 库存模型
        /// 0 库存独立
        /// 1 同一件商品不同门店有不同库存
        /// </summary>
        public int StockType { get; set; }
        /// <summary>
        /// 自动分配订单
        /// 0 不自动
        /// 1 自动
        /// </summary>
        public int IsAutoAssignOrder { get; set; }
        /// <summary>
        /// 自动分单范围 千米
        /// </summary>
        public string AutoAssignOrderRange { get; set; }
        /// <summary>
        /// 购物车单独结算
        /// </summary>
        public int ShopCartAlongSettlement { get; set; }
        /// <summary>
        /// 是否开启门店自提
        /// </summary>
        public int IsStoreSince { get; set; }
        /// <summary>
        /// 门点自提时间段 小时
        /// </summary>
        public string StoreSinceTimeJson { get; set; }
        /// <summary>
        /// 是否送货上门
        /// </summary>
        public int IsHomeDelivery { get; set; }
        /// <summary>
        /// 最早送货时间  下单后几个小时
        /// </summary>
        public string EarliestDeliveryTime { get; set; }
        /// <summary>
        /// 送货上门时间段 小时
        /// </summary>
        public string HomeDeliveryTimeJson { get; set; }
        /// <summary>
        /// 快递发货：同城Y米以外
        /// </summary>
        public int ExpressRange { get; set; }
        /// <summary>
        /// 店员配送：同城Y米以内才可以配送。
        /// </summary>
        public int StoreExpressRange { get; set; }
        /// <summary>
        /// 是否外部第三方支付
        /// </summary>
        public int IsOutPay { get; set; }
        /// <summary>
        /// 门店自提优惠(元)
        /// </summary>
        public int StoreSinceDiscount { get; set; }

    }
}
