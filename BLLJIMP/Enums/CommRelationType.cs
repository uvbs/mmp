using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Enums
{
    /// <summary>
    /// 内容关系类型
    /// </summary>
    public enum CommRelationType
    {
        /// <summary>
        /// JuActivity表的  文章、活动、动态、评论、回复  内容标签关联
        /// </summary>
        JuActivityTag,
        /// <summary>
        /// JuActivity表内容的收藏，mainid=juid，relationid-userid
        /// </summary>
        JuActivityFavorite,
        /// <summary>
        /// JuActivity表内容的点赞，主id: juId 从id：userId
        /// </summary>
        JuActivityPraise,
        /// <summary>
        /// JuActivity表内容的回复的回复，主id:回复的id，从id:被回复的id
        /// </summary>
        JuActivityReplyToReply,
        /// <summary>
        /// JuActivity表内容非法举报，mainid=juid，relationid-userid
        /// </summary>
        ReportJuActivityIllegalContent,
        /// <summary>
        /// JuActivity表内容关注，mainid=juid，relationid-userid
        /// </summary>
        JuActivityFollow,
        /// <summary>
        ///  Review表内容非法举报，mainid=ReviewMainId，relationid-userid
        /// </summary>
        ReportReviewIllegalContent,
        /// <summary>
        /// Review表内容的收藏，mainid=ReviewMainId，relationid-userid
        /// </summary>
        ReviewFavorite,
        /// <summary>
        /// Review表内容的点赞,主id: ReviewMainId 从id：userId
        /// </summary>
        ReviewPraise,
        /// <summary>
        /// 关注文章分类
        /// </summary>
        FollowArticleCategory,
        /// <summary>
        /// 关注用户  mainid 关注对象 relationid 谁去关注
        /// </summary>
        FollowUser,
        /// <summary>
        /// 申请成为专家
        /// </summary>
        ApplyToTutor,
        /// <summary>
        /// 邀请回答
        /// </summary>
        InviteAnswer,
        /// <summary>
        /// 每日案例
        /// </summary>
        DailyCase,
        /// <summary>
        /// 购买公开课
        /// </summary>
        ViewOpenClass,
        /// <summary>
        /// 商品收藏 mainid=userid relationid-productid
        /// </summary>
        ProductCollect,
        /// <summary>
        /// 楼盘收藏 mainid=userid relationid-productid
        /// </summary>
        HouseCollect,
        /// <summary>
        /// 站点是否自定义菜单，如果自定义则不显示默认菜单而显示自定义的菜单
        /// mainid=websiteOwner,relationid=空
        /// </summary>
        WebsiteOwnerIsCustomMenu,
        /// <summary>
        /// 站点是否不允许自动注册微信用户,mainid=userid，relationid=userid
        /// 如果 ExpandId == "1"，则不自动注册而且不会跳转到注册绑定页 
        /// 
        /// 并且虚拟了一个，
        /// 0自动注册
        /// 1手动注册(跳转注册页)
        /// 2手动注册(不跳转注册页)
        /// 
        /// </summary>
        WebsiteIsNotAutoRegNewWxUser,
        /// <summary>
        /// 购车车型库： mainid=userid,relationid=汽车品牌id
        /// </summary>
        BuyCarBrand,
        /// <summary>
        /// 养车车型库： mainid=userid,relationid=汽车品牌id
        /// </summary>
        ServiceCarBrand,
        /// <summary>
        /// 公司用户关系：mainid=companyId,relationid=userId
        /// </summary>
        CompanyUser,
        /// <summary>
        /// 汽车服务配件关系：mainid=serverid，relationid=partid
        /// </summary>
        CarServerPart,
        /// <summary>
        /// 同步efast：mainid=websiteowner
        /// </summary>
        SyncEfast,
        /// <summary>
        /// 同步驿氪：mainid=websiteowner
        /// </summary>
        SyncYike,
        /// <summary>
        /// 汽车服务默认部件：mainid=serverid，relationid=partsid，ex1为数量
        /// </summary>
        CarServerPartsDef,
        /// <summary>
        /// 汽车服务商户指定部件：mainid=serverid，relationid=partsid，expandid=sallerid，ex1为数量
        /// </summary>
        CarServerPartsSaller,
        /// <summary>
        /// 汽车服务商户关联：mainid=serverid,relationid=sallerid
        /// </summary>
        CarServerSaller,
        /// <summary>
        /// 商户车系关联：mainid=sallerid,relation=CarSeriesId,ex1=carbrandId,ex2=carbrandName,ex3=carSeriesCateId,ex4=carSeriesCateName,ex5=carSeriesName
        /// </summary>
        SallerCarSeries,
        /// <summary>
        /// 商家评分和评价：mainid=userid 评分者，relationId=userid 评分对象商家id，ex1= 商家信誉 分数值，ex2= 服务态度分数值，ex3= 评价内容 
        /// </summary>
        SallerRateScore,

        /// <summary>
        /// 汽车服务订单评分和评价：mainId=userId 评分者，relationId=orderId 订单id，ex1 = 评价分值，ex2 = 评价内容
        /// </summary>
        CarServerOrderRateScore,
        /// <summary>
        /// 实名认证  mainid=姓名,relationid=身份证号码,
        /// </summary>
        NameCertification,
        /// <summary>
        /// 站点未开通微信高级认证时授权页是否必须登录（需微信授权页面，自动跳转至登录页），
        /// </summary>
        WXAuthPageMustLogin,
        /// <summary>
        /// 好友申请
        /// </summary>
        FriendApply,
        /// <summary>
        /// 好友
        /// </summary>
        Friend,
        /// <summary>
        /// 打赏
        /// </summary>
        Reward
    }
}
