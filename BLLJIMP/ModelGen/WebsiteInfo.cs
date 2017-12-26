using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 网站信息
    /// </summary>
    [Serializable]
    public partial class WebsiteInfo : ZCBLLEngine.ModelTable
    {
        public WebsiteInfo()
        { }

        #region Model
        private string _websiteowner;
        private string _websitename;
        private string _websitedescription;
        private string _websitelogo;
        private string _creater;
        private DateTime _createdate = DateTime.Now;
        private string _signupactivityid;
        private string _coursemanagemenurname;
        private string _articlemanagemenurname;
        private string _mastermanagemenurname;
        private string _questionmanagemenurname;
        private string _usermanagemenurname;
        private string _signupcoursemenurname;
        private string _coursecate1;
        private string _coursecate2;
        private string _articlecate1;
        private string _articlecate2;
        private string _articlecate3;
        private string _articlecate4;
        private string _articlecate5;
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner
        {
            set { _websiteowner = value; }
            get { return _websiteowner; }
        }
        /// <summary>
        /// 网站名称
        /// </summary>
        public string WebsiteName
        {
            set { _websitename = value; }
            get { return _websitename; }
        }
        /// <summary>
        /// 网站描述
        /// </summary>
        public string WebsiteDescription
        {
            set { _websitedescription = value; }
            get { return _websitedescription; }
        }
        /// <summary>
        /// 网站LOGO
        /// </summary>
        public string WebsiteLogo
        {
            set { _websitelogo = value; }
            get { return _websitelogo; }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creater
        {
            set { _creater = value; }
            get { return _creater; }
        }
        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        /// <summary>
        /// 无用
        /// </summary>
        public string SignUpActivityID
        {
            set { _signupactivityid = value; }
            get { return _signupactivityid; }
        }
        /// <summary>
        /// 无用
        /// </summary>
        public string CourseManageMenuRName
        {
            set { _coursemanagemenurname = value; }
            get { return _coursemanagemenurname; }
        }
        /// <summary>
        /// 无用
        /// </summary>
        public string ArticleManageMenuRName
        {
            set { _articlemanagemenurname = value; }
            get { return _articlemanagemenurname; }
        }
        /// <summary>
        /// 无用
        /// </summary>
        public string MasterManageMenuRName
        {
            set { _mastermanagemenurname = value; }
            get { return _mastermanagemenurname; }
        }
        /// <summary>
        /// 无用
        /// </summary>
        public string QuestionManageMenuRName
        {
            set { _questionmanagemenurname = value; }
            get { return _questionmanagemenurname; }
        }
        /// <summary>
        /// 无用
        /// </summary>
        public string UserManageMenuRName
        {
            set { _usermanagemenurname = value; }
            get { return _usermanagemenurname; }
        }
        /// <summary>
        /// 无用
        /// </summary>
        public string SignUpCourseMenuRName
        {
            set { _signupcoursemenurname = value; }
            get { return _signupcoursemenurname; }
        }
        /// <summary>
        /// 无用
        /// </summary>
        public string CourseCate1
        {
            set { _coursecate1 = value; }
            get { return _coursecate1; }
        }
        /// <summary>
        /// 无用
        /// </summary>
        public string CourseCate2
        {
            set { _coursecate2 = value; }
            get { return _coursecate2; }
        }
        /// <summary>
        /// 无用
        /// </summary>
        public string ArticleCate1
        {
            set { _articlecate1 = value; }
            get { return _articlecate1; }
        }
        /// <summary>
        /// 无用
        /// </summary>
        public string ArticleCate2
        {
            set { _articlecate2 = value; }
            get { return _articlecate2; }
        }
        /// <summary>
        /// 无用
        /// </summary>
        public string ArticleCate3
        {
            set { _articlecate3 = value; }
            get { return _articlecate3; }
        }
        /// <summary>
        /// 无用
        /// </summary>
        public string ArticleCate4
        {
            set { _articlecate4 = value; }
            get { return _articlecate4; }
        }
        /// <summary>
        /// 无用
        /// </summary>
        public string ArticleCate5
        {
            set { _articlecate5 = value; }
            get { return _articlecate5; }
        }
        /// <summary>
        /// 无用
        /// </summary>
        public string ActivityManageMenuRName { get; set; }

        /// <summary>
        /// 商城别名
        /// </summary>
        public string MallMenuRName { get; set; }
        /// <summary>
        /// 网站统计别名
        /// </summary>
        public string WebSiteStatisticsMenuRName { get; set; }
        /// <summary>
        /// 加V 菜单 别名
        /// </summary>
        public string AddVMenuRName { get; set; }
        /// <summary>
        /// 企业核名菜单
        /// </summary>
        public string CompanyNuclearMenuRName { get; set; }
        /// <summary>
        /// 监测平台菜单
        /// </summary>
        public string MonitorMenuRName { get; set; }
        /// <summary>
        /// 文章活动顶部代码
        /// </summary>
        public string ArticleHeadCode { get; set; }
        /// <summary>
        /// 文章活动底部代码
        /// </summary>
        public string ArticleBottomCode { get; set; }

        /// <summary>
        /// 提交订单后的提示信息
        /// </summary>
        public string SumbitOrderPromptInformation { get; set; }

        /// <summary>
        /// 商品图片比例
        /// </summary>
        public string ProductImgRatio1 { get; set; }
        public string ProductImgRatio2 { get; set; }

        /// <summary>
        /// 商城描述
        /// </summary>
        public string ShopDescription { get; set; }
        /// <summary>
        /// 导航组
        /// </summary>
        public string ShopNavGroupName { get; set; }
        /// <summary>
        /// 底部导航组
        /// </summary>
        public string ShopFoottool { get; set; }


        /// <summary>
        /// 广告组
        /// </summary>
        public string ShopAdType { get; set; }
        ///// <summary>
        ///// 配送方式 1代表门店自取 2 代表快递上门 3代表 无需物流 
        ///// </summary>
        //public string DeliveryId { get; set; }
        /// <summary>
        /// 商城模板Id 0代表普通商城 1代表外卖
        /// </summary>
        public int MallTemplateId { get; set; }

        ///// <summary>
        ///// 是否在线支付 1代表在线支付 其它表示线下支付
        ///// </summary>
        //public string IsOnlinePay { get; set; }

        /// <summary>
        /// 配送最早时间 自订单开始 MinDeliveryDate 分钟后
        /// </summary>
        public int MinDeliveryDate { get; set; }

        /// <summary>
        /// 商城名字
        /// </summary>
        public string WXMallName { get; set; }
        /// <summary>
        ///会员卡专享提示信息
        /// </summary>
        public string WXMallMemberCardMessage { get; set; }

        /// <summary>
        /// 网站模板名称
        /// </summary>
        public string CompanyWebSiteTemplateName { get; set; }
        /// <summary>
        /// 短信签名
        /// </summary>
        public string SmsSignature { get; set; }

        /// <summary>
        /// 0 普通商城  1. 展示商城
        /// </summary>
        public string MallType { get; set; }

        /// <summary>
        /// 是否分销商城 0否 1是
        /// </summary>
        public int IsDistributionMall { get; set; }

        /// <summary>
        /// 首次成为分销 分销各级别对应的分成比率。 0.xxx,目前支持10级 无用
        /// </summary>
        public double DistributionRateLevel1 { get; set; }
        /// <summary>
        /// 分销各级别对应的分成比率。 0.xxx,目前支持10级 无用
        /// </summary>
        public double DistributionRateLevel2 { get; set; }
        /// <summary>
        /// 分销各级别对应的分成比率。 0.xxx,目前支持10级 无用
        /// </summary>
        public double DistributionRateLevel3 { get; set; }
        /// <summary>
        /// 分销各级别对应的分成比率。 0.xxx,目前支持10级 无用
        /// </summary>
        public double DistributionRateLevel4 { get; set; }
        /// <summary>
        /// 分销各级别对应的分成比率。 0.xxx,目前支持10级 无用
        /// </summary>
        public double DistributionRateLevel5 { get; set; }
        /// <summary>
        /// 分销各级别对应的分成比率。 0.xxx,目前支持10级 无用
        /// </summary>
        public double DistributionRateLevel6 { get; set; }
        /// <summary>
        /// 分销各级别对应的分成比率。 0.xxx,目前支持10级 无用
        /// </summary>
        public double DistributionRateLevel7 { get; set; }
        /// <summary>
        /// 分销各级别对应的分成比率。 0.xxx,目前支持10级 无用
        /// </summary>
        public double DistributionRateLevel8 { get; set; }
        /// <summary>
        /// 分销各级别对应的分成比率。 0.xxx,目前支持10级 无用
        /// </summary>
        public double DistributionRateLevel9 { get; set; }
        /// <summary>
        /// 分销各级别对应的分成比率。 0.xxx,目前支持10级 无用
        /// </summary>
        public double DistributionRateLevel10 { get; set; }

        /// <summary>
        /// 成为分销会员后再下单 上一级分销提成比例 无用
        /// </summary>
        public double DistributionMemberRateLevel1 { get; set; }
        /// <summary>
        /// 成为分销会员后再下单 上二级分销提成比例 无用
        /// </summary>
        public double DistributionMemberRateLevel2 { get; set; }
        /// <summary>
        /// 成为分销会员后再下单 上三级分销提成比例 无用
        /// </summary>
        public double DistributionMemberRateLevel3 { get; set; }

        /// <summary>
        /// 微信 JsApiTicket
        /// </summary>
        public string WeiXinJsapi_Ticket { get; set; }
        /// <summary>
        /// 微信 JsApiTicket 获取时间
        /// </summary>
        public DateTime? WeiXinJsapi_TicketGetTime { get; set; }
        /// <summary>
        /// 商城Logo
        /// </summary>
        public string WXMallBannerImage { get; set; }
        /// <summary>
        /// 任务通知模板ID
        /// </summary>
        public string TaskTemplateId { get; set; }
        /// <summary>
        /// 阿里云Oss的bucket名称
        /// </summary>
        public string OssBucket { get; set; }

        /// <summary>
        /// 微信卡券Tiket
        /// </summary>
        public string WeiXinCard_Ticket { get; set; }

        /// <summary>
        /// 微信卡券获取时间
        /// </summary>
        public DateTime? WeiXinCard_TicketGetTime { get; set; }
        /// <summary>
        /// 站点有效期
        /// </summary>
        public DateTime? WebsiteExpirationDate { get; set; }
        /// <summary>
        /// 线下分销申请活动ID
        /// </summary>
        public string DistributionOffLineApplyActivityID { get; set; }
        /// <summary>
        /// 站点分销级别
        /// </summary>
        public int DistributionOffLineLevel { get; set; }
        /// <summary>
        /// 用户显示分销级别
        /// </summary>
        public int DistributionOffLineShowLevel { get; set; }
        /// <summary>
        /// 佣金显示名称
        /// </summary>
        public string CommissionShowName { get; set; }
        /// <summary>
        /// 分销显示名称
        /// </summary>
        public string DistributionShowName { get; set; }
        /// <summary>
        /// 是否在前端显示分销比例
        /// </summary>
        public int IsShowDistributionOffLineRate { get; set; }
        /// <summary>
        /// 线下业务分销说明
        /// </summary>
        public string DistributionOffLineDescription { get; set; }

        /// <summary>
        /// 商城分销分享页面背景图
        /// </summary>
        public string DistributionShareQrcodeBgImg { get; set; }

        /// <summary>
        /// 业务分销-广告类型
        /// </summary>
        public string DistributionOffLineSlideType { get; set; }

        /// <summary>
        /// 业务分销-是否显示会员积分：1是 0否
        /// </summary>
        public int DistributionOffLineIsShowMemberScore { get; set; }

        /// <summary>
        /// 线下分销等待审核信息
        /// </summary>
        public string DistributionOffLineApplyWaitInfo { get; set; }

        /// <summary>
        /// 分销系统显示名称
        /// </summary>
        public string DistributionOffLineSystemShowName { get; set; }

        /// <summary>
        /// 用户是否可以自己发布话题
        /// </summary>
        public int IsEnableUserReleaseReview { get; set; }
        /// <summary>
        /// 可建的最大子账号数量
        /// </summary>
        public int MaxSubAccountCount { get; set; }
        /// <summary>
        /// 授权AppId
        /// </summary>
        public string AuthorizerAppId { get; set; }
        /// <summary>
        /// 授权AccessToken
        /// </summary>
        public string AuthorizerAccessToken { get; set; }
        /// <summary>
        /// 授权刷新 AccessToken
        /// </summary>
        public string AuthorizerRefreshToken { get; set; }
        /// <summary>
        /// 授权AccessToken最后更新时间
        /// </summary>
        public DateTime? AuthorizerAccessTokenUpdateTime { get; set; }
        /// <summary>
        /// 授权公众号原始ID
        /// </summary>
        public string AuthorizerUserName { get; set; }
        /// <summary>
        /// 授权方公众号类型，0代表订阅号，1代表由历史老帐号升级后的订阅号，2代表服务号
        /// </summary>
        public string AuthorizerServiceType { get; set; }
        /// <summary>
        /// 授权方认证类型，-1代表未认证，0代表微信认证，1代表新浪微博认证，2代表腾讯微博认证，3代表已资质认证通过但还未通过名称认证，4代表已资质认证通过、还未通过名称认证，但通过了新浪微博认证，5代表已资质认证通过、还未通过名称认证，但通过了腾讯微博认证
        /// </summary>
        public string AuthorizerVerifyType { get; set; }
        /// <summary>
        /// 授权公众号名称
        /// </summary>
        public string AuthorizerNickName { get; set; }
        /// <summary>
        /// 日志保存天数
        /// </summary>
        public int LogLimitDay { get; set; }
        /// <summary>
        /// 是否开始账户余额支付
        /// 0 不开启
        /// 1 已开启
        /// </summary>
        public int IsEnableAccountAmountPay { get; set; }
        /// <summary>
        /// 余额支付前端显示名称
        /// </summary>
        public string AccountAmountPayShowName { get; set; }

        /// <summary>
        /// 是否开启限制商品购买时间
        /// 0 不开启
        /// 1 已开启
        /// </summary>
        public int IsEnableLimitProductBuyTime { get; set; }

        /// <summary>
        /// 分销会员标准-有上级
        /// </summary>
        public int DistributionMemberStandardsHaveParent { get; set; }
        /// <summary>
        /// 分销会员标准-付过款
        /// </summary>
        public int DistributionMemberStandardsHavePay { get; set; }
        /// <summary>
        /// 分销会员标准-有交易完成的订单
        /// </summary>
        public int DistributionMemberStandardsHaveSuccessOrder { get; set; }

        /// <summary>
        /// 分销关系建立规则 关注二维码
        /// </summary>
        public int DistributionRelationBuildQrCode { get; set; }
        /// <summary>
        /// 分销关系建立规则 转发报名
        /// </summary>
        public int DistributionRelationBuildSpreadActivity { get; set; }
        /// <summary>
        /// 分销关系建立规则 商城下单
        /// </summary>
        public int DistributionRelationBuildMallOrder { get; set; }
        /// <summary>
        /// 是否有退款订单
        /// </summary>
        public int IsHaveUnReadRefundOrder { get; set; }

        /// <summary>
        /// 是否需要分销推荐码
        /// 1需要
        /// 0不需要
        /// </summary>
        public int IsNeedDistributionRecommendCode { get; set; }
        /// <summary>
        /// 是否显示排行榜 1是  0否
        /// </summary>
        public int IsShowForwardRank{get;set;}

        /// <summary>
        /// 排序方式  0 报名人数排名   1 阅读量排名
        /// </summary>
        public int SortType { get; set; }

        /// <summary>
        /// 至云开放平台appkey
        /// </summary>
        public string ComeoncloudOpenAppKey { get; set; }

        /// <summary>
        /// 是否自动同步数据 1同步 0不同步
        /// </summary>
        public int? IsSynchronizationData { get; set; }

        /// <summary>
        /// 是否绑定宏巍
        /// </summary>
        public int IsUnionHongware { get; set; }
        /// <summary>
        /// 微信绑定域名
        /// </summary>
        public string WeiXinBindDomain { get; set; }

        /// <summary>
        /// 用户绑定URL
        /// </summary>
        public string UserBindUrl { get; set; }
        /// <summary>
        /// 收货地址选择URL
        /// </summary>
        public string AddressSelectUrl { get; set; }
        /// <summary>
        /// 白名单IP,多个IP地址间用英文逗号分隔
        /// </summary>
        public string WhiteIP { get; set; }
        /// <summary>
        /// OrgCode
        /// </summary>
        public string OrgCode { get; set; }

        /// <summary>
        /// 外部接口域名
        /// </summary>
        public string ApiDomain { get; set; }
        /// <summary>
        /// Nick
        /// </summary>
        public string ApiNick { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string ApiName { get; set; }
        /// <summary>
        /// 微信AppId
        /// </summary>
        public string WeixinAppId { get; set; }
        /// <summary>
        /// ShopName
        /// </summary>
        public string ShopName { get; set; }
        /// <summary>
        /// 核销码
        /// </summary>
        public string HexiaoCode { get; set; }
        /// <summary>
        /// 商城统计限制天数
        /// </summary>
        public int MallStatisticsLimitDate { get; set; }
        /// <summary>
        /// 网易云信AppKey
        /// </summary>
        public string NIMAppKey { get; set; }

        /// <summary>
        /// 网易云信AppSecret
        /// </summary>
        public string NIMAppSecret { get; set; }
        /// <summary>
        /// 阿里股票AppKey
        /// </summary>
        public string AliAppKey { get; set; }
        /// <summary>
        /// 阿里股票AliAppSecret
        /// </summary>
        public string AliAppSecret { get; set; }
        /// <summary>
        /// 渠道显示名称
        /// </summary>
        public string ChannelShowName { get; set; }
        /// <summary>
        /// 显示原价
        /// </summary>
        public int IsShowOldPrice { get; set; }

        /// <summary>
        /// 是否允许用户开团  0允许  1不允许
        /// </summary>
        public int IsOpenGroup { get; set; }

        /// <summary>
        /// 显示库存 
        /// </summary>
        public int IsShowStock { get; set; }

        /// <summary>
        /// 库存超过多少不显示具体数据，显示库存充足，低于显示库存紧张，没有则显示无库存
        /// </summary>
        public int IsShowStockValue { get; set; }

        /// <summary>
        /// 主题颜色
        /// </summary>
        public string ThemeColor { get; set; }
        /// <summary>
        /// 积分显示名称
        /// </summary>
        public string ScorePayShowName { get; set; }
        /// <summary>
        /// 优惠券
        /// </summary>
        public string CardCouponShowName { get; set; }

        /// <summary>
        /// 是否只有全额付款时才返利积分：1是 0否，默认0
        /// </summary>
        public int IsRebateScoreMustAllCash { get; set; }

        /// <summary>
        /// 分销支持级别 1仅一级 2有二级 3有三级
        /// </summary>
        public int DistributionLimitLevel { get; set; }

        /// <summary>
        /// 分销员级别获取方式 0累计佣金计算 1系统设置等级
        /// </summary>
        public int DistributionGetWay { get; set; }

        /// <summary>
        /// 返积分取整类型：0四舍五入、1向上取整、2向下取整
        /// </summary>
        public int RebateScoreGetIntType { get; set; }

        /// <summary>
        /// 商城正常购买是否返积分，默认是1
        /// </summary>
        public int IsOrderRebateScoreByMallOrder { get; set; }

        /// <summary>
        /// 商城开团是否返积分，默认0
        /// </summary>
        public int IsOrderRebateScoreByCreateGroupBuy { get; set; }

        /// <summary>
        /// 商城参团是否返积分，默认0
        /// </summary>
        public int IsOrderRebateScoreByJoinGroupBuy { get; set; }
        /// <summary>
        /// 是否禁用分佣
        /// 1 不分佣
        /// 0 分佣
        /// </summary>
        public int IsDisabledCommission { get; set; }
        /// <summary>
        /// 是否要求商城订单送达时间，默认0 不要求
        /// </summary>
        public int IsClaimMallOrderArrivalTime { get; set; }
        
        private string totalAmountShowName;
        /// <summary>
        /// 余额（TotalAmount）显示名称
        /// </summary>
        public string TotalAmountShowName { 
            get{
                if (string.IsNullOrWhiteSpace(totalAmountShowName))
                {
                    totalAmountShowName = "余额";
                }
                return totalAmountShowName;
            }
            set {
                totalAmountShowName = value;
            }
        }
        /// <summary>
        /// 商品库存阈值
        /// </summary>
        public int ProductStockThreshold { get; set; }

        /// <summary>
        /// 是否显示商品销量  1显示  0不显示
        /// </summary>
        public int IsShowProductSaleCount { get; set; }

        /// <summary>
        /// 是否显示微信头像  0显示  1不显示
        /// </summary>
        public int IsShowAvatar { get; set; }
        private string toBeDistributionMemberUrl = "/customize/comeoncloud/Index.aspx?key=MallHome";
        /// <summary>
        /// 成为代言人链接
        /// </summary>
        public string ToBeDistributionMemberUrl 
        {
            get {

                if (string.IsNullOrWhiteSpace(toBeDistributionMemberUrl))
                {
                    return "/customize/comeoncloud/Index.aspx?key=MallHome";
                }
                return toBeDistributionMemberUrl; 
            }
            set { toBeDistributionMemberUrl = value; }
        }

        /// <summary>
        /// 微信昵称显示位置 0头像下方  1二维码下方
        /// </summary>
        public int WXNickShowPosition { get; set; }

        /// <summary>
        /// 二维码使用指南
        /// </summary>
        public string QRCodeUseGuide { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string distributionCommissionName = "奖励";
        /// <summary>
        /// 分销奖励别名
        /// </summary>
        public string DistributionCommissionName
        {
            get { return distributionCommissionName; }
            set { distributionCommissionName = value; }
        }
        /// <summary>
        /// 是否替换系统分销二维码 默认可以替换
        /// 0 可以替换
        /// 1 不替换
        /// </summary>
        public int DisableReplaceDistributonOwner { get; set; }
        /// <summary>
        /// 是否需要填写下单人手机 姓名   默认0，1则需要填写
        /// </summary>
        public int IsNeedMallOrderCreaterNamePhone { get; set; }
        /// <summary>
        /// “下单人”自定义名称
        /// </summary>
        public string NeedMallOrderCreaterNamePhoneRName { get; set; }

        /// <summary>
        /// 短信余额提醒值
        /// </summary>
        public int SmsAccountRemindValue { get; set; }

        /// <summary>
        /// 短信余额提醒手机  可提醒多个手机  中间用逗号隔开
        /// </summary>
        public string SmsAccountRemindPhones { get; set; }

        /// <summary>
        /// 提醒频率  0每天提醒一次   1仅提醒一次
        /// </summary>
        public int SmsAccountRemindFrequency { get; set; }
        /// <summary>
        /// 商品描述头
        /// </summary>
        public string MallDescTop { get; set; }
        /// <summary>
        /// </summary>
        public string MallDescBottom { get; set; }

        /// <summary>
        /// 是否启用自动评价 0未开启 1开启
        /// </summary>
        public int IsOrderAutoComment { get; set; }

        /// <summary>
        /// 订单自动评价时间
        /// </summary>
        public int OrderAutoCommentDay { get; set; }

        /// <summary>
        /// 订单自动好评内容
        /// </summary>
        public string OrderAutoCommentContent { get; set; }
        /// <summary>
        /// 是否隐藏后台顶部及logo
        /// </summary>
        public int IsHideAdminLogoAndTop { get; set; }


        /// <summary>
        /// DistributionGetWay 1系统设置等级时有效
        /// 自动更新等级满多少元
        /// </summary>
        public decimal AutoUpdateLevelMinAmout { get; set; }

        /// <summary>
        /// 自动更新等级满多少元自动更新的等级Id
        /// </summary>
        public string AutoUpdateLevelId { get; set; }

        /// <summary>
        /// 订单取消时间（分钟）
        /// </summary>
        public string OrderCancelMinute { get; set; }

        /// <summary>
        /// 商城订单支付成功后跳转链接
        /// </summary>
        public string MallOrderPaySuccessUrl { get; set; }
        /// <summary>
        /// 未成为代言人提示消息
        /// </summary>
        public string NotDistributionMsg { get; set; }
        /// <summary>
        /// 用户个人中心设置 支持json
        /// </summary>
        public string UserCenterFieldJson { get; set; }
        /// <summary>
        /// 0微信，1站点配置
        /// 如：昵称，头像，性别。
        /// </summary>
        public int UserInfoFirstShow { get; set; }

        /// <summary>
        /// app推送类型
        /// </summary>
        public string AppPushType { get; set; }
        /// <summary>
        /// app推送第三方AppId
        /// </summary>
        public string AppPushAppId { get; set; }
        /// <summary>
        /// app推送第三方AppKey
        /// </summary>
        public string AppPushAppKey { get; set; }
        /// <summary>
        /// app推送第三方AppSecret
        /// </summary>
        public string AppPushAppSecret { get; set; }
        /// <summary>
        /// app推送服务器用MasterSecret
        /// </summary>
        public string AppPushMasterSecret { get; set; }

        /// <summary>
        /// 是否开启商城自定义头部
        /// 1页面显示 2app显示 3页面和app显示
        /// </summary>
        public int IsCustomizeMallHead { get; set; }
        /// <summary>
        /// 自定义商城头部配置
        /// </summary>
        public string CustomizeMallHeadConfig { get; set; }
        /// <summary>
        /// 登陆页面配置
        /// </summary>
        public string LoginPageConfig { get; set; }
        /// <summary>
        /// 是否打开饿了么订单同步
        /// </summary>
        public int IsOpenElemeOrderSynchronous { get; set; }
        /// <summary>
        /// 饿了么Key
        /// </summary>
        public string ElemeAppKey { get; set; }
        /// <summary>
        /// 饿了么Secret
        /// </summary>
        public string ElemeAppSecret { get; set; }
        /// <summary>
        ///饿了么Token 
        /// </summary>
        public string ElemeAccessToken { get; set; }
        /// <summary>
        /// 饿了么Token最后更新时间
        /// </summary>
        public DateTime? ElemeTokenLastUpdateDate { get; set; }
        /// <summary>
        /// 商城积分支付比例
        /// </summary>
        public string MallScorePayRatio { get; set; }
        /// <summary>
        /// 会员管理页面按钮
        /// </summary>
        public string MemberMgrBtn { get; set; }
        /// <summary>
        /// 添加编辑商品 商户必填
        /// </summary>
        public int RequiredSupplier { get; set; }

        /// <summary>
        /// 微软云存储账号
        /// </summary>
        public string AzureAccountName { get; set; }

        /// <summary>
        /// 微软云存储账号Key
        /// </summary>
        public string AzureAccountKey { get; set; }

        /// <summary>
        /// 微软云存储 文件目录名
        /// </summary>
        public string AzureContainerName { get; set; }

        #endregion Model

    }
}

