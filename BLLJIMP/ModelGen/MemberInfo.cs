using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// Member:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class MemberInfo : ZCBLLEngine.ModelTable
    {
        public MemberInfo()
        { }
        #region Model
        private string _memberid;
        private string _userid;
        private string _name;
        private string _sex;
        private DateTime? _birthday;
        private string _mobile;
        private string _email;
        private string _qq;
        private string _tel;
        private string _website;
        private string _company;
        private string _title;
        private int? _memberstatus;
        private string _weiboid;
        private string _weiboscreenname;
        private string _groupid = "0";
        private string _cardimageurl;
        private string _address;
        private string _remark;
        private string _weixinopenid;
        private int? _membertype;
        private string _mobile2;
        private string _mobile3;
        private string _mobile4;
        private string _weiboid2;
        private string _weiboid3;
        private string _weiboid4;
        private string _weixinopenid2;
        private string _weixinopenid3;
        private string _weixinopenid4;
        private string _email2;
        private string _email3;
        private string _email4;
        /// <summary>
        /// MemberID
        /// </summary>
        public string MemberID
        {
            set { _memberid = value; }
            get { return _memberid; }
        }
        /// <summary>
        /// 所属用户ID
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            set
            {
                _name = Common.StringHelper.CutByMaxLength(value, 100);
            }
            get
            {
                return _name;
            }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex
        {
            set { _sex = Common.StringHelper.CutByMaxLength(value, 10); }
            get { return _sex; }
        }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday
        {
            set { _birthday = value; }
            get { return _birthday; }
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile
        {
            set { _mobile = Common.StringHelper.CutByMaxLength(value, 20); }
            get { return _mobile; }
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email
        {
            set { _email = Common.StringHelper.CutByMaxLength(value, 250); }
            get { return _email; }
        }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ
        {
            set { _qq = Common.StringHelper.CutByMaxLength(value, 50); }
            get { return _qq; }
        }
        /// <summary>
        /// 固定电话
        /// </summary>
        public string Tel
        {
            set { _tel = Common.StringHelper.CutByMaxLength(value, 50); }
            get { return _tel; }
        }
        /// <summary>
        /// 网址
        /// </summary>
        public string Website
        {
            set { _website = Common.StringHelper.CutByMaxLength(value, 500); }
            get { return _website; }
        }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company
        {
            set { _company = Common.StringHelper.CutByMaxLength(value, 200); }
            get { return _company; }
        }
        /// <summary>
        /// 职位
        /// </summary>
        public string Title
        {
            set { _title = Common.StringHelper.CutByMaxLength(value, 200); }
            get { return _title; }
        }
        /// <summary>
        /// 用户状态
        /// </summary>
        public int? MemberStatus
        {
            set { _memberstatus = value; }
            get { return _memberstatus; }
        }
        /// <summary>
        /// 微博用户UID 
        /// </summary>
        public string WeiboID
        {
            set { _weiboid = Common.StringHelper.CutByMaxLength(value, 50); }
            get { return _weiboid; }
        }
        /// <summary>
        /// 微博用户昵称
        /// </summary>
        public string WeiboScreenName
        {
            set { _weiboscreenname = Common.StringHelper.CutByMaxLength(value, 100); }
            get { return _weiboscreenname; }
        }
        /// <summary>
        /// 所属组ID
        /// </summary>
        public string GroupID
        {
            set { _groupid = value; }
            get { return _groupid; }
        }
        /// <summary>
        /// 名片图片地址
        /// </summary>
        public string CardImageUrl
        {
            set { _cardimageurl = value; }
            get { return _cardimageurl; }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address
        {
            set { _address = Common.StringHelper.CutByMaxLength(value, 1000); }
            get { return _address; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
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
        /// 客户类别 0客户类别-无1客户类别-潜在客户2客户类别-意向客户3客户类别-待签约客户4客户类别-会员客户
        /// </summary>
        public int? MemberType
        {
            set { _membertype = value; }
            get { return _membertype; }
        }
        /// <summary>
        /// 第二个手机号
        /// </summary>
        public string Mobile2
        {
            set { _mobile2 = value; }
            get { return _mobile2; }
        }
        /// <summary>
        /// 第三个手机号
        /// </summary>
        public string Mobile3
        {
            set { _mobile3 = value; }
            get { return _mobile3; }
        }
        /// <summary>
        /// 第四个手机号
        /// </summary>
        public string Mobile4
        {
            set { _mobile4 = value; }
            get { return _mobile4; }
        }
        /// <summary>
        /// 第二个WeiboID
        /// </summary>
        public string WeiboID2
        {
            set { _weiboid2 = value; }
            get { return _weiboid2; }
        }
        /// <summary>
        /// 第三个WeiboID
        /// </summary>
        public string WeiboID3
        {
            set { _weiboid3 = value; }
            get { return _weiboid3; }
        }
        /// <summary>
        /// 第四个WeiboID
        /// </summary>
        public string WeiboID4
        {
            set { _weiboid4 = value; }
            get { return _weiboid4; }
        }
        /// <summary>
        /// 第二个WeixinOPenID
        /// </summary>
        public string WeixinOpenID2
        {
            set { _weixinopenid2 = value; }
            get { return _weixinopenid2; }
        }
        /// <summary>
        /// 第三个WeixinOPenID
        /// </summary>
        public string WeixinOpenID3
        {
            set { _weixinopenid3 = value; }
            get { return _weixinopenid3; }
        }
        /// <summary>
        /// 第四个WeixinOPenID
        /// </summary>
        public string WeixinOpenID4
        {
            set { _weixinopenid4 = value; }
            get { return _weixinopenid4; }
        }
        /// <summary>
        /// 第二个邮箱
        /// </summary>
        public string Email2
        {
            set { _email2 = value; }
            get { return _email2; }
        }
        /// <summary>
        /// 第三个邮箱
        /// </summary>
        public string Email3
        {
            set { _email3 = value; }
            get { return _email3; }
        }
        /// <summary>
        /// 第四个邮箱
        /// </summary>
        public string Email4
        {
            set { _email4 = value; }
            get { return _email4; }
        }
        #endregion Model

    }
}

