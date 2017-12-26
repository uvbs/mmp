using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 投票信息
    /// </summary>
    public class VoteInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动标识
        /// </summary>
        public int AutoID { get; set; }
        /// <summary>
        /// 投票名称
        /// </summary>
        public string VoteName { get; set; }
        /// <summary>
        /// 分享图片
        /// </summary>
        public string VoteImage { get; set; }
        /// <summary>
        /// 分享描述
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 投票状态 
        /// 0关闭[停止投票] 
        /// 1 开启[列表模式]  
        /// 2 [展示模式]  
        /// 3 [PK模式]
        /// </summary>
        public int VoteStatus { get; set; }
        /// <summary>
        /// 创建者用户名
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }
        /// <summary>
        /// 是否无需购买 1 免费 0收费
        /// </summary>
        public int IsFree { get; set; }
        /// <summary>
        /// 可免费使用的票数
        /// </summary>
        public int FreeVoteCount { get; set; }
        /// <summary>
        /// 投票介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 线下支付链接
        /// </summary>
        public string OfflinePayUrl { get; set; }
        /// <summary>
        /// 投票类型 
        /// 0 代表图片投票
        /// 1 代表视频投票
        /// 2 投票后提交手机号抽奖
        /// </summary>
        public int VoteType { get; set; }
        /// <summary>
        /// 是否自动更新票数 0不自动更新1代表每天自动更新
        /// </summary>
        public int VoteCountAutoUpdate { get; set; }
        /// <summary>
        /// 投票停止日期
        /// </summary>
        public string StopDate { get; set; }
        /// <summary>
        /// Logo 图片
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// 选手列表页面底部内容
        /// </summary>
        public string BottomContent { get; set; }
        /// <summary>
        /// 使用积分
        /// </summary>
        public int UseScore { get; set; }
        /// <summary>
        /// 奖品
        /// </summary>
        public string Prize { get; set; }

        /// <summary>
        /// 扩展字段1 暂时存储 奖品类型 0积分 1优惠券
        /// </summary>
        public string Ex1 { get; set; }
        /// <summary>
        /// 扩展字段2  暂时存储 获得积分或优惠券编号
        /// </summary>
        public string Ex2 { get; set; }

        /// <summary>
        /// 首页背景图
        /// </summary>
        public string IndexBg { get; set; }

        /// <summary>
        /// 背景banner图
        /// </summary>
        public string BannerBg { get; set; }
        /// <summary>
        /// banner 高度
        /// </summary>
        public string BannerHeight { get; set; }

        /// <summary>
        /// 手持字样
        /// </summary>
        public string HandheldWords { get; set; }
        /// <summary>
        /// 手持图片
        /// </summary>
        public string HandheldImg { get; set; }
        /// <summary>
        /// 参赛宣言别名
        /// </summary>
        public string SignUpDeclarationRename { get; set; }
        /// <summary>
        /// 参赛宣言说明
        /// </summary>
        public string SignUpDeclarationDescription { get; set; }
        /// <summary>
        /// 合作伙伴图片：放在页面底部
        /// </summary>
        public string PartnerImg { get; set; }
        /// <summary>
        /// 分享标题
        /// </summary>
        public string ShareTitle { get; set; }
        /// <summary>
        /// 投票对象详情banner图
        /// </summary>
        public string VoteObjDetailBannerImg { get; set; }
        /// <summary>
        /// 投票列表详情banner图
        /// </summary>
        public string VoteObjListBannerImg { get; set; }
        /// <summary>
        /// 活动未开始海报
        /// </summary>
        public string NotStartPoster { get; set; }
        /// <summary>
        /// 背景音乐
        /// </summary>
        public string BgMusic { get; set; }
        /// <summary>
        /// 规则页
        /// </summary>
        public string RulePageHtml { get; set; }

        /// <summary>
        /// 投票页面背景色
        /// </summary>
        public string VotePageBgColor { get; set; }
        /// <summary>
        /// 首页底部菜单是否可以隐藏：1是，0否
        /// </summary>
        public int IsHideIndexFooterMenu { get; set; }
        /// <summary>
        /// 首页
        /// </summary>
        public string IndexPageHtml { get; set; }
        /// <summary>
        /// 底部导航按钮组，默认为空，可以选择导航里面其中一组
        /// </summary>
        public string FooterMenuGroup { get; set; }
        /// <summary>
        /// 投票参与者其他资料链接展示文本
        /// </summary>
        public string OtherInfoLinkText { get; set; }
        /// <summary>
        /// 是否隐藏报名：1是 0否
        /// </summary>
        public int IsHideSignUp { get; set; }
        /// <summary>
        /// 投票限制类型
        /// 0 每人最多可以投多少票
        /// 1 每人每天可以投多少票
        /// </summary>
        public int LimitType { get; set; }
        /// <summary>
        /// 每个选手最多票数
        /// </summary>
        public int VoteObjectLimitVoteCount { get; set; }




        /// <summary>
        /// 主题颜色
        /// </summary>
        public string ThemeColor { get; set; }

        /// <summary>
        /// 主题字色
        /// </summary>
        public string ThemeFontColor { get; set; }


    }
}
