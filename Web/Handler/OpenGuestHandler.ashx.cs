using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using System.Web.SessionState;
using System.Text;
using System.IO;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.Handler
{
    /// <summary>
    /// 
    /// </summary>
    public class OpenGuestHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 默认响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse();
        /// <summary>
        /// 短信响应模型
        /// </summary>
        SendSmsResponse respSms = new SendSmsResponse();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser("");
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJuActivity bllJuactivity = new BLLJuActivity();
        /// <summary>
        /// 短信
        /// </summary>
        BLLSMS bllSms = new BLLSMS("");
        /// <summary>
        /// 微信BLL
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new BLLWeixin("");
        /// <summary>
        /// 真实活动
        /// </summary>
        BLLActivity bllActivity = new BLLActivity("");

        /// <summary>
        /// 日志
        /// </summary>
        BLLLog bllLog = new BLLLog();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            string action = context.Request["Action"];
            switch (action)
            {

                case "Login":
                    result = Login(context);
                    break;
                case "UpdateToLogoutSession":
                    result = UpdateToLogoutSession(context);
                    break;
                //case "CreateQRcodeLogin":
                //    result = CreateQRcodeLogin(context);
                //    break;
                //case "ToQRcodeLogin":
                //    result = ToQRcodeLogin(context);
                //    break;
                //case "ChcekQRcodeLogin":
                //    result = ChcekQRcodeLogin(context);
                //    break;
                //case "WXShareRecord":
                //    result = WXShareRecord(context);
                // break;
                case "UpdateToLogoutSessionIsRedoOath":
                    result = UpdateToLogoutSessionIsRedoOath(context);
                    break;
                case "CheckLogin"://检查登录状态
                    result = CheckLogin(context);
                    break;
                case "RegUser"://注册新用户
                    result = RegUser(context);
                    break;
                case "UpdateArticleIPPVShareCount"://修改文章活动 IP PV 分享数
                    result = UpdateArticleIPPVShareCount(context);
                    break;
                case "AddGameEventDetail"://记录游戏事件
                    result = AddGameEventDetail(context);
                    break;
                //case "GetSmsVerificationCode"://获取手机验证码
                //    result = GetSmsVerificationCode(context);
                //    break;
                //case "SendSmsYouyi"://提交成功后发送消息(友谊互动)
                //    result = SendSmsYouyi(context);
                //    break;
                case "SumbitQuestionnaire"://提交问卷
                    result = SumbitQuestionnaire(context);
                    break;
                case "SumbitExam"://提交问卷
                    result = SumbitExam(context);
                    break;
                case "SignUpDataInfo"://报名字段
                    result = SignUpDataInfo(context);
                    break;
                case "GetActivityDataInfos":
                    result = GetActivityDataInfos(context);
                    break;
                case "GetADInfos"://报名人数列表
                    result = GetADInfos(context);
                    break;

                case "GetCurrentUserInfo":
                    result = GetCurrentUserInfo(context);
                    break;

                //case "GerVerCodeWubuHui":
                //    result = GerVerCodeWubuHui(context);
                //    break;

                case "AdminLogin":
                    result = AdminLogin(context);
                    break;
                case "GetAddVImageWap"://微信加V 移动设备端
                    result = GetAddVImageWap(context);
                    break;

            }

            context.Response.ClearContent();
            context.Response.Write(result);
        }

        private string FormartDate(object date)
        {
            return Convert.ToDateTime(date).ToString("HH:mm");
        }

        private string GetImg(object openId)
        {
            BLLJIMP.Model.UserInfo userInfo = bllUser.Get<BLLJIMP.Model.UserInfo>(string.Format(" WXOpenId='{0}'", openId));
            if (userInfo != null)
            {
                if (string.IsNullOrEmpty(userInfo.WXHeadimgurlLocal))
                {
                    return "/img/6.png";
                }
                else
                {
                    return userInfo.WXHeadimgurlLocal;
                }

            }
            return "/img/6.png";
        }

        /// <summary>
        /// 已报名人数列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetADInfos(HttpContext context)
        {
            string pageIndex = context.Request["PageIndex"];
            string activityId = context.Request["ActivityId"];

            JuActivityInfo juActivityInfo = bllJuactivity.GetJuActivityByActivityID(activityId);
            //if (juActivityInfo.IsShowPersonnelList==0)
            //{
            //    resp.ExInt = 1;
            //    return Common.JSONHelper.ObjectToJson(resp);
            //}
            List<BLLJIMP.Model.ActivityDataInfo> activityDataList = bllUser.GetColList<BLLJIMP.Model.ActivityDataInfo>(10, Convert.ToInt32(pageIndex), string.Format("ActivityID='{0}' AND IsDelete=0 ", activityId), " InsertDate DESC", "ActivityID,UID,Name,InsertDate,WeixinOpenID");
            if (activityDataList != null)
            {
                resp.Status = 0;
                //List<BLLJIMP.Model.ActivityDataInfo> newaDataInfos = new List<ActivityDataInfo>();
                //foreach (BLLJIMP.Model.ActivityDataInfo item in activityDataList)
                //{
                //    var model = new ActivityDataInfo();
                //    model.Name = item.Name;
                //    model.K2 = item.InsertDate.ToString("yyyy/MM/dd HH:mm");
                //    model.K1 = GetImg(item.WeixinOpenID);
                //    if (juActivityInfo.ShowPersonnelListType.Equals(1))
                //    {
                //        model.Name = model.Name.Substring(0, 1) + "**";
                //    }
                //    newaDataInfos.Add(model);

                //}

                //resp.ExObj = newaDataInfos;
                resp.ExObj = from p in activityDataList
                             select new
                             {
                                 Name = juActivityInfo.ShowPersonnelListType.Equals(1) ? p.Name.Substring(0, 1) + "**" : p.Name,
                                 K2 = p.InsertDate.ToString("yyyy/MM/dd HH:mm"),
                                 K1 = GetImg(p.WeixinOpenID)
                             };

                resp.ExInt = bllUser.GetList<BLLJIMP.Model.ActivityDataInfo>(string.Format("ActivityID='{0}' AND IsDelete=0", activityId)).Count;
            }
            else
            {
                resp.Status = 0;
                resp.ExObj = null;
            }
            BLLJIMP.Model.ActivityConfig aconfig = bllUser.Get<BLLJIMP.Model.ActivityConfig>(string.Format(" WebsiteOwner='{0}'", DataLoadTool.GetWebsiteInfoModel().WebsiteOwner));
            if (aconfig == null)
            {
                aconfig = new BLLJIMP.Model.ActivityConfig() { TheOrganizers = "" };
            }
            resp.ExStr = aconfig.TheOrganizers;

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 活动列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetActivityDataInfos(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            string activityName = context.Request["ActivityName"];
            string ctype = context.Request["ctype"];
            string sort = context.Request["sort"];
            string preId = context.Request["preId"];
            int totalCount = 0;
            BLLJIMP.Model.ActivityConfig activityConfig = new ActivityConfig();
            activityConfig = bllUser.Get<BLLJIMP.Model.ActivityConfig>(string.Format(" WebsiteOwner='{0}'", bllUser.WebsiteOwner));
            if (activityConfig == null)
            {
                activityConfig = new ActivityConfig();
            }
            bool showStopEndDateData = false;
            bool showHide = true;
            if (activityConfig.IsShowHideActivity == 1)
            {
                showStopEndDateData = true;
                showHide = false;
            }
            List<JuActivityInfo> data = bllJuactivity.QueryJuActivityData(null, out totalCount, null, null, null, null, activityName, pageIndex
                , pageSize, null, null, "activity", bllJuactivity.WebsiteOwner, null, ctype, preId, null, null, null, null, false, sort, false, showHide, false, "", "", showStopEndDateData);

            foreach (var item in data)
            {
                item.ActivityDescription = "";
            }
            if (data != null && data.Count != 0)
            {

                resp.Status = 1;
                resp.ExObj = data;
                resp.ExInt = activityConfig.ActivityStyle;
                resp.ExStr = totalCount.ToString();
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "没有数据";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 报名字段信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SignUpDataInfo(HttpContext context)
        {

            string activityId = context.Request["signUpId"];
            string pathName = context.Request["pathname"];
            if (string.IsNullOrEmpty(activityId))
            {
                resp.Status = -1;
                resp.Msg = "系统出错，请联系管理员!";
                goto OutF;

            }
            StringBuilder sbSignInHtml = new StringBuilder();
            List<BLLJIMP.Model.ActivityFieldMappingInfo> mapList = bllActivity.GetActivityFieldMappingList(activityId).Where(p => p.IsHideInSubmitPage != "1" && p.FieldName != "Name" && p.FieldName != "Phone").ToList();
            BLLJIMP.Model.ActivityInfo activityInfo = bllUser.Get<BLLJIMP.Model.ActivityInfo>(string.Format(" ActivityID='{0}'", activityId));

            //BLLJIMP.Model.UserInfo userInfo = bllUser.Get<BLLJIMP.Model.UserInfo>(string.Format(" UserId='{0}'", activityInfo.UserID));
            if (mapList != null)
            {
                resp.Status = 0;
                resp.ExObj = mapList;
                //sbSignInHtml.AppendFormat("<input id=\"loginName\" type=\"hidden\" value=\"{0}\" name=\"LoginName\" />", ZentCloud.Common.Base64Change.EncodeBase64ByUTF8(userInfo.UserID));//外部登录名
                //sbSignInHtml.AppendFormat("<input id=\"loginPwd\" type=\"hidden\" value=\"{0}\" name=\"LoginPwd\" />", ZentCloud.Common.DEncrypt.ZCEncrypt(userInfo.Password));//外部登录密码


                sbSignInHtml.AppendFormat("<input  type=\"hidden\" value=\"{0}\" name=\"MonitorPlanID\" />", bllJuactivity.GetJuActivityByActivityID(activityInfo.ActivityID).MonitorPlanID);

                try
                {
                    var par = pathName.Split('/');
                    if (par.Length >= 4)
                    {
                        var spreadUserIdhex = par[2];
                        int spreadId = Convert.ToInt32(spreadUserIdhex, 16);
                        var spreadUserInfo = bllUser.GetUserInfoByAutoID(spreadId);
                        sbSignInHtml.AppendFormat("<input  type=\"hidden\" value=\"{0}\" name=\"SpreadUserID\" />", spreadUserInfo.UserID);
                    }
                    JuActivityInfo juActivityInfo = bllJuactivity.GetJuActivityByActivityID(activityId);
                    if (juActivityInfo.IsShowPersonnelList == 0)
                    {
                        resp.ExInt = 1;
                    }

                }
                catch (Exception)
                {


                }
                resp.ExStr = sbSignInHtml.ToString();

            }


        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        //private string WXShareRecord(HttpContext context)
        //{
        //    //活动分享记录
        //    //活动ID
        //    string activityId = context.Request["aid"];
        //    string msg = context.Request["msg"];
        //    string stype = context.Request["stype"];
        //    string openId = "";
        //    string userId = "";
        //    string ext1 = context.Request["ext1"];

        //    try
        //    {
        //        userId = context.Session["userID"] == null ? "" : context.Session["userID"].ToString();
        //    }
        //    catch { }

        //    try
        //    {
        //        openId = context.Session["WXCurrOpenerOpenID"] == null ? "" : context.Session["WXCurrOpenerOpenID"].ToString();//WXCurrOpenerOpenID
        //    }
        //    catch { }

        //    ToLog("活动：" + activityId);
        //    ToLog("信息：" + msg);
        //    ToLog("用户：" + userId);
        //    ToLog("OpenID：" + openId);
        //    ToLog("ext1：" + ext1);

        //    return "";
        //    //throw new NotImplementedException();
        //}

        //private string CreateQRcodeLogin(HttpContext context)
        //{
        //    //创建登录凭证
        //    string tiketValue = Guid.NewGuid().ToString();
        //    Common.DataCache.SetCache(tiketValue, tiketValue);

        //    //构造手机登录地址
        //    StringBuilder strWapUrl = new StringBuilder();
        //    strWapUrl.AppendFormat("http://{0}/Handler/QRLogin.ashx?tiket={1}",
        //            context.Request.Url.Host,
        //            this.userBll.TransmitStringEnCode(tiketValue)
        //        );

        //    string str = Common.DataCache.GetCache(tiketValue).ToString();

        //    List<string> data = new List<string>();

        //    data.Add(this.userBll.TransmitStringEnCode(tiketValue));
        //    data.Add(strWapUrl.ToString());

        //    resp.Status = 1;
        //    resp.ExObj = data;

        //    return Common.JSONHelper.ObjectToJson(resp);

        //}

        //private string ChcekQRcodeLogin(HttpContext context)
        //{
        //    /* 
        //     * 电脑检查登录端
        //     * 检查登录凭证是否已经登录
        //     * 登录则修改登录状态
        //     * 
        //    */


        //    string tiketKey = context.Request["tiket"];

        //    if (string.IsNullOrWhiteSpace(tiketKey))
        //    {
        //        resp.Status = -1;
        //        resp.Msg = "登录凭据不能为空!";
        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }

        //    tiketKey = this.userBll.TransmitStringDeCode(tiketKey);

        //    if (Common.DataCache.GetCache(tiketKey) == null)
        //    {
        //        resp.Status = -1;
        //        resp.Msg = "登录凭据不存在!";
        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }

        //    string tiketValue = Common.DataCache.GetCache(tiketKey).ToString();

        //    resp.Msg = "tiketValue:" + tiketValue;

        //    if (string.IsNullOrWhiteSpace(tiketValue))
        //    {
        //        resp.Status = -1;
        //        resp.Msg = "登录凭据不存在!";
        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }

        //    if (tiketValue.EndsWith("-login"))
        //    {
        //        resp.Status = 1;
        //        resp.Msg = "登录成功!";

        //        //获取登录用户，并设置登录状态
        //        BLLJIMP.Model.UserInfo loginUser = (BLLJIMP.Model.UserInfo)Common.DataCache.GetCache(tiketKey + "-user");

        //        context.Session[SessionKey.LoginStatu] = 1;
        //        context.Session[SessionKey.UserID] = loginUser.UserID;
        //        context.Session[SessionKey.UserType] = loginUser.UserType;

        //        this.userBll.AddLoginLogs(loginUser.UserID);

        //        //销毁登录凭据及相关数据
        //        Common.DataCache.ClearCache(tiketKey);
        //        Common.DataCache.ClearCache(tiketKey + "-user");

        //        return Common.JSONHelper.ObjectToJson(resp);
        //    }

        //    resp.Status = 0;

        //    return Common.JSONHelper.ObjectToJson(resp);
        //}

        //private string ToQRcodeLogin(HttpContext context)
        //{
        //    /* 
        //     * 手机访问登录端
        //     * 检查有登录并授权过后才进行登录成功处理
        //     * 修改相关登录凭据
        //     * 跳转到用户中心页面
        //     * 
        //     */
        //    string tiketKey = context.Request["tiket"];

        //    if (string.IsNullOrWhiteSpace(tiketKey))
        //    {
        //        return "登录凭据不存在";
        //    }

        //    tiketKey = this.userBll.TransmitStringDeCode(tiketKey);

        //    if (Common.DataCache.GetCache(tiketKey) == null)
        //    {
        //        return "登录凭据不存在";
        //    }

        //    Common.DataCache.SetCache(tiketKey, tiketKey + "-login");
        //    Common.DataCache.SetCache(tiketKey + "-user", DataLoadTool.GetCurrUserModel());

        //    context.Response.Redirect("/FShare/Wap/UserHub.aspx");

        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// 更新为登出状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateToLogoutSession(HttpContext context)
        {
            context.Session.Clear();
            //context.Session["login"] = 0;
            resp.Status = 1;
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 更新为登出状态并重载微信头像
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateToLogoutSessionIsRedoOath(HttpContext context)
        {

            try
            {
                //删除redis 自动登陆key

                var comeoncloudAutoLoginToken = context.Request.Cookies[ZentCloud.Common.SessionKey.LoginCookie].Value;

                if (comeoncloudAutoLoginToken != null)
                {
                    RedisHelper.RedisHelper.KeyDelete(comeoncloudAutoLoginToken);
                }

            }
            catch (Exception ex)
            {

            }
            context.Session.Clear();
            context.Session.RemoveAll();
            resp.Status = 1;
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 登录用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Login(HttpContext context)
        {
            ZentCloud.BLLJIMP.Model.UserInfo modelUserInfo = new BLLJIMP.Model.UserInfo();
            string msg = string.Empty;
            string userId = context.Request["userID"];
            string pwd = context.Request["pwd"];

            if (string.IsNullOrWhiteSpace(userId))
            {
                resp.Status = 0;
                resp.Msg = "用户名不能为空!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrWhiteSpace(pwd))
            {
                resp.Status = 0;
                resp.Msg = "密码不能为空!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (!bllUser.FilterSql(userId))
            {
                resp.Status = 0;
                resp.Msg = "危险的参数值!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (!bllUser.FilterSql(pwd))
            {
                resp.Status = 0;
                resp.Msg = "危险的参数值!";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (this.bllUser.Login(userId, pwd, out modelUserInfo, out msg, this.bllUser.WebsiteOwner))
            {
                if (modelUserInfo.WebsiteOwner == bllUser.WebsiteOwner)
                {
                    resp.Status = 1;
                    context.Session[SessionKey.LoginStatu] = 1;
                    context.Session[SessionKey.UserID] = modelUserInfo.UserID;
                    context.Session[SessionKey.UserType] = modelUserInfo.UserType;
                    context.Session[SessionKey.WXCurrOpenerOpenIDKey] = modelUserInfo.WXOpenId;
                    ////如果openid为空则随机分配个随机数，然后更新用户表
                    //if (string.IsNullOrWhiteSpace(modelUserInfo.WXOpenId))
                    //{
                    //    modelUserInfo.WXOpenId = Guid.NewGuid().ToString();
                    //    context.Session[SessionKey.WXCurrOpenerOpenIDKey] = modelUserInfo.WXOpenId;
                    //    this.userBll.Update(modelUserInfo);
                    //}
                    this.bllUser.AddLoginLogs(modelUserInfo.UserID);

                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "用户名或密码错误!";

                }
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "用户名或密码错误!";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 检查登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string CheckLogin(HttpContext context)
        {
            if (bllUser.IsLogin)
            {
                resp.Status = 1;
            }
            else
            {
                resp.Status = 0;
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string RegUser(HttpContext context)
        {


            string userId = context.Request["userID"];
            string pwd = context.Request["pwd"];
            string verCode = context.Request["VerCode"];
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(verCode))
            {
                resp.Status = 0;
                resp.Msg = "信息不完整";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (context.Session["CheckCode"] == null)
            {
                resp.Status = 0;
                resp.Msg = "验证码必填";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (context.Session["CheckCode"].ToString().ToLower() != verCode.ToLower())
            {
                resp.Status = 0;
                resp.Msg = "验证码错误";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            string message;
            if (bllUser.RegUser(userId, pwd, out message))
            {
                resp.Status = 1;
                context.Session[SessionKey.UserID] = userId;
                context.Session[SessionKey.LoginStatu] = 1;
                context.Session[SessionKey.UserType] = "2";


            }
            else
            {
                resp.Status = 0;

            }
            resp.Msg = message;
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 更新 IP PV 分享数
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateArticleIPPVShareCount(HttpContext context)
        {
            string juActivityIDHex = context.Request["idhex"];
            int id = Convert.ToInt32(juActivityIDHex, 16);
            bool resultip = bllJuactivity.UpdateIPCount(id);
            bool resultpv = bllJuactivity.UpdatePVCount(id);
            bool reseultsharecount = bllJuactivity.UpdateTotalShareCount(id);
            return (resultip && resultpv && reseultsharecount).ToString();

        }
        /// <summary>
        /// 广告点击事件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddGameEventDetail(HttpContext context)
        {
            int planID = int.Parse(context.Request["PID"]);
            string clickUrl = context.Request["DataUrl"];
            BllGame bllGame = new BllGame();
            ZentCloud.BLLJIMP.Model.GameEventDetailInfoClick model = new BLLJIMP.Model.GameEventDetailInfoClick();
            model.GamePlanID = planID;
            model.ClickUrl = clickUrl;
            model.EventBrowserUserAgent = context.Request.UserAgent;
            model.EventBrowser = context.Request.Browser.Browser;
            model.EventDate = DateTime.Now;
            model.EventSysPlatform = context.Request.Browser.Platform;
            model.SourceIP = Common.MySpider.GetClientIP();
            model.IPLocation = Common.MySpider.GetIPLocation(model.SourceIP);
            model.WebsiteOwner = bllGame.WebsiteOwner;
            if (bllGame.AddGameEventDetailClick(model))
            {
                if (bllGame.UpdateGamePlanClickCount(planID))
                {
                    resp.Status = 1;
                }


            }
            else
            {
                resp.Status = 0;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetCurrentUserInfo(HttpContext context)
        {

            if (bllUser.IsLogin)
            {
                resp.IsSuccess = true;
                resp.Status = 1;
                resp.ExObj = bllUser.GetCurrentUserInfo();
            }
            else
            {
                string userId = bllUser.GetUserInfoByLoginCookie();
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    UserInfo user = bllUser.GetUserInfo(userId, bllUser.WebsiteOwner);
                    if (user != null)
                    {
                        context.Session[ZentCloud.Common.SessionKey.UserID] = user.UserID;
                        context.Session[ZentCloud.Common.SessionKey.LoginStatu] = 1; //设置登录状态
                        resp.IsSuccess = true;
                        resp.Status = 1;
                        resp.ExObj = user;
                    }
                    else
                    {
                        resp.Status = 0;
                    }
                }
                else
                {
                    resp.Status = 0;
                }
            }

            return Common.JSONHelper.ObjectToJson(resp);

        }

        ///// <summary>
        ///// 获取手机验证码
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public string GetSmsVerificationCode(HttpContext context)
        //{

        //    BLLSMS bllSms = new BLLSMS("");
        //    string Phone = context.Request["phone"];
        //    if (string.IsNullOrEmpty(Phone))
        //    {
        //        respSms.Status = 0;
        //        respSms.Msg = "请输入手机号";
        //        return Common.JSONHelper.ObjectToJson(respSms);


        //    }
        //    if (!Common.ValidatorHelper.PhoneNumLogicJudge(Phone))
        //    {
        //        respSms.Status = 0;
        //        respSms.Msg = "手机号码无效!";
        //        return Common.JSONHelper.ObjectToJson(respSms);

        //    }
        //    var LastSmsVerificationCode = bllSms.GetLastSmsVerificationCodeByPhone(Phone);
        //    if (LastSmsVerificationCode != null)
        //    {

        //        if ((DateTime.Now - LastSmsVerificationCode.InsertDate).TotalSeconds < 60)
        //        {
        //            respSms.Status = 0;
        //            respSms.Msg = "验证码限制每60秒发送一次";
        //            return Common.JSONHelper.ObjectToJson(respSms);

        //        }

        //    }
        //    bool isSuccess = false;
        //    string VerCode = "";
        //    string Msg = "";
        //    bllSms.CreateSmsVerificationCode(Phone, out VerCode, out isSuccess, out Msg);
        //    if (isSuccess)
        //    {
        //        HttpContext.Current.Session["SmsVerificationCode"] = VerCode;
        //        respSms.Status = 1;
        //    }
        //    respSms.VerCode = null;
        //    respSms.Msg = Msg;
        //    return Common.JSONHelper.ObjectToJson(respSms);

        //}


        ///// <summary>
        ///// 获取手机验证码 五步会
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public string GerVerCodeWubuHui(HttpContext context)
        //{

        //    BLLSMS bllSms = new BLLSMS("");
        //    string Phone = context.Request["phone"];
        //    if (string.IsNullOrEmpty(Phone))
        //    {
        //        respSms.Status = 0;
        //        respSms.Msg = "请输入手机号";
        //        return Common.JSONHelper.ObjectToJson(respSms);


        //    }
        //    if (!Common.ValidatorHelper.PhoneNumLogicJudge(Phone))
        //    {
        //        respSms.Status = 0;
        //        respSms.Msg = "手机号码无效!";
        //        return Common.JSONHelper.ObjectToJson(respSms);

        //    }
        //    var LastSmsVerificationCode = bllSms.GetLastSmsVerificationCodeByPhone(Phone);
        //    if (LastSmsVerificationCode != null)
        //    {

        //        if ((DateTime.Now - LastSmsVerificationCode.InsertDate).TotalSeconds < 60)
        //        {
        //            respSms.Status = 0;
        //            respSms.Msg = "验证码限制每60秒发送一次";
        //            return Common.JSONHelper.ObjectToJson(respSms);

        //        }

        //    }
        //    bool isSuccess = false;
        //    string VerCode = "";
        //    string Msg = "";
        //    string SendMsg="铛！铛！铛！欢迎您注册五步会，抬头看天空，飘来八个字：五步邀您干大事儿！请输入您的五步会通关密码：";
        //    string smsSignature="五步会";
        //    bllSms.CreateSmsVerificationCode(Phone, SendMsg, smsSignature, out VerCode, out isSuccess, out Msg);
        //    if (isSuccess)
        //    {
        //        HttpContext.Current.Session["SmsVerificationCode"] = VerCode;
        //        respSms.Status = 1;
        //    }
        //    respSms.VerCode = null;
        //    respSms.Msg = Msg;
        //    return Common.JSONHelper.ObjectToJson(respSms);

        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public string SendSmsYouyi(HttpContext context)
        //{
        //    //TODO: 该方法需要修改，在开放的接口无任何验证可能引发乱发短信问题，被其他程序员捉住bug宣传不好

        //    string phone = context.Request["phone"];
        //    if (string.IsNullOrEmpty(phone))
        //    {
        //        respSms.Status = 0;
        //        respSms.Msg = "请输入手机号";
        //        return Common.JSONHelper.ObjectToJson(respSms);


        //    }
        //    if (!Common.ValidatorHelper.PhoneNumLogicJudge(phone))
        //    {
        //        respSms.Status = 0;
        //        respSms.Msg = "手机号码无效!";
        //        return Common.JSONHelper.ObjectToJson(respSms);

        //    }
        //    string content = "您好，感谢对Jo Malone London祖·玛珑的钟爱，您的个人信息已成功提交。9月9日-10月31日，莅临北京新光天地或上海梅龙镇伊势丹百货祖·玛珑专柜并出示此条短信，即可臻享牡丹与胭红麂绒叠加香氛体验组，每人限领一份。领取期间如遇告罄，将改赠适合您的叠加香氛体验组。祖·玛珑专柜期待您的莅临体验。";
        //    if (bllSms.SendSms(phone, content))
        //    {
        //        respSms.Status = 1;

        //    }
        //    else
        //    {
        //        respSms.Msg = "发送失败";
        //    }
        //    return Common.JSONHelper.ObjectToJson(respSms);


        //}




        /// <summary>
        /// 提交问卷
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string SumbitQuestionnaire(HttpContext context)
        {

            string spreadUserId = context.Request["spread_userid"];
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            QuestionnaireRecordList list = new QuestionnaireRecordList();
            try
            {

                string jsonData = context.Request["JsonData"];
                list = Common.JSONHelper.JsonToModel<QuestionnaireRecordList>(jsonData);
                if (list.Data.Count > 0)
                {
                    string userId = "";
                    //检查是否已经提交过
                    if (!bllUser.IsLogin)
                    {
                        userId = "AnonymousUser" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + new Random().Next(0, 999);//匿名用户

                    }
                    else
                    {
                        userId = DataLoadTool.GetCurrUserID();

                    }

                    //检查是否已经提交过
                    if (!bllUser.IsLogin)
                    {
                        if (bllUser.GetCount<QuestionnaireRecord>(string.Format("IP='{0}' And QuestionnaireID={1}", context.Request.UserHostAddress, list.Data[0].QuestionnaireID)) > 0)
                        {
                            //已经投过
                            resp.Msg = "重复提交";
                            resp.Status = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                            return Common.JSONHelper.ObjectToJson(resp);

                        }
                    }
                    else
                    {
                        if (bllUser.GetCount<QuestionnaireRecord>(string.Format("UserId='{0}' And QuestionnaireID={1}", userId, list.Data[0].QuestionnaireID)) > 0)
                        {
                            //已经投过
                            resp.Msg = "重复提交";
                            resp.Status = (int)BLLJIMP.Enums.APIErrCode.IsRepeat;
                            return Common.JSONHelper.ObjectToJson(resp);

                        }
                    }
                    BLLQuestion bllQuestion = new BLLQuestion();
                    long recordId = Convert.ToInt64(bllQuestion.GetRecordGUID());

                    foreach (var item in list.Data)
                    {
                        ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail model = new BLLJIMP.Model.QuestionnaireRecordDetail();
                        model.UserID = userId;
                        model.QuestionnaireID = item.QuestionnaireID;
                        model.QuestionID = item.QuestionID;
                        model.AnswerID = item.AnswerID;
                        model.AnswerContent = item.AnswerContent;
                        model.RecordID = recordId;
                        //

                        if (!bllUser.Add(model))
                        {
                            tran.Rollback();
                            resp.Msg = "提交失败";
                            return Common.JSONHelper.ObjectToJson(resp);

                        }
                    }

                    QuestionnaireRecord record = new QuestionnaireRecord();
                    record.UserId = userId;
                    record.QuestionnaireID = list.Data[0].QuestionnaireID;
                    record.InsertDate = DateTime.Now;
                    record.IP = context.Request.UserHostAddress;
                    record.RecordID = recordId;
                    record.PreUserId = spreadUserId;
                    if (!bllUser.Add(record))
                    {
                        tran.Rollback();
                        resp.Msg = "提交失败";
                        return Common.JSONHelper.ObjectToJson(resp);
                    }
                    var questionnaire = bllUser.Get<Questionnaire>(string.Format("QuestionnaireID={0}", list.Data[0].QuestionnaireID));
                    if ((questionnaire.AddScore > 0) && (bllUser.IsLogin))
                    {

                        UserInfo currentUserInfo = bllUser.GetCurrentUserInfo();
                        UserScoreDetailsInfo scoreModel = new UserScoreDetailsInfo();
                        scoreModel.AddNote = "提交问卷:" + questionnaire.QuestionnaireName;
                        scoreModel.AddTime = DateTime.Now;
                        scoreModel.Score = questionnaire.AddScore;
                        scoreModel.UserID = userId;
                        scoreModel.ScoreType = "SubmitQuestionnaire";
                        scoreModel.RelationID = questionnaire.QuestionnaireID.ToString();
                        scoreModel.TotalScore = currentUserInfo.TotalScore;
                        scoreModel.WebSiteOwner = bllUser.WebsiteOwner;
                        if (bllUser.Update(currentUserInfo, string.Format("TotalScore+={0}", scoreModel.Score), string.Format(" AutoID={0}", currentUserInfo.AutoID)) <= 0)
                        {
                            tran.Rollback();
                            resp.Msg = "加积分失败";
                            return Common.JSONHelper.ObjectToJson(resp);
                        }
                        if (!bllUser.Add(scoreModel))
                        {
                            tran.Rollback();
                            resp.Msg = "增加积分记录失败";
                            return Common.JSONHelper.ObjectToJson(resp);
                        }

                        #region 通知

                        try
                        {
                            BLLWeixin bllWeiXin = new BLLWeixin();
                            bllWeiXin.SendTemplateMessageNotifyComm(currentUserInfo, "积分变动通知", string.Format("提交问卷:{0}\\n增加积分:{1}\\n总积分:{2}", questionnaire.QuestionnaireName, scoreModel.Score, scoreModel.TotalScore));
                            var websiteInfo = bllUser.GetWebsiteInfoModelFromDataBase();
                            if (websiteInfo.IsUnionHongware == 1)
                            {

                                Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(bllUser.WebsiteOwner);
                                var hongWareMemberInfo = hongWareClient.GetMemberInfo(currentUserInfo.WXOpenId);
                                if (hongWareMemberInfo.member != null)
                                {
                                    if (!hongWareClient.UpdateMemberScore(hongWareMemberInfo.member.mobile, currentUserInfo.WXOpenId, (float)scoreModel.Score))
                                    {



                                    }


                                }


                            }

                        }
                        catch
                        {


                        }


                        #endregion

                    }

                    //   
                    //}

                }
                else
                {
                    resp.Msg = "无任何选项";
                    return Common.JSONHelper.ObjectToJson(resp);
                }

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.Msg = ex.Message;
                return Common.JSONHelper.ObjectToJson(resp);

            }
            tran.Commit();
            resp.Status = 1;

            if (!string.IsNullOrEmpty(spreadUserId))
            {
                int count = bllActivity.GetCount<QuestionnaireRecord>(string.Format(" QuestionnaireID='{0}' AND PreUserId='{1}' ", list.Data[0].QuestionnaireID, spreadUserId));

                MonitorLinkInfo linkInfo = bllActivity.Get<MonitorLinkInfo>(string.Format(" WebsiteOwner='{0}' AND LinkName='{1}' AND MonitorPlanID='{2}'", bllActivity.WebsiteOwner, spreadUserId, list.Data[0].QuestionnaireID));

                if (linkInfo != null)
                {
                    linkInfo.AnswerCount = count;
                    bllActivity.Update(linkInfo);
                }
            }

            int submitCount = bllActivity.GetCount<QuestionnaireRecord>(string.Format(" QuestionnaireID={0}", list.Data[0].QuestionnaireID));

            Questionnaire questionModel = bllActivity.Get<Questionnaire>(string.Format(" QuestionnaireID={0} ", list.Data[0].QuestionnaireID));
            if (questionModel != null)
            {
                questionModel.SubmitCount = submitCount;
                bllActivity.Update(questionModel);
            }

            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 提交试卷
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string SumbitExam(HttpContext context)
        {
            BLLQuestion bllQuestion = new BLLQuestion();
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            int examId = 0;
            try
            {
                string jsonData = context.Request["JsonData"];


                QuestionnaireRecordList list = Common.JSONHelper.JsonToModel<QuestionnaireRecordList>(jsonData);

                var currUserInfo = bllUser.GetCurrentUserInfo();
                if (list.Data.Count > 0)
                {
                    examId = list.Data[0].QuestionnaireID;
                    if (bllUser.GetCount<QuestionnaireRecord>(string.Format("UserId='{0}' And QuestionnaireID={1}", currUserInfo.UserID, list.Data[0].QuestionnaireID)) > 0)
                    {
                        //已经投过
                        resp.Msg = "您已经提交过了";
                        return Common.JSONHelper.ObjectToJson(resp);

                    }

                    long recordId = Convert.ToInt64(bllQuestion.GetRecordGUID());
                    foreach (var item in list.Data)
                    {
                        ZentCloud.BLLJIMP.Model.QuestionnaireRecordDetail model = new BLLJIMP.Model.QuestionnaireRecordDetail();
                        model.UserID = currUserInfo.UserID;
                        model.QuestionnaireID = item.QuestionnaireID;
                        model.QuestionID = item.QuestionID;
                        model.AnswerID = item.AnswerID;
                        model.AnswerContent = item.AnswerContent;
                        model.RecordID = recordId;

                        if (!bllUser.Add(model))
                        {
                            tran.Rollback();
                            resp.Msg = "提交失败";
                            return Common.JSONHelper.ObjectToJson(resp);

                        }
                    }

                    QuestionnaireRecord record = new QuestionnaireRecord();
                    record.UserId = currUserInfo.UserID;
                    record.QuestionnaireID = list.Data[0].QuestionnaireID;
                    record.InsertDate = DateTime.Now;
                    record.IP = context.Request.UserHostAddress;
                    record.RecordID = recordId;
                    if (!bllUser.Add(record))
                    {
                        tran.Rollback();
                        resp.Msg = "提交失败";
                        return Common.JSONHelper.ObjectToJson(resp);
                    }
                    //var questionnaire = bllUser.Get<Questionnaire>(string.Format("QuestionnaireID={0}", list.Data[0].QuestionnaireID));
                    //if ((questionnaire.AddScore > 0) && (bllUser.IsLogin))
                    //{

                    //    UserInfo currentUserInfo = bllUser.GetCurrentUserInfo();
                    //    UserScoreDetailsInfo scoreModel = new UserScoreDetailsInfo();
                    //    scoreModel.AddNote = "提交试卷:" + questionnaire.QuestionnaireName;
                    //    scoreModel.AddTime = DateTime.Now;
                    //    scoreModel.Score = questionnaire.AddScore;
                    //    scoreModel.UserID = userId;
                    //    scoreModel.ScoreType = "SubmitQuestionnaire";
                    //    scoreModel.RelationID = questionnaire.QuestionnaireID.ToString();
                    //    scoreModel.TotalScore = currentUserInfo.TotalScore;
                    //    scoreModel.WebSiteOwner = bllUser.WebsiteOwner;
                    //    if (bllUser.Update(currentUserInfo, string.Format("TotalScore+={0}", scoreModel.Score), string.Format(" AutoID={0}", currentUserInfo.AutoID)) <= 0)
                    //    {
                    //        tran.Rollback();
                    //        resp.Msg = "加积分失败";
                    //        return Common.JSONHelper.ObjectToJson(resp);
                    //    }
                    //    if (!bllUser.Add(scoreModel))
                    //    {
                    //        tran.Rollback();
                    //        resp.Msg = "增加积分记录失败";
                    //        return Common.JSONHelper.ObjectToJson(resp);
                    //    }

                    //    #region 通知

                    //    try
                    //    {
                    //        BLLWeixin bllWeiXin = new BLLWeixin();
                    //        bllWeiXin.SendTemplateMessageNotifyComm(currentUserInfo, "积分变动通知", string.Format("提交问卷:{0}\\n增加积分:{1}\\n总积分:{2}", questionnaire.QuestionnaireName, scoreModel.Score, scoreModel.TotalScore));
                    //        var websiteInfo = bllUser.GetWebsiteInfoModelFromDataBase();
                    //        if (websiteInfo.IsUnionHongware == 1)
                    //        {

                    //            Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(bllUser.WebsiteOwner);
                    //            var hongWareMemberInfo = hongWareClient.GetMemberInfo(currentUserInfo.WXOpenId);
                    //            if (hongWareMemberInfo.member != null)
                    //            {
                    //                if (!hongWareClient.UpdateMemberScore(hongWareMemberInfo.member.mobile, currentUserInfo.WXOpenId, (float)scoreModel.Score))
                    //                {



                    //                }


                    //            }


                    //        }

                    //    }
                    //    catch
                    //    {


                    //    }


                    //    #endregion

                    //}

                    //   
                    //}

                }
                else//自动提交
                {
                    QuestionnaireRecord record = new QuestionnaireRecord();
                    record.UserId = currUserInfo.UserID;
                    record.QuestionnaireID = int.Parse(context.Request["examId"]);
                    record.InsertDate = DateTime.Now;
                    record.IP = context.Request.UserHostAddress;
                    record.RecordID = 0;
                    if (bllQuestion.Add(record))
                    {
                        resp.Status = 1;
                    }
                    return Common.JSONHelper.ObjectToJson(resp);
                }

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.Msg = ex.Message;
                return Common.JSONHelper.ObjectToJson(resp);

            }
            tran.Commit();
            resp.Status = 1;

            //if (!string.IsNullOrEmpty(spreadUserId))
            //{
            //    int count = bllActivity.GetCount<QuestionnaireRecord>(string.Format(" QuestionnaireID='{0}' AND PreUserId='{1}' ", list.Data[0].QuestionnaireID, spreadUserId));

            //    MonitorLinkInfo linkInfo = bllActivity.Get<MonitorLinkInfo>(string.Format(" WebsiteOwner='{0}' AND LinkName='{1}' AND MonitorPlanID='{2}'", bllActivity.WebsiteOwner, spreadUserId, list.Data[0].QuestionnaireID));

            //    if (linkInfo != null)
            //    {
            //        linkInfo.AnswerCount = count;
            //        bllActivity.Update(linkInfo);
            //    }
            //}

            int submitCount = bllActivity.GetCount<QuestionnaireRecord>(string.Format(" QuestionnaireID={0}", examId));
            Questionnaire questionModel = bllActivity.Get<Questionnaire>(string.Format(" QuestionnaireID={0} ", examId));
            if (questionModel != null)
            {
                questionModel.SubmitCount = submitCount;
                bllActivity.Update(questionModel);
            }

            return Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AdminLogin(HttpContext context)
        {
            string userName = context.Request["username"];
            string pwd = context.Request["pwd"];
            if (userName.Length.Equals(0) || pwd.Length.Equals(0))
            {
                resp.Msg = "用户名密码不能为空";
                goto outoff;
            }
            if (!bllUser.FilterSql(userName))
            {
                resp.Msg = "危险的参数值";
                goto outoff;
            }
            if (!bllUser.FilterSql(pwd))
            {
                resp.Msg = "危险的参数值";
                goto outoff;
            }

            #region IP限制


            string userHostAddress = context.Request.UserHostAddress;
            var count = DataCache.GetCache(userHostAddress);
            if (count != null)
            {
                int newCount = int.Parse(count.ToString()) + 1;
                DataCache.SetCache(userHostAddress, newCount);
                int limitCount = 15;
                if (newCount >= limitCount)
                {
                    resp.Msg = "登录错误过于频繁，请稍后再试";
                    goto outoff;
                }
            }
            else
            {
                DataCache.SetCache(userHostAddress, 1, DateTime.MaxValue, new TimeSpan(4, 0, 0));
            }
            #endregion
            ZentCloud.BLLJIMP.Model.UserInfo modelUserInfo;
            string msg;

            if (this.bllUser.Login(userName, pwd, out modelUserInfo, out msg, this.bllUser.WebsiteOwner))
            {
                if (modelUserInfo.IsDisable.Equals("1"))
                {
                    resp.Msg = "账号已被禁用!";
                    goto outoff;
                }
                //1 站点所有者
                //2 子账号
                //3 超级管理员可访问
                if (modelUserInfo.UserID == bllUser.WebsiteOwner || modelUserInfo.UserType == 1 || (modelUserInfo.IsSubAccount == "1" && modelUserInfo.WebsiteOwner == bllUser.WebsiteOwner)||(modelUserInfo.UserType==7))
                {


                }
                else
                {
                    resp.Msg = "无权访问!";
                    goto outoff;
                }

                context.Session[SessionKey.LoginStatu] = 1;
                context.Session[SessionKey.UserID] = modelUserInfo.UserID;
                //bllUser.AddLoginLogs();
                bllLog.Add(ZentCloud.BLLJIMP.Enums.EnumLogType.Login, ZentCloud.BLLJIMP.Enums.EnumLogTypeAction.SignIn, modelUserInfo.UserID, "登录");
                resp.Status = 1;
                DataCache.SetCache(userHostAddress, 1);
            }
            else
            {

                resp.Msg = "用户名或密码错误";
            }

        outoff:
            return Common.JSONHelper.ObjectToJson(resp);
        }






        //private void ToLog(string msg)
        //{
        //    try
        //    {
        //        using (StreamWriter sw = new StreamWriter(@"C:\test1.txt", true, Encoding.GetEncoding("gb2312")))
        //        {
        //            sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), msg));
        //        }
        //    }
        //    catch { }
        //}

        /// <summary>
        /// 获取加V分类
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetAddVImageWap(HttpContext context)
        {
            var currentUserInfo = bllUser.GetCurrentUserInfo();

            if (currentUserInfo == null)
            {
                return "当前未登陆";
            }

            string dir = context.Request["dir"];//加载加V分类

            if (string.IsNullOrEmpty(currentUserInfo.WXHeadimgurl))
            {
                return "获取不到头像，请点击'更新头像'";
            }

            //string imgOrgPath = context.Server.MapPath(currentUserInfo.WXHeadimgurlLocal.Replace(Common.ConfigHelper.GetConfigString("serverImgPath"), ""));

            string imgOrgPath = context.Server.MapPath(new BLLJuActivity().DownLoadRemoteImage(currentUserInfo.WXHeadimgurl));

            string imgBorderPath = context.Server.MapPath("/FileUpload/WXADDV/border/" + currentUserInfo.UserID + "/");
            
            if (!Directory.Exists(imgBorderPath))
            {
                Directory.CreateDirectory(imgBorderPath);
            }

            imgBorderPath += Guid.NewGuid().ToString() + ".jpg";

            List<string> imgWatermarkPathList = new List<string>();
            string[] arrFiles = System.IO.Directory.GetFiles(context.Server.MapPath(string.Format("/img/WXADDV/{0}/{1}/", bllUser.WebsiteOwner, dir)));
            if (arrFiles.Length > 0)
                imgWatermarkPathList = arrFiles.ToList();

            List<string> imgVList = new List<string>();
            List<string> fileNameList = new List<string>(); //文件名列表
            ZentCloud.Common.ImgWatermarkHelper imgHelper = new ZentCloud.Common.ImgWatermarkHelper();
            imgHelper.ImgAddBord(imgOrgPath, imgBorderPath);
            StringBuilder sbHtml = new StringBuilder();
            foreach (var item in imgWatermarkPathList)
            {

                string imgVGuid = Guid.NewGuid().ToString();
                
                string imgVstrLocal = context.Server.MapPath("/FileUpload/WXADDV/" + currentUserInfo.UserID + "/");

                if (!Directory.Exists(imgVstrLocal))
                {
                    Directory.CreateDirectory(imgVstrLocal);
                }

                imgVstrLocal += imgVGuid + ".jpg";

                string imgVstr = "/FileUpload/WXADDV/" + currentUserInfo.UserID + "/" + imgVGuid + ".jpg";

                imgHelper.SaveWatermark(imgBorderPath, item, 1f, ZentCloud.Common.ImgWatermarkHelper.WatermarkPosition.RigthBottom, 0, imgVstrLocal, 0.3f);//, 0.25f
                imgVList.Add(imgVstr);
                fileNameList.Add(item.Split('\\')[item.Split('\\').Length - 1].Replace(".png", null));

            }
            for (int i = 0; i < imgVList.Count; i++)
            {
                sbHtml.AppendFormat("<div style=\"width: 120px; float: left; padding: 5px;text-align:center;\"> <img alt=\"\" width=\"120px\" src=\"{0}\" /><br/><label>{1}</label> </div>", imgVList[i], GetAddVMapingKeyByValue(string.Format("V_{0}_{1}", dir, fileNameList[i])));

            }
            return sbHtml.ToString();


        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetAddVMapingKeyByValue(string value)
        {
            WXAddVMaping addvMap = bllUser.Get<WXAddVMaping>(string.Format("AddVValue='{0}' And WebsiteOwner='{1}'", value, bllUser.WebsiteOwner));
            if (addvMap != null)
            {
                return addvMap.AddVKey;
            }
            return "";

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    /// <summary>
    /// 问卷记录集合
    /// </summary>
    [Serializable]
    public class QuestionnaireRecordList
    {
        public List<QuestionnaireRecordDetailModel> Data { get; set; }
    }
    /// <summary>
    /// 提交问卷记录 反序列化模型
    /// </summary>
    [Serializable]
    public class QuestionnaireRecordDetailModel
    {
        /// <summary>
        /// 问卷编号 对应ZCJ_Questionnaire
        /// </summary>
        public int QuestionnaireID { get; set; }
        /// <summary>
        /// 问题编号 对应ZCJ_Question
        /// </summary>
        public int QuestionID { get; set; }
        /// <summary>
        /// 问题选项编号 对应ZCJ_Answer
        /// </summary>
        public int? AnswerID { get; set; }
        /// <summary>
        /// 答案选项 用于填空
        /// </summary>
        public string AnswerContent { get; set; }

        /// <summary>
        /// 推广人id
        /// </summary>
        public string PreUserId { get; set; }

    }


}
