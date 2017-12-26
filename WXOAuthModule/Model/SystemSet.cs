using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLLWXOAuthModule.Model
{
    /// <summary>
    /// 系统设置
    /// </summary>
    public class SystemSet : ZentCloud.ZCBLLEngine.ModelTable
    {
        #region Model
        private int _autoid;
        private string _weiboappkey;
        private string _weiboappsecret;
        private string _weibocallbackurl;
        private string _edmbaseurl;
        /// <summary>
        /// 
        /// </summary>
        public int AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WeiboAppKey
        {
            set { _weiboappkey = value; }
            get { return _weiboappkey; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WeiboAppSecret
        {
            set { _weiboappsecret = value; }
            get { return _weiboappsecret; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WeiboCallbackUrl
        {
            set { _weibocallbackurl = value; }
            get { return _weibocallbackurl; }
        }
        /// <summary>
        /// EDM基本链接：包括检测页面，图片网络路径等EDM相关
        /// </summary>
        public string EDMBaseUrl
        {
            set { _edmbaseurl = value; }
            get { return _edmbaseurl; }
        }

        /// <summary>
        /// 微信加密签名
        /// </summary>
        public string WeixinSignature { get; set; }
        /// <summary>
        /// 微信时间戳
        /// </summary>
        public string WeixinTimestamp { get; set; }
        /// <summary>
        /// 微信随机数
        /// </summary>
        public string WeixinNonce { get; set; }
        /// <summary>
        /// 微信随机字符串
        /// </summary>
        public string WeixonEchostr { get; set; }

        /// <summary>
        /// 微信推广域名
        /// </summary>
        public string weiXinAdDomain { get; set; }

        /// <summary>
        /// 微信注册关键字
        /// </summary>
        public string WeiXinRegKey { get; set; }

        /// <summary>
        /// 微信转发关键字
        /// </summary>
        public string WeiXinSpreadKey { get; set; }

        /// <summary>
        /// 微信排行关键字
        /// </summary>
        public string WeiXinSpreadRankKey { get; set; }

        /// <summary>
        /// 文章关键字 快捷构造链接
        /// </summary>
        public string WXArticleKey { get; set; }

        /// <summary>
        /// 信息提交表单非会员提示信息
        /// </summary>
        public string IsNotWXMemberSignInTipMsg { get; set; }
        /// <summary>
        /// 微信窗口当前打开人的OpenID关键字，可用于链接参数关键字，注意不会动态生成的如js需手动更改
        /// </summary>
        public string WXCurrOpenerOpenIDKey { get; set; }
        /// <summary>
        /// 用户自增ID16进制关键字
        /// </summary>
        public string UserAutoIDHexKey { get; set; }
        /// <summary>
        /// 微信当前用户拉取到的信息关键字
        /// </summary>
        public string WXCurrOpenerUserInfoKey { get; set; }
        /// <summary>
        /// 微信网页授权返回的AccessToken键值
        /// </summary>
        public string WXOAuthAccessTokenEntityKey { get; set; }




        /// <summary>
        /// 微信会员中心菜单关键字(针对未认证的微信)
        /// </summary>
        public string WeiXinMemberCenterMenuKey { get; set; }
        /// <summary>
        /// 微信AppId
        /// </summary>
        public string WeixinAppId { get; set; }
        /// <summary>
        /// 微信AppSecret
        /// </summary>
        public string WeixinAppSecret { get; set; }

        #endregion Model
    }
}
