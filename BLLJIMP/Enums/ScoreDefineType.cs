using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Enums
{
    /// <summary>
    /// 积分规则
    /// </summary>
    public enum ScoreDefineType
    {
        /// <summary>
        /// 充值
        /// </summary>
        Recharge = 0,
        /// <summary>
        /// 注册得积分
        /// </summary>
        Register = 1,
        /// <summary>
        /// 分享得积分
        /// </summary>
        ShareRegister = 2,
        /// <summary>
        /// 通过分享的链接注册得额外积分
        /// </summary>
        RegisterFromShare = 3,
        /// <summary>
        /// 每日首次登录获得积分
        /// </summary>
        DayLogin = 4,
        /// <summary>
        /// 分享问题得积分
        /// </summary>
        ShareQuestions = 5,
        /// <summary>
        /// 分享新闻、问答
        /// </summary>
        ShareArticle = 6,
        /// <summary>
        /// 分享案例
        /// </summary>
        ShareCase = 7,
        /// <summary>
        /// 发布问题得积分
        /// </summary>
        AddQuestions = 8,
        /// <summary>
        /// 发布新闻、问答
        /// </summary>
        AddArticle = 9,
        /// <summary>
        /// 发布案例
        /// </summary>
        AddCase = 10,
        /// <summary>
        /// 回答问题
        /// </summary>
        AnswerQuestions = 11,
        /// <summary>
        /// 被点赞
        /// </summary>
        BePraise = 12,
        /// <summary>
        /// 被警告
        /// </summary>
        BeWarned = 13,
        /// <summary>
        /// 购买课程
        /// </summary>
        OpenClass = 14,
        /// <summary>
        /// 问题被删除
        /// </summary>
        DelQuestions = 15,
        /// <summary>
        /// 文章被删除
        /// </summary>
        DelArticle = 16,
        /// <summary>
        /// 回答被删除
        /// </summary>
        DelAnswer = 17,
        /// <summary>
        /// 评论被删除
        /// </summary>
        DelReview = 18,
        /// <summary>
        /// 管理员修改
        /// </summary>
        ManageUpdate = 19,
        /// <summary>
        /// 抽奖
        /// </summary>
        Lottery = 20,
       /// <summary>
        /// 活动签到
        /// </summary>
        SignIn = 21,
        /// <summary>
        /// 阅读分类
        /// </summary>
        ReadCategory=22,
        /// <summary>
        /// 完善个人资料
        /// </summary>
        UpdateMyInfo=23,
        /// <summary>
        /// 订单付款
        /// </summary>
        OrderPay=24,
        /// <summary>
        /// 订单提交
        /// </summary>
        OrderSubmit=25,
        /// <summary>
        /// 订单交易成功
        /// </summary>
        OrderSuccess=26,
        /// <summary>
        /// 订单交易成功
        /// </summary>
        OrderCancel = 27,
        /// <summary>
        /// 阅读文章
        /// </summary>
        ReadArticle=28,
        /// <summary>
        /// 答题获取积分
        /// </summary>
        QuestionnaireSet=29,
        /// <summary>
        /// 关注微信公众号得积分
        /// </summary>
        WeixinSubscribeAddScore=30,
        /// <summary>
        /// 推荐关注公众号加积分
        /// </summary>
        RecommendWeixinSubscribeAddScore=31,
        /// <summary>
        /// 推荐报名活动加积分
        /// </summary>
        RecommendSignUpActivityAddScore=32,
        /// <summary>
        /// 微转发加积分
        /// </summary>
        ForwardArticle=33,
        /// <summary>
        ///LBS签到
        /// </summary>
        LBSSignIn = 34,
        /// <summary>
        /// 自定义
        /// </summary>
        Customize = 35,
        /// <summary>
        /// 打赏
        /// </summary>
        Reward = 36,
        /// <summary>
        /// 分享网站
        /// </summary>
        ShareWebsite = 37,
        /// <summary>
        /// 收到打赏
        /// </summary>
        GetReward = 38,
        /// <summary>
        /// 用户提现
        /// </summary>
        WithdrawCash = 39,
        /// <summary>
        /// 提现退积分
        /// </summary>
        RefuseWithdrawCash = 40,
        /// <summary>
        /// 提现退积分
        /// </summary>
        SendMessage = 41,        
        /// <summary>
        /// 账户余额(不可提现)
        /// </summary>
        AccountAmount = 42,
        /// <summary>
        /// 余额(可提现)
        /// </summary>
        TotalAmount=43,
        /// <summary>
        /// 系统通知
        /// </summary>
        System = 99
    }
}
