using CommonPlatform.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.API.forbes;

namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    /// loginapi 的摘要说明
    /// </summary>
    public class loginapi : IHttpHandler, IRequiresSessionState
    {
        DefaultResponse resp = new DefaultResponse();
        /// <summary>
        /// 基路径 形式如 http://dev.comeoncloud.net
        /// </summary>
        private string basePath;
        /// <summary>
        /// 网站所有者
        /// </summary>
        private string webSiteOwner;
        /// <summary>
        /// 用户业务逻辑
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        /// <summary>
        /// 短信业务逻辑
        /// </summary>
        BLLSMS bllSms = new BLLSMS("");
        /// <summary>
        /// 字典业务逻辑（省市区）
        /// </summary>
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        /// <summary>
        /// 用户扩展模块
        /// </summary>
        BLLUserExpand bllUserExpand = new BLLUserExpand();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                webSiteOwner = bllUser.WebsiteOwner;
                basePath = string.Format("http://{0}{1}", context.Request.Url.Host, context.Request.Url.Port != 80 ? ":" + context.Request.Url.Port.ToString() : "");

                string action = context.Request["action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.errmsg = "action not exist";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {

                resp.errcode = -1;
                resp.errmsg = ex.ToString();
                result = Common.JSONHelper.ObjectToJson(resp);
            }
            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                //返回 jsonp数据
                context.Response.Write(string.Format("{0}({1})", context.Request["callback"], result));
            }
            else
            {
                //返回json数据
                context.Response.Write(result);
            }
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Login(HttpContext context)
        {

            Login apiResult = new Login();
            string userName = context.Request["userid"];
            string pwd = context.Request["pwd"];
            string msg;

            string hasCheckCode = context.Request["hascheckcode"];
            string checkCode = context.Request["checkcode"];
            object serverCheckCode = context.Session["CheckCode"];

            if (!string.IsNullOrWhiteSpace(hasCheckCode) && hasCheckCode == "1" && serverCheckCode != null)
            {
                if (
                    string.IsNullOrWhiteSpace(checkCode)
                        ||
                    !checkCode.Equals(serverCheckCode.ToString(), StringComparison.OrdinalIgnoreCase)
                    )
                {
                    apiResult.message = "验证码错误";
                    apiResult.errcode = (int)BLLJIMP.Enums.APIErrCode.CheckCodeErr;
                    return Common.JSONHelper.ObjectToJson(apiResult);
                }
            }

            UserInfo userInfo = new UserInfo();
            if (bllUser.Login(userName, pwd, out userInfo, out msg, this.webSiteOwner))
            {
                if (DateTime.Now.ToString("yyyy-MM-dd") != userInfo.LastLoginDate.ToString("yyyy-MM-dd"))
                {
                    bllUser.AddUserScoreDetail(userInfo.UserID, EnumStringHelper.ToString(ScoreDefineType.DayLogin), this.webSiteOwner, null, null);
                }
                bllUser.UpdateLoginInfo(userInfo);

                context.Session[SessionKey.LoginStatu] = 1;
                context.Session[SessionKey.UserID] = userInfo.UserID;

                //绑定微信
                BindWXUser(context, userInfo.UserID);

                apiResult.issuccess = true;
                apiResult.userid = userInfo.UserID;
                apiResult.headimg = this.bllUser.GetUserDispalyAvatar(userInfo);
                apiResult.userName = this.bllUser.GetUserDispalyName(userInfo);
                apiResult.avatar = this.bllUser.GetUserDispalyAvatar(userInfo);
                apiResult.phone = userInfo.Phone;
                apiResult.id = userInfo.AutoID;
                apiResult.score = userInfo.TotalScore;
                apiResult.im_token = userInfo.IMToken;
            }
            else
            {
                apiResult.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResult.message = msg;
            }
            return Common.JSONHelper.ObjectToJson(apiResult);

        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Logout(HttpContext context)
        {
            context.Session.RemoveAll();

            HttpCookie loginCookie = HttpContext.Current.Request.Cookies[ZentCloud.Common.SessionKey.LoginCookie];
            if (loginCookie != null)
            {
                loginCookie.Expires = DateTime.Today.AddDays(-1);
                context.Response.Cookies.Add(loginCookie);
            }
            resp.isSuccess = true;
            return Common.JSONHelper.ObjectToJson(resp);
        } 
        
        /// <summary>
        /// 注销登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Loginout(HttpContext context)
        {
            if (!bllUser.IsLogin)
            {
                resp.errcode = -2;
                resp.errmsg = "尚未登录";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            context.Session.RemoveAll();
            resp.errmsg = "操作成功";
            return Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Reg(HttpContext context)
        {
            string phone = context.Request["phone"];
            string pwd = context.Request["pwd"];
            string verCode = context.Request["vercode"];
            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入手机号";
                goto outoff;
            }
            if ((!phone.StartsWith("1")) || (!phone.Length.Equals(11)))
            {
                resp.errcode = 2;
                resp.errmsg = "手机号格式不正确";
                goto outoff;
            }
            if (string.IsNullOrEmpty(pwd))
            {
                resp.errcode = 3;
                resp.errmsg = "请输入密码";
                goto outoff;
            }
            if (string.IsNullOrEmpty(verCode))
            {
                resp.errcode = 4;
                resp.errmsg = "请输入验证码";
                goto outoff;
            }
            if (bllUser.GetUserInfo(phone, webSiteOwner) != null)
            {
                resp.errcode = 5;
                resp.errmsg = "此手机号已经被注册";
                goto outoff;
            }
            ////验证码检查
            var lastSmsVerificationCode = bllSms.GetLastSmsVerificationCode(phone);
            if (lastSmsVerificationCode == null)
            {
                resp.errcode = 6;
                resp.errmsg = "请先获取手机验证码";
                goto outoff;
            }
            if (!lastSmsVerificationCode.VerificationCode.Equals(verCode))
            {
                resp.errcode = 7;
                resp.errmsg = "验证码不正确";
                goto outoff;
            }
            ////
            UserInfo regUser = new UserInfo();
            regUser.WXHeadimgurl = basePath + "/img/persion.png";
            regUser.UserID = phone;
            regUser.Password = pwd;
            regUser.WebsiteOwner = webSiteOwner;
            regUser.UserType = 2;
            regUser.Regtime = DateTime.Now;
            regUser.LastLoginDate = DateTime.Now;
            if (bllUser.Add(regUser))
            {
                resp.errcode = 0;
                resp.errmsg = "注册成功";

                context.Session[SessionKey.LoginStatu] = 1;
                context.Session[SessionKey.UserID] = regUser.UserID;

                //绑定微信
                BindWXUser(context, regUser.UserID);

                goto outoff;
            }
            else
            {
                resp.errcode = 6;
                resp.errmsg = "注册失败";
                goto outoff;
            }
        outoff:
            return Common.JSONHelper.ObjectToJson(resp);

        } 
        
        #region 注册模块
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UserRegister(HttpContext context)
        {
            string pId = context.Request["pId"];
            string name = context.Request["name"];
            string email = context.Request["email"];
            string pwd = context.Request["pwd"];
            string verCode = context.Request["vercode"];
            object serverCheckCode = context.Session["CheckCode"];
            string IsSHowInfo = context.Request["IsSHowInfo"];

            if (!CheckRegister(name, email, pwd, verCode, serverCheckCode))
            {
                return Common.JSONHelper.ObjectToJson(resp);
            }
            UserInfo regUser = new UserInfo();
            regUser.UserID = string.Format("WebPC{0}{1}", ZentCloud.Common.StringHelper.GetDateTimeNum(), ZentCloud.Common.Rand.Str(5));//WXUser+时间字符串+随机5位数字
            regUser.TrueName = name;
            regUser.Email = email;
            regUser.Password = pwd;
            regUser.WebsiteOwner = this.webSiteOwner;
            regUser.UserType = 2;
            regUser.Regtime = DateTime.Now;
            string ip = ZentCloud.Common.MySpider.GetClientIP();
            regUser.RegIP = ip;
            regUser.LastLoginIP = ip;
            regUser.LastLoginDate = DateTime.Now;
            regUser.LoginTotalCount = 1;

            if (bllUser.Add(regUser))
            {
                context.Session[SessionKey.LoginStatu] = 1;
                context.Session[SessionKey.UserID] = regUser.UserID;
                bllUser.AddUserScoreDetail(regUser.UserID, EnumStringHelper.ToString(ScoreDefineType.Register), this.webSiteOwner, null, null);

                if (!string.IsNullOrWhiteSpace(pId))
                {
                    string pUserId = MySpider.Base64Change.DecodeBase64(pId);
                    UserInfo shareUser = bllUser.GetUserInfo(pUserId);
                    if (shareUser != null)
                    {
                        bllUser.AddUserScoreDetail(regUser.UserID, EnumStringHelper.ToString(ScoreDefineType.RegisterFromShare), this.webSiteOwner, null, null);
                        bllUser.AddUserScoreDetail(shareUser.UserID, EnumStringHelper.ToString(ScoreDefineType.ShareRegister), this.webSiteOwner, null, null);
                    }
                    bllUserExpand.AddUserExpand(BLLJIMP.Enums.UserExpandType.IsSHowInfo, regUser.UserID, IsSHowInfo);
                }

                //绑定微信
                BindWXUser(context, regUser.UserID);
                return Common.JSONHelper.ObjectToJson(new
                {
                    isSuccess = true,
                    user = new
                    {
                        id = bllUser.GetUserAutoID(regUser.UserID),
                        userid = regUser.UserID,
                        userName = this.bllUser.GetUserDispalyName(regUser),
                        avatar = this.bllUser.GetUserDispalyAvatar(regUser)
                    }
                });
            }
            else
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.RegisterFailure;
                resp.errmsg = "注册失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 律师注册
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string LawyerRegister(HttpContext context)
        {
            string pId = context.Request["pId"];
            string name = context.Request["name"];
            string email = context.Request["email"];
            string pwd = context.Request["pwd"];
            string verCode = context.Request["vercode"];

            object serverCheckCode = context.Session["CheckCode"];

            if(!CheckRegister(name, email, pwd, verCode, serverCheckCode))
            {
                return Common.JSONHelper.ObjectToJson(resp);
            }
            string company = context.Request["company"];
            string position = context.Request["postion"];
            string idCardNo = context.Request["idCardNo"];//身份证
            string phone = context.Request["phone"];
            string tel = context.Request["tel"];
            string province = context.Request["province"];
            string city = context.Request["city"];
            string avatar = context.Request["avatar"];
            string licensePhoto = context.Request["licensePhoto"];
            string companyAddress = context.Request["companyaddress"];

            string IDPhoto1 = context.Request["IDPhoto1"];
            string IDPhoto2 = context.Request["IDPhoto2"];
            string IsSHowInfo = context.Request["IsSHowInfo"];
            string LawyerLicenseNo = context.Request["LawyerLicenseNo"];
            

            if (!CheckLawyerRegister(company, idCardNo, position, phone, tel, province, city, licensePhoto, companyAddress))
            {
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrWhiteSpace(IDPhoto1))
            {
                resp.errcode = 20;
                resp.errmsg = "请上传身份证照片！";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrWhiteSpace(IDPhoto2))
            {
                resp.errcode = 21;
                resp.errmsg = "请上传身份证照片！";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrWhiteSpace(avatar))
            {
                resp.errcode = 22;
                resp.errmsg = "请上传您的免冠照！";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            UserInfo regUser = new UserInfo();
            regUser.UserID = string.Format("WebPC{0}{1}", ZentCloud.Common.StringHelper.GetDateTimeNum(), ZentCloud.Common.Rand.Str(5));//WXUser+时间字符串+随机5位数字
            regUser.TrueName = name;
            regUser.Email = email;
            regUser.Password = pwd;
            regUser.Company = company;
            regUser.Postion = position;
            regUser.Phone = phone;
            regUser.WXHeadimgurl = avatar;

            regUser.ProvinceCode = province;
            if (!string.IsNullOrWhiteSpace(province)) regUser.Province = bllKeyValueData.GetDataDefVaule("Province", province);
            regUser.CityCode = city;
            if (!string.IsNullOrWhiteSpace(city)) regUser.City = bllKeyValueData.GetDataDefVaule("City", city);

            regUser.WebsiteOwner = this.webSiteOwner;
            regUser.UserType = 4;
            regUser.Regtime = DateTime.Now;
            string ip = ZentCloud.Common.MySpider.GetClientIP();
            regUser.RegIP = ip;
            regUser.LastLoginIP = ip;
            regUser.LastLoginDate = DateTime.Now;
            regUser.LoginTotalCount = 1;

            if (bllUser.Add(regUser))
            {
                bllUser.AddUserScoreDetail(regUser.UserID, EnumStringHelper.ToString(ScoreDefineType.Register), this.webSiteOwner, null, null);
                if (!string.IsNullOrWhiteSpace(pId))
                {
                    string pUserId = MySpider.Base64Change.DecodeBase64(pId);
                    UserInfo shareUser = bllUser.GetUserInfo(pUserId);
                    if (shareUser != null)
                    {
                        bllUser.AddUserScoreDetail(regUser.UserID, EnumStringHelper.ToString(ScoreDefineType.RegisterFromShare), this.webSiteOwner, null, null);
                        bllUser.AddUserScoreDetail(shareUser.UserID, EnumStringHelper.ToString(ScoreDefineType.ShareRegister), this.webSiteOwner, null, null);
                    }
                }
                bllUserExpand.AddUserExpand(UserExpandType.IdCardNo, regUser.UserID, idCardNo);
                bllUserExpand.AddUserExpand(UserExpandType.UserTel, regUser.UserID, tel);
                bllUserExpand.AddUserExpand(UserExpandType.UserCompanyAddress, regUser.UserID, companyAddress);
                bllUserExpand.AddUserExpand(UserExpandType.LawyerLicensePhoto, regUser.UserID, licensePhoto);
                bllUserExpand.AddUserExpand(UserExpandType.IDPhoto1, regUser.UserID, IDPhoto1);
                bllUserExpand.AddUserExpand(UserExpandType.IDPhoto2, regUser.UserID, IDPhoto2);
                bllUserExpand.AddUserExpand(UserExpandType.IsSHowInfo, regUser.UserID, IsSHowInfo);
                bllUserExpand.AddUserExpand(UserExpandType.LawyerLicenseNo, regUser.UserID, LawyerLicenseNo);
                
                context.Session[SessionKey.LoginStatu] = 1;
                context.Session[SessionKey.UserID] = regUser.UserID;

                //绑定微信
                BindWXUser(context, regUser.UserID);

                return Common.JSONHelper.ObjectToJson(new
                {
                    isSuccess = true,
                    user = new
                    {
                        id = bllUser.GetUserAutoID(regUser.UserID),
                        userid =regUser.UserID,
                        userName = this.bllUser.GetUserDispalyName(regUser),
                        avatar = this.bllUser.GetUserDispalyAvatar(regUser)
                    }
                });
            }
            else
            {
                resp.errcode = 999;
                resp.errmsg = "注册失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        } 
        /// <summary>
        /// 注册检查
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="email">邮箱</param>
        /// <param name="pwd">密码</param>
        /// <param name="vercode">验证码</param>
        /// <param name="serverCheckCode">session验证码</param>
        /// <returns></returns>
        private bool CheckRegister(string name, string email, string pwd, string vercode, object serverCheckCode)
        {
            if (string.IsNullOrEmpty(name))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入姓名";
                return false;
            }

            if (string.IsNullOrEmpty(email))
            {
                resp.errcode = 2;
                resp.errmsg = "请输入邮箱";
                return false;
            }
            if (string.IsNullOrEmpty(pwd))
            {
                resp.errcode = 3;
                resp.errmsg = "请输入密码";
                return false;
            }
            if (string.IsNullOrEmpty(vercode))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.CheckCodeErr;
                resp.errmsg = "请输入验证码";
                return false;
            }

            if (!MySpider.MyRegex.EmailLogicJudge(email))
            {
                resp.errcode = 5;
                resp.errmsg = "邮箱格式不正确";
                return false;
            }

            if (serverCheckCode == null)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.CheckCodeErr;
                resp.errmsg = "验证码超时";
                return false;
            }

            if (!vercode.Equals(serverCheckCode.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.CheckCodeErr;
                resp.errmsg = "验证码错误";
                return false;
            }

            if (bllUser.GetUserInfoByEmail(email) != null)
            {
                resp.errcode =(int)BLLJIMP.Enums.APIErrCode.EmailIsHave;
                resp.errmsg = "此邮箱已经被注册";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 律师注册参数检查
        /// </summary>
        /// <param name="company">公司</param>
        /// <param name="postion">职位</param>
        /// <param name="phone">手机</param>
        /// <param name="tel">座机</param>
        /// <param name="province">省份</param>
        /// <param name="city">城市</param>
        /// <param name="companyaddress">公司地址</param>
        /// <returns></returns>
        private bool CheckLawyerRegister(string company, string idCardNo, string postion, string phone, string tel, string province, string city, string licensePhoto, string companyaddress)
        {
            if (string.IsNullOrEmpty(company))
            {
                resp.errcode = 8;
                resp.errmsg = "请输入公司";
                return false;
            }
            if (string.IsNullOrEmpty(idCardNo))
            {
                resp.errcode = 18;
                resp.errmsg = "请输入身份证";
                return false;
            }
            //if (string.IsNullOrEmpty(postion))
            //{
            //    resp.errcode = 9;
            //    resp.errmsg = "请输入职位";
            //    return false;
            //}

            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 10;
                resp.errmsg = "请输入手机";
                return false;
            }
            if (string.IsNullOrEmpty(tel))
            {
                resp.errcode = 11;
                resp.errmsg = "请输入座机";
                return false;
            }
            //if (string.IsNullOrEmpty(province))
            //{
            //    resp.errcode = 12;
            //    resp.errmsg = "请选择省份";
            //    return false;
            //}

            //if (string.IsNullOrEmpty(city))
            //{
            //    resp.errcode = 13;
            //    resp.errmsg = "请选择城市";
            //    return false;
            //}
            if (string.IsNullOrEmpty(licensePhoto))
            {
                resp.errcode = 17;
                resp.errmsg = "请上传律师执业证";
                return false;
            }
            if (string.IsNullOrEmpty(companyaddress))
            {
                resp.errcode = 14;
                resp.errmsg = "请输入公司地址";
                return false;
            }

            if (!ZentCloud.Common.PageValidate.IsMobile(phone))
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PhoneFormatError;
                resp.errmsg = "手机格式错误";
                return false;
            }

            //if (!ZentCloud.Common.PageValidate.IsPhone(tel))
            //{
            //    resp.errcode = 16;
            //    resp.errmsg = "座机格式错误";
            //    return false;
            //}

            //if (bllUser.GetUserInfoByPhone(phone) != null)
            //{
            //    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PhoneIsHave;
            //    resp.errmsg = "此手机已经被注册";
            //    return false;
            //}
            return true;
        }

        #endregion


        private void BindWXUser(HttpContext context,string userId)
        {
            var userInfo = this.bllUser.GetUserInfo(userId);
            var openId = context.Session["currWXOpenId"] == null ? "" : context.Session["currWXOpenId"].ToString();

            //用户第一次登陆微信，绑定微信账号
            if (string.IsNullOrWhiteSpace(userInfo.WXOpenId) && !string.IsNullOrWhiteSpace(openId))
            {
                if (bllUser.UpdateUserWxOpenId(userInfo.UserID, openId, bllUser.WebsiteOwner))
                {}
            }
        }

        /// <summary>
        /// 获取邮箱验证码
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetEmailCheckCode(HttpContext context)
        {
            string email = context.Request["email"];
            if (MySpider.MyRegex.EmailLogicJudge(email))
            {
                string code = GenerateCheckCode(context);
                MySpider.MyEmail bllEmail = new MySpider.MyEmail();
                string Step5SmtpHost = System.Configuration.ConfigurationManager.AppSettings["Step5SmtpHost"];
                string step5EmaiSendUserName = System.Configuration.ConfigurationManager.AppSettings["Step5EmaiSendUserName"];
                string step5EmaiSendUserPwd = System.Configuration.ConfigurationManager.AppSettings["Step5EmaiSendUserPwd"];
                string step5SmtpPort = System.Configuration.ConfigurationManager.AppSettings["Step5SmtpPort"];
                string errorStr = "";

                bllEmail.SendSMTPEMail(Step5SmtpHost, step5EmaiSendUserName, step5EmaiSendUserPwd, email, step5EmaiSendUserName
                    , "易劳邮箱验证码", "验证码：" + code, new List<string>(), "易劳平台", false, Encoding.Default, out errorStr,int.Parse(step5SmtpPort));

                if (string.IsNullOrWhiteSpace(errorStr))
                {
                    resp.isSuccess = true;
                }
                else
                {
                    resp.errmsg = errorStr;
                }
            }
            else
            {
                resp.errmsg = "邮箱格式不能识别！";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        private string GenerateCheckCode(HttpContext context) //产生随机验证码字符函数
        {
            int number;
            char code;
            string checkCode = String.Empty;

            System.Random random = new Random();

            for (int i = 0; i < 4; i++) //字符和数字的混合.长度为5。其实i的大小可以自由设置
            {
                number = random.Next();
                // if(number % 2 == 0) //偶数
                code = (char)('0' + (char)(number % 10));
                //else
                // code = (char)('A' + (char)(number % 26));
                checkCode += code.ToString();
            }
            context.Session["CheckCode"] = checkCode.ToLower();
            //把产生的验证码保存到COOKIE中
            return checkCode;//返回结果以供CreateCheckCodeImage()函数使用
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}