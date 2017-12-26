using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BLLWXOAuthModule.Model;
using System.IO;
using ZCJson;
using ZentCloud.BLLJIMP;
using CommonPlatform.Helper;



namespace BLLWXOAuthModule
{

    /// <summary>
    /// 微信AccessToken实体
    /// </summary>
    [Serializable]
    public class WXOAuthAccessTokenEntity
    {
        /// <summary>
        /// 网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        /// <summary>
        /// 用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
        /// </summary>
        [JsonProperty("openid")]
        public string OpenId { get; set; }
        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔 snsapi_base snsapi_userinfo
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// UnionId 只有绑定微信开放平台时才会有值
        /// </summary>
        [JsonProperty("unionid")]
        public string UnionId { get; set; }

    }

    /// <summary>
    /// 微信OAuth2.0处理Module，通过网页授权认证后，可获得当前用户的 openid 及 用户相关信息（微信提供的）
    /// </summary>
    public class WXOAuthModule : IHttpModule
    {
        /// <summary>
        /// 用户BLL
        /// </summary>
        ZentCloud.BLLJIMP.BLLUser bllUser = new ZentCloud.BLLJIMP.BLLUser("");
        /// <summary>
        /// 微信开放平台BLL
        /// </summary>
        ZentCloud.BLLJIMP.BLLWeixinOpen bllWeixinOpen = new BLLWeixinOpen();
        /// <summary>
        /// Redis
        /// </summary>
        BLLRedis bllRedis = new BLLRedis();


        BLLWebSite bllWebSite = new BLLWebSite();

        /// <summary>
        /// 是否已经授权给开放平台
        /// </summary>
        bool isAuthToOpen = false;
        /// <summary>
        /// 授权域名
        /// </summary>
        string authOpenDoMain = ZentCloud.Common.ConfigHelper.GetConfigString("WeixinOpenOAuthDoMain");
        /// <summary>
        /// AppId
        /// </summary>
        string weixinAppId = "";
        /// <summary>
        /// 微信密钥
        /// </summary>
        string weixinAppSecret = "";
        /// <summary>
        /// 当前绝对地址
        /// </summary>
        string currentUrl = "";
        /// <summary>
        /// 路由
        /// </summary>
        string ngRoute = "";
        /// <summary>
        ///当前虚拟路径 不包含参数
        /// </summary>
        string currentPath = "";
        /// <summary>
        ///链接缓存key
        /// </summary>
        string redirectUrlCacheKey = "";
        /// <summary>
        /// 当前站点信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.WebsiteInfo currentWebsiteInfo = new ZentCloud.BLLJIMP.Model.WebsiteInfo();//当前站点信息
        /// <summary>
        /// 当前站点所有者信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currentWebsiteOwnerInfo = new ZentCloud.BLLJIMP.Model.UserInfo();//当前站点所有者信息
        /// <summary>
        /// 用户信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo = new ZentCloud.BLLJIMP.Model.UserInfo();
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {

        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(AcquireRequestStateForLogin);
        }

        /// <summary>
        /// 微信授权自动登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AcquireRequestStateForLogin(object sender, EventArgs e)
        {
            try
            {

                HttpContext context = HttpContext.Current;
                currentUrl = context.Request.Url.ToString();//当前绝对地址
                redirectUrlCacheKey =  Guid.NewGuid().ToString(); //生成链接guid

                //isAuthToOpen = false;//是否已经授权给开放平台
                
                currentPath = context.Request.Path.ToLower();//当前虚拟路径 不包含参数
                if (!context.Request.UserAgent.ToLower().Contains("micromessenger"))
                {
                    //ToLog("非微信浏览器进入不执行授权");
                    return;//非微信浏览器进入不执行授权
                }
                //只检查aspx、ashx、chtml /m 后缀名的文件
                if (
                    !ZentCloud.Common.IOHelper.GetExtraName(currentPath).Equals("aspx")
                    &&
                    !ZentCloud.Common.IOHelper.GetExtraName(currentPath).Equals("chtml")
                    &&
                    !ZentCloud.Common.IOHelper.GetExtraName(currentPath).Equals("ashx")
                    &&
                    currentPath != "/m"
                    )
                {
                    ToLog(currentPath + "not extention");
                    return;
                }
                //排除
                List<string> pageV2List = new List<string>(){
                    "/test/testsession.aspx",
                    "/serv/api/member/getsmscode.ashx",
                    "/serv/api/member/registerbinding.ashx",
                    "/serv/api/member/binding.ashx"
                };
                if (pageV2List.Contains(currentPath)) return;

                ////注册页不授权
                //if (currentPath.EndsWith("/app/member/wap/registerbinding.aspx"))
                //{
                //    ToLog("注册页不授权");
                //    return;
                //}




                #region CallBack处理阶段
                if (currentPath.EndsWith("wxcallback.aspx"))
                {

                    // if (!GetWebsiteInfoSet(context)) return; //站点设置
                    WXCallbackAction(context);
                    return;
                }
                #endregion
                
                ZentCloud.BLLJIMP.Model.Other.WxAutoLoginToken wxAutoLoginToken = new ZentCloud.BLLJIMP.Model.Other.WxAutoLoginToken();

                #region 已经登录 Openid不为空检查

                ToLog("判断 是否已经登录 Openid不为空检查  url" + currentUrl);

                if (bllUser.IsLogin)//已经登录不授权
                {
                    ToLog("已经登录不授权  或者已有openid也不授权  url:" + currentUrl + " redirectUrl:");//context.Session["redirectUrl"]
                    return;
                }
                else
                {
                    ToLog("没登陆");
                    try
                    {

                        
                        //从cookie 获取自动登陆token 并自动登陆
                        var comeoncloudAutoLoginToken = context.Request.Cookies[ZentCloud.Common.SessionKey.LoginCookie].Value;

                        ToLog("从cookie 获取自动登陆token： " + comeoncloudAutoLoginToken);

                        if (comeoncloudAutoLoginToken != null)
                        {
                            wxAutoLoginToken = RedisHelper.RedisHelper.StringGet<ZentCloud.BLLJIMP.Model.Other.WxAutoLoginToken>(comeoncloudAutoLoginToken);

                            ToLog("从redis获取到的 token： " + JsonConvert.SerializeObject(wxAutoLoginToken));

                            if (wxAutoLoginToken != null && wxAutoLoginToken.IsUAuth == 1)
                            {
                                context.Session[ZentCloud.Common.SessionKey.UserID] = wxAutoLoginToken.Uid;
                                context.Session[ZentCloud.Common.SessionKey.LoginStatu] = 1; //设置登录状态
                                context.Session["currWXOpenId"] = wxAutoLoginToken.Oid;
                                ToLog("通过 wxAutoLoginToken 自动登陆");
                                return;
                            }
                        }
                    }
                    catch(Exception ex) {
                        ToLog("取cookie异常：" + ex.Message);
                    }
                    
                    currentWebsiteInfo = bllUser.GetWebsiteInfoModelFromDataBase();//当前站点信息

                    ToLog("currWXOpenId:" + context.Session["currWXOpenId"]);
                    //如果当前openid不为空，则尝试查询用户，有用户则直接登陆
                    if (context.Session["currWXOpenId"] != null)
                    {
                        
                        var openIdUser = bllUser.GetUserInfoByOpenId(context.Session["currWXOpenId"].ToString());
                        ToLog("如果当前openid不为空，则尝试查询用户，有用户则直接登陆:" + JsonConvert.SerializeObject(openIdUser));

                        if (openIdUser != null)
                        {
                            context.Session[ZentCloud.Common.SessionKey.UserID] = openIdUser.UserID;
                            context.Session[ZentCloud.Common.SessionKey.LoginStatu] = 1; //设置登录状态                            
                            ToLog("通过 openId查找用户 自动登陆");
                            return;
                        }

                    }

                    

                    if (context.Session["currWXOpenId"] != null && !CheckNotAutoRegNewWxUser(context, currentUrl))
                    {
                        //TODO：需要跳到注册页却没跳？

                        ToLog("curr:" + currentUrl + "，当存在Openid检查站点是否需要手动注册，需要跳转到注册页");
                        return; //当存在Openid检查站点是否需要手动注册，需要跳转到注册页
                    }

                }
                #endregion

                ToLog("进入开始微信授权处理，currentUrl：" + currentUrl);

                //url加一个临时时间戳处理

                if (context.Request.QueryString.Count == 0)
                {
                    currentUrl += "?zy_t=" + ZentCloud.Common.DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now);
                }
                else
                {
                    currentUrl += "&zy_t=" + ZentCloud.Common.DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now);
                }

                #region ngRoute处理
                ngRoute = context.Request["ngroute"];//ng路由

                if (string.IsNullOrWhiteSpace(ngRoute))
                {
                    ngRoute = context.Request["ngrout"];//ng路由
                }

                if (!string.IsNullOrWhiteSpace(ngRoute))
                {
                    currentUrl = currentUrl + "#" + ngRoute;
                } 
                #endregion

                #region 判断阶段及开始授权处理阶段
                ToLog("curr:" + currentUrl + "GetWebsiteInfoSet start");
                ToLog("isAuthToOpen:" + isAuthToOpen.ToString());
                if (!GetWebsiteInfoSet(context))
                {
                    ToLog("curr:" + currentUrl + "，GetWebsiteInfoSet return");
                    return;//站点设置
                }
                ToLog("curr:" + currentUrl + " GetWebsiteInfoSet ok");
                ToLog("isAuthToOpen:" + isAuthToOpen.ToString());
                if (ToOAuth(context, wxAutoLoginToken))
                {
                    return;
                }
                return;
                #endregion
            }
            catch (System.Threading.ThreadAbortException)
            {
                //不进行任何操作   
            }
            catch (System.Exception ex)
            {
                ToLog("path:\t" + HttpContext.Current.Request.Url.AbsoluteUri + "\t" + ex.ToString());
            }
        }

        //获取站点所有者信息
        private bool GetWebsiteInfoSet(HttpContext context)
        {
            ToLog("into GetWebsiteInfoSet");

            if (bllWeixinOpen.IsAuthToOpen())
            {
                ToLog(" bllWeixinOpen.IsAuthToOpen() is true ");

                //服务号且经过微信认证才授权
                if (currentWebsiteInfo.AuthorizerServiceType == "2" && currentWebsiteInfo.AuthorizerVerifyType == "0")
                {
                    ToLog(" currentWebsiteInfo.AuthorizerServiceType == \"2\" && currentWebsiteInfo.AuthorizerVerifyType == \"0\"： isAuthToOpen = true ");

                    isAuthToOpen = true;
                    ToLog(string.Format(" websiteInfo:websiteOwner:{0}url:{1}", currentWebsiteInfo.WebsiteOwner, context.Request.Url.ToString()));
                }
                else
                {
                    isAuthToOpen = false;
                }
            }
            else
            {
                isAuthToOpen = false;
            }

            if (authOpenDoMain == context.Request.Url.Host)//当前域名是开放平台授权域名
            {
                ToLog("当前域名是开放平台授权域名 curr:" + currentUrl);
                isAuthToOpen = true;
                ToLog(string.Format(" websiteInfoauthOpenDoMain:websiteOwner:{0}url:{1}", currentWebsiteInfo.WebsiteOwner, context.Request.Url.ToString()));
            }
            else
            {
                // isAuthToOpen = false;
            }
            ToLog("websiteOwner:" + System.Web.HttpContext.Current.Session["WebsiteOwner"].ToString());
            currentWebsiteOwnerInfo = bllUser.GetCurrWebSiteUserInfo();//当前站点所有者信息
            ToLog("currentWebsiteOwnerInfo" + currentWebsiteOwnerInfo.UserID);
            weixinAppId = currentWebsiteOwnerInfo.WeixinAppId;
            ToLog("weixinAppId" + weixinAppId);
            weixinAppSecret = currentWebsiteOwnerInfo.WeixinAppSecret;
            ToLog("weixinAppSecret" + weixinAppSecret);
            if ((!currentWebsiteOwnerInfo.WeixinIsAdvancedAuthenticate.Equals(1)) && (!isAuthToOpen))
            {
                //未开通高级授权,开放平台也未授权
                ToLog("未开通微信高级认证的不处理:" + currentWebsiteOwnerInfo.UserID);
                return false;//未开通微信高级认证的不处理
            }
            if ((string.IsNullOrEmpty(weixinAppId)) && (string.IsNullOrEmpty(weixinAppSecret)) && (!isAuthToOpen))
            {
                //虽然设置了高级授权,但是没填写AppId, AppSecret,且没经过开放平台授权
                return false;
            }

            ToLog("GetWebsiteInfoSet 的末尾 ,isAuthToOpen = " + isAuthToOpen);
            return true;
        }

        private bool WXCallbackAction(HttpContext context)
        {

            ZentCloud.BLLJIMP.Model.Other.WxAutoLoginToken wxAutoLoginToken = new ZentCloud.BLLJIMP.Model.Other.WxAutoLoginToken() { IsUAuth = 0 };
            ToLog("callback处理");
            //接收微信授权返回参数
            string code = context.Request["code"];
            string appId = context.Request["appid"];//授权给开放平台时返回此参数 默认授权无此参数
            string state = context.Request["state"];//state 原样返回
            // state = HttpUtility.UrlDecode(state);//state
            ToLog(" into module /wxcallback.aspx code: " + code);
            string redirectUrl = state;//授权完成跳转的链接
            ToLog(" state: " + state);
            try
            {
                redirectUrl = RedisHelper.RedisHelper.StringGet(state);//默认用Redis
            }
            catch (Exception)
            {
                redirectUrl = ZentCloud.Common.DataCache.GetCache(state).ToString();//通过state的cachekey读取链接
            }
            ToLog(" redirectUrl: " + redirectUrl);

            //if (context.Session["redirectUrl"] != null)
            //{
            //    redirectUrl = context.Session["redirectUrl"].ToString();//默认授权用
            //}
            //else
            //{
            //    ToLog("context.Session[redirectUrl] is null");
            //    return;
            //}
            //如果传入code为空，跳过处理
            if (string.IsNullOrWhiteSpace(code))
            {
                ToLog("context.Request[code] is null");
                return false;
            }
            string getAccessTokenUrl = "";
            ToLog("appid:" + appId);
            if (!string.IsNullOrEmpty(appId))
            {

                isAuthToOpen = true;
            }
            else
            {
                isAuthToOpen = false;
                currentWebsiteInfo = bllUser.GetWebsiteInfoModelFromDataBase();//当前站点信息
                currentWebsiteOwnerInfo = bllUser.GetCurrWebSiteUserInfo();//当前站点所有者信息
                ToLog("currentWebsiteOwnerInfo" + currentWebsiteOwnerInfo.UserID);
                weixinAppId = currentWebsiteOwnerInfo.WeixinAppId;
                ToLog("weixinAppId" + weixinAppId);
                weixinAppSecret = currentWebsiteOwnerInfo.WeixinAppSecret;
                ToLog("weixinAppSecret" + weixinAppSecret);
            }

            #region 默认授权
            if (!isAuthToOpen)//默认授权
            {
                getAccessTokenUrl = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                    weixinAppId,
                    weixinAppSecret,
                    code);
            }
            #endregion

            #region 开放平台授权
            else
            {

                //todo:
                currentWebsiteInfo = bllUser.Get<ZentCloud.BLLJIMP.Model.WebsiteInfo>(string.Format(" AuthorizerAppId='{0}'", appId));
                currentWebsiteOwnerInfo = bllUser.GetUserInfo(currentWebsiteInfo.WebsiteOwner, currentWebsiteInfo.WebsiteOwner);

                //开放平台多两个参数  component_appid,component_access_token
                getAccessTokenUrl =
                string.Format("https://api.weixin.qq.com/sns/oauth2/component/access_token?appid={0}&code={1}&grant_type=authorization_code&component_appid={2}&component_access_token={3}",
                    appId,
                    code,
                    bllWeixinOpen.ComponentAppId, bllWeixinOpen.GetComponentAccessToken());
            }
            #endregion
            ToLog("tokenmodel:" + getAccessTokenUrl);

            //获取OpenID及Access_token
            string accessTokenSource = ZentCloud.Common.MySpider.GetPageSourceForUTF8(getAccessTokenUrl);
            if (string.IsNullOrWhiteSpace(accessTokenSource))
            {
                ToLog("ZentCloud.Common.MySpider.GetPageSourceForUTF8(getTokenUrl) is null");
                context.Response.Redirect(redirectUrl);
                return false;
            }
            ToLog("accessTokenSource:" + accessTokenSource);

            WXOAuthAccessTokenEntity accessTokenModel = ZCJson.JsonConvert.DeserializeObject<WXOAuthAccessTokenEntity>(accessTokenSource);

            ToLog("start accessTokenSource process");

            if (accessTokenModel == null)
            {
                ToLog("ZCJson.JsonConvert.DeserializeObject<WXOAuthAccessTokenEntity>(accessTokenSource) is null");
                context.Response.Redirect(redirectUrl);
                return false;
            }
            if (string.IsNullOrEmpty(accessTokenModel.OpenId))
            {
                ToLog("accessTokenModel.OpenId is null");
                context.Response.Redirect(redirectUrl);
                return false;
            }
            else
            {
                context.Session["currWXOpenId"] = accessTokenModel.OpenId;
                ToLog("currWXOpenId：" + context.Session["currWXOpenId"].ToString());
            }
            if (accessTokenModel.Scope.Contains("snsapi_userinfo"))
            {
                ToLog("获取用户头像昵称 start ");
                string wxUserInfoSourceJson = ZentCloud.Common.MySpider.GetPageSourceForUTF8(string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}",
                        accessTokenModel.AccessToken,
                        accessTokenModel.OpenId
                    ));
                ZentCloud.BLLJIMP.Model.Weixin.WeixinUserInfo wxUserInfo = ZCJson.JsonConvert.DeserializeObject<ZentCloud.BLLJIMP.Model.Weixin.WeixinUserInfo>(wxUserInfoSourceJson);
                accessTokenModel.UnionId = wxUserInfo.UnionID;
                wxAutoLoginToken.IsUAuth = 1;
            }
            if (string.IsNullOrEmpty(appId))//默认授权
            {
                currentUserInfo = bllUser.GetUserInfoByOpenId(accessTokenModel.OpenId);


            }
            else//开放平台
            {
                //var websiteInfo = bllUser.Get<ZentCloud.BLLJIMP.Model.WebsiteInfo>(string.Format(" AuthorizerAppId='{0}'", appId));
                currentUserInfo = bllUser.GetUserInfoByOpenId(accessTokenModel.OpenId,  currentWebsiteInfo.WebsiteOwner);
            }
            if (currentUserInfo == null && !string.IsNullOrWhiteSpace(accessTokenModel.UnionId))
            {
                currentUserInfo = bllUser.GetUserInfoByWXUnionID(accessTokenModel.UnionId);
            }
            if (currentUserInfo == null)
            {
                //判断当前站点是否不允许微信自动注册新用户
                if (!CheckNotAutoRegNewWxUser(context, redirectUrl))
                {
                    ToLog("不自动注册:" + accessTokenModel.OpenId);
                    //context.Response.Redirect(redirectUrl);
                    //if (!isAuthToOpen)//默认授权
                    //{
                       
                    //}
                    //else//微信开放平台授权
                    //{
                    //    string signKey = ZentCloud.Common.ConfigHelper.GetConfigString("WeixinOpenWebOAuthKey");// 开放平台网页授权Md5 Key
                    //    string sign = ZentCloud.Common.DEncrypt.GetMD5(currentUserInfo.WXOpenId + signKey);//签名
                    //    //开放平台授权，跳到统一处理Handler
                    context.Response.Redirect(string.Format("http://{0}/WeixinOpen/WebOAuthNotLogin.ashx?openid={1}&redirecturl={2}", ZentCloud.Common.MyRegex.GetDoMainByUrl(redirectUrl), accessTokenModel.OpenId, HttpUtility.UrlEncode(redirectUrl)));


                    //}
                    return false;
                }
                else
                {
                    ToLog("自动注册:" + accessTokenModel.OpenId);
                    #region 注册用户
                    //注册用户
                    currentUserInfo = new ZentCloud.BLLJIMP.Model.UserInfo();
                    currentUserInfo.UserID = string.Format("WXUser{0}", Guid.NewGuid().ToString());//Guid
                    currentUserInfo.Password = ZentCloud.Common.Rand.Str_char(12);
                    currentUserInfo.UserType = 2;
                    currentUserInfo.WebsiteOwner = currentWebsiteOwnerInfo.UserID;
                    //if (isAuthToOpen)//开放平台授权
                    //{
                    //    var websiteInfo = bllUser.Get<ZentCloud.BLLJIMP.Model.WebsiteInfo>(string.Format(" AuthorizerAppId='{0}'", appId));
                    //    currentUserInfo.WebsiteOwner = websiteInfo.WebsiteOwner;
                    
                    //}
                    currentUserInfo.Regtime = DateTime.Now;
                    //currentUserInfo.WXAccessToken = accessTokenModel.AccessToken;
                    //currentUserInfo.WXRefreshToken = accessTokenModel.RefreshToken;
                    currentUserInfo.WXOpenId = accessTokenModel.OpenId;
                    //currentUserInfo.WXScope = accessTokenModel.Scope;
                    currentUserInfo.RegIP = ZentCloud.Common.MySpider.GetClientIP();
                    currentUserInfo.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
                    currentUserInfo.LastLoginDate = DateTime.Now;
                    currentUserInfo.LoginTotalCount = 1;
                    currentUserInfo.WXUnionID = accessTokenModel.UnionId;
                    if (!new BLLCommRelation().ExistRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, currentUserInfo.WebsiteOwner, ""))
                    {
                        ToLog(string.Format("---自动注册,openid:{0},currentWebsiteInfo.WebsiteOwner:{1}", accessTokenModel.OpenId, currentUserInfo.WebsiteOwner));
                        bllUser.Add(currentUserInfo);
                    }
                    #endregion
                }
            }
            if (currentUserInfo != null)
            {
                if ((!string.IsNullOrEmpty(accessTokenModel.UnionId)) && (string.IsNullOrEmpty(currentUserInfo.WXUnionID)))//如果有 UnionId 则更新
                {
                    currentUserInfo.WXUnionID = accessTokenModel.UnionId;
                    bllUser.Update(currentUserInfo, string.Format("WXUnionID='{0}'", currentUserInfo.WXUnionID), string.Format(" UserId='{0}' ", currentUserInfo.UserID));
                }
                if ((!string.IsNullOrEmpty(accessTokenModel.OpenId)) && (string.IsNullOrEmpty(currentUserInfo.WXOpenId)))
                {
                    currentUserInfo.WXOpenId = accessTokenModel.OpenId;
                    bllUser.Update(currentUserInfo, string.Format("WXOpenId='{0}'", currentUserInfo.WXOpenId), string.Format(" UserId='{0}' ", currentUserInfo.UserID));
                }
            }

            ToLog("用户名：" + currentUserInfo.UserID + "WebsiteOwner=" + currentWebsiteOwnerInfo.UserID);

           // if (!BLL.GetCheck()) return false;

            #region 获取用户头像昵称
            if (accessTokenModel.Scope.Contains("snsapi_userinfo"))
            {
                ToLog("获取用户头像昵称 start ");
                string wxUserInfoSourceJson = ZentCloud.Common.MySpider.GetPageSourceForUTF8(string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}",
                        accessTokenModel.AccessToken,
                        accessTokenModel.OpenId
                    ));

                ToLog("wxUserInfoSourceJson:" + wxUserInfoSourceJson);
                if (!string.IsNullOrWhiteSpace(wxUserInfoSourceJson))
                {
                    ZentCloud.BLLJIMP.Model.Weixin.WeixinUserInfo wxUserInfo = ZCJson.JsonConvert.DeserializeObject<ZentCloud.BLLJIMP.Model.Weixin.WeixinUserInfo>(wxUserInfoSourceJson);
                    //context.Session["currWeixinUserInfo"] = wxUserInfo;
                    if (currentUserInfo != null)
                    {
                        //ToLog("处理下，头像都取132的");
                        //if (wxUserInfo.HeadImgUrl.EndsWith("/0"))
                        //{
                        //    ToLog(wxUserInfo.HeadImgUrl);
                        //    wxUserInfo.HeadImgUrl = wxUserInfo.HeadImgUrl.Substring(0, wxUserInfo.HeadImgUrl.Length - 2);
                        //    ToLog(wxUserInfo.HeadImgUrl);
                        //    wxUserInfo.HeadImgUrl += "/132";
                        //    ToLog(wxUserInfo.HeadImgUrl);
                        //}

                        if (string.IsNullOrWhiteSpace(wxUserInfo.NickName))
                        {
                            //context.Response.Redirect(redirectUrl);//高级授权没拿到头像,重新授权
                        }
                        //else
                        //{
                           
                        //}


                        ToLog("开始更新用户微信信息");

                        //更新用户微信信息
                        currentUserInfo.WXCity = wxUserInfo.City;
                        currentUserInfo.WXCountry = wxUserInfo.Country;
                        currentUserInfo.WXHeadimgurl = wxUserInfo.HeadImgUrl;
                        currentUserInfo.WXNickname = wxUserInfo.NickName;
                        currentUserInfo.WXPrivilege = ZentCloud.Common.JSONHelper.ObjectToJson(wxUserInfo.Privilege);
                        currentUserInfo.WXProvince = wxUserInfo.Province;
                        currentUserInfo.WXSex = wxUserInfo.Sex;

                        ToLog("数据库用户AutoId：" + currentUserInfo.AutoID);
                        ToLog("数据库用户信息：" + JsonConvert.SerializeObject(currentUserInfo));
                        ToLog("微信头像 wxUserInfo.HeadImgUrl:" + wxUserInfo.HeadImgUrl);

                        var updateResult = bllUser.Update(currentUserInfo, string.Format(" WXHeadimgurl='{0}',WXNickname='{1}',WXProvince='{2}',WXCity='{3}'", wxUserInfo.HeadImgUrl, wxUserInfo.NickName.Replace("'", ""), wxUserInfo.Province.Replace("'", ""), wxUserInfo.City.Replace("'", "")), string.Format(" UserId='{0}' ", currentUserInfo.UserID));

                        //currentUserInfo.Province

                        ToLog("更新结果：" + updateResult.ToString());



                    }

                }
                else
                {
                    ToLog("WxUserInfoSource is null");
                }
            }
            #endregion

            ToLog("登陆成功，设置session和跳转");
            ToLog("isAuthToOpen：" + isAuthToOpen);
            //ToLog("有无redirectUrl的session：" + (context.Session["redirectUrl"] != null));
            //if (context.Session["redirectUrl"] != null)
            //{
            //    ToLog("redirectUrl：" + context.Session["redirectUrl"].ToString());
            //}

            wxAutoLoginToken.Uid = currentUserInfo.UserID;
            wxAutoLoginToken.Oid = currentUserInfo.WXOpenId;
            wxAutoLoginToken.IsUAuth = string.IsNullOrWhiteSpace(currentUserInfo.WXNickname) ? 0 : 1;

            //创建key 存入redis
            var key = "ltk:" + ZentCloud.Common.DEncrypt.ZCEncrypt(currentUserInfo.UserID);

            //key 加上微信appid
            var wxAppidKey = bllWebSite.GetWebsiteWXAppIdKey(currentWebsiteInfo.WebsiteOwner);

            if (!string.IsNullOrWhiteSpace(wxAppidKey))
            {
                key += wxAppidKey;
            }

            try
            {
                //过滤掉异常，防止redis缓存异常导致授权登陆失败

                ToLog("开始设置自动登陆tiken到redis和cookie，key:" + key);

                HttpCookie cookie = new HttpCookie(ZentCloud.Common.SessionKey.LoginCookie);
                cookie.Value = key;
                cookie.Expires = DateTime.Now.AddDays(30);
                context.Response.Cookies.Add(cookie);

                ToLog("自动登陆cookie设置成功");
                RedisHelper.RedisHelper.StringSetSerialize(key, wxAutoLoginToken, new TimeSpan(30,0,0,0));
                ToLog("自动登陆Redis设置成功");
            }
            catch (Exception ex)
            {
                ToLog("设置自动登陆tiken到redis和cookie异常：" + ex.Message);
            }

            //redirectUrl 加上一个时间戳 



            #region 默认授权
            if (!isAuthToOpen)//默认授权
            {
                //设置用户会话ID
                context.Session[ZentCloud.Common.SessionKey.UserID] = currentUserInfo.UserID;
                context.Session[ZentCloud.Common.SessionKey.LoginStatu] = 1; //设置登录状态
                context.Session["currWXOpenId"] = accessTokenModel.OpenId;
                context.Response.Redirect(redirectUrl);
                return false;
            }
            #endregion

            #region 微信开放平台授权
            else
            {
                string signKey = ZentCloud.Common.ConfigHelper.GetConfigString("WeixinOpenWebOAuthKey");// 开放平台网页授权Md5 Key
                string sign = ZentCloud.Common.DEncrypt.GetMD5(currentUserInfo.WXOpenId + signKey);//签名
                //开放平台授权，跳到统一处理Handler
                context.Response.Redirect(string.Format("http://{0}/WeixinOpen/WebOAuth.ashx?openid={1}&redirecturl={2}&sign={3}&websiteowner={4}&autologinkey={5}", ZentCloud.Common.MyRegex.GetDoMainByUrl(redirectUrl), currentUserInfo.WXOpenId, HttpUtility.UrlEncode(redirectUrl), sign,currentUserInfo.WebsiteOwner,HttpUtility.UrlEncode(key)));
                //ZentCloud.BLLJIMP.Model.WeixinOpenOAuthTemp tokenRecord = bllUser.Get<ZentCloud.BLLJIMP.Model.WeixinOpenOAuthTemp>(string.Format(" Token='{0}'", state));
                //if (tokenRecord != null)
                //{
                //    context.Response.Redirect(string.Format("http://{0}/WeixinOpen/WebOAuth.ashx?openid={1}&token={2}&sign={3}", ZentCloud.Common.MyRegex.GetDoMainByUrl(tokenRecord.Url), currentUserInfo.WXOpenId, state, sign));

                //}
                //else
                //{
                //    ToLog("tokenRecord null");
                //}    
 
                return false;
            }
            #endregion
        }

        private bool CheckNotAutoRegNewWxUser(HttpContext context, string redirectUrl)
        {
            ToLog("CheckNotAutoRegNewWxUser:" + ZentCloud.Common.JSONHelper.ObjectToJson(currentWebsiteInfo));
            if (string.IsNullOrWhiteSpace(currentWebsiteInfo.WebsiteOwner)) return false;

            //判断当前站点是否不允许微信自动注册新用户
            var autoNotRegNew = new BLLCommRelation().GetRelationInfo(ZentCloud.BLLJIMP.Enums.CommRelationType.WebsiteIsNotAutoRegNewWxUser, currentWebsiteInfo.WebsiteOwner, "");
            ToLog("AutoNotRegNew" + ZentCloud.Common.JSONHelper.ObjectToJson(autoNotRegNew));
            if (autoNotRegNew!=null)
            {
                if (autoNotRegNew.ExpandId != "1")
                {
                    var url = "/App/Member/Wap/RegisterBinding.aspx";
                    if (redirectUrl.IndexOf(url, StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        url += "?redirect=" + HttpUtility.UrlEncode(redirectUrl);
                        context.Response.Redirect(url);
                    }

                    //context.Response.Redirect("/App/Member/Wap/RegisterBinding.aspx?redirect=" + HttpUtility.UrlEncode(redirectUrl));
                } 
                return false;
            }
            return true;
        }

        /// <summary>
        /// 判断阶段及开始授权处理阶段
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool ToOAuth(HttpContext context, ZentCloud.BLLJIMP.Model.Other.WxAutoLoginToken wxAutoLoginToken)
        {

            ModuleFilterInfo matchModel = new ModuleFilterInfo();
            string scope = "snsapi_base";
            ToLog("检查路径是否需要微信授权：" + currentPath);

            //TODO:判断微信授权路径也存到redis
            
            //查询不在授权区则跳出处理
            if (!CheckIsToWXOAuth(currentPath, out matchModel))
            {
                ToLog(currentPath + "不在授权区");
                return false;
            }
            else
            {
                ToLog(currentPath + "在授权区");
                scope = matchModel.Ex1;
            }
            ToLog(currentPath + "开始检查授权 scope");
            scope = CheckCompentPage(context, scope, currentUrl);
            if (context.Session["scope"] != null)//用session 中的scope授权
            {
                scope = context.Session["scope"].ToString();
            }
            ToLog(currentPath + "检查授权scope 结束，scope:" + scope);

            if (wxAutoLoginToken != null)
            {
                if (scope == "snsapi_base" && !string.IsNullOrWhiteSpace(wxAutoLoginToken.Uid))
                {
                    context.Session[ZentCloud.Common.SessionKey.UserID] = wxAutoLoginToken.Uid;
                    context.Session[ZentCloud.Common.SessionKey.LoginStatu] = 1; //设置登录状态
                    context.Session["currWXOpenId"] = wxAutoLoginToken.Oid;
                    ToLog("通过 wxAutoLoginToken 自动登陆");
                    return false;
                }
            }
            

            //context.Session["wxscope"] = scope;
            //构造callBackUrl
            string callBackUrl = HttpUtility.UrlEncode(string.Format("http://{0}/wxcallback.aspx", context.Request.Url.Host));
            if (isAuthToOpen)//开放平台授权 callback到指定域名
            {
                callBackUrl = HttpUtility.UrlEncode(string.Format("http://{0}/wxcallback.aspx", authOpenDoMain));

            }
            string oauthUrl = "";

            ToLog(currentPath + "开始构造oauthUrl，isAuthToOpen:" + isAuthToOpen + currentUrl);


            #region 默认授权
            if (!isAuthToOpen)//默认授权
            {
                ToLog(currentPath + "开始构造默认授权 oauthUrl");

                oauthUrl = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect",
                    weixinAppId,
                    callBackUrl,
                    scope,
                    redirectUrlCacheKey
                );

                ToLog(currentPath + "结束构造默认授权 oauthUrl：" + oauthUrl);
            }
            #endregion

            #region 开放平台授权
            else//授权给开放平台 多一个参数component_appid
            {

                ToLog(currentPath + "开始构开放平台授权 oauthUrl");

                if (currentWebsiteInfo == null)
                {
                    ToLog(currentPath + "  currentWebsiteInfo == null");
                }
                else
                {
                    ToLog(currentPath + "  currentWebsiteInfo:" + JsonConvert.SerializeObject(currentWebsiteInfo));
                }


                if (string.IsNullOrEmpty(currentWebsiteInfo.AuthorizerAppId))
                {
                    ToLog(currentPath + " return false;  bllWeixinOpen.ComponentAppId:" + bllWeixinOpen.ComponentAppId);

                    return false;
                }

                ToLog(currentPath + "  bllWeixinOpen.ComponentAppId:" + bllWeixinOpen.ComponentAppId);

                oauthUrl = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}&component_appid={4}#wechat_redirect",
                   currentWebsiteInfo.AuthorizerAppId,
                   callBackUrl,
                   scope,
                   redirectUrlCacheKey,
                   bllWeixinOpen.ComponentAppId
               );

                ToLog(currentPath + "结束构造开放平台授权 oauthUrl：" + oauthUrl);

                //     ZentCloud.BLLJIMP.Model.WeixinOpenOAuthTemp tokenTemp = new ZentCloud.BLLJIMP.Model.WeixinOpenOAuthTemp();
                //     tokenTemp.Token = Guid.NewGuid().ToString();
                //     tokenTemp.Url = currentUrl;
                //     bllUser.Add(tokenTemp);
                //     oauthUrl = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}&component_appid={4}#wechat_redirect",
                //currentWebsiteInfo.AuthorizerAppId,
                //callBackUrl,
                //scope,
                //tokenTemp.Token,
                //bllWeixinOpen.ComponentAppId
                //);
            }
            #endregion

            ToLog("记录redirectUrl的Session：" + currentUrl);
            //context.Session["redirectUrl"] = currentUrl; //授权完成跳转页面
            try
            {
                RedisHelper.RedisHelper.StringSet(redirectUrlCacheKey, currentUrl, new TimeSpan(0, 0, 30));
            }
            catch (Exception)
            {
               ZentCloud.Common.DataCache.SetCache(redirectUrlCacheKey, currentUrl, DateTime.Now.AddSeconds(30));//记录当前连接cache过期时间30秒
                
            }


            ToLog(" redirectUrlCacheKey: " + redirectUrlCacheKey);
            ToLog(" currentUrl: " + currentUrl);

            //访问授权链接
            context.Response.Redirect(oauthUrl);

            return true;
        }

        /// <summary>
        /// 检查是否需要授权
        /// </summary>
        /// <param name="path"></param>
        /// <param name="matchModel"></param>
        /// <returns></returns>
        private bool CheckIsToWXOAuth(string path, out ModuleFilterInfo matchModel)
        {
            ToLog("进入检查是否需要授权");
            List<ModuleFilterInfo> pathList = new List<ModuleFilterInfo>();
            var sourcePathList = bllRedis.GetModuleFilterInfoList().Where(p => p.FilterType == "WXOAuth");
            foreach (var item in sourcePathList)
            {
                pathList.Add(new ModuleFilterInfo()
                {
                    FilterType=item.FilterType,
                    PagePath = item.PagePath,
                    FilterDescription = item.FilterDescription,
                    MatchType = item.MatchType,
                    Ex1=item.Ex1
                });
            }

            //try
            //{

            //    ToLog("开始从redis获取授权列表");

            //    //从redis读取数据，在修改的时候记得更新redis key
            //    pathList = RedisHelper.RedisHelper.Get<List<ModuleFilterInfo>>("WXModuleFilter");
            //    if (pathList == null) pathList = new List<ModuleFilterInfo>();

            //    ToLog("从redis读取授权数据：" + (pathList == null ? "null" : JsonConvert.SerializeObject(pathList)));

            //}
            //catch (Exception ex)
            //{
            //    ToLog(ex.ToString());
            //}

            //if (pathList.Count == 0)
            //{
            //    pathList = new BLL().GetList<ModuleFilterInfo>(" FilterType = 'WXOAuth' ");

            //    try
            //    {
            //        RedisHelper.RedisHelper.Set("WXModuleFilter", pathList);
            //    }
            //    catch (Exception ex)
            //    {
            //        ToLog(ex.ToString());
            //    }
            //}
            

            //ZentCloud.Common.DataCache.

            if (pathList.Count > 0)
            {
                //先进行全文匹配
                List<ModuleFilterInfo> searchResult = pathList.Where(p => p.PagePath.Equals(path, StringComparison.OrdinalIgnoreCase) && p.MatchType.Equals("all")).ToList();

                if (searchResult.Count > 0)
                {
                    matchModel = searchResult[0];
                    return true;
                }

                //开头匹配
                searchResult = pathList.Where(p => path.StartsWith(p.PagePath, StringComparison.OrdinalIgnoreCase) && p.MatchType.Equals("start")).ToList();
                if (searchResult.Count > 0)
                {
                    matchModel = searchResult[0];
                    return true;
                }

                //结尾匹配
                searchResult = pathList.Where(p => path.EndsWith(p.PagePath, StringComparison.OrdinalIgnoreCase) && p.MatchType.Equals("end")).ToList();
                if (searchResult.Count > 0)
                {
                    matchModel = searchResult[0];
                    return true;
                }

                //包含匹配
                searchResult = pathList.Where(p => path.ToLower().Contains(p.PagePath.ToLower()) && p.MatchType.Equals("contains")).ToList();
                if (searchResult.Count > 0)
                {
                    matchModel = searchResult[0];
                    return true;
                }

            }
            matchModel = null;
            return false;
        }
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="log"></param>
        private void ToLog(string log)
        {
            try
            {

                //return;
                //oauth.comeoncloud.net
                //if (currentUrl.ToLower().IndexOf("lanyueliang.comeoncloud.net") > -1 || currentUrl.ToLower().IndexOf("oauth.comeoncloud.net") > -1)
                //{

                //    using (StreamWriter sw = new StreamWriter(@"D:\log\wxoath.txt", true, Encoding.GetEncoding("gb2312")))
                //    {
                //        sw.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString(), log));
                //    }

                //    return;
                //}

                //if (currentUrl.ToLower().IndexOf("/serv/api/admin/config/get.ashx") > -1)
                //{
                //    return;
                //}
                //if (currentUrl.ToLower().IndexOf("/serv/api/admin/user/islogin.ashx") > -1)
                //{
                //    return;
                //}

                //using (StreamWriter sw = new StreamWriter(@"D:\WXOpenOAuthDevLog.txt", true, Encoding.GetEncoding("gb2312")))
                //{
                //    sw.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString(), log));
                //}


            }
            catch { }
        }

        /// <summary>
        /// 检查组件页面是否需要高级授权
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nScope"></param>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        private string CheckCompentPage(HttpContext context, string nScope, string redirectUrl)
        {
            if (nScope == "snsapi_userinfo") return nScope;
            string compentPage1 = "/customize/comeoncloud/index.aspx";
            string compentPage2 = "/customize/comeoncloud/m/index.aspx";
            string ngPage = redirectUrl.ToLower();
            if (!ngPage.Contains(compentPage1) && !ngPage.Contains(compentPage2)) return nScope;

            var cgid = context.Request["cgid"];
            var key = context.Request["key"];
            if (string.IsNullOrWhiteSpace(key) && string.IsNullOrWhiteSpace(cgid)) return nScope;
            //替换配置
            ZentCloud.BLLJIMP.BLLComponent bll = new ZentCloud.BLLJIMP.BLLComponent();
            ZentCloud.BLLJIMP.Model.Component model = new ZentCloud.BLLJIMP.Model.Component();
            if (!string.IsNullOrWhiteSpace(key))
            {
                model = bll.GetComponentByKey(key, bll.WebsiteOwner);
            }
            else
            {
                model = bll.Get<ZentCloud.BLLJIMP.Model.Component>(string.Format(" WebsiteOwner='{0}' AND AutoId={1}", bll.WebsiteOwner, cgid));
            }
            if (model.IsWXSeniorOAuth == 1)
                return "snsapi_userinfo";

            return nScope;
        }


    }
}
