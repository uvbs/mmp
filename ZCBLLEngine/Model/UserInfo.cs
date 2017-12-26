using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.ZCBLLEngine.Model
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Serializable]
    public partial class UserInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        public UserInfo()
        { }
        #region Model
        private int _autoid;
        private string _userid;
        private string _password;
        private int _usertype = 0;
        private string _truename;
        private string _company;
        private string _phone;
        private string _email;
        private DateTime? _regtime;
        private decimal? _account;
        private int? _points;
        private string _weiboid;
        private string _weiboscreenname;
        private string _weiboaccesstoken;
        private int? _weiboaccessstatus;
        private long? _permissiongroupid;
        private int? _emailpoints;
        private decimal _creditacount = 0;
        public string TagName { get; set; }
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 登陆密码
        /// </summary>
        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 用户类型 
        /// 1为超级管理员
        /// 2为普通用户
        /// 3为律师，
        /// 4注册待审核律师
        /// 5为商户 saller 
        /// 6公司 
        /// 7供应商
        /// 8供应商渠道
        /// </summary>
        public int UserType
        {
            set { _usertype = value; }
            get { return _usertype; }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string TrueName
        {
            set { _truename = value; }
            get { return _truename; }
        }
        /// <summary>
        /// 公司名
        /// </summary>
        public string Company
        {
            set { _company = value; }
            get { return _company; }
        }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? Regtime
        {
            set { _regtime = value; }
            get { return _regtime; }
        }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal? Account
        {
            set { _account = value; }
            get { return _account; }
        }
        /// <summary>
        /// 剩余点数
        /// </summary>
        public int? Points
        {
            set { _points = value; }
            get { return _points; }
        }
        /// <summary>
        /// 无用
        /// </summary>
        public string WeiboID
        {
            set { _weiboid = value; }
            get { return _weiboid; }
        }
        /// <summary>
        /// 微博用户昵称 无用
        /// </summary>
        public string WeiboScreenName
        {
            set { _weiboscreenname = value; }
            get { return _weiboscreenname; }
        }
        /// <summary>
        /// 微博授权 无用
        /// </summary>
        public string WeiboAccessToken
        {
            set { _weiboaccesstoken = value; }
            get { return _weiboaccesstoken; }
        }
        /// <summary>
        /// 授权状态 无用
        /// </summary>
        public int? WeiboAccessStatus
        {
            set { _weiboaccessstatus = value; }
            get { return _weiboaccessstatus; }
        }
        /// <summary>
        /// 所属权限组ID
        /// </summary>
        public long? PermissionGroupID
        {
            set { _permissiongroupid = value; }
            get { return _permissiongroupid; }
        }
        /// <summary>
        /// 邮件点数 无用
        /// </summary>
        public int? EmailPoints
        {
            set { _emailpoints = value; }
            get
            {

                if (_emailpoints == null)
                    return 0;
                return _emailpoints;
            }
        }

        /// <summary>
        /// 微信公众号原始ID
        /// </summary>
        public string WeixinPubOrgID { get; set; }

        /// <summary>
        /// 微信API地址
        /// </summary>
        public string WeixinAPIUrl { get; set; }

        /// <summary>
        /// 微信Token
        /// </summary>
        public string WeixinToken { get; set; }

        /// <summary>
        /// 微信公众号名称
        /// </summary>
        public string WeixinPublicName { get; set; }

        /// <summary>
        /// 多张图片
        /// </summary>
        public string Images { get; set; }

        /// <summary>
        /// 微信公众号
        /// </summary>
        public string WeixinPublicNum { get; set; }

        /// <summary>
        /// 是否开启微信注册功能：1、0
        /// </summary>
        public int WeixinIsOpenReg { get; set; }

        /// <summary>
        /// 微信注册是否进行短信验证码验证: 1、0
        /// </summary>
        public int WeixinRegIsVerifySMS { get; set; }
        /// <summary>
        /// EDM发件人名称列表，逗号分隔 无用
        /// </summary>
        public string EDMSenderNameList { get; set; }
        /// <summary>
        /// EDM回复邮箱列表，逗号分隔 无用
        /// </summary>
        public string EDMReplyEmailList { get; set; }
        /// <summary>
        /// 是否使用自定义Appkey 无用
        /// </summary>
        public int WeiboIsUseOwnAppkey { get; set; }
        /// <summary>
        /// 微博自定义key 无用
        /// </summary>
        public string WeiboOwnAppkey { get; set; }
        /// <summary>
        /// 微博自定义AppSecret 无用
        /// </summary>
        public string WeiboOwnAppSecret { get; set; }
        /// <summary>
        /// 微博自定义AccessToken 无用
        /// </summary>
        public string WeiboOwnAccessToken { get; set; }

        /// <summary>
        /// 是否启用微信自定义菜单 0表示不启用 1表示启用 无用
        /// </summary>
        public int WeixinIsEnableMenu { get; set; }
        /// <summary>
        /// 是否开通高级认证 0或空 表示未开通微信高级认证 1表示已开通微信高级认证(微信授权用)
        /// </summary>
        public int WeixinIsAdvancedAuthenticate { get; set; }
        /// <summary>
        /// 微信 WeixinAppId
        /// </summary>
        public string WeixinAppId { get; set; }

        /// <summary>
        /// 微信 AppSecret
        /// </summary>
        public string WeixinAppSecret { get; set; }

        /// <summary>
        /// 微信Logo 图片
        /// </summary>
        public string WXLogoImg { get; set; }

        /// <summary>
        /// 是否进行了微信认证
        /// </summary>
        public int IsWeixinVerify { get; set; }


        private string _accesstoken;
        private string _refreshtoken;
        private string _scope;
        private string _wxnickname;
        private int? _wxsex;
        private string _wxprovince;
        private string _wxcity;
        private string _wxcountry;
        private string _wxheadimgurl;
        private string _wxprivilege;

        /// <summary>
        /// 网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        /// </summary>
        public string WXAccessToken
        {
            set { _accesstoken = value; }
            get { return _accesstoken; }
        }
        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string WXRefreshToken
        {
            set { _refreshtoken = value; }
            get { return _refreshtoken; }
        }
        /// <summary>
        /// 应用授权作用域，snsapi_base （不弹出授权页面，直接跳转，只能获取用户openid），snsapi_userinfo （弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息）
        /// </summary>
        public string WXScope
        {
            set { _scope = value; }
            get { return _scope; }
        }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string WXNickname
        {
            set { _wxnickname = value; }
            get
            {
                //if (string.IsNullOrWhiteSpace(_wxnickname) && !string.IsNullOrWhiteSpace(WXOpenId))
                //{
                //    return "微信用户";
                //}
                return _wxnickname;
            }
        }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public int? WXSex
        {
            set { _wxsex = value; }
            get { return _wxsex; }
        }
        /// <summary>
        /// 用户个人资料填写的省份
        /// </summary>
        public string WXProvince
        {
            set { _wxprovince = value; }
            get { return _wxprovince; }
        }
        /// <summary>
        /// 普通用户个人资料填写的城市
        /// </summary>
        public string WXCity
        {
            set { _wxcity = value; }
            get { return _wxcity; }
        }
        /// <summary>
        /// 国家，如中国为CN
        /// </summary>
        public string WXCountry
        {
            set { _wxcountry = value; }
            get { return _wxcountry; }
        }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        /// </summary>
        public string WXHeadimgurl
        {
            set { _wxheadimgurl = value; }
            get
            {
                return
                _wxheadimgurl;
            }
        }
        /// <summary>
        /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）
        /// </summary>
        public string WXPrivilege
        {
            set { _wxprivilege = value; }
            get { return _wxprivilege; }
        }

        /// <summary>
        /// 微信用户OpenID
        /// </summary>
        public string WXOpenId { get; set; }

        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public string WXExpiresIn { get; set; }

        /// <summary>
        /// 用户登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 职位 Position
        /// </summary>
        public string Postion { get; set; }

        /// <summary>
        /// 文章来源类型：AddFriend添加好友、WebSite网站 无用
        /// </summary>
        public string ArticleSourceType { get; set; }

        /// <summary>
        /// 文章来源微信号 无用
        /// </summary>
        public string ArticleSourceWXHao { get; set; }

        /// <summary>
        /// 文章来源网站地址 无用
        /// </summary>
        public string ArticleSourceWebSite { get; set; }

        /// <summary>
        /// 来源名称 无用
        /// </summary>
        public string ArticleSourceName { get; set; }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        public string LastLoginIP { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginDate { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginTotalCount { get; set; }
        /// <summary>
        /// 最后登录城市
        /// </summary>
        public string LastLoginCity { get; set; }
        /// <summary>
        /// 最后登录经度
        /// </summary>
        public string LastLoginLongitude { get; set; }
        /// <summary>
        /// 最后登录纬度
        /// </summary>
        public string LastLoginLatitude { get; set; }

        /// <summary>
        /// 注册IP
        /// </summary>
        public string RegIP { get; set; }

        /// <summary>
        /// 是否进行了手机验证
        /// </summary>
        public int IsPhoneVerify { get; set; }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// 积分，历史累计积分减去消费过的
        /// </summary>
        public double TotalScore { get; set; }

        /// <summary>
        /// 历史累计积分，所有获得过的积分总合
        /// </summary>
        public double HistoryTotalScore { get; set; }
        /// <summary>
        /// 无用
        /// </summary>
        public DateTime? ToHFUserDate { get; set; }
        /// <summary>
        /// 无用
        /// </summary>
        public string HFPmsGroupStr { get; set; }

        /// <summary>
        /// 是否已被禁用用户
        /// </summary>
        public int IsDisable { get; set; }

        /// <summary>
        /// 微信客服OpenId(单客服) 无用
        /// </summary>
        public string WeiXinKeFuOpenId { get; set; }
        /// <summary>
        /// 商城Logo 无用
        /// </summary>
        public string WXMallBannerImage { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区/县
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 镇
        /// </summary>
        public string Town { get; set; }
        /// <summary>
        /// 街道地址 不含省市区
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 所在地区
        /// </summary>
        public string AddressArea { get; set; }

        /// <summary>
        /// 性别 0女 1男
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 是否是子账户 1 是子账户 其它不是子账户
        /// </summary>
        public string IsSubAccount { get; set; }

        /// <summary>
        /// 子账户可新建的投票数
        /// </summary>
        public int VoteCount { get; set; }

        /// <summary>
        /// 关注自动回复关键字
        /// </summary>
        public string SubscribeKeyWord { get; set; }

        /// <summary>
        /// 用户账户剩余的票数
        /// </summary>
        public int? AvailableVoteCount { get; set; }
        /// <summary>
        /// 上一次更新用户票数时间(只在投票为免费的情况下有效)
        /// </summary>
        public DateTime? LastUpdateVoteCountTime { get; set; }

        /// <summary>
        /// 是否关注公众号已加分 无用
        /// </summary>
        public int ISWXmpScoreAdded { get; set; }

        /// <summary>
        /// AccessTokenTicks 过期时的ticks
        /// </summary>
        public long WXAccessTokenExpireTicks { get; set; }

        /// <summary>
        ///关注数量
        /// </summary>
        public int FollowingCount { get; set; }

        /// <summary>
        ///粉丝数量
        /// </summary>
        public int FansCount { get; set; }

        /// <summary>
        ///会员级别
        /// </summary>
        public int MemberLevel { get; set; }

        /// <summary>
        ///访问权限级别，默认是0（未注册用户），注册用户为>=1，值越大访问权限越大，与文章，活动的权限对应。
        ///比如：权限为3的用户可以访问权限为0,1,2,3的文章
        ///值为0的文章，活动表示公开给所有人访问
        /// </summary>
        public int AccessLevel { get; set; }

        /// <summary>
        /// 商城分销冻结金额
        /// </summary>
        public decimal FrozenAmount { get; set; }
        /// <summary>
        /// 商城分销账户余额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        ///分销上线的UserId
        /// </summary>
        public string DistributionOwner { get; set; }
        /// <summary>
        /// 浏览数
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 扩展字段1
        /// </summary>
        public string Ex1 { get; set; }
        /// <summary>
        /// 扩展字段2 会员卡号
        /// </summary>
        public string Ex2 { get; set; }
        /// <summary>
        /// 扩展字段3
        /// </summary>
        public string Ex3 { get; set; }
        /// <summary>
        /// 扩展字段4
        /// </summary>
        public string Ex4 { get; set; }
        /// <summary>
        /// 扩展字段5
        /// </summary>
        public string Ex5 { get; set; }
        /// <summary>
        /// 扩展字段6
        /// </summary>
        public string Ex6 { get; set; }
        /// <summary>
        /// 扩展字段7
        /// </summary>
        public string Ex7 { get; set; }
        /// <summary>
        /// 扩展字段8
        /// </summary>
        public string Ex8 { get; set; }
        /// <summary>
        /// 扩展字段9
        /// </summary>
        public string Ex9 { get; set; }
        /// <summary>
        /// 扩展字段10
        /// </summary>
        public string Ex10 { get; set; }
        /// <summary>
        /// 扩展字段11
        /// </summary>
        public string Ex11 { get; set; }
        /// <summary>
        /// 扩展字段12
        /// </summary>
        public string Ex12 { get; set; }
        /// <summary>
        /// 扩展字段13
        /// </summary>
        public string Ex13 { get; set; }
        /// <summary>
        /// 扩展字段14
        /// </summary>
        public string Ex14 { get; set; }
        /// <summary>
        /// 扩展字段15
        /// 1 表示是自动设置等级
        /// </summary>
        public string Ex15 { get; set; }
        /// <summary>
        /// 多个图片
        /// </summary>
        public string imgs { get; set; }
        /// <summary>
        /// 省份Key
        /// </summary>
        public string ProvinceCode { get; set; }
        /// <summary>
        /// 城市Key
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// 区县Key
        /// </summary>
        public string DistrictCode { get; set; }
        /// <summary>
        /// 小镇Key
        /// </summary>
        public string TownCode { get; set; }
        /// <summary>
        /// 用户微信UnionID
        /// </summary>
        public string WXUnionID { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 淘宝Id
        /// </summary>
        public string TaobaoId { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 服务类型：1养车、0购车
        /// </summary>
        public int? CarServerType { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 月薪
        /// </summary>
        public int Salary { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IdentityCard { get; set; }
        /// <summary>
        /// 信用金
        /// </summary>
        public decimal CreditAcount
        {
            set { _creditacount = value; }
            get { return _creditacount; }
        }
        /// <summary>
        ///线下分销账户总金额
        /// </summary>
        public decimal DistributionOffLineTotalAmount { get; set; }
        /// <summary>
        /// 线下分销账户冻结金额
        /// </summary>
        public decimal DistributionOffLineFrozenAmount { get; set; }
        /// <summary>
        /// 线下分销上级用户名
        /// </summary>
        public string DistributionOffLinePreUserId { get; set; }
        /// <summary>
        /// 线下分销累计佣金金额
        /// </summary>
        public decimal HistoryDistributionOffLineTotalAmount { get; set; }

        /// <summary>
        /// 线上分销(商城)累计佣金金额
        /// </summary>
        public decimal HistoryDistributionOnLineTotalAmount { get; set; }
        /// <summary>
        /// 分销二维码Tiket
        /// </summary>
        public string DistributionWXQrcodeLimitTicket { get; set; }
        /// <summary>
        /// 分销二维码Url
        /// </summary>
        public string DistributionWxQrcodeLimitUrl { get; set; }
        /// <summary>
        /// 分销下一级用户数 渠道直接会员数
        /// </summary>
        public int DistributionDownUserCountLevel1 { get; set; }
        /// <summary>
        /// 分销下二级用户数
        /// </summary>
        public int DistributionDownUserCountLevel2 { get; set; }
        /// <summary>
        /// 分销下三级用户数
        /// </summary>
        public int DistributionDownUserCountLevel3 { get; set; }

        /// <summary>
        ///所有会员数
        /// </summary>
        public int DistributionDownUserCountAll { get; set; }
        /// <summary>
        /// 0 未关注公众号
        /// 1 已关注公众号
        /// </summary>
        public int IsWeixinFollower { get; set; }
        /// <summary>
        /// 会员审核状态 0未申请 1待审核 2未通过审核  9通过审核
        /// 颂和 0未审 9已审
        /// </summary>
        public int MemberApplyStatus { get; set; }
        /// <summary>
        /// 会员申请时间
        /// </summary>
        public DateTime MemberApplyTime { get; set; }

        /// <summary>
        /// 会员开始时间（通过审核时间）
        /// </summary>
        public DateTime MemberStartTime { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal AccountAmount { get; set; }
        /// <summary>
        /// 文章ID
        /// </summary>
        public string ArticleID { get; set; }
        /// <summary>
        /// 手机1
        /// </summary>
        public string Phone1 { get; set; }
        /// <summary>
        /// 手机2
        /// </summary>
        public string Phone2 { get; set; }
        /// <summary>
        /// 手机3
        /// </summary>
        public string Phone3 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string LoginCookie { get; set; }

        /// <summary>
        /// 自己销售额
        /// </summary>
        public decimal DistributionSaleAmountLevel0 { get; set; }
        /// <summary>
        /// 一级销售额 渠道直接销售额
        /// </summary>
        public decimal DistributionSaleAmountLevel1 { get; set; }
        /// <summary>
        /// 二级销售额
        /// </summary>
        public decimal DistributionSaleAmountLevel2 { get; set; }
        /// <summary>
        /// 三级销售额
        /// </summary>
        public decimal DistributionSaleAmountLevel3 { get; set; }

        /// <summary>
        ///累计销售额
        /// </summary>
        public decimal DistributionSaleAmountAll { get; set; }
        /// <summary>
        /// 核销码
        /// </summary>
        public string HexiaoCode { get; set; }

        /// <summary>
        /// 关注时间
        /// </summary>
        public string SubscribeTime { get; set; }

        /// <summary>
        /// 取消关注时间
        /// </summary>
        public string UnSubscribeTime { get; set; }
        /// <summary>
        /// 剩余刮奖次数
        /// </summary>
        public int LotteryCount { get; set; }
        /// <summary>
        /// 云信Token
        /// </summary>
        public string IMToken { get; set; }
        /// <summary>
        /// 查看类型 0普通  1保密(隐藏一些信息，如手机等)
        /// </summary>
        public int ViewType { get; set; }

        /// <summary>
        /// 在线时长（小时）
        /// </summary>
        public int OnlineTimes { get; set; }

        /// <summary>
        /// 预估累计佣金
        /// </summary>
        public decimal HistoryDistributionOnLineTotalAmountEstimate { get; set; }
        /// <summary>
        ///  此用户所属渠道
        /// </summary>
        public string Channel { get; set; }
        /// <summary>
        /// 此用户的上级渠道(只有此用户是渠道时有值)
        /// </summary>
        public string ParentChannel { get; set; }
        /// <summary>
        ///渠道等级Id
        /// </summary>
        public string ChannelLevelId { get; set; }
        /// <summary>
        /// 管理账号
        /// </summary>
        public string MgrUserId { get; set; }
        /// <summary>
        /// 渠道二维码Url
        /// </summary>
        public string DistributionWxQrcodeLimitUrlChannel { get; set; }
        /// <summary>
        /// 渠道名称
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 是否分销员二维码
        /// </summary>
        public string IsFirstLevelDistribution { get; set; }
        /// <summary>
        /// 账面佣金（预估）
        /// </summary>
        public decimal AccountAmountEstimate { get; set; }
        /// <summary>
        /// 住房公积金
        /// </summary>
        public decimal AccumulationFund { get; set; }
        /// <summary>
        /// 0实单用户 1空单用户
        /// </summary>
        public int EmptyBill { get; set; }
        /// <summary>
        /// 注册用户 （替他人注册时才存在）
        /// </summary>
        public string RegUserID { get; set; }
        /// <summary>
        /// 支付密码
        /// </summary>
        public string PayPassword { get; set; }
        /// <summary>
        /// 数据导入时源数据库记录ID
        /// </summary>
        public long FromId { get; set; }
        /// <summary>
        /// 股权数
        /// </summary>
        public int Stock { get; set; }
        /// <summary>
        /// 注册方式
        /// 线上，线下，余额
        /// </summary>
        public string RegisterWay { get; set; }
        /// <summary>
        /// 是否锁定
        /// 0正常 1锁定（颂和禁止升级，转账，报单，提现）
        /// </summary>
        public int IsLock { get; set; }
        /// <summary>
        ///供应商渠道等级Id
        /// </summary>
        public string SupplierLevelId { get; set; }
        /// <summary>
        /// 身份证正面照片
        /// </summary>
        public string IdentityCardPhotoFront { get; set; }
        /// <summary>
        /// 身份证反面照片
        /// </summary>
        public string IdentityCardPhotoBehind { get; set; }
        /// <summary>
        /// 身份证手持半身照片
        /// </summary>
        public string IdentityCardPhotoHandheld { get; set; }
        /// <summary>
        /// 企业资质证书1
        /// </summary>
        public string BusinessIntelligenceCertificatePhoto1 { get; set; }
        /// <summary>
        /// 企业资质证书2
        /// </summary>
        public string BusinessIntelligenceCertificatePhoto2 { get; set; }
        /// <summary>
        /// 企业资质证书3
        /// </summary>
        public string BusinessIntelligenceCertificatePhoto3 { get; set; }
        /// <summary>
        /// 企业资质证书4
        /// </summary>
        public string BusinessIntelligenceCertificatePhoto4 { get; set; }
        /// <summary>
        /// 企业资质证书5
        /// </summary>
        public string BusinessIntelligenceCertificatePhoto5 { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public int IsOnLine { get; set; }
        /// <summary>
        /// 资质证书的企业
        /// </summary>
        public string IntelligenceCertificateBusiness { get; set; }



        #endregion Model
    }
}
