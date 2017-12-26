using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using ZentCloud.BLLJIMP.Model;
using System.Xml;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 微信开放平台
    /// </summary>
    public class BLLWeixinOpen : BLL
    {

        /// <summary>
        /// 微信开放平台 公众号消息校验Token
        /// </summary>
        string Token = ZentCloud.Common.ConfigHelper.GetConfigString("WeixinOpenToken");
        /// <summary>
        /// 微信开放平台 公众号消息加解密Key
        /// </summary>
        string AESKey = ZentCloud.Common.ConfigHelper.GetConfigString("WeixinOpenAESKey");
        /// <summary>
        /// 微信开放平台 组件AppId
        /// </summary>
        public string ComponentAppId
        {
            get
            {

                return ZentCloud.Common.ConfigHelper.GetConfigString("WeixinOpenAppId");

            }
        }


        /// <summary>
        /// 获取组件AccessToken
        /// </summary>
        /// <param name="componentAppId">组件AppId</param>
        /// <param name="componentAppsecret">组件AppSecret</param>
        /// <param name="componentVerifyTicket">组件VerifyTicket</param>
        /// <returns>ComponentAccessToken</returns>
        private string GetComponentAccessToken(string componentAppId, string componentAppsecret, string componentVerifyTicket)
        {

            //string token = string.Empty;
            var dataObj = new
            {
                component_appid = componentAppId,
                component_appsecret = componentAppsecret,
                component_verify_ticket = componentVerifyTicket

            };
            string data = ZentCloud.Common.JSONHelper.ObjectToJson(dataObj);
            ZentCloud.Common.HttpInterFace webRequest = new Common.HttpInterFace();
            var result = webRequest.PostWebRequest(data, "https://api.weixin.qq.com/cgi-bin/component/api_component_token", Encoding.UTF8);

            ComponentAccessTokenModel componentAccessTokenModel = ZentCloud.Common.JSONHelper.JsonToModel<ComponentAccessTokenModel>(result);

            return componentAccessTokenModel.component_access_token;



        }
        /// <summary>
        /// 获取组件AccessToken
        /// </summary>
        /// <returns></returns>
        public string GetComponentAccessToken()
        {
            var systemSet = GetSysSet();
            if (systemSet.ComponentAccessTokenUpdateTime != null)
            {
                DateTime lastUpdateTime = (DateTime)systemSet.ComponentAccessTokenUpdateTime;
                if (lastUpdateTime.AddSeconds(7000) > DateTime.Now)
                {
                    return systemSet.ComponentAccessToken;
                }

            }
            //取新的AccessToken
            var accessToken = GetComponentAccessToken(systemSet.ComponentAppId, systemSet.ComponentAppsecret, GetComponentVerifyTicket());

            if (!string.IsNullOrEmpty(accessToken))
            {
                systemSet.ComponentAccessTokenUpdateTime = DateTime.Now;
                systemSet.ComponentAccessToken = accessToken;
                Update(systemSet);

            }
            return accessToken;


        }


        /// <summary>
        /// 获取预授权码
        /// </summary>
        /// <returns></returns>
        public string GetPreAuthCode()
        {
            string accessToken = GetComponentAccessToken();
            var systemSet = GetSysSet();
            var dataObj = new
            {
                component_appid = systemSet.ComponentAppId

            };
            string data = ZentCloud.Common.JSONHelper.ObjectToJson(dataObj);
            ZentCloud.Common.HttpInterFace webRequest = new Common.HttpInterFace();
            var result = webRequest.PostWebRequest(data, string.Format("https://api.weixin.qq.com/cgi-bin/component/api_create_preauthcode?component_access_token={0}", accessToken), Encoding.UTF8);

            ComponentPreAuthCodeModel model = ZentCloud.Common.JSONHelper.JsonToModel<ComponentPreAuthCodeModel>(result);
            return model.pre_auth_code;
        }

        /// <summary>
        /// 使用授权码换取公众号的接口调用凭据和授权信息
        /// </summary>
        /// <returns></returns>
        public AuthorizationInfoModel GetQueryAuth(string authCode)
        {
            var systemSet = GetSysSet();
            var dataObj = new
            {
                component_appid = systemSet.ComponentAppId,
                authorization_code = authCode

            };
            string data = ZentCloud.Common.JSONHelper.ObjectToJson(dataObj);
            ZentCloud.Common.HttpInterFace webRequest = new Common.HttpInterFace();
            var result = webRequest.PostWebRequest(data, string.Format("https://api.weixin.qq.com/cgi-bin/component/api_query_auth?component_access_token={0}", GetComponentAccessToken()), Encoding.UTF8);

            AuthorizationInfoModel model = ZentCloud.Common.JSONHelper.JsonToModel<AuthorizationInfoModel>(result);
            return model;

        }

        /// <summary>
        /// 授权成功处理
        /// </summary>
        /// <param name="authCode">AuthCode</param>
        /// <returns>成功失败</returns>
        public bool AuthSuccess(string authCode)
        {
            if (System.Web.HttpContext.Current.Session["TempOauthWebsiteOwner"] == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["TempOauthWebsiteOwner"].ToString()))
            {
                return false;
            }
            var authInfo = GetQueryAuth(authCode);
            WebsiteInfo currentWebsiteInfo = Get<WebsiteInfo>(string.Format("WebsiteOwner='{0}'", System.Web.HttpContext.Current.Session["TempOauthWebsiteOwner"].ToString()));
            if (authInfo==null)
            {
                return false;
            }
            currentWebsiteInfo.AuthorizerAppId = authInfo.authorization_info.authorizer_appid;
            currentWebsiteInfo.AuthorizerAccessToken = authInfo.authorization_info.authorizer_access_token;
            currentWebsiteInfo.AuthorizerRefreshToken = authInfo.authorization_info.authorizer_refresh_token;
            currentWebsiteInfo.AuthorizerAccessTokenUpdateTime = DateTime.Now;
            currentWebsiteInfo.WeixinAppId = authInfo.authorization_info.authorizer_appid;
            var authBaseInfo = GetAuthorizerInfo(authInfo.authorization_info.authorizer_appid);
            if (authBaseInfo != null)
            {
                JToken token = JToken.Parse(authBaseInfo);
                currentWebsiteInfo.AuthorizerNickName = token["authorizer_info"]["nick_name"].ToString();
                currentWebsiteInfo.AuthorizerUserName = token["authorizer_info"]["user_name"].ToString();
                currentWebsiteInfo.AuthorizerServiceType = token["authorizer_info"]["service_type_info"]["id"].ToString();
                currentWebsiteInfo.AuthorizerVerifyType = token["authorizer_info"]["verify_type_info"]["id"].ToString();

            }
            if (Update(currentWebsiteInfo))
            {

                return true;


            }
            return false;

        }
        /// <summary>
        /// 获取授权公众号的接口调用凭据（令牌）
        /// </summary>
        /// <param name="authorizerAppId">授权公众号AppId</param>
        /// <param name="authorizerRefreshToken">授权公众号刷新Token</param>
        /// <returns></returns>
        private string GetAuthorizerToken(string authorizerAppId, string authorizerRefreshToken)
        {

            BLLWeixin bllWeixin = new BLLWeixin();

            bllWeixin.ToBLLWeixinLog("GetAuthorizerToken Open");
            var systemSet = GetSysSet();
            var dataObj = new
            {
                component_appid = systemSet.ComponentAppId,
                authorizer_appid = authorizerAppId,
                authorizer_refresh_token = authorizerRefreshToken

            };

            string data = ZentCloud.Common.JSONHelper.ObjectToJson(dataObj);

            bllWeixin.ToBLLWeixinLog("GetAuthorizerToken Open，data：" + data);

            ZentCloud.Common.HttpInterFace webRequest = new Common.HttpInterFace();
            var result = webRequest.PostWebRequest(data, string.Format("https://api.weixin.qq.com/cgi-bin/component/api_authorizer_token?component_access_token={0}", GetComponentAccessToken()), Encoding.UTF8);

            AuthorizerAccessTokenModel model = ZentCloud.Common.JSONHelper.JsonToModel<AuthorizerAccessTokenModel>(result);

            bllWeixin.ToBLLWeixinLog("GetAuthorizerToken Open，AuthorizerAccessTokenModel：" + data);

            //
            //更新 authorizer_access_token
            // 
            var currentWebsiteInfo = GetWebsiteInfoModelFromDataBase();
            if (!string.IsNullOrEmpty(model.authorizer_access_token)&&!string.IsNullOrEmpty(model.authorizer_refresh_token))
            {
                currentWebsiteInfo.AuthorizerAccessTokenUpdateTime = DateTime.Now;
                currentWebsiteInfo.AuthorizerAccessToken = model.authorizer_access_token;
                currentWebsiteInfo.AuthorizerRefreshToken = model.authorizer_refresh_token;
                Update(currentWebsiteInfo);

            }
            return model.authorizer_access_token;


        }
        /// <summary>
        /// 获取授权公众号的接口调用凭据（令牌）
        /// </summary>
        /// <param name="authorizerAppId">授权公众号AppId</param>
        /// <param name="authorizerRefreshToken">授权公众号刷新Token</param>
        /// <param name="websiteOwner">站点所有者s</param>
        /// <returns></returns>
        private string GetAuthorizerToken(string authorizerAppId, string authorizerRefreshToken, string websiteOwner)
        {

            BLLWeixin bllWeixin = new BLLWeixin();
            bllWeixin.ToBLLWeixinLog("GetAuthorizerToken Open");
            var systemSet = GetSysSet();
            var dataObj = new
            {
                component_appid = systemSet.ComponentAppId,
                authorizer_appid = authorizerAppId,
                authorizer_refresh_token = authorizerRefreshToken

            };

            string data = ZentCloud.Common.JSONHelper.ObjectToJson(dataObj);

            bllWeixin.ToBLLWeixinLog("GetAuthorizerToken Open，data：" + data);

            ZentCloud.Common.HttpInterFace webRequest = new Common.HttpInterFace();
            var result = webRequest.PostWebRequest(data, string.Format("https://api.weixin.qq.com/cgi-bin/component/api_authorizer_token?component_access_token={0}", GetComponentAccessToken()), Encoding.UTF8);
            bllWeixin.ToBLLWeixinLog("result" + result);
            AuthorizerAccessTokenModel model = ZentCloud.Common.JSONHelper.JsonToModel<AuthorizerAccessTokenModel>(result);

            bllWeixin.ToBLLWeixinLog("GetAuthorizerToken Open，AuthorizerAccessTokenModel：" + data);

            //
            //更新 authorizer_access_token
            // 
            var currentWebsiteInfo = GetWebsiteInfoModelFromDataBase(websiteOwner);
            if (!string.IsNullOrEmpty(model.authorizer_access_token))
            {
                currentWebsiteInfo.AuthorizerAccessTokenUpdateTime = DateTime.Now;
                currentWebsiteInfo.AuthorizerAccessToken = model.authorizer_access_token;
                currentWebsiteInfo.AuthorizerRefreshToken = model.authorizer_refresh_token;
                Update(currentWebsiteInfo);
            }
            //
            return model.authorizer_access_token;


        }

        /// <summary>
        /// 获取授权公众号的接口调用凭据（令牌）Token 当前站点
        /// </summary>
        /// <returns></returns>
        public string GetAuthorizerToken()
        {

            var currentWebsiteInfo = GetWebsiteInfoModelFromDataBase();
            if (currentWebsiteInfo.AuthorizerAccessTokenUpdateTime != null)
            {
                DateTime lastUpdateTime = (DateTime)currentWebsiteInfo.AuthorizerAccessTokenUpdateTime;
                if (lastUpdateTime.AddSeconds(7000) > DateTime.Now)
                {
                    return currentWebsiteInfo.AuthorizerAccessToken;
                }

            }
            return GetAuthorizerToken(currentWebsiteInfo.AuthorizerAppId, currentWebsiteInfo.AuthorizerRefreshToken);


        }
        /// <summary>
        /// 获取授权公众号的接口调用凭据（令牌）Token 指定站点
        /// </summary>
        /// <returns></returns>
        public string GetAuthorizerToken(string websiteOwner)
        {

            var websiteInfo = Get<WebsiteInfo>(string.Format(" WebsiteOwner='{0}'", websiteOwner));

            if (websiteInfo.AuthorizerAccessTokenUpdateTime != null)
            {

                DateTime lastUpdateTime = (DateTime)websiteInfo.AuthorizerAccessTokenUpdateTime;
                if (lastUpdateTime.AddSeconds(7000) > DateTime.Now)
                {
                    ToLog("have" + websiteInfo.AuthorizerAccessToken);
                    return websiteInfo.AuthorizerAccessToken;
                }

            }
            return GetAuthorizerToken(websiteInfo.AuthorizerAppId, websiteInfo.AuthorizerRefreshToken, websiteOwner);


        }



        /// <summary>
        /// 获取component_verify_ticket
        /// </summary>
        /// <returns></returns>
        public string GetComponentVerifyTicket()
        {
            return GetSysSet().ComponentVerifyTicket;
        }

        /// <summary>
        /// 是否已经通过开放平台授权
        /// true 开放平台授权
        /// false 默认授权
        /// </summary>
        /// <returns></returns>
        public bool IsAuthToOpen()
        {

            var websiteOwnerInfo = GetCurrWebSiteUserInfo();
            if ((!string.IsNullOrEmpty(websiteOwnerInfo.WeixinAppId)) && (!string.IsNullOrEmpty(websiteOwnerInfo.WeixinAppSecret)))//已经填写 appid appsecret
            {

                return false;
            }
            var currentWebsiteInfo = GetWebsiteInfoModelFromDataBase();
            if (!string.IsNullOrEmpty(currentWebsiteInfo.AuthorizerAppId))
            {

                return true;
            }
            return false;



        }
        /// <summary>
        /// 消息解密
        /// </summary>
        /// <param name="sign">签名串</param>
        /// <param name="timeStamp">时间戳</param>
        /// <param name="nonce">随机串</param>
        /// <param name="data">密文</param>
        /// <param name="msg">明文</param>
        /// <returns></returns>
        public bool DecryptMsg(string sign, string timeStamp, string nonce, string data, out string msg)
        {
            msg = "";
            Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(Token, AESKey, ComponentAppId);
            string sMsg = "";  //解析之后的明文
            int ret = 0;
            ret = wxcpt.DecryptMsg(sign, timeStamp, nonce, data, ref sMsg);
            if (ret != 0)
            {
                return false;

            }
            msg = sMsg;
            return true;

        }

        /// <summary>
        /// 消息加密
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="timeStamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        public string EncryptMsg(string msg, string timeStamp, string nonce)
        {
            Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(Token, AESKey, ComponentAppId);
            string sMsg = "";  //解析之后的明文
            int ret = 0;
            ret = wxcpt.EncryptMsg(msg, timeStamp, nonce, ref sMsg);
            if (ret != 0)
            {
                return "fail";

            }
            msg = sMsg;
            return msg;

        }



        /// <summary>
        /// xml字符串转成Dictionary
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public Dictionary<string, string> XmlToDictionary(string xml)
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            foreach (XmlElement item in xmlDoc.DocumentElement.ChildNodes)
            {
                string key = item.Name;
                string value = item.InnerText;
                if ((!string.IsNullOrEmpty(key)) && (!string.IsNullOrEmpty(value)))
                {
                    parameters.Add(key, value);

                }


            }

            //xmlDoc.Save(string.Format("D:\\WeixinOpen\\OAuthNotify{0}.xml", DateTime.Now.ToString("yyyyMMddHHmmssfff")));//写入日志
            return parameters;

        }
        /// <summary>
        /// 获取授权方的公众号帐号基本信息
        /// </summary>
        /// <param name="authorizerAppId">授权公众号AppId</param>
        /// <returns></returns>
        public string GetAuthorizerInfo(string authorizerAppId)
        {
            var dataObj = new
            {
                component_appid = ComponentAppId,
                authorizer_appid = authorizerAppId

            };
            string data = ZentCloud.Common.JSONHelper.ObjectToJson(dataObj);
            ZentCloud.Common.HttpInterFace webRequest = new Common.HttpInterFace();
            var result = webRequest.PostWebRequest(data, string.Format("https://api.weixin.qq.com/cgi-bin/component/api_get_authorizer_info?component_access_token={0}", GetComponentAccessToken()), Encoding.UTF8);

            return result;


        }
        /// <summary>
        /// 获取授权方的选项设置信息
        /// </summary>
        /// <param name="authorizerAppId">授权公众号AppId</param>
        /// <param name="optionName">选项</param>
        /// <returns></returns>
        public object GetAuthorizerInfo(string authorizerAppId, string optionName)
        {
            var dataObj = new
            {
                component_appid = ComponentAppId,
                authorizer_appid = authorizerAppId,
                option_name = optionName

            };
            string data = ZentCloud.Common.JSONHelper.ObjectToJson(dataObj);
            ZentCloud.Common.HttpInterFace webRequest = new Common.HttpInterFace();
            var result = webRequest.PostWebRequest(data, string.Format("https://api.weixin.qq.com/cgi-bin/component/ api_get_authorizer_option?component_access_token={0}", GetComponentAccessToken()), Encoding.UTF8);

            return result;


        }
        /// <summary>
        /// 设置授权方的选项信息
        /// </summary>
        /// <param name="authorizerAppId">授权公众号AppId</param>
        /// <param name="optionName">选项</param>
        /// <param name="optionName">选项值</param>
        /// <returns></returns>
        public object SetAuthorizerInfo(string authorizerAppId, string optionName, string optionValue)
        {
            var dataObj = new
            {
                component_appid = ComponentAppId,
                authorizer_appid = authorizerAppId,
                option_name = optionName,
                option_value = optionValue

            };
            string data = ZentCloud.Common.JSONHelper.ObjectToJson(dataObj);
            ZentCloud.Common.HttpInterFace webRequest = new Common.HttpInterFace();
            var result = webRequest.PostWebRequest(data, string.Format("https://api.weixin.qq.com/cgi-bin/component/ api_set_authorizer_option?component_access_token{0}", GetComponentAccessToken()), Encoding.UTF8);

            return result;


        }


        /// <summary>
        /// 组件Token模型
        /// </summary>
        public class ComponentAccessTokenModel
        {
            /// <summary>
            /// 组件token
            /// </summary>
            public string component_access_token { get; set; }
            /// <summary>
            /// 过期时间
            /// </summary>
            public int expires_in { get; set; }


        }


        /// <summary>
        /// 预授权码模型
        /// </summary>
        public class ComponentPreAuthCodeModel
        {
            /// <summary>
            /// 预授权码
            /// </summary>
            public string pre_auth_code { get; set; }
            /// <summary>
            /// 过期时间
            /// </summary>
            public int expires_in { get; set; }


        }
        /// <summary>
        /// 授权模型
        /// </summary>
        public class AuthorizerAccessTokenModel
        {
            /// <summary>
            ///接口调用凭据
            /// </summary>
            public string authorizer_access_token { get; set; }
            /// <summary>
            /// 过期时间
            /// </summary>
            public int expires_in { get; set; }
            /// <summary>
            /// 刷新Token
            /// </summary>
            public string authorizer_refresh_token { get; set; }


        }

        /// <summary>
        /// 授权信息模型
        /// </summary>
        public class AuthorizationInfoModel
        {
            /// <summary>
            /// 授权信息
            /// </summary>
            public AuthorizationInfo authorization_info { get; set; }
        }
        /// <summary>
        /// 授权信息
        /// </summary>
        public class AuthorizationInfo
        {
            /// <summary>
            /// 公众号appid
            /// </summary>
            public string authorizer_appid { get; set; }
            /// <summary>
            /// 公众号授权 AccessToken
            /// </summary>
            public string authorizer_access_token { get; set; }
            /// <summary>
            /// 过期秒数
            /// </summary>
            public int expires_in { get; set; }
            /// <summary>
            /// 刷新 Token
            /// </summary>
            public string authorizer_refresh_token { get; set; }
            /// <summary>
            /// 权限列表
            /// </summary>
            public List<FuncInfo> func_info { get; set; }

        }
        /// <summary>
        ///权限信息
        /// </summary>
        public class FuncInfo
        {
            /// <summary>
            /// 权限集列表
            /// </summary>
            public FuncScopeCategory funcscope_category { get; set; }

        }
        /// <summary>
        /// 权限分类模型
        /// </summary>
        public class FuncScopeCategory
        {
            /// <summary>
            /// 权限Id
            /// </summary>
            public int id { get; set; }
        }





    }
}
