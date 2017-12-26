using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP
{
    public enum TransacType
    {
        //SMSAddPlan,
        //SMSAddDetail,
        //MeetingAdd,
        //MemberAdd,
        //WeiboAdd,
        //SMSAddTrigger,
        //CacheGet,
        //MenuAdd,
        //PermissionAdd,
        //PermissionGroupAdd,
        //EmailAdd,
        //EmailAddressAdd,
        //MemberGroupAdd,
        //EdmLinkAdd,
        /// <summary>
        ///ZCJ_WeixinMsgDetails 添加
        /// </summary>
        WeinxinDetailsAdd,
        /// <summary>
        /// ZCJ_WeixinMsgDetailsImgsInfo 添加
        /// </summary>
        WeixinMsgDetailsImgsAdd,
        /// <summary>
        /// ZCJ_WeixinReplyRuleInfo 添加
        /// </summary>
        WeixinReplyRuleAdd,
        /// <summary>
        /// ZCJ_WeixinReplyRuleImgsInfo 添加
        /// </summary>
        WeixinReplyRuleImgAdd,
        /// <summary>
        /// ZCJ_WeixinMsgSourceInfo 添加
        /// </summary>
        WeixinSourceAdd,
        /// <summary>
        /// ZCJ_WeixinMemberInfo 添加
        /// </summary>
        WeixinMemberAdd,
        //WeixinFlowAdd,
        //SignInAdd,
        /// <summary>
        /// ZCJ_ActivityInfo 添加
        /// </summary>
        ActivityAdd,
        //RemindAdd,
        //WeiboUserCollectAdd,
        //WeiBoEventDetailinfoAdd,
        //UserPersonalizeDataAdd,
        //HelpCategoryAdd,
        //WeiboSpiderPlanAdd,
        /// <summary>
        /// ZCJ_WeixinMenu 添加
        /// </summary>
        WeixinMenuAdd,
        //FeedBackID,
        /// <summary>
        /// 
        /// </summary>
        DialogueID,
        /// <summary>
        /// ZCJ_MonitorPlan 添加
        /// </summary>
        MonitorPlanID,
        /// <summary>
        /// ZCJ_MonitorLinkInfo 添加
        /// </summary>
        MonitorLinkID,
        //MonitorDetailID,
        /// <summary>
        /// ZCJ_WeixinSpread 添加
        /// </summary>
        WeixinSpreadID,
        //WeiboCCDBSearchPlanAdd,
        //WeiboRepostPlanAdd,
        //WeiboCommentsPlanAdd,
        //AddJuMasterUserLinkerInfo,
        //AddJuMasterFeedBackInfo,
        //AddJuMasterFeedBackDialogue,
        /// <summary>
        /// ZCJ_WeixinSpread 添加
        /// </summary>
        WXMemberInfoAdd,
        //AddJuMasterID,
        /// <summary>
        /// 添加商城商品
        /// </summary>
        AddWXMallProductID,
        /// <summary>
        /// 添加商城订单
        /// </summary>
        AddWXMallOrderInfo,
        /// <summary>
        /// 添加问卷
        /// </summary>
        AddQuestionnaire,
        /// <summary>
        /// 添加问卷-问题
        /// </summary>
        AddQuestion,
        /// <summary>
        /// 添加问答选项
        /// </summary>
        AddAnswer,
        /// <summary>
        /// 微信操作
        /// </summary>
        WXBroadcast = 100,
        /// <summary>
        /// 发送系统消息
        /// </summary>
        SendSystemNotice = 200,
        /// <summary>
        /// 添加投票
        /// </summary>
        AddVoteId,
        /// <summary>
        /// 通用而不指定类型
        /// </summary>
        CommAdd,
        /// <summary>
        /// 添加卡券
        /// </summary>
        AddCardCoupon,
        /// <summary>
        /// 添加到购物车
        /// </summary>
        AddShoppingCart,
        /// <summary>
        /// 添加商品特征量
        /// </summary>
        AddProductProperty,
        /// <summary>
        /// 添加商品特征量 值
        /// </summary>
        AddProductPropertyValue,
        /// <summary>
        /// 添加商城订单
        /// </summary>
        AddMallOrder,
        /// <summary>
        /// 添加商品SKU
        /// </summary>
        AddProductSku,
        /// <summary>
        /// 添加运费模板
        /// </summary>
        AddFreightTemplate,
        /// <summary>
        /// 添加限时特卖活动
        /// </summary>
        AddPromotionActivity,
        /// <summary>
        /// 添加抽奖活动
        /// </summary>
        AddLottery,
        /// <summary>
        /// 添加组件
        /// </summary>
        AddComponent,
        /// <summary>
        /// 政策
        /// </summary>
        AddPolicy,
        /// <summary>
        /// 网点
        /// </summary>
        AddOutlets,
        /// <summary>
        /// 支付注册会员订单
        /// </summary>
        PayRegisterOrder,
        /// <summary>
        /// 支付充值订单
        /// </summary>
        PayRechargeOrder,
        /// <summary>
        /// 线下注册注册会员
        /// </summary>
        OfflineRegister
    };
}
