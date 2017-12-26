using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BLLWXOAuthModule.Model;
using System.IO;
using ZCJson;
using ZentCloud.BLLJIMP;
namespace BLLWXOAuthModule
{

    /// <summary>
    /// 微信CallBack返回参数
    /// </summary>
    [Serializable]
    public class WXOAuthCallBackStateEntity
    {
        public string Path { get; set; }
      
    }

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
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

    }

    /// <summary>
    /// 微信OAuth2.0处理Module，通过网页授权认证后，可获得当前用户的 openid 及 用户相关信息（微信提供的）
    /// </summary>
    public class WXOAuthModule : IHttpModule
    {

        public void Dispose()
        {
            
        }
        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestStateForOpenIdLogin);
        }
        /// <summary>
        /// Session里面已存的OpenID
        /// </summary>
        string currOpenIDSession = string.Empty;
        /// <summary>
        /// 授权作用于平台用户登录(openId直接为平台登录用户)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_AcquireRequestStateForOpenIdLogin(object sender, EventArgs e)
        {
            try
            {
                HttpContext context = HttpContext.Current;
                ZentCloud.BLLJIMP.BLLUser userBll = new ZentCloud.BLLJIMP.BLLUser("");
                SystemSet systemSet = userBll.Get<SystemSet>("");
                ZentCloud.BLLJIMP.Model.WebsiteInfo currWebsiteInfoModel = (ZentCloud.BLLJIMP.Model.WebsiteInfo)context.Session["WebsiteInfoModel"];
                ZentCloud.BLLJIMP.Model.UserInfo currWebsiteOwner = userBll.GetUserInfo(currWebsiteInfoModel.WebsiteOwner);
                string currPath = context.Request.Path.ToLower();
                if (!context.Request.UserAgent.ToLower().Contains("micromessenger"))
                {
                    return;//非微信浏览器进入不执行授权
                }
                //验证aspx、ashx、chtml页面
                if (!ZentCloud.Common.IOHelper.GetExtraName(currPath).Equals("aspx") && !ZentCloud.Common.IOHelper.GetExtraName(currPath).Equals("chtml") && !ZentCloud.Common.IOHelper.GetExtraName(currPath).Equals("ashx"))
                    return;

                ZentCloud.BLLJIMP.Model.UserInfo userInfo = new ZentCloud.BLLJIMP.Model.UserInfo();
                string WeixinAppId = currWebsiteOwner.WeixinAppId;
                string WeixinAppSecret = currWebsiteOwner.WeixinAppSecret;
                if (!currWebsiteOwner.WeixinIsAdvancedAuthenticate.Equals(1))//未开通微信高级认证的不处理
                {
                    return;
                }
                #region CallBack处理阶段
                //ToLog("判断是不是callback");
                if (currPath.StartsWith("/wxcallback.aspx"))
                {
                    //ToLog("正在处理callback");
                    //接收Code及state
                    string code = context.Request["code"];
                    string state = context.Session["state"].ToString(); //context.Request["state"];//如果state太长微信的userInfo会报错，解决方案为将改字段放到session里可以了
                    WXOAuthCallBackStateEntity callBackStateInfo = new WXOAuthCallBackStateEntity();
                    if (!string.IsNullOrWhiteSpace(state))
                    {
                        callBackStateInfo = ZentCloud.Common.JSONHelper.JsonToModel<WXOAuthCallBackStateEntity>(ZentCloud.Common.Base64Change.DecodeBase64ByUTF8(state));
                    }
                    else
                    {
                        ToLog("context.Session[state] is null");
                        return;
                    }
                    //如果传入code为空，跳过处理
                    if (string.IsNullOrWhiteSpace(code))
                    {
                        ToLog("context.Request[code] is null");
                        return;
                    }
                    string getTokenUrl =
                        string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                            WeixinAppId,//systemset.WeixinAppId,
                            WeixinAppSecret,//systemset.WeixinAppSecret,
                            code );
                       

                    //获取OpenID及Access_token
                    string accessTokenSource = ZentCloud.Common.MySpider.GetPageSourceForUTF8(getTokenUrl);
                    if (string.IsNullOrWhiteSpace(accessTokenSource))
                    {
                        ToLog("ZentCloud.Common.MySpider.GetPageSourceForUTF8(getTokenUrl) is null");
                        return;
                    }
                    //ToLog("CallBack accessTokenSource:" + accessTokenSource);

                    WXOAuthAccessTokenEntity accessTokenModel = ZCJson.JsonConvert.DeserializeObject<WXOAuthAccessTokenEntity>(accessTokenSource);
                    if (accessTokenModel == null)
                    {
                       ToLog("ZCJson.JsonConvert.DeserializeObject<WXOAuthAccessTokenEntity>(accessTokenSource) is null");
                        return;
                    }
                    //ToLog("callback accessTokenModel:" + accessTokenModel);

                    //存储当前accessTokenModel到Session
                    context.Session[systemSet.WXOAuthAccessTokenEntityKey] = accessTokenModel;

                    //存储当前微信OpenID到Session，一旦存储有OpenID值和已登录标识，则不会再进行授权处理
                    context.Session[systemSet.WXCurrOpenerOpenIDKey] = accessTokenModel.OpenId;

                    //获取用户当前的openid-判断用户是否已注册-如果未注册则自动生成一个随机userId进行注册
                    userInfo = new ZentCloud.BLLJIMP.BLLUser("").GetUserInfoByOpenId(accessTokenModel.OpenId);

                    if (userInfo == null)
                    {
                        //ToLog("callback注册新用户");
                        //注册用户
                        userInfo = new ZentCloud.BLLJIMP.Model.UserInfo();
                        userInfo.UserID = string.Format("WXUser{0}{1}", ZentCloud.Common.StringHelper.GetDateTimeNum(), ZentCloud.Common.Rand.Str(5));//WXUser+时间字符串+随机5位数字
                        userInfo.Password = ZentCloud.Common.Rand.Str_char(12);
                        userInfo.UserType = 2;
                        userInfo.WebsiteOwner = currWebsiteOwner.UserID;
                        userInfo.Regtime = DateTime.Now;
                        userInfo.WXAccessToken = accessTokenModel.AccessToken;
                        userInfo.WXRefreshToken = accessTokenModel.RefreshToken;
                        userInfo.WXOpenId = accessTokenModel.OpenId;
                        userInfo.WXScope = accessTokenModel.Scope;
                        userInfo.RegIP = ZentCloud.Common.MySpider.GetClientIP();
                        userInfo.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
                        userInfo.LastLoginDate = DateTime.Now;
                        userInfo.LoginTotalCount = 1;
                        userBll.Add(userInfo);
                        //List<ZentCloud.BLLPermission.Model.UserPmsGroupRelationInfo> userPmsGroupList = new List<ZentCloud.BLLPermission.Model.UserPmsGroupRelationInfo>() {
                        //    new ZentCloud.BLLPermission.Model.UserPmsGroupRelationInfo()//分配用户组为基本用户组
                        //    {
                        //        UserID = userInfo.UserID,
                        //        GroupID = 110578
                        //    },
                        //    new ZentCloud.BLLPermission.Model.UserPmsGroupRelationInfo()//分配用户组为教育普通用户组
                        //    {
                        //        UserID = userInfo.UserID,
                        //        GroupID = 130334
                        //    }
                        //};

                        //userBll.AddList<ZentCloud.BLLPermission.Model.UserPmsGroupRelationInfo>(userPmsGroupList);

                        //userInfo.HFPmsGroupStr = userInfo.HFUserPmsGroup;
                        //userBll.Update(userInfo);

                    }
                    else
                    {
                        //ToLog("老用户登录");

                        //更新用户信息
                        //userInfo.WXAccessToken = accessTokenModel.AccessToken;
                        //userInfo.WXRefreshToken = accessTokenModel.RefreshToken;
                        //userInfo.WXOpenId = accessTokenModel.OpenId;
                        //userInfo.WXScope = accessTokenModel.Scope;
                        //userInfo.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
                        //userInfo.LastLoginDate = DateTime.Now;
                        //userInfo.LoginTotalCount++;

                        //userBll.Update(userInfo);
                    }


                    try
                    {
                        //如果是UserInfo方式，拉区微信用户信息并存储到会话，待后续页面使用
                        if (accessTokenModel.Scope.Contains("snsapi_userinfo"))
                        {
                            //ToLog("Callback正在拉取信息");

                            string userInfoSource = ZentCloud.Common.MySpider.GetPageSourceForUTF8(string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}",
                                    accessTokenModel.AccessToken,
                                    accessTokenModel.OpenId
                                ));

                            if (!string.IsNullOrWhiteSpace(userInfoSource))
                            {
                                ZentCloud.BLLJIMP.Model.Weixin.WeixinUserInfo weixinUserInfo = ZCJson.JsonConvert.DeserializeObject<ZentCloud.BLLJIMP.Model.Weixin.WeixinUserInfo>(userInfoSource);
                                context.Session[systemSet.WXCurrOpenerUserInfoKey] = weixinUserInfo;

                                //更新用户微信信息
                                userInfo.WXCity = weixinUserInfo.City;
                                userInfo.WXCountry = weixinUserInfo.Country;
                                userInfo.WXHeadimgurl = weixinUserInfo.HeadImgUrl;
                                ////当用户是从无昵称变为有昵称，则认为是注册用户，且当前为正式注册时间
                                //if (string.IsNullOrWhiteSpace(userInfo.WXNickname) && !string.IsNullOrWhiteSpace(weixinUserInfo.NickName))
                                //{
                                //   // userInfo.RegIP = ZentCloud.Common.MySpider.GetClientIP();
                                //    //userInfo.Regtime = DateTime.Now;
                                //}
                                userInfo.WXNickname = weixinUserInfo.NickName;
                                userInfo.WXPrivilege = ZentCloud.Common.JSONHelper.ObjectToJson(weixinUserInfo.Privilege);
                                userInfo.WXProvince = weixinUserInfo.Province;
                                userInfo.WXSex = weixinUserInfo.Sex;
                                userBll.Update(userInfo);


                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //ToLog("更新userInfo异常：" + ex.Message);
                        //如果拉区信息异常，则后续网页自行判断处理（可以自行根据accessToken拉取）
                    }


                    /*
                     * -- 2014.1.7
                     * 1.添加授权判断，如果没有昵称则返回重新用userInfo授权；
                     * 2.只有页面类型是userInfo授权类型的才执行该操作，本身是Base的不处理；
                     * 3.如果是userInfo页面类型，新用户也必须重新执行userInfo授权； 
                     * 
                     * 进入场景
                     * 1.新用户访问base页面；
                     * 2.新用户访问userinfo页面；
                     * 3.老用户访问base页面；
                     * 4.老用户访问userinfo页面；
                     * 
                     * 首次进入都是base授权，
                     * 
                     * userinfo页面下，判断是不是有微信昵称,没有微信昵称则需要进行userinfo授权
                     * 由base页面进入userinfo页面，再次进行base授权，然后由该阶段进行判断是否是需要重定向到userinfo授权
                     * 
                     * 
                     * 
                     */

                    if (context.Session["weixinscope"] == null)
                        context.Session["weixinscope"] = "";
                    //context.Session["PageMatchModel"]

                    //ToLog("开始进行正式用户判断");

                    if (context.Session["PageMatchModel"] != null)
                    {

                        ModuleFilterInfo pageMatchModel = (ModuleFilterInfo)context.Session["PageMatchModel"];
                        if (pageMatchModel.Ex1 == "snsapi_userinfo" && string.IsNullOrWhiteSpace(userInfo.WXNickname) && context.Session["weixinscope"].ToString() != "snsapi_userinfo")
                        {
                            //ToLog("不是正式用户");

                            context.Session[systemSet.WXCurrOpenerOpenIDKey] = "";
                            context.Session["weixinscope"] = "snsapi_userinfo";

                            context.Session["isRedoOath"] = "1";

                            //再次访问目标链接(未取到CurrOpenID，会重新进行UserInfo授权)
                            context.Response.Redirect(callBackStateInfo.Path);
                            return;
                        }
                        //ToLog("是正式用户");
                    }



                    //设置用户ID
                    context.Session[ZentCloud.Common.SessionKey.UserID] = userInfo.UserID;
                    //修改登录标识
                    context.Session[ZentCloud.Common.SessionKey.LoginStatu] = 1;
                    context.Response.Redirect(callBackStateInfo.Path);
                    return;
                }
                #endregion

                #region 判断阶段及开始授权处理阶段
                List<ModuleFilterInfo> FilterList = userBll.GetList<ModuleFilterInfo>(" FilterType = 'WXOAuth' ");
                currOpenIDSession = context.Session[systemSet.WXCurrOpenerOpenIDKey] == null ? "" : context.Session[systemSet.WXCurrOpenerOpenIDKey].ToString();
                if (context.Session[ZentCloud.Common.SessionKey.LoginStatu] == null)
                    context.Session[ZentCloud.Common.SessionKey.LoginStatu] = 0;

                ModuleFilterInfo matchModel = new ModuleFilterInfo();

                //判断是否是返回重新进行userInfo 授权
                string isRedoOath = context.Session["isRedoOath"] == null ? "" : context.Session["isRedoOath"].ToString();

                //检查如果已登录且openId也存在，则跳过授权处理，扫一扫登录也需要重新验证
                if (context.Session[ZentCloud.Common.SessionKey.LoginStatu].ToString().Equals("1")&& !string.IsNullOrWhiteSpace(currOpenIDSession))
                {

                    //增加判断，如果是在base授权区进入到UserInfo授权区，则需要重新授权，否则跳过-2013.12.26
                    //添加逻辑：并且用户没有昵称的时候才会去授权
                    if (context.Session["weixinscope"] != null)
                    {
                        string tmpscope = context.Session["weixinscope"].ToString();

                        if (tmpscope == "snsapi_base")
                        {
                            if (CheckIsToWXOAuth(currPath, out matchModel))
                            {
                                //ToLog("matchModel.Ex1:" + matchModel.Ex1);
                                userInfo = new ZentCloud.BLLJIMP.BLLUser("").GetUserInfoByOpenId(context.Session[systemSet.WXCurrOpenerOpenIDKey] == null ? "" : context.Session[systemSet.WXCurrOpenerOpenIDKey].ToString());
                                if (userInfo == null)
                                    userInfo = new ZentCloud.BLLJIMP.Model.UserInfo();
                                if (matchModel.Ex1 == "snsapi_userinfo" && string.IsNullOrWhiteSpace(userInfo.WXNickname))
                                {
                                    //不跳出而进行重新授权


                                }
                                else
                                {
                                    return;
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }




                if (isRedoOath == "1")
                {
                    context.Session["weixinscope"] = "snsapi_userinfo";
                    context.Session["isRedoOath"] = "0";
                }
                else
                {
                    context.Session["weixinscope"] = "snsapi_base";
                }



                string scope = "snsapi_base";
                if (context.Session["weixinscope"] != null)
                    scope = context.Session["weixinscope"].ToString();

                if (string.IsNullOrWhiteSpace(scope))
                {
                    context.Session["weixinscope"] = "snsapi_base";
                    scope = "snsapi_base";
                }



                //查询不在授权区则跳出处理
                if (!CheckIsToWXOAuth(currPath, out matchModel))
                {
                    //ToLog("不在授权区");
                    return;
                }
                //ToLog("在授权区");

                //保存当前页面匹配实体
                context.Session["PageMatchModel"] = matchModel;

                //构造callBackUrl
                string callBackUrl = string.Format("http://{0}/wxcallback.aspx", context.Request.Url.Host);

                //构造返回参数
                WXOAuthCallBackStateEntity stateInfo = new WXOAuthCallBackStateEntity()
                {
                    Path = context.Request.Url.AbsoluteUri
                };


                string oauthUrl = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect",
                WeixinAppId,
                callBackUrl,
                scope,
                "STATE"
                );



                //防止state过长微信的userInfo类型会报错，将该字段目前存session，后面有必要可以再改为其他映射关系
                context.Session["state"] = ZentCloud.Common.Base64Change.EncodeBase64ByUTF8(ZentCloud.Common.JSONHelper.ObjectToJson(stateInfo));


                //访问授权链接
                context.Response.Redirect(oauthUrl);
                #endregion

                /*
             * 判断阶段：
             * 
             * (判断是否需拦截（CallBack回来，结尾带上标签#WxCallBack）----该方式不成立，如果转发分享这个链接到朋友圈之类的就不行了)
             * 获取用户名，获取Session中存储的CurrOpenID及UserID
             * 判断当前用户是否已进行微信认证,只有微信认证过的用户才允许使用网页授权
             * 判断Session，是否已有CurrOpenID，判断新的用户名是否等于旧的用户名：如果已有CurrOpenID但是新的用户名跟旧的用户名不一致，则重新授权获取当前的OpenID
             * (这样判断解决问题：比如有两张页面，第一张页面是张三发的报名，第二张页面是李四发的报名，而openID在张三和李四这边都不一样)
             * 
             * 判断是否符合拦截条件
             * 
             * 最后不管符合不符合拦截条件，如果UserID存在有并是数据库用户，都存储用户ID
             * 
             * --------------------------------------
             * 
             * 开始授权处理阶段：
             * 
             * 获取配置的高级接口参数
             * 构造CallBackUrl
             * 构造返回参数：目前传递Path(当前路径)，UserID
             * 构造授权链接
             * 访问授权链接
             * 
             * ---------------------------------------
             * CallBack处理阶段:
             * 接收参数 code，Path，UserId,CurrOpenId
             * 构造授权API
             * 获取OpenID
             * 构造目标链接：Path + UserID 
             * 存储当前微信OpenID到Session
             * 访问最终目标链接(已取到CurrOpenID)
             * 
             * 2013.12.06修改逻辑：自动判断授权模式
             * callback回来后，根据openId查询获取是不是会员，如果不是会员，则重新返回授权用userInfo方式
             * 注册会员那边自动添加拉取到的微信会员信息
             * 
             * 
             * 2013.12.19
             * 更改后的方法升级：1.直接授权后登录；2.不是会员的则会自动注册会员；3.openId直接是登录凭据；
             * 
             * 判断是否授权依据是登录标识为登出，登出状态才进行授权登录，openId为空也进行登录
             * 
             * 获取用户当前的openid-判断用户是否已注册-如果未注册则自动生成一个随机userId进行注册
             * 
             * 登录标识标为已登录，并获取openId及其他信息
             * 
             * 
             * 
             */

            }
            catch (Exception ex)
            {
                //ToLog(ex.Message);
            }
        }
   

        private bool CheckIsToWXOAuth(string pushContent, out ModuleFilterInfo matchModel)
        {
            List<ModuleFilterInfo> keywordList = new BLL().GetList<ModuleFilterInfo>(" FilterType = 'WXOAuth' ");

            if (keywordList.Count > 0)
            {
                //先进行全文匹配
                var searchResult = keywordList.Where(p => p.PagePath.Equals(pushContent, StringComparison.OrdinalIgnoreCase) && p.MatchType.Equals("all")).ToList();

                if (searchResult.Count > 0)
                {
                    matchModel = searchResult[0];
                    return true;
                }

                //开头匹配
                searchResult = keywordList.Where(p => pushContent.StartsWith(p.PagePath, StringComparison.OrdinalIgnoreCase) && p.MatchType.Equals("start")).ToList();
                if (searchResult.Count > 0)
                {
                    matchModel = searchResult[0];
                    return true;
                }

                //结尾匹配
                searchResult = keywordList.Where(p => pushContent.EndsWith(p.PagePath, StringComparison.OrdinalIgnoreCase) && p.MatchType.Equals("end")).ToList();
                if (searchResult.Count > 0)
                {
                    matchModel = searchResult[0];
                    return true;
                }

                //包含匹配
                searchResult = keywordList.Where(p => pushContent.ToLower().Contains(p.PagePath.ToLower()) && p.MatchType.Equals("contains")).ToList();
                if (searchResult.Count > 0)
                {
                    matchModel = searchResult[0];
                    return true;
                }

            }
            matchModel = null;
            return false;
        }

        private void ToLog(string msg)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"C:\log.txt", true, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), msg));
                }
            }
            catch { }
        }


    }
}
