using System;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微信会员信息表
    /// </summary>
    [Serializable]
    public partial class WeixinMemberInfo : ZentCloud.ZCBLLEngine.ModelTable
    {
        public WeixinMemberInfo()
        { }
        #region Model
        private string _weixinmemberid;
        private string _userid;
        private string _weixinopenid;
        private string _userweixinpuborgid;
        private string _name;
        private string _gender;
        private string _mobile;
        private string _email;
        private DateTime _firstvisitdate = DateTime.Now;
        private DateTime? _lastvisitdate;
        private DateTime? _regdate;
        private string _curraction;
        /// <summary>
        /// 会员ID
        /// </summary>
        public string WeixinMemberID
        {
            set { _weixinmemberid = value; }
            get { return _weixinmemberid; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 微信OpenID
        /// </summary>
        public string WeixinOpenID
        {
            set { _weixinopenid = value; }
            get { return _weixinopenid; }
        }
        /// <summary>
        /// 公众号ID
        /// </summary>
        public string UserWeixinPubOrgID
        {
            set { _userweixinpuborgid = value; }
            get { return _userweixinpuborgid; }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender
        {
            set { _gender = value; }
            get { return _gender; }
        }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile
        {
            set { _mobile = value; }
            get { return _mobile; }
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 初次访问时间
        /// </summary>
        public DateTime FirstVisitDate
        {
            set { _firstvisitdate = value; }
            get { return _firstvisitdate; }
        }
        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime? LastVisitDate
        {
            set { _lastvisitdate = value; }
            get { return _lastvisitdate; }
        }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? RegDate
        {
            set { _regdate = value; }
            get { return _regdate; }
        }
        /// <summary>
        /// 用户当前动作
        /// </summary>
        public string CurrAction
        {
            set { _curraction = value; }
            get { return _curraction; }
        }
        /// <summary>
        /// 是否是已注册用户
        /// </summary>
        public int IsRegMember { get; set; }

        /// <summary>
        /// 注册验证码
        /// </summary>
        public string RegVerifyCode { get; set; }

        /// <summary>
        /// 当前微信流程步骤
        /// </summary>
        public string FlowStep { get; set; }

        /// <summary>
        /// 已经做过的流程ID集合
        /// </summary>
        public string FlowIDHistory { get; set; }

        #endregion Model

    }
}
