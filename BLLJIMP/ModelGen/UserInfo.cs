using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// �û���
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
        /// �Զ����
        /// </summary>
        public int AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// �û���
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// ��½����
        /// </summary>
        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// �û����� 
        /// 1Ϊ��������Ա
        /// 2Ϊ��ͨ�û�
        /// 3Ϊ��ʦ��
        /// 4ע��������ʦ
        /// 5Ϊ�̻� saller 
        /// 6��˾ 
        /// 7��Ӧ��
        /// 8��Ӧ������
        /// </summary>
        public int UserType
        {
            set { _usertype = value; }
            get { return _usertype; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string TrueName
        {
            set { _truename = value; }
            get { return _truename; }
        }
        /// <summary>
        /// ��˾��
        /// </summary>
        public string Company
        {
            set { _company = value; }
            get { return _company; }
        }
        /// <summary>
        /// �ֻ�����
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// �����ַ
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// ע��ʱ��
        /// </summary>
        public DateTime? Regtime
        {
            set { _regtime = value; }
            get { return _regtime; }
        }
        /// <summary>
        /// ���
        /// </summary>
        public decimal? Account
        {
            set { _account = value; }
            get { return _account; }
        }
        /// <summary>
        /// ʣ�����
        /// </summary>
        public int? Points
        {
            set { _points = value; }
            get { return _points; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string WeiboID
        {
            set { _weiboid = value; }
            get { return _weiboid; }
        }
        /// <summary>
        /// ΢���û��ǳ� ����
        /// </summary>
        public string WeiboScreenName
        {
            set { _weiboscreenname = value; }
            get { return _weiboscreenname; }
        }
        /// <summary>
        /// ΢����Ȩ ����
        /// </summary>
        public string WeiboAccessToken
        {
            set { _weiboaccesstoken = value; }
            get { return _weiboaccesstoken; }
        }
        /// <summary>
        /// ��Ȩ״̬ ����
        /// </summary>
        public int? WeiboAccessStatus
        {
            set { _weiboaccessstatus = value; }
            get { return _weiboaccessstatus; }
        }
        /// <summary>
        /// ����Ȩ����ID
        /// </summary>
        public long? PermissionGroupID
        {
            set { _permissiongroupid = value; }
            get { return _permissiongroupid; }
        }
        /// <summary>
        /// �ʼ����� ����
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
        /// ΢�Ź��ں�ԭʼID
        /// </summary>
        public string WeixinPubOrgID { get; set; }

        /// <summary>
        /// ΢��API��ַ
        /// </summary>
        public string WeixinAPIUrl { get; set; }

        /// <summary>
        /// ΢��Token
        /// </summary>
        public string WeixinToken { get; set; }

        /// <summary>
        /// ΢�Ź��ں�����
        /// </summary>
        public string WeixinPublicName { get; set; }

        /// <summary>
        /// ����ͼƬ
        /// </summary>
        public string Images { get; set; }

        /// <summary>
        /// ΢�Ź��ں�
        /// </summary>
        public string WeixinPublicNum { get; set; }

        /// <summary>
        /// �Ƿ���΢��ע�Ṧ�ܣ�1��0
        /// </summary>
        public int WeixinIsOpenReg { get; set; }

        /// <summary>
        /// ΢��ע���Ƿ���ж�����֤����֤: 1��0
        /// </summary>
        public int WeixinRegIsVerifySMS { get; set; }
        /// <summary>
        /// EDM�����������б����ŷָ� ����
        /// </summary>
        public string EDMSenderNameList { get; set; }
        /// <summary>
        /// EDM�ظ������б����ŷָ� ����
        /// </summary>
        public string EDMReplyEmailList { get; set; }
        /// <summary>
        /// �Ƿ�ʹ���Զ���Appkey ����
        /// </summary>
        public int WeiboIsUseOwnAppkey { get; set; }
        /// <summary>
        /// ΢���Զ���key ����
        /// </summary>
        public string WeiboOwnAppkey { get; set; }
        /// <summary>
        /// ΢���Զ���AppSecret ����
        /// </summary>
        public string WeiboOwnAppSecret { get; set; }
        /// <summary>
        /// ΢���Զ���AccessToken ����
        /// </summary>
        public string WeiboOwnAccessToken { get; set; }

        /// <summary>
        /// �Ƿ�����΢���Զ���˵� 0��ʾ������ 1��ʾ���� ����
        /// </summary>
        public int WeixinIsEnableMenu { get; set; }
        /// <summary>
        /// �Ƿ�ͨ�߼���֤ 0��� ��ʾδ��ͨ΢�Ÿ߼���֤ 1��ʾ�ѿ�ͨ΢�Ÿ߼���֤(΢����Ȩ��)
        /// </summary>
        public int WeixinIsAdvancedAuthenticate { get; set; }
        /// <summary>
        /// ΢�� WeixinAppId
        /// </summary>
        public string WeixinAppId { get; set; }

        /// <summary>
        /// ΢�� AppSecret
        /// </summary>
        public string WeixinAppSecret { get; set; }

        /// <summary>
        /// ΢��Logo ͼƬ
        /// </summary>
        public string WXLogoImg { get; set; }

        /// <summary>
        /// �Ƿ������΢����֤
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
        /// ��ҳ��Ȩ�ӿڵ���ƾ֤,ע�⣺��access_token�����֧�ֵ�access_token��ͬ
        /// </summary>
        public string WXAccessToken
        {
            set { _accesstoken = value; }
            get { return _accesstoken; }
        }
        /// <summary>
        /// �û�ˢ��access_token
        /// </summary>
        public string WXRefreshToken
        {
            set { _refreshtoken = value; }
            get { return _refreshtoken; }
        }
        /// <summary>
        /// Ӧ����Ȩ������snsapi_base ����������Ȩҳ�棬ֱ����ת��ֻ�ܻ�ȡ�û�openid����snsapi_userinfo ��������Ȩҳ�棬��ͨ��openid�õ��ǳơ��Ա����ڵء����ң���ʹ��δ��ע������£�ֻҪ�û���Ȩ��Ҳ�ܻ�ȡ����Ϣ��
        /// </summary>
        public string WXScope
        {
            set { _scope = value; }
            get { return _scope; }
        }
        /// <summary>
        /// �û��ǳ�
        /// </summary>
        public string WXNickname
        {
            set { _wxnickname = value; }
            get
            {
                //if (string.IsNullOrWhiteSpace(_wxnickname) && !string.IsNullOrWhiteSpace(WXOpenId))
                //{
                //    return "΢���û�";
                //}
                return _wxnickname;
            }
        }
        /// <summary>
        /// �û����Ա�ֵΪ1ʱ�����ԣ�ֵΪ2ʱ��Ů�ԣ�ֵΪ0ʱ��δ֪
        /// </summary>
        public int? WXSex
        {
            set { _wxsex = value; }
            get { return _wxsex; }
        }
        /// <summary>
        /// �û�����������д��ʡ��
        /// </summary>
        public string WXProvince
        {
            set { _wxprovince = value; }
            get { return _wxprovince; }
        }
        /// <summary>
        /// ��ͨ�û�����������д�ĳ���
        /// </summary>
        public string WXCity
        {
            set { _wxcity = value; }
            get { return _wxcity; }
        }
        /// <summary>
        /// ���ң����й�ΪCN
        /// </summary>
        public string WXCountry
        {
            set { _wxcountry = value; }
            get { return _wxcountry; }
        }
        /// <summary>
        /// �û�ͷ�����һ����ֵ����������ͷ���С����0��46��64��96��132��ֵ��ѡ��0����640*640������ͷ�񣩣��û�û��ͷ��ʱ����Ϊ��
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
        /// �û���Ȩ��Ϣ��json ���飬��΢���ֿ��û�Ϊ��chinaunicom��
        /// </summary>
        public string WXPrivilege
        {
            set { _wxprivilege = value; }
            get { return _wxprivilege; }
        }

        /// <summary>
        /// ΢���û�OpenID
        /// </summary>
        public string WXOpenId { get; set; }

        /// <summary>
        /// access_token�ӿڵ���ƾ֤��ʱʱ�䣬��λ���룩
        /// </summary>
        public string WXExpiresIn { get; set; }

        /// <summary>
        /// �û���¼��
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// ְλ Position
        /// </summary>
        public string Postion { get; set; }

        /// <summary>
        /// ������Դ���ͣ�AddFriend��Ӻ��ѡ�WebSite��վ ����
        /// </summary>
        public string ArticleSourceType { get; set; }

        /// <summary>
        /// ������Դ΢�ź� ����
        /// </summary>
        public string ArticleSourceWXHao { get; set; }

        /// <summary>
        /// ������Դ��վ��ַ ����
        /// </summary>
        public string ArticleSourceWebSite { get; set; }

        /// <summary>
        /// ��Դ���� ����
        /// </summary>
        public string ArticleSourceName { get; set; }

        /// <summary>
        /// ����¼IP
        /// </summary>
        public string LastLoginIP { get; set; }

        /// <summary>
        /// ����¼ʱ��
        /// </summary>
        public DateTime LastLoginDate { get; set; }

        /// <summary>
        /// ��¼����
        /// </summary>
        public int LoginTotalCount { get; set; }
        /// <summary>
        /// ����¼����
        /// </summary>
        public string LastLoginCity { get; set; }
        /// <summary>
        /// ����¼����
        /// </summary>
        public string LastLoginLongitude { get; set; }
        /// <summary>
        /// ����¼γ��
        /// </summary>
        public string LastLoginLatitude { get; set; }

        /// <summary>
        /// ע��IP
        /// </summary>
        public string RegIP { get; set; }

        /// <summary>
        /// �Ƿ�������ֻ���֤
        /// </summary>
        public int IsPhoneVerify { get; set; }
        /// <summary>
        /// վ��������
        /// </summary>
        public string WebsiteOwner { get; set; }

        /// <summary>
        /// ���֣���ʷ�ۼƻ��ּ�ȥ���ѹ���
        /// </summary>
        public double TotalScore { get; set; }

        /// <summary>
        /// ��ʷ�ۼƻ��֣����л�ù��Ļ����ܺ�
        /// </summary>
        public double HistoryTotalScore { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public DateTime? ToHFUserDate { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string HFPmsGroupStr { get; set; }

        /// <summary>
        /// �Ƿ��ѱ������û�
        /// </summary>
        public int IsDisable { get; set; }

        /// <summary>
        /// ΢�ſͷ�OpenId(���ͷ�) ����
        /// </summary>
        public string WeiXinKeFuOpenId { get; set; }
        /// <summary>
        /// �̳�Logo ����
        /// </summary>
        public string WXMallBannerImage { get; set; }

        /// <summary>
        /// ʡ
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// ��
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// ��/��
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// ��
        /// </summary>
        public string Town { get; set; }
        /// <summary>
        /// �ֵ���ַ ����ʡ����
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// ���ڵ���
        /// </summary>
        public string AddressArea { get; set; }

        /// <summary>
        /// �Ա� 0Ů 1��
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// �Ƿ������˻� 1 �����˻� �����������˻�
        /// </summary>
        public string IsSubAccount { get; set; }

        /// <summary>
        /// ���˻����½���ͶƱ��
        /// </summary>
        public int VoteCount { get; set; }

        /// <summary>
        /// ��ע�Զ��ظ��ؼ���
        /// </summary>
        public string SubscribeKeyWord { get; set; }

        /// <summary>
        /// �û��˻�ʣ���Ʊ��
        /// </summary>
        public int? AvailableVoteCount { get; set; }
        /// <summary>
        /// ��һ�θ����û�Ʊ��ʱ��(ֻ��ͶƱΪ��ѵ��������Ч)
        /// </summary>
        public DateTime? LastUpdateVoteCountTime { get; set; }

        /// <summary>
        /// �Ƿ��ע���ں��Ѽӷ� ����
        /// </summary>
        public int ISWXmpScoreAdded { get; set; }

        /// <summary>
        /// AccessTokenTicks ����ʱ��ticks
        /// </summary>
        public long WXAccessTokenExpireTicks { get; set; }

        /// <summary>
        ///��ע����
        /// </summary>
        public int FollowingCount { get; set; }

        /// <summary>
        ///��˿����
        /// </summary>
        public int FansCount { get; set; }

        /// <summary>
        ///��Ա����
        /// </summary>
        public int MemberLevel { get; set; }

        /// <summary>
        ///����Ȩ�޼���Ĭ����0��δע���û�����ע���û�Ϊ>=1��ֵԽ�����Ȩ��Խ�������£����Ȩ�޶�Ӧ��
        ///���磺Ȩ��Ϊ3���û����Է���Ȩ��Ϊ0,1,2,3������
        ///ֵΪ0�����£����ʾ�����������˷���
        /// </summary>
        public int AccessLevel { get; set; }

        /// <summary>
        /// �̳Ƿ���������
        /// </summary>
        public decimal FrozenAmount { get; set; }
        /// <summary>
        /// �̳Ƿ����˻����
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        ///�������ߵ�UserId
        /// </summary>
        public string DistributionOwner { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// ��չ�ֶ�1
        /// </summary>
        public string Ex1 { get; set; }
        /// <summary>
        /// ��չ�ֶ�2 ��Ա����
        /// </summary>
        public string Ex2 { get; set; }
        /// <summary>
        /// ��չ�ֶ�3
        /// </summary>
        public string Ex3 { get; set; }
        /// <summary>
        /// ��չ�ֶ�4
        /// </summary>
        public string Ex4 { get; set; }
        /// <summary>
        /// ��չ�ֶ�5
        /// </summary>
        public string Ex5 { get; set; }
        /// <summary>
        /// ��չ�ֶ�6
        /// </summary>
        public string Ex6 { get; set; }
        /// <summary>
        /// ��չ�ֶ�7
        /// </summary>
        public string Ex7 { get; set; }
        /// <summary>
        /// ��չ�ֶ�8
        /// </summary>
        public string Ex8 { get; set; }
        /// <summary>
        /// ��չ�ֶ�9
        /// </summary>
        public string Ex9 { get; set; }
        /// <summary>
        /// ��չ�ֶ�10
        /// </summary>
        public string Ex10 { get; set; }
        /// <summary>
        /// ��չ�ֶ�11
        /// </summary>
        public string Ex11 { get; set; }
        /// <summary>
        /// ��չ�ֶ�12
        /// </summary>
        public string Ex12 { get; set; }
        /// <summary>
        /// ��չ�ֶ�13
        /// </summary>
        public string Ex13 { get; set; }
        /// <summary>
        /// ��չ�ֶ�14
        /// </summary>
        public string Ex14 { get; set; }
        /// <summary>
        /// ��չ�ֶ�15
        /// 1 ��ʾ���Զ����õȼ�
        /// </summary>
        public string Ex15 { get; set; }
        /// <summary>
        /// ���ͼƬ
        /// </summary>
        public string imgs { get; set; }
        /// <summary>
        /// ʡ��Key
        /// </summary>
        public string ProvinceCode { get; set; }
        /// <summary>
        /// ����Key
        /// </summary>
        public string CityCode { get; set; }
        /// <summary>
        /// ����Key
        /// </summary>
        public string DistrictCode { get; set; }
        /// <summary>
        /// С��Key
        /// </summary>
        public string TownCode { get; set; }
        /// <summary>
        /// �û�΢��UnionID
        /// </summary>
        public string WXUnionID { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// �Ա�Id
        /// </summary>
        public string TaobaoId { get; set; }
        /// <summary>
        /// ͷ��
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// �������ͣ�1������0����
        /// </summary>
        public int? CarServerType { get; set; }

        /// <summary>
        /// ����ǩ��
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ��н
        /// </summary>
        public int Salary { get; set; }

        /// <summary>
        /// ���֤
        /// </summary>
        public string IdentityCard { get; set; }
        /// <summary>
        /// ���ý�
        /// </summary>
        public decimal CreditAcount
        {
            set { _creditacount = value; }
            get { return _creditacount; }
        }
        /// <summary>
        ///���·����˻��ܽ��
        /// </summary>
        public decimal DistributionOffLineTotalAmount { get; set; }
        /// <summary>
        /// ���·����˻�������
        /// </summary>
        public decimal DistributionOffLineFrozenAmount { get; set; }
        /// <summary>
        /// ���·����ϼ��û���
        /// </summary>
        public string DistributionOffLinePreUserId { get; set; }
        /// <summary>
        /// ���·����ۼ�Ӷ����
        /// </summary>
        public decimal HistoryDistributionOffLineTotalAmount { get; set; }

        /// <summary>
        /// ���Ϸ���(�̳�)�ۼ�Ӷ����
        /// </summary>
        public decimal HistoryDistributionOnLineTotalAmount { get; set; }
        /// <summary>
        /// ������ά��Tiket
        /// </summary>
        public string DistributionWXQrcodeLimitTicket { get; set; }
        /// <summary>
        /// ������ά��Url
        /// </summary>
        public string DistributionWxQrcodeLimitUrl { get; set; }
        /// <summary>
        /// ������һ���û��� ����ֱ�ӻ�Ա��
        /// </summary>
        public int DistributionDownUserCountLevel1 { get; set; }
        /// <summary>
        /// �����¶����û���
        /// </summary>
        public int DistributionDownUserCountLevel2 { get; set; }
        /// <summary>
        /// �����������û���
        /// </summary>
        public int DistributionDownUserCountLevel3 { get; set; }

        /// <summary>
        ///���л�Ա��
        /// </summary>
        public int DistributionDownUserCountAll { get; set; }
        /// <summary>
        /// 0 δ��ע���ں�
        /// 1 �ѹ�ע���ں�
        /// </summary>
        public int IsWeixinFollower { get; set; }
        /// <summary>
        /// ��Ա���״̬ 0δ���� 1����� 2δͨ�����  9ͨ�����
        /// �̺� 0δ�� 9����
        /// </summary>
        public int MemberApplyStatus { get; set; }
        /// <summary>
        /// ��Ա����ʱ��
        /// </summary>
        public DateTime MemberApplyTime { get; set; }

        /// <summary>
        /// ��Ա��ʼʱ�䣨ͨ�����ʱ�䣩
        /// </summary>
        public DateTime MemberStartTime { get; set; }

        /// <summary>
        /// �˻����
        /// </summary>
        public decimal AccountAmount { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        public string ArticleID { get; set; }
        /// <summary>
        /// �ֻ�1
        /// </summary>
        public string Phone1 { get; set; }
        /// <summary>
        /// �ֻ�2
        /// </summary>
        public string Phone2 { get; set; }
        /// <summary>
        /// �ֻ�3
        /// </summary>
        public string Phone3 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string LoginCookie { get; set; }

        /// <summary>
        /// �Լ����۶�
        /// </summary>
        public decimal DistributionSaleAmountLevel0 { get; set; }
        /// <summary>
        /// һ�����۶� ����ֱ�����۶�
        /// </summary>
        public decimal DistributionSaleAmountLevel1 { get; set; }
        /// <summary>
        /// �������۶�
        /// </summary>
        public decimal DistributionSaleAmountLevel2 { get; set; }
        /// <summary>
        /// �������۶�
        /// </summary>
        public decimal DistributionSaleAmountLevel3 { get; set; }

        /// <summary>
        ///�ۼ����۶�
        /// </summary>
        public decimal DistributionSaleAmountAll { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        public string HexiaoCode { get; set; }

        /// <summary>
        /// ��עʱ��
        /// </summary>
        public string SubscribeTime { get; set; }

        /// <summary>
        /// ȡ����עʱ��
        /// </summary>
        public string UnSubscribeTime { get; set; }
        /// <summary>
        /// ʣ��ν�����
        /// </summary>
        public int LotteryCount { get; set; }
        /// <summary>
        /// ����Token
        /// </summary>
        public string IMToken { get; set; }
        /// <summary>
        /// �鿴���� 0��ͨ  1����(����һЩ��Ϣ�����ֻ���)
        /// </summary>
        public int ViewType { get; set; }

        /// <summary>
        /// ����ʱ����Сʱ��
        /// </summary>
        public int OnlineTimes { get; set; }

        /// <summary>
        /// Ԥ���ۼ�Ӷ��
        /// </summary>
        public decimal HistoryDistributionOnLineTotalAmountEstimate { get; set; }
        /// <summary>
        ///  ���û���������
        /// </summary>
        public string Channel { get; set; }
        /// <summary>
        /// ���û����ϼ�����(ֻ�д��û�������ʱ��ֵ)
        /// </summary>
        public string ParentChannel { get; set; }
        /// <summary>
        ///�����ȼ�Id
        /// </summary>
        public string ChannelLevelId { get; set; }
        /// <summary>
        /// �����˺�
        /// </summary>
        public string MgrUserId { get; set; }
        /// <summary>
        /// ������ά��Url
        /// </summary>
        public string DistributionWxQrcodeLimitUrlChannel { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// �Ƿ����Ա��ά��
        /// </summary>
        public string IsFirstLevelDistribution { get; set; }
        /// <summary>
        /// ����Ӷ��Ԥ����
        /// </summary>
        public decimal AccountAmountEstimate { get; set; }
        /// <summary>
        /// ס��������
        /// </summary>
        public decimal AccumulationFund { get; set; }
        /// <summary>
        /// 0ʵ���û� 1�յ��û�
        /// </summary>
        public int EmptyBill { get; set; }
        /// <summary>
        /// ע���û� ��������ע��ʱ�Ŵ��ڣ�
        /// </summary>
        public string RegUserID { get; set; }
        /// <summary>
        /// ֧������
        /// </summary>
        public string PayPassword { get; set; }
        /// <summary>
        /// ���ݵ���ʱԴ���ݿ��¼ID
        /// </summary>
        public long FromId { get; set; }
        /// <summary>
        /// ��Ȩ��
        /// </summary>
        public int Stock { get; set; }
        /// <summary>
        /// ע�᷽ʽ
        /// ���ϣ����£���V1��V2�Ż�ȯ
        /// </summary>
        public string RegisterWay { get; set; }
        /// <summary>
        /// �Ƿ�����
        /// 0���� 1�������̺ͽ�ֹ������ת�ˣ����������֣�
        /// </summary>
        public int IsLock { get; set; }
        /// <summary>
        ///��Ӧ�������ȼ�Id
        /// </summary>
        public string SupplierLevelId { get; set; }
        /// <summary>
        /// ���֤������Ƭ
        /// </summary>
        public string IdentityCardPhotoFront { get; set; }
        /// <summary>
        /// ���֤������Ƭ
        /// </summary>
        public string IdentityCardPhotoBehind { get; set; }
        /// <summary>
        /// ���֤�ְֳ�����Ƭ
        /// </summary>
        public string IdentityCardPhotoHandheld { get; set; }
        /// <summary>
        /// ��ҵ����֤��1
        /// </summary>
        public string BusinessIntelligenceCertificatePhoto1 { get; set; }
        /// <summary>
        /// ��ҵ����֤��2
        /// </summary>
        public string BusinessIntelligenceCertificatePhoto2 { get; set; }
        /// <summary>
        /// ��ҵ����֤��3
        /// </summary>
        public string BusinessIntelligenceCertificatePhoto3 { get; set; }
        /// <summary>
        /// ��ҵ����֤��4
        /// </summary>
        public string BusinessIntelligenceCertificatePhoto4 { get; set; }
        /// <summary>
        /// ��ҵ����֤��5
        /// </summary>
        public string BusinessIntelligenceCertificatePhoto5 { get; set; }

        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public int IsOnLine { get; set; }
        /// <summary>
        /// ����֤�����ҵ
        /// </summary>
        public string IntelligenceCertificateBusiness { get; set; }
        /// <summary>
        /// �󶨵Ĺ�Ӧ��Id
        /// </summary>
        public string BindId { get; set; }


        #endregion Model
    }
}
