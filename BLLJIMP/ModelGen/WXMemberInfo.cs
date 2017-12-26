using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微信会员
    /// </summary>
    [Serializable]
    public partial class WXMemberInfo : ZCBLLEngine.ModelTable
    {
        #region Model
        private long _memberid;
        private string _userid;
        private string _name;
        private string _phone;
        private string _email;
        private string _company;
        private string _postion;
        private string _weixinnumber;
        private string _weixinopenid;
        private DateTime _insertdate = DateTime.Now;
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
        /// 
        /// </summary>
        public long MemberID
        {
            set { _memberid = value; }
            get { return _memberid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Company
        {
            set { _company = value; }
            get { return _company; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Postion
        {
            set { _postion = value; }
            get { return _postion; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WeixinNumber
        {
            set { _weixinnumber = value; }
            get { return _weixinnumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WeixinOpenID
        {
            set { _weixinopenid = value; }
            get { return _weixinopenid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime InsertDate
        {
            set { _insertdate = value; }
            get { return _insertdate; }
        }
        /// <summary>
        /// 网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        /// </summary>
        public string AccessToken
        {
            set { _accesstoken = value; }
            get { return _accesstoken; }
        }
        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string RefreshToken
        {
            set { _refreshtoken = value; }
            get { return _refreshtoken; }
        }
        /// <summary>
        /// 应用授权作用域，snsapi_base （不弹出授权页面，直接跳转，只能获取用户openid），snsapi_userinfo （弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息）
        /// </summary>
        public string Scope
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
            get { return _wxnickname; }
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
            get { return _wxheadimgurl; }
        }
        /// <summary>
        /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）
        /// </summary>
        public string WXPrivilege
        {
            set { _wxprivilege = value; }
            get { return _wxprivilege; }
        }
        #endregion Model
    }
}
