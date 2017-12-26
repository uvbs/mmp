using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using ZentCloud.BLLJIMP.Model;
using System.Web;
using System.Data;
using ZentCloud.Common;
using ZentCloud.ZCBLLEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 用户BLL
    /// </summary>
    public class BLLUser : BLL
    {
        /// <summary>
        /// 短信BLL
        /// </summary>
        BLLSMS bllSms = new BLLSMS("");

        public BLLUser(string userID)
            : base(userID)
        {

        }
        public BLLUser()
        {

        }

        ///<summary>
        /// 用户登录
        /// </summary>
        /// <param name="userID">登录ID</param>
        /// <param name="pwd">登录密码</param>
        /// <param name="user">返回用户</param>
        /// <returns></returns>
        public bool Login(string userID, string pwd, out ZentCloud.BLLJIMP.Model.UserInfo modelUserInfo, out string msg, string websiteOwner = "", string applyStatus = "")
        {
            ToLog("Into Login");
            modelUserInfo = GetUserInfo(userID, websiteOwner);
            ToLog("GetUserInfo sucess");
            if (modelUserInfo == null && ZentCloud.Common.MyRegex.PhoneNumLogicJudge(userID))
            {
                //手机号登陆
                modelUserInfo = Get<UserInfo>(string.Format(" Phone = '{0}' AND WebsiteOwner = '{1}' ", userID, websiteOwner));
                //modelUserInfo = Get<UserInfo>(string.Format(" Email = '{0}'", userID));
            }

            if (modelUserInfo == null && MySpider.MyRegex.EmailLogicJudge(userID))
            {
                //邮箱账号登陆
                modelUserInfo = Get<UserInfo>(string.Format(" Email = '{0}' AND WebsiteOwner = '{1}' ", userID, websiteOwner));
                //modelUserInfo = Get<UserInfo>(string.Format(" Email = '{0}'", userID));
            }

            if (modelUserInfo == null)
            {
                msg = "用户不存在！";
                return false;
            }

            if (!modelUserInfo.Password.Equals(pwd))
            {
                msg = "密码错误！";
                return false;
            }

            if (!modelUserInfo.IsDisable.Equals(0))
            {
                msg = "账号已被禁用！";
                return false;
            }
            if (!string.IsNullOrEmpty(applyStatus))
            {
                if (modelUserInfo.MemberApplyStatus == 1)
                {
                    msg = "账号还在审核中！";
                    return false;
                }
                if (modelUserInfo.MemberApplyStatus == 2)
                {
                    msg = "账号审核未通过！";
                    return false;
                }
            }
            msg = "登录成功！";
            return true;
        }
        /// <summary>
        /// 用户登录-密码己加密
        /// </summary>
        /// <param name="userID">用户名</param>
        /// <param name="pwdEncrypt">加密后的密码</param>
        /// <param name="modelUserInfo">用户模型</param>
        /// <param name="msg">返回信息</param>
        /// <returns></returns>
        public bool LoginEncrypt(string userID, string pwdEncrypt, out ZentCloud.BLLJIMP.Model.UserInfo modelUserInfo, out string msg)
        {
            modelUserInfo = Get<ZentCloud.BLLJIMP.Model.UserInfo>(string.Format("UserId = '{0}'", userID));
            if (modelUserInfo == null)
            {
                msg = "用户不存在！";
                return false;
            }

            if (!FormsAuthentication.HashPasswordForStoringInConfigFile(modelUserInfo.Password, "MD5").Equals(pwdEncrypt))
            {
                msg = "密码错误！";
                return false;
            }

            msg = "登录成功！";
            return true;
        }

        ///<summary>
        /// 用户登录
        /// </summary>
        /// <param name="userID">登录ID</param>
        /// <param name="pwd">登录密码</param>
        /// <returns></returns>
        public bool Login(string userID, string pwd)
        {
            ZentCloud.BLLJIMP.Model.UserInfo modelUserInfo = new Model.UserInfo();
            string msg = string.Empty;
            return Login(userID, pwd, out modelUserInfo, out msg);

        }

        /// <summary>
        /// 短信登录
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool SmsLogin(string phone, string smsVercode, out string msg)
        {
            msg = "";
            var lastSmsVerCode = bllSms.GetLastSmsVerificationCode(phone);
            if (lastSmsVerCode == null)
            {
                msg = "请先获取手机验证码";
                return false;
            }
            if (lastSmsVerCode.VerificationCode != smsVercode)
            {
                msg = "手机验证码错误";
                return false;
            }
            if ((DateTime.Now - lastSmsVerCode.InsertDate).TotalMinutes >= 5)
            {
                msg = "手机验证码已过期,请重新获取";
                return false;
            }
            return true;
        }

        /// <summary>
        /// md5密码登录
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="md5Pwd"></param>
        /// <returns></returns>
        public bool LoginMD5Pwd(string userID, string md5Pwd)
        {
            try
            {
                return Common.DEncrypt.GetMD5(GetUserInfo(userID).Password.ToLower()).Equals(md5Pwd.ToLower());
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 臻云加密登录
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="md5Pwd"></param>
        /// <returns></returns>
        public bool LoginZCEncrypt(string userID, string md5Pwd)
        {
            try
            {
                return Common.DEncrypt.ZCEncrypt(GetUserInfo(userID).Password).Equals(md5Pwd);
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Model.UserInfo GetUserInfo(string userId)
        {
            //return Get<Model.UserInfo>(string.Format(" UserID = '{0}' And WebsiteOwner='{1}'", userId, WebsiteOwner));

            if (!string.IsNullOrWhiteSpace(WebsiteOwner) && userId != "jubit")
            {
                return GetUserInfo(userId, WebsiteOwner);
            }
            else
            {
                return Get<Model.UserInfo>(string.Format(" UserID = '{0}' ", userId));
            }

        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Model.UserInfo GetUserInfo(string userID, string websiteOwner)
        {
            if (userID == "jubit")
            {
                return GetUserInfo(userID);
            }

            if (string.IsNullOrWhiteSpace(websiteOwner))
            {
                return GetUserInfo(userID);
            }

            return Get<Model.UserInfo>(string.Format(" UserID = '{0}' And WebsiteOwner='{1}' ", userID, websiteOwner));

        }

        /// <summary>
        /// 获取缓存里的用户信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoByCache(string userID, string websiteOwner)
        {
            UserInfo result = null;

            try
            {
                var key = string.Format("{0}:User:{1}", websiteOwner, userID);

                var cacheDataStr = RedisHelper.RedisHelper.StringGet(key);

                if (string.IsNullOrWhiteSpace(cacheDataStr))
                {
                    result = GetUserInfo(userID, websiteOwner);
                    cacheDataStr = JsonConvert.SerializeObject(result);
                    RedisHelper.RedisHelper.StringSet(key, cacheDataStr);
                }
                else
                {
                    result = JsonConvert.DeserializeObject<UserInfo>(cacheDataStr);
                }
            }
            catch (Exception ex)
            {
                result = GetUserInfo(userID, websiteOwner);
            }

            return result;
        }

        /// <summary>
        /// 获取用户信息根据手机号
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        public Model.UserInfo GetUserInfoByPhone(string phone, string websiteOwner = "")
        {
            if (string.IsNullOrWhiteSpace(websiteOwner)) websiteOwner = WebsiteOwner;
            return Get<Model.UserInfo>(string.Format(" Phone = '{0}' And WebsiteOwner='{1}' order by AutoID DESC", phone, WebsiteOwner));
        }


        public Model.UserInfo GetUserInfoByCompany(string company)
        {
            return Get<UserInfo>(string.Format(" Company='{0}' ", company));
        }

        /// <summary>
        /// 根据手机号检查是否有账号
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        public Model.UserInfo GetUserInfoByAllPhone(string phone)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebSiteOwner='{0}' ", WebsiteOwner);
            sbWhere.AppendFormat(" AND (Phone='{0}'Or Phone1 = '{0}' Or Phone2 = '{0}' Or Phone3 = '{0}') ", phone);
            sbWhere.AppendFormat(" AND (WXOpenId Is Null Or WXOpenId='') ");
            sbWhere.AppendFormat(" Order By AutoID DESC ");
            return Get<Model.UserInfo>(sbWhere.ToString());
        }
        /// <summary>
        /// 获取所有注册公司列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyWord"></param>
        /// <param name="userType"></param>
        /// <param name="applyStatus"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<Model.UserInfo> GetUserInfoByAllCompany(int pageSize, int pageIndex, string keyWord, string userType, string applyStatus, out int totalCount)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' ", WebsiteOwner);

            sbWhere.AppendFormat(" AND UserID>''  ");
            sbWhere.AppendFormat(" AND  Password>'' ");
            if (!string.IsNullOrEmpty(userType) && userType == "2")
            {
                sbWhere.AppendFormat(" AND Phone>'' ");
            }
            if (!string.IsNullOrEmpty(userType) && userType == "6")
            {
                sbWhere.AppendFormat(" AND Company>'' ");
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                if (!string.IsNullOrEmpty(userType) && userType == "6")
                    sbWhere.AppendFormat(" AND Company like '%{0}%' ", keyWord);
                else if (!string.IsNullOrEmpty(userType) && userType == "2")
                    sbWhere.AppendFormat(" AND (WXNickname like '%{0}%' or Phone like '%{0}%') ", keyWord);
                else
                {
                    sbWhere.AppendFormat(" AND TrueName like '%{0}%' ", keyWord);
                }
            }
            if (!string.IsNullOrEmpty(userType))
            {
                sbWhere.AppendFormat(" AND UserType={0}", userType);
            }
            if (!string.IsNullOrEmpty(applyStatus))
            {
                sbWhere.AppendFormat(" AND MemberApplyStatus={0}", applyStatus);
            }
            totalCount = GetCount<UserInfo>(sbWhere.ToString());
            return GetLit<UserInfo>(pageSize, pageIndex, sbWhere.ToString(), " AutoID DESC ");
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <param name="tags"></param>
        /// <param name="distriBution"></param>
        /// <param name="nameOrWxName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<UserInfo> GetUserList(int pageIndex, int pageSize, string keyWord,
            string tags, string distriBution, string nameOrWxName, string userName, out int totalCount,
            string regTimeFrom = "", string regTimeTo = "", string autoIds = "", string colName = "", string keyWordType = "")
        {
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebSiteOwner='{0}' And UserID!='{0}' AND IsDisable=0", WebsiteOwner));
            if (!string.IsNullOrWhiteSpace(autoIds))
            {
                sbWhere.AppendFormat(" AND AutoID In ({0})", autoIds);
            }
            if (!string.IsNullOrEmpty(distriBution))
            {
                sbWhere.AppendFormat(" AND DistributionOwner is not null And DistributionOwner !=''");
            }
            if (!string.IsNullOrEmpty(nameOrWxName))
            {
                sbWhere.AppendFormat(" AND TrueName is not null And WXNickname is not null");
            }
            if (!string.IsNullOrEmpty(userName))
            {
                sbWhere.AppendFormat(" AND TrueName is not null");
            }

            if (!string.IsNullOrEmpty(keyWordType) && !string.IsNullOrEmpty(keyWord))
            {
                if (keyWordType == "key_truename" && !string.IsNullOrEmpty(keyWord))
                {
                    sbWhere.AppendFormat("And  TrueName like '{0}%' ", keyWord);
                }
                else if (keyWordType == "key_nickname" && !string.IsNullOrEmpty(keyWord))
                {
                    sbWhere.AppendFormat("And  WXNickName like '{0}%' ", keyWord);
                }
                else if (keyWordType == "key_phone" && !string.IsNullOrEmpty(keyWord))
                {
                    sbWhere.AppendFormat("And  Phone='{0}' ", keyWord);
                }
            }

            if (string.IsNullOrEmpty(keyWordType) && !string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat("And ( UserID  like '%{0}%' ", keyWord);
                sbWhere.AppendFormat("OR TrueName  like '%{0}%' ", keyWord);
                sbWhere.AppendFormat("OR Phone  like '%{0}%' ", keyWord);
                sbWhere.AppendFormat("OR WXNickName  like '%{0}%' ) ", keyWord);
            }
            if (!string.IsNullOrEmpty(tags))
            {
                string[] tagNameArray = tags.Split(',');
                sbWhere.AppendFormat(" AND( ");
                for (int i = 0; i < tagNameArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(tagNameArray[i]))
                    {
                        if (i > 0)
                        {
                            sbWhere.AppendFormat(" OR TagName like '%{0}%' ", tagNameArray[i]);
                        }
                        else
                        {
                            sbWhere.AppendFormat(" TagName like '%{0}%' ", tagNameArray[i]);
                        }

                    }
                }
                sbWhere.AppendFormat(") ");
            }

            if (!string.IsNullOrEmpty(regTimeFrom))
            {
                sbWhere.AppendFormat(" And RegTime>='{0}'", regTimeFrom);
            }
            if (!string.IsNullOrEmpty(regTimeTo))
            {
                sbWhere.AppendFormat(" And RegTime<='{0}'", regTimeTo);
            }

            totalCount = GetCount<UserInfo>(sbWhere.ToString());
            if (string.IsNullOrWhiteSpace(colName))
            {
                return GetLit<UserInfo>(pageSize, pageIndex, sbWhere.ToString());
            }
            else
            {
                return GetColList<UserInfo>(pageSize, pageIndex, sbWhere.ToString(), colName);
            }

        }

        /// <summary>
        /// 根据WXUnionID获取用户信息
        /// </summary>
        /// <param name="unionId"></param>
        /// <returns></returns>
        public Model.UserInfo GetUserInfoByWXUnionID(string unionId, string websiteOwner = "")
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WXUnionID ='{0}' ", unionId);
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbWhere.AppendFormat(" AND WebSiteOwner = '{0}' ", websiteOwner);
            return Get<Model.UserInfo>(sbWhere.ToString());
        }

        ///// <summary>
        ///// 根据站点获取用户信息
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        //public Model.UserInfo GetUserInfoByWebSite(string userID)
        //{
        //    //return Get<Model.UserInfo>(string.Format(" UserID = '{0}' And WebsiteOwner='{1}'", userID, WebsiteOwner));
        //    return Get<Model.UserInfo>(string.Format(" UserID = '{0}'", userID));
        //}

        /// <summary>
        /// 获取用户信息(根据16进制自增ID)
        /// </summary>
        /// <param name="autoIDHex"></param>
        /// <returns></returns>
        public Model.UserInfo GetUserInfoByAutoIDHex(string autoIDHex)
        {
            int aid = Convert.ToInt32(autoIDHex, 16);
            return Get<Model.UserInfo>(string.Format(" AutoID = {0} ", aid));
        }

        /// <summary>
        /// 获取用户信息(根据制自增ID)
        /// </summary>
        /// <param name="autoIDHex"></param>
        /// <returns></returns>
        public Model.UserInfo GetUserInfoByAutoID(int autoID)
        {
            return Get<Model.UserInfo>(string.Format(" AutoID = {0} ", autoID));
        }
        /// <summary>
        /// 获取用户信息(根据制自增ID)
        /// </summary>
        /// <param name="autoIDHex"></param>
        /// <returns></returns>
        public Model.UserInfo GetUserInfoByAutoID(int autoID, string websiteowner)
        {
            return Get<Model.UserInfo>(string.Format(" AutoID = {0} And WebsiteOwner='{1}'", autoID, websiteowner));
        }

        /// <summary>
        /// 获取多个用户信息(根据制自增ID)
        /// </summary>
        /// <param name="autoIDHex"></param>
        /// <returns></returns>
        public List<Model.UserInfo> GetUsersByAutoID(string autoIDs, string websiteowner)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" AutoID In ({0})", autoIDs);
            sbWhere.AppendFormat(" And WebsiteOwner='{0}'", websiteowner);
            return GetList<Model.UserInfo>(sbWhere.ToString());
        }
        /// <summary>
        /// 判断昵称是否重复
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public Model.UserInfo GetUserInfoByNickName(string nickName)
        {
            return Get<Model.UserInfo>(string.Format(" WebsiteOwner='{0}' AND WXNickname='{1}' ", WebsiteOwner, nickName));
        }
        /// <summary>
        /// 获取用户信息(根据制自增ID) ,坐标
        /// </summary>
        /// <param name="autoIDHex"></param>
        /// <returns></returns>
        public UserInfo GetRangeUserInfoByAutoID(int autoID, string websiteowner, string longitude, string latitude)
        {
            List<UserInfo> list = GetColList<UserInfo>(1, 1
                , string.Format(" AutoID = {0}  And WebsiteOwner='{1}' "
                , autoID, websiteowner)
                , string.Format("*,dbo.fnGetDistance({0},{1},[LastLoginLongitude],[LastLoginLatitude]) [Distance]"
                , longitude, latitude));
            if (list.Count == 0) return null;
            return list[0];
        }

        /// <summary>
        /// 根据openId获取用户信息
        /// </summary>
        /// <param name="wxOpenId"></param>
        /// <returns></returns>
        public Model.UserInfo GetUserInfoByOpenId(string wxOpenId)
        {
            if (string.IsNullOrWhiteSpace(wxOpenId))
                return null;
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebSiteOwner='{0}' ", WebsiteOwner);
            sbWhere.AppendFormat(" AND WXOpenId = '{0}' ", wxOpenId);

            return Get<Model.UserInfo>(sbWhere.ToString());
        }
        /// <summary>
        /// 根据OpenId获取用户信息
        /// </summary>
        /// <param name="wxOpenId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public Model.UserInfo GetUserInfoByOpenId(string wxOpenId, string websiteOwner)
        {
            if (string.IsNullOrWhiteSpace(wxOpenId))
                return null;
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebSiteOwner='{0}' ", websiteOwner);
            sbWhere.AppendFormat(" AND WXOpenId = '{0}' ", wxOpenId);

            return Get<Model.UserInfo>(sbWhere.ToString());
        }
        /// <summary>
        /// 根据openId获取用户信息客户端同步用
        /// </summary>
        /// <param name="wxOpenId"></param>
        /// <returns></returns>
        public Model.UserInfo GetUserInfoByOpenIdClient(string wxOpenId)
        {
            if (string.IsNullOrWhiteSpace(wxOpenId))
                return null;
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("  WXOpenId = '{0}' ", wxOpenId);

            return Get<Model.UserInfo>(sbWhere.ToString());
        }
        ///// <summary>
        ///// 获取指定用户剩余点数
        ///// </summary>
        ///// <param name="UserID"></param>
        ///// <returns></returns>
        //public int GetPoints()
        //{

        //    Model.UserInfo userModel = Get<Model.UserInfo>(string.Format("UserID = '{0}'", this.UserID));

        //    if (userModel != null)
        //        return (int)userModel.Points;

        //    return 0;
        //}

        ///// <summary>
        ///// 获取指定用户邮件剩余点数
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        //public int GetEDMPoits(string userID)
        //{
        //    Model.UserInfo userModel = Get<Model.UserInfo>(string.Format("UserID = '{0}'", this.UserID));

        //    if (userModel != null)
        //        return userModel.EmailPoints == null ? 0 : userModel.EmailPoints.Value;

        //    return 0;
        //}

        //public bool Consume(string userID, int points, string tractype, string tracNote)
        //{
        //    return false;
        //}

        //#region 角色权限相关



        //#endregion

        /// <summary>
        /// 登录日志
        /// </summary>
        public void AddLoginLogs(string userId = "", string websiteOwner = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId)) userId = UserID;
                if (string.IsNullOrWhiteSpace(websiteOwner)) websiteOwner = WebsiteOwner;
                UserLoginLogs model = new UserLoginLogs();
                model.UserID = userId;
                model.WebsiteOwner = websiteOwner;
                model.IP = Common.MySpider.GetClientIP();
                model.IPLocation = Common.MySpider.GetIPLocation(model.IP);
                model.Browser = HttpContext.Current.Request.Browser == null ? "" : HttpContext.Current.Request.Browser.ToString();
                model.BrowserID = HttpContext.Current.Request.Browser.Id;
                if (HttpContext.Current.Request.Browser.Beta)
                {
                    model.BrowserIsBata = "测试版";
                }
                else
                {
                    model.BrowserIsBata = "正式版";
                }

                model.BrowserVersion = HttpContext.Current.Request.Browser.Version;
                model.InsertDate = DateTime.Now;
                model.SystemByte = HttpContext.Current.Request.Browser.Platform;

                if (HttpContext.Current.Request.Browser.Win16)
                {
                    model.SystemPlatform = "16位系统";
                }
                else
                {
                    if (HttpContext.Current.Request.Browser.Win32)
                    {
                        model.SystemPlatform = "32位系统";
                    }
                    else
                    {
                        model.SystemPlatform = "64位系统";
                    }
                }

                if (!model.IP.Equals("127.0.0.1"))//本地调拭不记录
                {
                    Add(model);
                }

            }
            catch { }


        }
        //修改最后登录信息
        public void UpdateLastLoginInfo(string userId = "", string lastLoginCity = ""
            , string longitude = "", string latitude = "", string websiteOwner = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    userId = UserID;
                }

                StringBuilder setPms = new StringBuilder();
                setPms.AppendFormat("LastLoginIP='{0}',LastLoginDate=GetDate(),LoginTotalCount=LoginTotalCount+1"
                    , Common.MySpider.GetClientIP());
                if (!string.IsNullOrWhiteSpace(lastLoginCity)) setPms.AppendFormat(",LastLoginCity='{0}'", lastLoginCity);
                if (!string.IsNullOrWhiteSpace(longitude)) setPms.AppendFormat(",LastLoginLongitude='{0}'", longitude);
                if (!string.IsNullOrWhiteSpace(latitude)) setPms.AppendFormat(",LastLoginLatitude='{0}'", latitude);

                StringBuilder setWhere = new StringBuilder();
                setWhere.AppendFormat("UserID='{0}'", userId);
                if (!string.IsNullOrWhiteSpace(websiteOwner)) setWhere.AppendFormat(",WebsiteOwner='{0}'", websiteOwner);
                Update(new UserInfo(), setPms.ToString()
                    , setWhere.ToString());
            }
            catch { }
        }
        /// <summary>
        /// 检查手机号码是否已被验证
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool CheckPhoneIsVerify(string phone)
        {
            if (GetCount<UserInfo>(string.Format(" Phone = '{0}' and IsPhoneVerify = 1 ", phone)) > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否已补足用户基本信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool IsAllUserBaseInfo(string userID)
        {
            UserInfo model = GetUserInfo(userID);

            if (
                string.IsNullOrWhiteSpace(model.TrueName)
                ||
                string.IsNullOrWhiteSpace(model.Phone)
                ||
                string.IsNullOrWhiteSpace(model.Email)
                ||
                string.IsNullOrWhiteSpace(model.Company)
                ||
                string.IsNullOrWhiteSpace(model.Postion)
                )
                return false;
            return true;
        }

        ///// <summary>
        ///// 加用户积分
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="score"></param>
        ///// <param name="note"></param>
        ///// <returns></returns>
        //public bool AddUserScore(string userId, float score, string note = "", string scoreType = "")
        //{
        //    //插入明细

        //    //更新用户字段

        //    UserScoreDetailsInfo scoreModel = new UserScoreDetailsInfo();
        //    UserInfo currUser = GetUserInfo(userId);

        //    currUser.TotalScore += score;

        //    scoreModel.AddNote = note;
        //    scoreModel.AddTime = DateTime.Now;
        //    scoreModel.Score = score;
        //    scoreModel.UserID = userId;
        //    scoreModel.ScoreType = scoreType;

        //    ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
        //    try
        //    {
        //        if (Update(currUser, tran) && Add(scoreModel, tran))
        //        {
        //            tran.Commit();
        //            return true;
        //        }
        //        else
        //        {
        //            tran.Rollback();
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        tran.Rollback();
        //        return false;
        //    }
        //}

        /// <summary>
        /// 注册用户 
        /// </summary>
        /// <returns></returns>
        public bool RegUser(string userId, string pwd, out string msg)
        {

            if (GetUserInfo(userId) != null)
            {
                msg = "此账号已被注册，请输入别的账号";
                return false;
            }
            if (!FilterSql(userId))
            {
                msg = "非法账号";
                return false;
            }
            if (!FilterSql(pwd))
            {
                msg = "非法密码";
                return false;
            }
            UserInfo userInfo = new UserInfo();
            userInfo.UserID = userId;
            userInfo.Password = pwd;
            userInfo.UserType = 2;
            userInfo.WebsiteOwner = WebsiteOwner;
            userInfo.Regtime = DateTime.Now;
            userInfo.WXScope = "snsapi_base";

            userInfo.RegIP = ZentCloud.Common.MySpider.GetClientIP();
            userInfo.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
            userInfo.LastLoginDate = DateTime.Now;
            userInfo.LoginTotalCount = 1;

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                if (!Add(userInfo, tran))
                {
                    tran.Rollback();
                    msg = "注册失败";
                    return false;
                }
                else
                {
                    tran.Commit();
                    msg = "注册成功! ";
                    return true;


                    //var group1 = new ZentCloud.BLLPermission.Model.UserPmsGroupRelationInfo()
                    //        {
                    //            UserID = userInfo.UserID,
                    //            GroupID = 110578
                    //        };
                    //var group2 = new ZentCloud.BLLPermission.Model.UserPmsGroupRelationInfo()
                    //         {
                    //             UserID = userInfo.UserID,
                    //             GroupID = 130334
                    //         };

                    //if (Add(group1,tran)&&Add(group2,tran))
                    //{
                    //    tran.Commit();
                    //    msg = "注册成功! ";
                    //    return true;
                    //}
                    //else
                    //{
                    //    tran.Rollback();
                    //    msg = "注册失败";
                    //    return false;
                    //}
                }
            }
            catch (Exception ex)
            {

                tran.Rollback();
                msg = ex.Message;
                return false;

            }



        }


        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="fromUserId"></param>
        /// <param name="toUserId"></param>
        /// <returns></returns>
        public bool Follow(string fromUserId, string toUserId)
        {
            if (!CheckFollow(fromUserId, toUserId))
            {
                return false;
            }
            else
            {


                UserFollowChain followChain = new UserFollowChain();
                followChain.FromUserId = fromUserId;
                followChain.ToUserId = toUserId;

                ZentCloud.ZCBLLEngine.BLLTransaction bllTransaction = new ZentCloud.ZCBLLEngine.BLLTransaction();
                if (!this.Add(followChain, bllTransaction))
                {
                    bllTransaction.Rollback();
                    return false;
                }

                UserInfo userInfo = GetUserInfo(fromUserId);
                userInfo.FollowingCount += 1;
                if (this.Update(userInfo, string.Format(" FollowingCount={0}", userInfo.FollowingCount), string.Format(" AutoID={0}", userInfo.AutoID)) < 1)
                {
                    bllTransaction.Rollback();
                    return false;
                }
                userInfo = GetUserInfo(toUserId);
                userInfo.FansCount += 1;
                if (this.Update(userInfo, string.Format(" FansCount={0}", userInfo.FansCount), string.Format(" AutoID={0}", userInfo.AutoID)) < 1)
                {
                    bllTransaction.Rollback();
                    return false;
                }

                bllTransaction.Commit();
                return true;
            }
        }
        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="fromUserId"></param>
        /// <param name="toUserId"></param>
        /// <returns></returns>
        public bool CancelFollow(string fromUserId, string toUserId)
        {
            UserFollowChain followChain = this.Get<UserFollowChain>(string.Format("fromUserId='{0}' and toUserId='{1}'", fromUserId, toUserId));

            if (followChain == null)   //用户原本就未关注
            {
                return true;
            }
            else
            {
                ZentCloud.ZCBLLEngine.BLLTransaction bllTransaction = new ZentCloud.ZCBLLEngine.BLLTransaction();
                if (this.Delete(followChain) <= 0)
                {
                    bllTransaction.Rollback();
                    return false;
                }
                UserInfo userInfo = GetUserInfo(fromUserId);
                userInfo.FollowingCount -= 1;
                if (this.Update(userInfo, string.Format(" FollowingCount={0}", userInfo.FollowingCount), string.Format(" AutoID={0}", userInfo.AutoID)) < 1)
                {
                    bllTransaction.Rollback();
                    return false;
                }

                userInfo = GetUserInfo(toUserId);
                userInfo.FansCount -= 1;
                if (this.Update(userInfo, string.Format(" FansCount={0}", userInfo.FansCount), string.Format(" AutoID={0}", userInfo.AutoID)) < 1)
                {
                    bllTransaction.Rollback();
                    return false;
                }

                bllTransaction.Commit();
                return true;
            }
        }
        /// <summary>
        /// 是否已关注
        /// </summary>
        /// <param name="fromUserId"></param>
        /// <param name="toUserId"></param>
        /// <returns></returns>
        public bool CheckFollow(string fromUserId, string toUserId)
        {
            if (fromUserId.Equals(toUserId))
            {
                return false;
            }
            int count = this.GetCount<Model.UserFollowChain>(string.Format("fromUserId='{0}' and toUserId='{1}'", fromUserId, toUserId));
            return count > 0 ? false : true;
        }

        //public UserInfo GetUserInfoFromId(string userId)
        //{
        //    return this.Get<UserInfo>(string.Format("UserId = '{0}'", userId));
        //}

        /// <summary>
        /// 获取用户粉丝数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserFlowerCount(string userId)
        {
            return GetCount<UserFollowChain>(string.Format("ToUserId='{0}'", userId));
        }
        /// <summary>
        /// 获取用户关注数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserAttentionCount(string userId)
        {
            return GetCount<UserFollowChain>(string.Format("FromUserId='{0}'", userId));
        }

        /// <summary>
        /// 获取用户展示的名称
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public string GetUserDispalyName(UserInfo userInfo, bool hideName = false)
        {
            if (userInfo == null) return "  ";
            if (userInfo.UserID == userInfo.WebsiteOwner) return "系统";
            return GetUserDispalyName(userInfo.WXNickname, userInfo.TrueName, hideName);
        }
        /// <summary>
        /// 获取用户展示的名称
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public string GetUserDispalyName(string wxNickName, string trueName, bool hideName = false)
        {
            string result = string.Empty;

            WebsiteInfo site = GetWebsiteInfoModelFromDataBase();
            if (site == null || site.UserInfoFirstShow == 0)
            {
                if (!string.IsNullOrWhiteSpace(trueName))
                    result = trueName;
                if (!string.IsNullOrWhiteSpace(wxNickName))
                    result = wxNickName;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(wxNickName))
                    result = wxNickName;
                if (!string.IsNullOrWhiteSpace(trueName))
                    result = trueName;
            }
            if (string.IsNullOrWhiteSpace(result))
                result = "  ";
            return GetUserShowDispalyName(result, hideName);
        }
        /// <summary>
        /// 显示名称修改
        /// </summary>
        /// <param name="dispalyName"></param>
        /// <param name="hideName"></param>
        /// <returns></returns>
        public string GetUserShowDispalyName(string dispalyName, bool hideName = false)
        {
            if (hideName && dispalyName != "  " && dispalyName.Length > 2)
            {
                dispalyName = dispalyName.Substring(0, 2) + "*".PadLeft(dispalyName.Length - 2, '*');
            }
            else if (hideName && dispalyName != "  " && dispalyName != "系统" && dispalyName.Length > 1)
            {
                dispalyName = dispalyName.Substring(0, 1) + "*".PadLeft(dispalyName.Length - 1, '*');
            }
            return dispalyName;
        }

        /// <summary>
        /// 获取用户展示的手机
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public string GetUserDispalyPhone(string phone, bool hidePhone = false)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                phone = "  ";
            }
            if (hidePhone && phone != "  " && phone.Length > 7)
            {
                phone = phone.Substring(0, 3) + "*".PadLeft(phone.Length - 7, '*') + phone.Substring(phone.Length - 4);
            }
            return phone;
        }
        /// <summary>
        /// 获取用户展示的名称
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public string GetUserDispalyName(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return "";
            }
            string result = string.Empty;
            if (userId == WebsiteOwner)
            {
                return "系统";
            }
            UserInfo userInfo = GetColByKey<UserInfo>("UserID", userId, "AutoID,TrueName,WXNickname", websiteOwner: WebsiteOwner);
            if (userInfo==null)
            {
                return "";
           
            }
            return GetUserDispalyName(userInfo.WXNickname, userInfo.TrueName);
        }

        /// <summary>
        /// 获取用户展示头像
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetUserAutoID(string userId, string websiteOwner = "")
        {
            if (string.IsNullOrWhiteSpace(websiteOwner)) websiteOwner = WebsiteOwner;
            UserInfo userInfo = GetColByKey<UserInfo>("UserID", userId, "AutoID", websiteOwner: websiteOwner);
            if (userInfo != null)
            {
                return userInfo.AutoID;
            }
            return 0;
        }
        /// <summary>
        /// 获取用户展示头像
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserDispalyAvatar(string wxHeadimgurl, string avatar)
        {
            string result = "http://file.comeoncloud.net/img/europejobsites.png";

            WebsiteInfo site = GetWebsiteInfoModelFromDataBase();
            if (site == null || site.UserInfoFirstShow == 0)
            {
                if (!string.IsNullOrWhiteSpace(avatar))
                    result = avatar;
                if (!string.IsNullOrWhiteSpace(wxHeadimgurl))
                    result = wxHeadimgurl;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(wxHeadimgurl))
                    result = wxHeadimgurl;
                if (!string.IsNullOrWhiteSpace(avatar))
                    result = avatar;
            }
            return result;
        }
        /// <summary>
        /// 获取用户展示头像
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public string GetUserDispalyAvatar(UserInfo userInfo)
        {
            return GetUserDispalyAvatar(userInfo.WXHeadimgurl, userInfo.Avatar);
        }
        /// <summary>
        /// 获取性别Int
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public int GetSexInt(Model.UserInfo userInfo)
        {
            int sexInt = 0;
            WebsiteInfo site = GetWebsiteInfoModelFromDataBase();
            if (site == null || site.UserInfoFirstShow == 0)
            {
                if (!string.IsNullOrWhiteSpace(userInfo.Gender))
                {
                    if (Int32.TryParse(userInfo.Gender, out sexInt) && sexInt == 0)
                        sexInt = 2;
                }
                if (userInfo.WXSex.HasValue)
                    sexInt = userInfo.WXSex.Value;
            }
            else
            {
                if (userInfo.WXSex.HasValue)
                    sexInt = userInfo.WXSex.Value;

                if (!string.IsNullOrWhiteSpace(userInfo.Gender))
                {
                    if (Int32.TryParse(userInfo.Gender, out sexInt) && sexInt == 0)
                        sexInt = 2;
                }
            }
            return sexInt;
        }

        /// <summary>
        /// 获取性别Int
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetSexInt(string userId, string websiteOwner = "")
        {
            if (string.IsNullOrWhiteSpace(websiteOwner)) websiteOwner = WebsiteOwner;
            UserInfo userInfo = GetColByKey<UserInfo>("UserID", userId, "AutoID,WXSex,Gender", websiteOwner: websiteOwner);
            return GetSexInt(userInfo);
        }
        /// <summary>
        /// 获取性别字符串
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public string GetSex(int sexInt)
        {
            if (sexInt == 1)
            {
                return "男";
            }
            else if (sexInt == 2)
            {
                return "女";
            }
            return " ";
        }
        /// <summary>
        /// 获取性别字符串
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public string GetSex(Model.UserInfo userInfo)
        {
            int sexInt = GetSexInt(userInfo);
            return GetSex(sexInt);
        }

        /// <summary>
        /// 获取性别字符串
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetSex(string userId, string websiteOwner = "")
        {
            int sexInt = GetSexInt(userId, websiteOwner);
            return GetSex(sexInt);
        }

        /// <summary>
        /// 检查用户是否是专家
        /// </summary>
        /// <returns></returns>
        public bool IsTutor(ZentCloud.BLLJIMP.Model.UserInfo userInfo, string websiteOwner = "")
        {
            if (string.IsNullOrWhiteSpace(websiteOwner)) websiteOwner = WebsiteOwner;
            TutorInfo tutor = GetColByKey<TutorInfo>("UserId", userInfo.UserID, "AutoId", websiteOwner: websiteOwner);
            return tutor != null ? true : false;
        }
        /// <summary>
        /// 获取用户等级
        /// </summary>
        /// <param name="historyTotalScore"></param>
        /// <returns></returns>
        public UserLevelConfig GetUserLevelByHistoryTotalScore(double historyTotalScore, string levelType = null)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebSiteOwner='{0}'", WebsiteOwner);
            if (!string.IsNullOrWhiteSpace(levelType)) sbSql.AppendFormat(" AND LevelType='{0}'", levelType);
            sbSql.AppendFormat(" Order by LevelNumber ASC");

            foreach (var item in GetList<UserLevelConfig>(sbSql.ToString()))
            {
                if ((historyTotalScore >= item.FromHistoryScore) && ((historyTotalScore <= item.ToHistoryScore)))
                {
                    return item;
                }

            }
            return null;


        }

        public UserLevelConfig GetUserLevelByLevelNumber(int levelNumber, string levelType = null)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebSiteOwner='{0}'", WebsiteOwner);
            if (!string.IsNullOrWhiteSpace(levelType)) sbSql.AppendFormat(" AND LevelType='{0}'", levelType);
            sbSql.AppendFormat(" Order by LevelNumber ASC");

            //var levelList = GetList<UserLevelConfig> (sbSql.ToString());

            foreach (var item in GetList<UserLevelConfig>(sbSql.ToString()))
            {
                if (levelNumber == item.LevelNumber)
                {
                    return item;
                }

            }
            return null;


        }


        /// <summary>
        /// 检查用户是否是专家
        /// </summary>
        /// <returns></returns>
        public bool IsTutor(string userId, string websiteOwner = "")
        {
            if (string.IsNullOrWhiteSpace(websiteOwner)) websiteOwner = WebsiteOwner;
            TutorInfo tutor = GetColByKey<TutorInfo>("UserId", userId, "AutoId", websiteOwner: websiteOwner);
            return tutor != null ? true : false;
        }


        /// <summary>
        /// 通过邮箱获取用户信息
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Model.UserInfo GetUserInfoByEmail(string email)
        {
            return Get<Model.UserInfo>(string.Format(" Email = '{0}' And websiteOwner='{1}' ", email, WebsiteOwner));
        }

        ///// <summary>
        ///// 通过手机获取用户信息
        ///// </summary>
        ///// <param name="email"></param>
        ///// <returns></returns>
        //public Model.UserInfo GetUserInfoByPhone(string phone)
        //{
        //    return Get<Model.UserInfo>(string.Format(" Phone = '{0}' And websiteOwner='{1}' ", phone, WebsiteOwner));
        //}

        /// <summary>
        /// 整加单个列值数量
        /// </summary>
        /// <param name="col"></param>
        /// <param name="userId"></param>
        /// <param name="jid"></param>
        /// <returns></returns>
        public bool PlusNumericalCol(string col, string userId, int value = 1)
        {
            var result = Update(new UserInfo(), string.Format(" {0} = {0} + ({1}) ", col, value), string.Format(" UserID = '{0}' ", userId)) > 0;

            return result;
        }
        /// <summary>
        /// 整加单个列值数量
        /// </summary>
        /// <param name="col"></param>
        /// <param name="userId"></param>
        /// <param name="jid"></param>
        /// <returns></returns>
        public bool PlusNumericalColDouble(string col, string userId, double value = 1)
        {
            var result = Update(new UserInfo(), string.Format(" {0} = {0} + ({1}) ", col, value), string.Format(" UserID = '{0}' ", userId)) > 0;

            return result;
        }

        public List<UserInfo> GetUsers(string fields, string userIds)
        {
            List<UserInfo> userList = new List<UserInfo>();
            if (!string.IsNullOrWhiteSpace(fields) && !string.IsNullOrWhiteSpace(userIds))
            {
                string ids = "'" + userIds.Replace(",", "','") + "'";
                string sqlStr = string.Format("SELECT {0} FROM ZCJ_UserInfo WHERE UserID IN ({1}) ;", fields, ids, WebsiteOwner);
                userList = Query<UserInfo>(sqlStr);
            }
            return userList;
        }
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<UserInfo> GetAllUsers(string fields, string websiteOwner, string where = "")
        {
            List<UserInfo> userList = new List<UserInfo>();
            if (!string.IsNullOrWhiteSpace(fields))
            {
                string sqlStr = string.Format("SELECT {0} FROM ZCJ_UserInfo WHERE IsDisable=0 ", fields, websiteOwner);
                if (!string.IsNullOrWhiteSpace(where))
                {
                    sqlStr += " AND " + where;
                }
                userList = Query<UserInfo>(sqlStr);
            }
            return userList;
        }
        /// <summary>
        /// 获取某关系用户
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="mainId"></param>
        /// <returns></returns>
        public List<Model.UserInfo> GetRelationUserList(Enums.CommRelationType rtype, string mainId)
        {
            string type = CommonPlatform.Helper.EnumStringHelper.ToString(rtype);
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" SELECT A.AutoID,UserID,WXNickname,TrueName,WXHeadimgurl FROM [ZCJ_UserInfo] A ");
            strWhere.AppendFormat(" INNER JOIN [ZCJ_CommRelationInfo] ON [RelationId]=[UserID] ");
            strWhere.AppendFormat(" WHERE RelationType = '{0}' ", type);
            strWhere.AppendFormat(" AND  MainId = '{0}' ", mainId);
            List<UserInfo> users = Query<UserInfo>(strWhere.ToString());
            return users;
        }

        /// <summary>
        /// 获取某关系用户列表
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="mainId"></param>
        /// <returns></returns>
        public List<Model.UserInfo> GetRelationUserList(int pageSize, int pageIndex, Enums.CommRelationType rtype, string mainId, string RelationId, string keyword, out int tatol, bool isid = false)
        {

            string type = CommonPlatform.Helper.EnumStringHelper.ToString(rtype);
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" SELECT AutoID,UserID,WXNickname,TrueName,WebsiteOwner,WXHeadimgurl,Avatar,Phone,ViewType,TotalScore,OnlineTimes,Description,RelationTime as LastLoginDate FROM ( ");
            strWhere.AppendFormat("     SELECT ROW_NUMBER() OVER ( ");
            strWhere.AppendFormat(" 	    ORDER BY  ");
            strWhere.AppendFormat("         [RelationTime] DESC ) NUM ");
            strWhere.AppendFormat("     ,A.AutoID,A.UserID,A.WXNickname,A.TrueName,A.WebsiteOwner,A.WXHeadimgurl,A.Avatar,A.Phone,A.ViewType,A.TotalScore,A.OnlineTimes,A.Description,B.RelationTime  ");
            strWhere.AppendFormat("     FROM [ZCJ_UserInfo] A  ");
            strWhere.AppendFormat("     INNER JOIN [ZCJ_CommRelationInfo] B ON [RelationType] = '{0}' ", rtype);
            if (!isid)
            {
                if (!string.IsNullOrWhiteSpace(mainId)) strWhere.AppendFormat("         AND B.[MainId] = '{0}' AND B.[RelationId]=A.[UserID] ", mainId);
                if (!string.IsNullOrWhiteSpace(RelationId)) strWhere.AppendFormat("         AND B.[RelationId] = '{0}' AND B.[MainId]=A.[UserID] ", RelationId);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(mainId)) strWhere.AppendFormat("         AND B.[MainId] = '{0}' AND A.[AutoID]=B.[RelationId]  ", mainId);
                if (!string.IsNullOrWhiteSpace(RelationId)) strWhere.AppendFormat("         AND B.[RelationId] = '{0}' AND A.[AutoID]=B.[MainId] ", RelationId);
            }

            if (!string.IsNullOrWhiteSpace(keyword)) strWhere.AppendFormat("     WHERE (A.TrueName like '%{0}%' or A.WXNickname like '%{0}%') ", keyword);
            strWhere.AppendFormat(" ) AS TEMP WHERE NUM  BETWEEN ({1}-1)* {0}+1 AND {1}*{0}; ", pageSize, pageIndex);
            List<UserInfo> users = Query<UserInfo>(strWhere.ToString());

            strWhere = new StringBuilder();
            strWhere.AppendFormat("     SELECT COUNT(1) ");
            strWhere.AppendFormat("     FROM [ZCJ_UserInfo] A  ");
            strWhere.AppendFormat("     INNER JOIN [ZCJ_CommRelationInfo] B ON [RelationType] = '{0}' ", rtype);
            if (!isid)
            {
                if (!string.IsNullOrWhiteSpace(mainId)) strWhere.AppendFormat("         AND B.[MainId] = '{0}' AND B.[RelationId]=A.[UserID] ", mainId);
                if (!string.IsNullOrWhiteSpace(RelationId)) strWhere.AppendFormat("         AND B.[RelationId] = '{0}' AND B.[MainId]=A.[UserID] ", RelationId);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(mainId)) strWhere.AppendFormat("         AND B.[MainId] = '{0}' AND A.[AutoID]=B.[RelationId] ", mainId);
                if (!string.IsNullOrWhiteSpace(RelationId)) strWhere.AppendFormat("         AND B.[RelationId] = '{0}' AND A.[AutoID]=B.[MainId] ", RelationId);
            }
            if (!string.IsNullOrWhiteSpace(keyword)) strWhere.AppendFormat("     WHERE (A.TrueName like '%{0}%' or A.WXNickname like '%{0}%') ", keyword);
            DataSet ds = Query(strWhere.ToString());
            tatol = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            return users;
        }

        /// <summary>
        /// 获取可邀请回答用户
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="rtype"></param>
        /// <param name="relationId"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<Model.UserInfo> GetCanInvitUsers(int pageSize, int pageIndex, Enums.CommRelationType rtype, string relationId, string keyword, out int total)
        {
            string type = CommonPlatform.Helper.EnumStringHelper.ToString(rtype);
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" SELECT AutoID,UserID,WXNickname,TrueName,WXHeadimgurl,Convert(bit,(CASE WHEN CC IS NULL THEN 0 ELSE 1 END)) IsTutor,ViewCount FROM ( ");
            strWhere.AppendFormat("     SELECT ROW_NUMBER() OVER ( ");
            strWhere.AppendFormat(" 	    ORDER BY  ");
            strWhere.AppendFormat("         (CASE WHEN C.UserId IS NULL THEN 0 ELSE 1 END)  DESC ");
            strWhere.AppendFormat("         ,ViewCount DESC) NUM ");
            strWhere.AppendFormat("     ,A.AutoID ");
            strWhere.AppendFormat("     ,A.UserID,WXNickname,TrueName,WXHeadimgurl,C.UserId CC,ViewCount  ");
            strWhere.AppendFormat("     FROM [ZCJ_UserInfo] A  ");
            strWhere.AppendFormat("     INNER JOIN [ZCJ_CommRelationInfo] ON [MainId]=[UserID] ");
            strWhere.AppendFormat("     LEFT JOIN [ZCJ_TutorInfo] C ON A.UserID= C.UserId ");
            strWhere.AppendFormat("     WHERE RelationType = '{0}' ", rtype);
            strWhere.AppendFormat("     AND RelationId = '{0}' ", relationId);
            if (!string.IsNullOrWhiteSpace(keyword)) strWhere.AppendFormat("     AND (TrueName like '%{0}%' or WXNickname like '%{0}%') ", keyword);
            strWhere.AppendFormat(" ) AS TEMP WHERE NUM  BETWEEN ({1}-1)* {0}+1 AND {1}*{0}; ", pageSize, pageIndex);
            List<UserInfo> users = Query<UserInfo>(strWhere.ToString());

            strWhere = new StringBuilder();
            strWhere.AppendFormat("     SELECT COUNT(1) ");
            strWhere.AppendFormat("     FROM [ZCJ_UserInfo] A  ");
            strWhere.AppendFormat("     INNER JOIN [ZCJ_CommRelationInfo] ON [MainId]=[UserID] ");
            strWhere.AppendFormat("     LEFT JOIN [ZCJ_TutorInfo] C ON A.UserID= C.UserId ");
            strWhere.AppendFormat("     WHERE RelationType = '{0}' ", rtype);
            strWhere.AppendFormat("     AND  RelationId = '{0}' ", relationId);
            if (!string.IsNullOrWhiteSpace(keyword)) strWhere.AppendFormat("     AND (TrueName like '%{0}%' or WXNickname like '%{0}%') ", keyword);
            DataSet ds = Query(strWhere.ToString());

            total = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

            return users;
        }

        /// <summary>
        /// 获取无某关系用户
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="topNum"></param>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<Model.UserInfo> GetNoRelationUserList(Enums.CommRelationType rtype, int topNum, string userId, string websiteOwner)
        {
            string type = CommonPlatform.Helper.EnumStringHelper.ToString(rtype);
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" SELECT TOP {0} AutoID,UserID,WXNickname,TrueName,WXHeadimgurl FROM (", topNum);
            strWhere.AppendFormat(" SELECT ROW_NUMBER() OVER (ORDER BY ViewCount DESC) NUM, AutoID,UserID,WXNickname,TrueName,WXHeadimgurl FROM [ZCJ_UserInfo] ");
            strWhere.AppendFormat(" WHERE  websiteOwner = '{0}' ", websiteOwner);
            if (!string.IsNullOrWhiteSpace(userId)) strWhere.AppendFormat(" AND  not exists (select 1 from [ZCJ_CommRelationInfo] where RelationType = '{0}' and  [MainId] = [UserID] and [RelationId] ='{1}') ", type, userId);
            strWhere.AppendFormat(" ) AS TEMP WHERE NUM<20 ORDER BY NEWID(); ");
            List<UserInfo> userList = Query<UserInfo>(strWhere.ToString());
            return userList;
        }


        /// <summary>
        /// 获取某用户的赞的数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetPraiseCount(string userId)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" WITH A AS( ");
            strWhere.AppendFormat(" SELECT COUNT(1) [JuActivityPraiseCount] FROM [ZCJ_CommRelationInfo] A ");
            strWhere.AppendFormat(" INNER JOIN [ZCJ_JuActivityInfo] B ON B.[JuActivityID]=A.[MainId] AND [ArticleType] IN ('Article','Question') ");
            strWhere.AppendFormat(" INNER JOIN [ZCJ_UserInfo] C ON B.[UserID]=C.[UserID] AND C.[UserID]='{0}' ", userId);
            strWhere.AppendFormat(" WHERE RelationType='JuActivityPraise' ");
            strWhere.AppendFormat(" ), ");
            strWhere.AppendFormat(" B AS ( ");
            strWhere.AppendFormat(" SELECT COUNT(1) [ReviewPraiseCount] FROM [ZCJ_CommRelationInfo] A ");
            strWhere.AppendFormat(" INNER JOIN [ZCJ_ReviewInfo] B ON B.[ReviewMainId]=A.[MainId] ");
            strWhere.AppendFormat(" INNER JOIN [ZCJ_UserInfo] C ON B.[UserID]=C.[UserID] AND C.[UserID]='{0}' ", userId);
            strWhere.AppendFormat(" WHERE RelationType='ReviewPraise' ");
            strWhere.AppendFormat(" ) ");
            strWhere.AppendFormat(" SELECT [JuActivityPraiseCount]+[ReviewPraiseCount] [PraiseCount] FROM A,B ");
            System.Data.DataSet ds = Query(strWhere.ToString());
            return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 加用户积分明细
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="score"></param>
        /// <param name="note"></param>
        /// <param name="nscore"></param>
        /// <returns></returns>
        public bool AddUserScoreDetail(string userId, string type, string websiteOwner, int? nscore = null, string remark = "")
        {
            string msg = "";
            return AddUserScoreDetail(userId, type, websiteOwner, out msg, nscore, remark, "", true);
        }
        /// <summary>
        /// 加用户积分明细
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="score"></param>
        /// <param name="note"></param>
        /// <param name="nscore"></param>
        /// <returns></returns>
        public bool AddUserScoreDetail(string userId, string type, string websiteOwner, out string msg, int? nscore = null
            , string remark = "", string ex1 = "", bool mustHaveDefine = true, string relationID = null, string juActivityType = "")
        {
            ToLog(string.Format("进入 AddUserScoreDetail,userId:{0},type:{1}, nscore:{2},remark:{3},relationID:{4},mustHaveDefine:{5}",
                userId,
                type,
                nscore,
                remark,
                relationID,
                mustHaveDefine
                ));
            msg = "";
            BLLScoreDefine bllScoreDefine = new BLLScoreDefine();
            ScoreDefineInfo scoreDefineInfo = bllScoreDefine.GetScoreDefineInfo(type, websiteOwner);
            //插入明细
            if (scoreDefineInfo == null && mustHaveDefine)
            {
                msg = "未找到该规则";
                return false;
            }

            double score = 0;
            if (scoreDefineInfo != null)
            {
                score = scoreDefineInfo.Score;
            }
            if (nscore.HasValue)
            {
                score = nscore.Value;
            }

            if (score == 0)
            {
                msg = "积分不能为0";
                return false;
            }


            if (scoreDefineInfo != null)
            {
                ToLog("找到积分规则");

                if (scoreDefineInfo.ScoreType == "ReadArticle" || scoreDefineInfo.ScoreType == "ReadCategory" || scoreDefineInfo.ScoreType == "ReadType" || scoreDefineInfo.ScoreType == "ShareArticle")
                {


                    //判断是否已经有了重复id
                    if (!string.IsNullOrWhiteSpace(relationID))
                    {
                        ToLog("进入判断是否已经有了重复id");
                        var tmpWhere = string.Format(" ScoreType = '{0}' AND RelationID = '{1}' AND UserId = '{2}' AND WebSiteOwner = '{3}' ",
                            type,
                            relationID,
                            userId,
                            websiteOwner
                        );
                        ToLog("tmpWhere:" + tmpWhere);
                        var tmpUserScoreDetail = Get<UserScoreDetailsInfo>(tmpWhere);

                        if (tmpUserScoreDetail != null)
                        {
                            ToLog("重复添加积分");
                            msg = "重复添加积分";
                            return false;
                        }
                        ToLog("没有重复添加积分");
                    }



                    if (!string.IsNullOrWhiteSpace(juActivityType) && scoreDefineInfo.ScoreType == "ReadArticle")
                    {
                        if (juActivityType.ToLower() == "article")
                        {
                            //defName = "阅读文章";
                            scoreDefineInfo.Name = "阅读文章";
                            scoreDefineInfo.Description = "阅读文章";
                        }
                        else
                        {
                            scoreDefineInfo.Name = "阅读活动";
                            scoreDefineInfo.Description = "阅读活动";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(juActivityType) && scoreDefineInfo.ScoreType == "ShareArticle")
                    {
                        if (juActivityType.ToLower() == "article")
                        {
                            scoreDefineInfo.Name = "分享文章";
                            scoreDefineInfo.Description = "分享文章";
                        }
                        else
                        {
                            scoreDefineInfo.Name = "分享活动";
                            scoreDefineInfo.Description = "分享活动";
                        }
                    }

                    if (string.IsNullOrWhiteSpace(ex1))
                    {
                        msg = "关联ID为空";
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(scoreDefineInfo.Ex1))
                    {
                        msg = "未配置关联ID";
                        return false;
                    }
                    if (scoreDefineInfo.ScoreType == "ReadCategory" && bllScoreDefine.ExistsScoreDefine("ReadArticle", websiteOwner))
                    {
                        return false;
                    }
                    if (scoreDefineInfo.ScoreType == "ReadType" &&
                        (bllScoreDefine.ExistsScoreDefine("ReadArticle", websiteOwner) || bllScoreDefine.ExistsScoreDefine("ReadCategory", websiteOwner)))
                    {
                        return false;
                    }
                    if (scoreDefineInfo.Ex1 != "0")
                    {
                        string nex1 = ex1.ToLower();
                        string[] exs = scoreDefineInfo.Ex1.ToLower().Split(',');
                        if (!exs.Contains(nex1))
                        {
                            msg = "配置关联ID未包含" + ex1;
                            return false;
                        }
                    }
                }

                if (scoreDefineInfo.TotalLimit > 0)
                {
                    double nTotal = GetUserDayScoreSUM(userId, type, false);
                    if (scoreDefineInfo.TotalLimit < nTotal + score)
                    {
                        ToLog("所得总积分超限制");
                        msg = scoreDefineInfo.Name + "所得总积分超限制";
                        return false;
                    }
                }
                if (scoreDefineInfo.DayLimit > 0)
                {
                    double nTotal = GetUserDayScoreSUM(userId, type, true);
                    if (scoreDefineInfo.DayLimit < nTotal + score)
                    {
                        msg = scoreDefineInfo.Name + "每日所得积分超限制";
                        return false;
                    }
                }
            }

            ToLog("开始更新用户积分");

            //更新用户字段
            UserScoreDetailsInfo scoreModel = new UserScoreDetailsInfo();
            UserInfo currUser = GetUserInfo(userId, websiteOwner);

            scoreModel.AddNote = scoreDefineInfo != null ? scoreDefineInfo.Description : remark;
            if (!string.IsNullOrEmpty(remark))
            {
                scoreModel.AddNote = remark;
            }
            scoreModel.AddTime = DateTime.Now;
            scoreModel.Score = score;
            scoreModel.UserID = userId;
            scoreModel.ScoreType = scoreDefineInfo != null ? scoreDefineInfo.ScoreType : type;

            if (!string.IsNullOrWhiteSpace(relationID)) scoreModel.RelationID = relationID;

            currUser.TotalScore += score;
            scoreModel.TotalScore = currUser.TotalScore;
            scoreModel.WebSiteOwner = websiteOwner;

            string defName = scoreDefineInfo != null ? scoreDefineInfo.Name : remark;


            BLLTransaction tran = new BLLTransaction();
            try
            {
                if (Update(currUser, tran) && Add(scoreModel, tran))
                {
                    msg = defName + "添加完成";
                    tran.Commit();
                    try
                    {
                        BLLSystemNotice bllSystemNotice = new BLLSystemNotice();
                        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
                        string scoreDispalyName = bllKeyValueData.GetDataDefVaule("ScoreDispalyName", "1", "积分", websiteOwner);

                        string sysContent = "";
                        string skey = (scoreModel.Score > 0 ? "获得" : "消费");
                        if (scoreModel.AddNote.Contains(skey))
                        {
                            sysContent = scoreModel.AddNote + "，余" + scoreDispalyName + "：" + scoreModel.TotalScore;
                        }
                        else
                        {
                            sysContent = scoreModel.AddNote + "，" + (scoreModel.Score > 0 ? "获得" : "消费") + scoreModel.Score + scoreDispalyName + "，余" + scoreDispalyName + "：" + scoreModel.TotalScore;
                        }

                        bllSystemNotice.SendNotice(BLLJIMP.BLLSystemNotice.NoticeType.SystemMessage, null, null, new List<UserInfo>() { currUser }, sysContent);

                        BLLWeixin bllWeiXin = new BLLWeixin();
                        var sendMsg = defName;

                        if (!string.IsNullOrEmpty(remark) && remark != sendMsg)
                        {
                            sendMsg += " " + remark;
                        }

                        if (score > 0)
                        {
                            bllWeiXin.SendTemplateMessageNotifyComm(currUser, scoreDispalyName + "变动通知", string.Format("{0}\\n增加{3}:{1}\\n总{3}:{2}", sendMsg, score, scoreModel.TotalScore, scoreDispalyName));
                        }
                        else if (score < 0)
                        {
                            bllWeiXin.SendTemplateMessageNotifyComm(currUser, scoreDispalyName + "变动通知", string.Format("{0}\\减少{3}:{1}\\n总{3}:{2}", sendMsg, score, scoreModel.TotalScore, scoreDispalyName));
                        }


                        var websiteInfo = GetWebsiteInfoModelFromDataBase();

                        if (websiteInfo.IsUnionHongware == 1)
                        {

                            Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(currUser.WebsiteOwner);
                            var hongWareMemberInfo = hongWareClient.GetMemberInfo(currUser.WXOpenId);
                            if (hongWareMemberInfo.member != null)
                            {
                                if (!hongWareClient.UpdateMemberScore(hongWareMemberInfo.member.mobile, currUser.WXOpenId, (float)score))
                                {

                                }
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        ToLog("积分执行错误：" + ex.Message);
                    }

                    ToLog("积分执行完毕");

                    return true;
                }
                else
                {
                    msg = defName + "添加出错";
                    tran.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg = defName + "添加出错";
                tran.Rollback();
                return false;
            }
        }


        /// <summary>
        /// 用户当日某类积分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="scoreEvent"></param>
        /// <returns></returns>
        public double GetUserDayScoreSUM(string userId, string type, bool inNowDay = true, string relationID = null)
        {
            //StringBuilder sbSql = new StringBuilder();
            //sbSql.AppendFormat(" [ScoreType]='{0}' ", type);
            //sbSql.AppendFormat(" AND [UserID]='{0}' ", userId);
            //if (inNowDay) sbSql.AppendFormat(" AND datediff(day,[AddTime],getdate())=0 ");
            //if (!string.IsNullOrWhiteSpace(relationID)) sbSql.AppendFormat(" AND [RelationID]='{0}' ", relationID);
            //List<UserScoreDetailsInfo> list = GetColList<UserScoreDetailsInfo>(int.MaxValue, 1, sbSql.ToString(), "AutoID,Score");
            //if (list.Count == 0) return 0;
            //return list.Sum(p => p.Score);

            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" Select Sum(Score) From ZCJ_UserScoreDetailsInfo Where Score>0 ");
            sbSql.AppendFormat(" And [ScoreType]='{0}' ", type);
            sbSql.AppendFormat(" And [UserID]='{0}' ", userId);
            if (inNowDay)
            {
                sbSql.AppendFormat(" AND datediff(day,[AddTime],getdate())=0 ");
            };
            if (!string.IsNullOrWhiteSpace(relationID)) sbSql.AppendFormat(" AND [RelationID]='{0}' ", relationID);
            var result = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sbSql.ToString());
            if (result != null)
            {
                return double.Parse(result.ToString());
            }
            return 0;
        }
        /// <summary>
        /// 用户当日某事件积分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public double GetUserDayScoreSUMEvent(string userId, string scoreEvent, bool inNowDay = true, string relationID = null)
        {
            StringBuilder sbSql = new StringBuilder();

            sbSql.AppendFormat(" Select Sum(Score) From ZCJ_UserScoreDetailsInfo Where Score>0 ");
            sbSql.AppendFormat(" And [ScoreEvent]='{0}' ", scoreEvent);
            sbSql.AppendFormat(" And [UserID]='{0}' ", userId);
            if (inNowDay)
            {
                sbSql.AppendFormat(" AND datediff(day,[AddTime],getdate())=0 ");
            };
            if (!string.IsNullOrWhiteSpace(relationID)) sbSql.AppendFormat(" AND [RelationID]='{0}' ", relationID);
            var result = ZentCloud.ZCBLLEngine.BLLBase.GetSingle(sbSql.ToString());
            if (result != null)
            {
                return double.Parse(result.ToString());
            }
            return 0;

        }

        /// <summary>
        /// 用户获得积分次数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetUserScoreNum(string userId, string type, bool inNowDay = true, string relationID = null)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" [ScoreType]='{0}' ", type);
            sbSql.AppendFormat(" AND [UserID]='{0}' ", userId);
            if (inNowDay) sbSql.AppendFormat(" AND datediff(day,[AddTime],getdate())=0 ");
            if (!string.IsNullOrWhiteSpace(relationID)) sbSql.AppendFormat(" AND [RelationID]='{0}' ", relationID);
            return GetCount<UserScoreDetailsInfo>(sbSql.ToString());
        }


        /// <summary>
        /// 获取积分列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="userId"></param>
        /// <param name="keyword"></param>
        /// <param name="totalCount">总数</param>
        /// <param name="balanceType">收支类型</param>
        /// <returns></returns>
        public List<Model.UserScoreDetailsInfo> GetScoreDetailsList(int pageSize, int pageIndex, string userId, string keyword, out int totalCount, int balanceType = 0)
        {
            return GetScoreDetailsList(pageSize, pageIndex, userId, keyword, out totalCount, (Enums.UserScoreBalanceType)balanceType);
        }

        /// <summary>
        /// 获取积分规则列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="userId"></param>
        /// <param name="keyword"></param>
        /// <param name="totalCount">总数</param>
        /// <param name="balanceType">收支类型</param>
        /// <param name="type">积分类型</param>
        /// <returns></returns>
        public List<Model.UserScoreDetailsInfo> GetScoreDetailsList(int pageSize, int pageIndex, string userId, string keyword, out int totalCount, Enums.UserScoreBalanceType balanceType = Enums.UserScoreBalanceType.All,
            string type = "", string scoreEvent = "", string relationId = "", string startTime = "", string stopTime = "")
        {
            totalCount = GetScoreDetailCount(WebsiteOwner, balanceType, userId, keyword, type, scoreEvent, relationId, startTime, stopTime);
            return GetScoreDetaiList(pageSize, pageIndex, WebsiteOwner, balanceType, userId, keyword, type, scoreEvent, relationId, startTime, stopTime);
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="balanceType"></param>
        /// <param name="userId"></param>
        /// <param name="keyword"></param>
        /// <param name="type"></param>
        /// <param name="scoreEvent"></param>
        /// <param name="relationId"></param>
        /// <returns></returns>
        public List<Model.UserScoreDetailsInfo> GetScoreDetaiList(int rows, int page, string websiteOwner, Enums.UserScoreBalanceType balanceType,
            string userId, string keyword, string type = "", string scoreEvent = "", string relationId = "", string startTime = "", string stopTime = "")
        {
            return GetLit<Model.UserScoreDetailsInfo>(rows, page, GetScoreDetailParam(websiteOwner, balanceType, userId, keyword, type, scoreEvent, relationId, startTime, stopTime),
                " AutoID Desc ");
        }
        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="balanceType"></param>
        /// <param name="userId"></param>
        /// <param name="keyword"></param>
        /// <param name="type"></param>
        /// <param name="scoreEvent"></param>
        /// <param name="relationId"></param>
        /// <returns></returns>
        public int GetScoreDetailCount(string websiteOwner, Enums.UserScoreBalanceType balanceType, string userId, string keyword, string type = "",
            string scoreEvent = "", string relationId = "", string startTime = "", string stopTime = "")
        {
            return GetCount<Model.UserScoreDetailsInfo>(GetScoreDetailParam(websiteOwner, balanceType, userId, keyword, type, scoreEvent, relationId, startTime, stopTime));
        }
        /// <summary>
        /// 拼查询条件
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="balanceType"></param>
        /// <param name="userId"></param>
        /// <param name="keyword"></param>
        /// <param name="type"></param>
        /// <param name="scoreEvent"></param>
        /// <param name="relationId"></param>
        /// <returns></returns>
        public string GetScoreDetailParam(string websiteOwner, Enums.UserScoreBalanceType balanceType, string userId, string keyword,
            string type = "", string scoreEvent = "", string relationId = "", string startTime = "", string stopTime = "")
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendFormat(" WebSiteOwner='{0}' ", websiteOwner);
            if (!string.IsNullOrWhiteSpace(userId)) sbSQL.AppendFormat(" And UserID='{0}' ", userId);

            if (!string.IsNullOrWhiteSpace(type)) { sbSQL.AppendFormat(" And ScoreType='{0}' ", type); }
            else { sbSQL.AppendFormat(" And ScoreType!='AccountAmount' And ScoreType!='TotalAmount' "); }

            if (balanceType == Enums.UserScoreBalanceType.Income) { sbSQL.Append(" AND Score > 0 "); }
            else if (balanceType == Enums.UserScoreBalanceType.Pay) { sbSQL.Append(" AND Score < 0 "); }

            if (!string.IsNullOrWhiteSpace(scoreEvent)) sbSQL.AppendFormat(" AND ScoreEvent = '{0}' ", scoreEvent);
            if (!string.IsNullOrWhiteSpace(relationId)) sbSQL.AppendFormat(" AND RelationId = '{0}' ", relationId);
            if (!string.IsNullOrWhiteSpace(keyword)) sbSQL.AppendFormat(" AND AddNote like '%{0}%' ", keyword);
            if (!string.IsNullOrEmpty(startTime)) sbSQL.AppendFormat(" AND AddTime>='{0}' ", DateTime.Parse(startTime));
            if (!string.IsNullOrEmpty(stopTime)) sbSQL.AppendFormat(" AND AddTime<='{0}' ", DateTime.Parse(stopTime));

            return sbSQL.ToString();
        }
        /// <summary>
        /// 收藏文章列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="showHide"></param>
        /// <returns></returns>
        public System.Data.DataSet GetUserJuActivityFavoriteList(int pageSize, int pageIndex, string userId, string websiteOwner, bool showHide = true)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" WITH A AS( ");
            strWhere.AppendFormat(" SELECT ROW_NUMBER() OVER (ORDER BY A.[AutoId] DESC) NUM ");
            strWhere.AppendFormat(" ,B.[UserID],B.[ActivityName],B.[ArticleType] ");
            strWhere.AppendFormat(" ,B.[Summary],B.[Tags],A.[RelationTime] ");
            strWhere.AppendFormat(" ,B.[CommentCount],B.[PV],B.[JuActivityID] ");
            strWhere.AppendFormat(" FROM [ZCJ_CommRelationInfo] A ");
            strWhere.AppendFormat(" INNER JOIN [ZCJ_JuActivityInfo] B ON A.[MainId]=B.[JuActivityID] ");
            strWhere.AppendFormat("     AND B.[IsDelete]=0  ");
            if (!showHide) strWhere.AppendFormat("     AND B.[IsHide]=0  ");
            strWhere.AppendFormat("     AND A.[RelationType]='JuActivityFavorite' ");
            if (!string.IsNullOrWhiteSpace(websiteOwner)) strWhere.AppendFormat("     AND [WebsiteOwner]='{0}' ", websiteOwner);
            strWhere.AppendFormat(" WHERE  A.[RelationType] = 'JuActivityFavorite' AND A.[RelationId]='{0}' ", userId);
            strWhere.AppendFormat(" ) ");
            strWhere.AppendFormat(" SELECT * FROM A WHERE NUM BETWEEN ({1}-1)* {0}+1 AND {1}*{0}; ", pageSize, pageIndex);

            strWhere.AppendFormat(" SELECT COUNT(1)[TOTALCOUNT] ");
            strWhere.AppendFormat(" FROM [ZCJ_CommRelationInfo] A ");
            strWhere.AppendFormat(" INNER JOIN [ZCJ_JuActivityInfo] B ON A.[MainId]=B.[JuActivityID] ");
            strWhere.AppendFormat("     AND B.[IsDelete]=0  ");
            if (!showHide) strWhere.AppendFormat("     AND B.[IsHide]=0  ");
            strWhere.AppendFormat("     AND A.[RelationType]='JuActivityFavorite' ");
            if (!string.IsNullOrWhiteSpace(websiteOwner)) strWhere.AppendFormat("     AND [WebsiteOwner]='{0}' ", websiteOwner);
            strWhere.AppendFormat(" WHERE A.[RelationType] = 'JuActivityFavorite' AND A.[RelationId]='{0}' ", userId);
            System.Data.DataSet ds = Query(strWhere.ToString());
            return ds;
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<UserInfo> GetNewUserList(int pageSize, int pageIndex, string websiteOwner)
        {
            return GetLit<UserInfo>(pageSize, pageIndex, string.Format(" [WebsiteOwner]='{0}' ", websiteOwner), string.Format("[AutoID] desc"));
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool UpdatePassword(UserInfo userInfo)
        {
            return Update(userInfo, string.Format(" Password='{0}'", userInfo.Password), string.Format(" AutoID='{0}'", userInfo.AutoID)) > 0;
        }
        /// <summary>
        /// 编辑资料
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool UpdateUserInfo(UserInfo userInfo)
        {
            StringBuilder setPms = new StringBuilder();
            setPms.AppendFormat(" WXSex={0} ", userInfo.WXSex);
            setPms.AppendFormat(" , Gender='{0}' ", userInfo.Gender);
            if (!string.IsNullOrWhiteSpace(userInfo.Phone)) setPms.AppendFormat(" , Phone='{0}' ", userInfo.Phone);
            setPms.AppendFormat(" , ProvinceCode='{0}' ", userInfo.ProvinceCode);
            setPms.AppendFormat(" , Province='{0}' ", userInfo.Province);
            setPms.AppendFormat(" , CityCode='{0}' ", userInfo.CityCode);
            setPms.AppendFormat(" , City='{0}' ", userInfo.City);
            if (!string.IsNullOrWhiteSpace(userInfo.Company)) setPms.AppendFormat(" , Company='{0}' ", userInfo.Company);
            if (!string.IsNullOrWhiteSpace(userInfo.Postion)) setPms.AppendFormat(" , Postion='{0}' ", userInfo.Postion);
            if (!string.IsNullOrWhiteSpace(userInfo.WXHeadimgurl)) setPms.AppendFormat(" , WXHeadimgurl='{0}' ", userInfo.WXHeadimgurl);
            if (!string.IsNullOrEmpty(userInfo.TrueName)) setPms.AppendFormat(" , TrueName='{0}' ", userInfo.TrueName);
            return Update(
                    userInfo,
                    setPms.ToString(),
                    string.Format(" AutoID='{0}'", userInfo.AutoID)
                ) > 0;
        }

        public bool IsVip(UserInfo userInfo)
        {
            BLLUserExpand bllUserExpand = new BLLUserExpand();
            return bllUserExpand.ExistUserExpand(Enums.UserExpandType.UserIsVip, userInfo.UserID);
        }
        public bool IsVip(string userId)
        {
            BLLUserExpand bllUserExpand = new BLLUserExpand();
            return bllUserExpand.ExistUserExpand(Enums.UserExpandType.UserIsVip, userId);
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyword"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="tatol"></param>
        /// <param name="userType">1管理员，2普通用户，3律师，4律师申请者，5商户</param>
        /// <returns></returns>
        public List<UserInfo> GetUserList(int pageSize, int pageIndex, string keyword, string websiteOwner, out int tatol, string userType = "")
        {
            StringBuilder sbSql = new StringBuilder();

            sbSql.AppendFormat(" 1=1 ");

            if (!string.IsNullOrWhiteSpace(userType)) sbSql.AppendFormat(" AND UserType={0} ", userType);

            if (!string.IsNullOrWhiteSpace(keyword)) sbSql.AppendFormat(" AND ( UserID like '{0}%' OR TrueName like '{0}%' OR Email like '{0}%' ) ", keyword);

            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbSql.AppendFormat(" AND [WebsiteOwner]='{0}' ", websiteOwner);

            tatol = GetCount<UserInfo>(sbSql.ToString());

            return GetLit<UserInfo>(pageSize, pageIndex, sbSql.ToString(), string.Format("[AutoID] desc"));
        }
        /// <summary>
        /// 附近用户列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="userType"></param>
        /// <param name="city"></param>
        /// <param name="gender"></param>
        /// <param name="tag"></param>
        /// <param name="keyword"></param>
        /// <param name="range"></param>
        /// <param name="sort"></param>
        /// <param name="tatol"></param>
        /// <returns></returns>
        public List<UserInfo> GetRangeUserList(int pageSize, int pageIndex, string websiteOwner, string userType, string province, string city, string district
            , string lastLoginCity, string gender, string longitude, string latitude, string ex1, string ex2, string ex3, string ex4, string ex5, string keyword
            , string tag, string sort, out int tatol, int? range = 3)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" 1=1 ");
            sbSql.AppendFormat(" AND [WXNickname] IS NOT NULL ");


            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbSql.AppendFormat(" AND [WebsiteOwner]='{0}' ", websiteOwner);
            if (!string.IsNullOrWhiteSpace(userType)) sbSql.AppendFormat(" AND UserType={0} ", userType);
            if (!string.IsNullOrWhiteSpace(province)) sbSql.AppendFormat(" AND Province='{0}' ", province);
            if (!string.IsNullOrWhiteSpace(city)) sbSql.AppendFormat(" AND City='{0}' ", city);
            if (!string.IsNullOrWhiteSpace(district)) sbSql.AppendFormat(" AND District='{0}' ", district);
            if (!string.IsNullOrWhiteSpace(lastLoginCity)) sbSql.AppendFormat(" AND LastLoginCity='{0}' ", lastLoginCity);
            if (!string.IsNullOrWhiteSpace(gender)) sbSql.AppendFormat(" AND Gender='{0}' ", gender);
            if (!string.IsNullOrWhiteSpace(ex1)) sbSql.AppendFormat(" AND Ex1='{0}' ", ex1);
            if (!string.IsNullOrWhiteSpace(ex2)) sbSql.AppendFormat(" AND Ex2='{0}' ", ex2);
            if (!string.IsNullOrWhiteSpace(ex3)) sbSql.AppendFormat(" AND Ex3='{0}' ", ex3);
            if (!string.IsNullOrWhiteSpace(ex4)) sbSql.AppendFormat(" AND Ex4='{0}' ", ex4);
            if (!string.IsNullOrWhiteSpace(ex5)) sbSql.AppendFormat(" AND Ex5='{0}' ", ex5);

            if (range.HasValue && !string.IsNullOrWhiteSpace(longitude) && !string.IsNullOrWhiteSpace(latitude))
            {
                sbSql.AppendFormat(" AND [LastLoginLongitude] IS NOT NULL ");
                sbSql.AppendFormat(" AND dbo.fnGetDistance({0},{1},[LastLoginLongitude],[LastLoginLatitude])<{2} "
                    , longitude, latitude, range);

            }
            if (!string.IsNullOrEmpty(tag))
            {

                string[] tagNameArray = tag.Split(',');
                sbSql.AppendFormat(" AND( ");
                for (int i = 0; i < tagNameArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(tagNameArray[i]))
                    {
                        if (i > 0)
                        {
                            sbSql.AppendFormat(" OR TagName like '%{0}%' ", tagNameArray[i]);
                        }
                        else
                        {
                            sbSql.AppendFormat(" TagName like '%{0}%' ", tagNameArray[i]);
                        }

                    }
                }
                sbSql.AppendFormat(") ");
            }
            if (!string.IsNullOrWhiteSpace(keyword)) sbSql.AppendFormat(" AND ( UserID like '{0}%' OR TrueName like '{0}%' OR Email like '{0}%' ) ", keyword);


            string order1 = "[LastLoginDate] desc,";
            string order2 = "";
            if (range.HasValue && !string.IsNullOrWhiteSpace(longitude) && !string.IsNullOrWhiteSpace(latitude))
            {
                order2 = string.Format("dbo.fnGetDistance({0},{1},[LastLoginLongitude],[LastLoginLatitude]) asc,", longitude, latitude);
            }

            string order = order1 + order2;
            switch (sort)
            {
                case "1":
                    order = order2 + order1;
                    break;
            }
            order = order.TrimEnd(',');

            tatol = GetCount<UserInfo>(sbSql.ToString());

            string fieldStr = "*,-1 [Distance]";
            if (range.HasValue && !string.IsNullOrWhiteSpace(longitude) && !string.IsNullOrWhiteSpace(latitude))
            {
                fieldStr = string.Format("*,dbo.fnGetDistance({0},{1},[LastLoginLongitude],[LastLoginLatitude]) [Distance]", longitude, latitude);
            }
            return GetColList<UserInfo>(pageSize, pageIndex, sbSql.ToString(), order
                , fieldStr);
        }
        /// <summary>
        /// 获取用户类型
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        public string GetUserTypeName(int userType)
        {
            string res = "";
            switch (userType)
            {
                case 1:
                    res = "管理员";
                    break;
                case 2:
                case 4:
                    res = "普通用户";
                    break;
                case 3:
                    res = "律师";
                    break;
                default:
                    break;
            }
            return res;
        }
        /// <summary>
        /// 修改登录信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool UpdateLoginInfo(UserInfo userInfo)
        {
            ToLog(" UpdateLoginInfo ");

            StringBuilder setPms = new StringBuilder();
            setPms.AppendFormat(" LastLoginIP='{0}' ", ZentCloud.Common.MySpider.GetClientIP());
            setPms.AppendFormat(" , LastLoginDate='{0}' ", DateTime.Now);
            setPms.AppendFormat(" , LoginTotalCount='{0}' ", userInfo.LoginTotalCount + 1);

            ToLog(" UpdateLoginInfo setPms: " + setPms.ToString());

            ToLog(" UpdateLoginInfo AutoID: " + userInfo.AutoID);

            var result = Update(
                    userInfo,
                    setPms.ToString(),
                    string.Format(" AutoID='{0}'", userInfo.AutoID)
                ) > 0;



            ToLog(" UpdateLoginInfo result: " + result);

            return result;
        }

        /// <summary>
        /// 修改用户类型
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UpdateUserType(string userId, int type)
        {
            StringBuilder setPms = new StringBuilder();
            setPms.AppendFormat(" UserType={0} ", type);
            return Update(
                    new UserInfo(),
                    setPms.ToString(),
                    string.Format(" UserID='{0}'", userId)
                ) > 0;
        }

        /// <summary>
        /// 修改用户类型
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UpdateUserType(int autoId, int type)
        {
            StringBuilder setPms = new StringBuilder();
            setPms.AppendFormat(" UserType={0} ", type);
            return Update(
                    new UserInfo(),
                    setPms.ToString(),
                    string.Format(" AutoID={0}", autoId)
                ) > 0;
        }

        /// <summary>
        /// 注册用户 -根据手机
        /// </summary>
        /// <returns></returns>
        public bool RegByPhone(string phone, string pwd, string pwdConfirm, out string msg, string nickName = "", string currWXOpenId = "", string viewType = "")
        {
            if (GetUserInfoByPhone(phone) != null)
            {
                msg = "此手机号已被注册，请输入别的手机号";
                return false;
            }
            if (string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(pwdConfirm))
            {
                msg = "密码必填";
                return false;
            }

            if (pwd != pwdConfirm)
            {
                msg = "密码不一致";
                return false;
            }

            UserInfo userInfo = new UserInfo();
            userInfo.UserID = string.Format("Phone{0}", Guid.NewGuid().ToString());
            userInfo.Phone = phone;
            userInfo.Password = pwd;
            userInfo.UserType = 2;
            userInfo.WebsiteOwner = WebsiteOwner;
            userInfo.Regtime = DateTime.Now;
            userInfo.WXScope = "snsapi_base";
            userInfo.WXNickname = nickName;
            userInfo.TrueName = nickName;
            userInfo.RegIP = ZentCloud.Common.MySpider.GetClientIP();
            userInfo.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
            userInfo.LastLoginDate = DateTime.Now;
            userInfo.LoginTotalCount = 1;
            if (!string.IsNullOrWhiteSpace(currWXOpenId)) userInfo.WXOpenId = currWXOpenId;
            if (!string.IsNullOrEmpty(viewType)) userInfo.ViewType = int.Parse(viewType);
            if (Add(userInfo))
            {
                msg = "注册成功";
                return true;
            }
            else
            {
                msg = "注册失败";
                return false;
            }
        }

        public bool UpdateUserWxOpenId(string userId, string openId, string websiteOwner)
        {
            if (GetUserInfoByOpenId(openId, websiteOwner) == null)
            {
                return Update(new UserInfo(), string.Format(" WXOpenId='{0}' ", openId), string.Format(" UserId = '{0}' And WebsiteOwner='{1}' ", userId, websiteOwner)) > 0;
            }
            return false;
        }


        /// <summary>
        /// 记录信用消耗
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="nAcount"></param>
        /// <param name="rmk"></param>
        /// <returns></returns>
        public bool AddUserCreditAcountDetails(string userId, string type, string websiteOwner, decimal? nAcount, string rmk = "")
        {
            BLLDefineCreditAcount bllDefine = new BLLDefineCreditAcount();
            DefineCreditAcount defineInfo = bllDefine.GetDefine(type, websiteOwner);
            //插入明细
            if (defineInfo == null) return false;
            decimal acount = nAcount.HasValue ? nAcount.Value : defineInfo.CreditAcount;
            if (acount == 0) return false;

            UserInfo currUser = GetUserInfo(userId);
            if (currUser == null) return false;

            if (defineInfo.DayLimit > 0)
            {
                decimal nTotal = GetUserDayCreditAcountSUM(userId, type) + acount;
                if (defineInfo.DayLimit < nTotal) return false;
            }

            //更新用户字段
            UserCreditAcountDetails creditAcountModel = new UserCreditAcountDetails();


            if (string.IsNullOrWhiteSpace(rmk))
            {
                creditAcountModel.AddNote = defineInfo.Description;
            }
            else
            {
                creditAcountModel.AddNote = rmk;
            }
            creditAcountModel.AddTime = DateTime.Now;
            creditAcountModel.CreditAcount = acount;
            creditAcountModel.UserID = userId;
            creditAcountModel.Type = defineInfo.Type;

            //计算剩余信用金
            currUser.CreditAcount = currUser.CreditAcount + acount;
            creditAcountModel.UserCreditAcount = currUser.CreditAcount;

            ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                if (Update(currUser, tran)
                    && Add(creditAcountModel, tran))
                {
                    tran.Commit();
                    return true;
                }
                else
                {
                    tran.Rollback();
                    return false;
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// 用户当日某类积分
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public decimal GetUserDayCreditAcountSUM(string userId, string type)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" [Type]='{0}' ", type);
            sbSql.AppendFormat(" AND [UserID]='{0}' ", userId);
            sbSql.AppendFormat(" AND datediff(day,[AddTime],'{0}')=0 ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            List<UserCreditAcountDetails> list = GetList<UserCreditAcountDetails>(sbSql.ToString());
            return list.Sum(p => p.CreditAcount);
        }
        /// <summary>
        /// 获取消费信用金列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        /// <param name="dTime"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<UserCreditAcountDetails> GetUserCreditAcountDetailsByMonth(int pageIndex, int pageSize, string userId, DateTime dTime, out int totalCount)
        {
            StringBuilder sbWhere = new StringBuilder("1=1 ");
            sbWhere.AppendFormat(" AND UserID='{0}' ", userId);
            sbWhere.AppendFormat(" AND [AddTime]>= '{0}'", dTime.ToString("yyyy-MM-01 00:00:00"));
            sbWhere.AppendFormat(" AND [AddTime]< '{0}'", dTime.AddMonths(1).ToString("yyyy-MM-01 00:00:00"));
            totalCount = GetCount<UserCreditAcountDetails>(sbWhere.ToString());
            return GetLit<UserCreditAcountDetails>(pageSize, pageIndex, sbWhere.ToString(), " AddTime Desc");
        }
        /// <summary>
        /// 查询账户列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="trueName"></param>
        /// <returns></returns>
        public List<UserInfo> GetSubAccountList(string websiteOwner, string userId, string trueName, string isDisable = "")
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("WebsiteOwner='{0}' And IsSubAccount='1'", websiteOwner);
            if (!string.IsNullOrEmpty(isDisable))
            {
                sbWhere.AppendFormat(" AND IsDisable={0}", int.Parse(isDisable));
            }
            List<UserInfo> List = GetList<UserInfo>(sbWhere.ToString());
            if (!string.IsNullOrWhiteSpace(userId))
                List = List.Where(p => p.UserID.Contains(userId)).ToList();
            if (!string.IsNullOrWhiteSpace(trueName))
                List = List.Where(p => p.TrueName.Contains(trueName)).ToList();
            return List;
        }

        /// <summary>
        /// 拼接查询语句
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="trueName"></param>
        /// <param name="phone"></param>
        /// <param name="company"></param>
        /// <param name="postion"></param>
        /// <param name="memberApplyStatus"></param>
        /// <returns></returns>
        private string GetMemberApplyParam(string websiteOwner, string keyword, string memberApplyStatus)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("WebsiteOwner='{0}'", websiteOwner);
            sbWhere.AppendFormat(" AND MemberApplyStatus>0 ");//只查申请的
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sbWhere.AppendFormat(" AND ( TrueName like '%{0}%' Or Phone like '%{0}%' Or Company like '%{0}%' Or Postion like '%{0}%') ", keyword);
            }
            if (!string.IsNullOrWhiteSpace(memberApplyStatus))
            {
                sbWhere.AppendFormat(" AND memberApplyStatus={0} ", memberApplyStatus);
            }

            return sbWhere.ToString();
        }
        /// <summary>
        /// 查询会员申请列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="trueName"></param>
        /// <param name="phone"></param>
        /// <param name="company"></param>
        /// <param name="postion"></param>
        /// <param name="memberApplyStatus"></param>
        /// <returns></returns>
        public List<UserInfo> GetMemberApplyList(int rows, int page, string websiteOwner, string keyword, string memberApplyStatus)
        {
            return GetLit<UserInfo>(rows, page, GetMemberApplyParam(websiteOwner, keyword, memberApplyStatus), "MemberApplyTime Desc");
        }
        /// <summary>
        /// 查询会员申请数
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="keyword"></param>
        /// <param name="memberApplyStatus"></param>
        /// <returns></returns>
        public int GetMemberApplyCount(string websiteOwner, string keyword, string memberApplyStatus)
        {
            return GetCount<UserInfo>(GetMemberApplyParam(websiteOwner, keyword, memberApplyStatus));
        }

        /// <summary>
        /// 获取有效新增会员
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="memberStandard"></param>
        /// <param name="memberStandardFields"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int GetMemberCount(string websiteOwner, int memberStandard, string memberStandardFields, string startDate, string endDate)
        {
            StringBuilder sbWhere = new StringBuilder();
            List<string> memberStandardFieldList = new List<string>();
            if (!string.IsNullOrWhiteSpace(memberStandardFields))
            {
                memberStandardFieldList = memberStandardFields.Split(',').Where(p => p.Trim().Equals("")).ToList();
            }
            sbWhere.AppendFormat("WebsiteOwner='{0}'", websiteOwner);
            sbWhere.AppendFormat(" AND AccessLevel > 0");
            if (memberStandard == 1)
            {
                sbWhere.AppendFormat(" AND Phone > '' ");
                sbWhere.AppendFormat(" AND IsPhoneVerify = 1 ");
            }
            else if (memberStandard == 2 || memberStandard == 3)
            {
                sbWhere.AppendFormat(" AND IsPhoneVerify = 1 ");
                if (memberStandard == 3)
                    sbWhere.AppendFormat(" AND MemberApplyStatus = 9 ");

                foreach (string field in memberStandardFieldList)
                {
                    sbWhere.AppendFormat(" AND {0} > '' ", field);
                }
            }
            sbWhere.AppendFormat(" AND MemberStartTime >= '{0}' ", DateTime.Parse(startDate).ToString("yyyy-MM-dd"));
            sbWhere.AppendFormat(" AND MemberStartTime < '{0}' ", DateTime.Parse(endDate).AddDays(1).ToString("yyyy-MM-dd"));
            return GetCount<UserInfo>(sbWhere.ToString());
        }


        /// <summary>
        /// 关键字检查
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public bool FilterSql(string keyWord)
        {
            List<string> list = new List<string>();
            list.Add("'");
            list.Add("‘");
            list.Add("\"");
            list.Add("insert");
            list.Add("update");
            list.Add("delete");
            list.Add("drop");
            list.Add("truncate");
            list.Add("select");
            list.Add("exec");
            list.Add("varchar");
            list.Add("creat");
            list.Add("declare");
            list.Add("cursor");
            list.Add("begin");
            list.Add("open");
            list.Add("<--");
            list.Add("-->");
            foreach (string item in list)
            {
                if (keyWord.ToUpper().IndexOf(item.ToUpper()) != -1)
                {
                    return false;
                }
            }
            return true;

        }

        /// <summary>
        /// 更新账户余额
        /// </summary>
        /// <param name="autoId"></param>
        /// <param name="amount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool UpdateAccountAmount(string operaterUserId, string autoId, decimal amount, string addNote, out string msg)
        {
            BLLLog bllLog = new BLLLog();
            msg = "";
            UserInfo userInfo = GetUserInfoByAutoID(int.Parse(autoId));
            if (userInfo == null || (userInfo.WebsiteOwner != WebsiteOwner))
            {
                msg = "用户无效";
                return false;
            }
            decimal oldAmount = userInfo.AccountAmount;// 100 -1000
            userInfo.AccountAmount += amount;
            if (userInfo.AccountAmount < 0)
            {
                amount = -oldAmount;
                userInfo.AccountAmount = 0;

            }
            string str = string.Empty;
            if (amount > 0)
            {
                str = "+";
            }

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new BLLTransaction();
            try
            {
                #region 宏巍
                var websiteInfo = GetWebsiteInfoModelFromDataBase();
                if (websiteInfo.IsUnionHongware == 1)
                {
                    Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(websiteInfo.WebsiteOwner);
                    if (!hongWareClient.UpdateMemberBlance(userInfo.Phone, userInfo.WXOpenId, (float)amount))
                    {
                        msg = "更新宏巍余额失败";
                        tran.Rollback();
                        return false;
                    }

                }
                #endregion
                if (Update(userInfo, string.Format("AccountAmount={0}", userInfo.AccountAmount), string.Format(" AutoId={0}", userInfo.AutoID), tran) <= 0)
                {
                    msg = "充值失败";
                    tran.Rollback();
                    return false;

                }
                //UserCreditAcountDetails record = new UserCreditAcountDetails();
                //record.WebsiteOwner = WebsiteOwner;
                //record.Operator = operaterUserId;
                //record.UserID = userInfo.UserID;
                //record.CreditAcount = amount;
                //record.SysType = "AccountAmount";
                //record.AddTime = DateTime.Now;
                //record.AddNote = "余额变动" + amount;


                UserScoreDetailsInfo record = new UserScoreDetailsInfo();
                record.AddTime = DateTime.Now;
                record.Score = (double)amount;
                record.TotalScore = (double)userInfo.AccountAmount;
                //record.ScoreType = "UpdateAccountAmount";
                record.UserID = userInfo.UserID;

                record.AddNote = string.IsNullOrWhiteSpace(addNote) ? "余额变动" + amount : addNote;

                //scoreRecord.RelationID = orderInfo.OrderID;
                record.WebSiteOwner = WebsiteOwner;
                record.ScoreType = "AccountAmount";
                if (!(Add(record)))
                {
                    msg = "插入记录失败";
                    tran.Rollback();
                    return false;
                }
                bllLog.Add(BLLJIMP.Enums.EnumLogType.Member, BLLJIMP.Enums.EnumLogTypeAction.Update, bllLog.GetCurrUserID(), "[" + bllLog.GetCurrUserID() + "]修改了用户[" + userInfo.UserID + "]余额[" + str + "" + amount + "]");
                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                tran.Rollback();
            }
            return false;

        }


        /// <summary>
        ///信用金 余额记录
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="userId">用户</param>
        /// <param name="sysType">系统类型,默认账户余额</param>
        /// <returns></returns>
        public List<UserCreditAcountDetails> GetUserCreditAcountDetailsList(int pageIndex, int pageSize, string userId, out int totalCount, string sysType = "AccountAmount")
        {
            StringBuilder sbWhere = new StringBuilder(string.Format(" SysType='{0}' And WebsiteOwner='{1}'", sysType, WebsiteOwner));
            if (!string.IsNullOrEmpty(userId))
            {
                sbWhere.AppendFormat(" And UserId='{0}'", userId);
            }
            totalCount = GetCount<UserCreditAcountDetails>(sbWhere.ToString());
            return GetLit<UserCreditAcountDetails>(pageSize, pageIndex, sbWhere.ToString(), " AutoId DESC");



        }

        /// <summary>
        /// 获取我的粉丝列表
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        public List<UserInfo> GetFansInfoList(int pageIndex, int pageSize, string articleId, string userId, out int totalCount)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" WebsiteOwner='{0}' ", WebsiteOwner);
            if (!string.IsNullOrEmpty(articleId))
            {
                strWhere.AppendFormat(" AND ArticleId='{0}'", articleId);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                strWhere.AppendFormat(" AND DistributionOwner='{0}' ", userId);
            }
            totalCount = GetCount<UserInfo>(strWhere.ToString());
            return GetLit<UserInfo>(pageSize, pageIndex, strWhere.ToString());
        }
        /// <summary>
        /// 获取全部粉丝列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="articleId"></param>
        /// <param name="userId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<UserInfo> GetFansInfoLists(int pageIndex, int pageSize, string articleId, string userId, out int totalCount)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" WebsiteOwner='{0}' ", WebsiteOwner);
            if (!string.IsNullOrEmpty(articleId))
            {
                strWhere.AppendFormat(" AND ArticleId='{0}'", articleId);
            }
            if (string.IsNullOrEmpty(userId))
            {
                strWhere.AppendFormat(" AND DistributionOwner!='' ");
            }
            totalCount = GetCount<UserInfo>(strWhere.ToString());
            return GetLit<UserInfo>(pageSize, pageIndex, strWhere.ToString());
        }
        /// <summary>
        /// 检查是否分销员
        /// </summary>
        /// <returns></returns>
        public bool IsDistributionMember(UserInfo userInfo, bool mustHavePay = false)
        {
            if (userInfo == null || userInfo.AutoID == 0) return false;
            if (userInfo.UserID.Contains("_FirstLevelDistribution_"))
            {
                return true;//渠道二维码 默认是分销员
            }

            BLLDistribution bllDist = new BLLDistribution();
            BLLWeixin test = new BLLWeixin();
            bool result = false;
            //test.ToBLLWeixinLog("IsDistributionMember start");
            try
            {
                WebsiteInfo websiteModel = GetWebsiteInfoModelFromDataBase(userInfo.WebsiteOwner);

                //颂和只有memberlevel大于0才是代言人

                if (userInfo.MemberLevel < 1 && userInfo.WebsiteOwner == "songhe")
                {
                    return false;
                }

                //test.ToBLLWeixinLog("websiteModel:" + JSONHelper.ObjectToJson(websiteModel));

                bool haveParent = true;//有上级
                bool havePay = true;//有支付的订单
                bool haveSuccessOrder = true;//有交易成功的订单
                if (websiteModel.DistributionMemberStandardsHaveParent == 1)
                {
                    haveParent = !string.IsNullOrEmpty(userInfo.DistributionOwner) ? true : false;

                }
                var distributionMemberStandardsHavePay = websiteModel.DistributionMemberStandardsHavePay == 1 ? true : false;

                if (mustHavePay)
                {
                    distributionMemberStandardsHavePay = true;
                }

                if (distributionMemberStandardsHavePay)
                {
                    int payInt = GetCount<WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}' AND OrderUserID='{1}' AND PaymentStatus=1 And Status!='已取消' And IsRefund!=1", userInfo.WebsiteOwner, userInfo.UserID));
                    havePay = payInt > 0;
                    //test.ToBLLWeixinLog(string.Format(" WebsiteOwner='{0}' AND OrderUserID='{1}' AND PaymentStatus=1 ", WebsiteOwner, userInfo.UserID));
                    //test.ToBLLWeixinLog(string.Format(" payInt='{0}' ", payInt));
                    //test.ToBLLWeixinLog(string.Format("userid:{0} havePay:{1} username:{2}", userInfo.AutoID, havePay, userInfo.UserID));
                }
                if (websiteModel.DistributionMemberStandardsHaveSuccessOrder == 1)
                {
                    haveSuccessOrder = GetCount<WXMallOrderInfo>(string.Format(" Websiteowner='{0}' AND  OrderUserID='{1}' AND Status='交易成功' ", userInfo.WebsiteOwner, userInfo.UserID)) > 0;
                }

                result = haveParent && havePay && haveSuccessOrder;

                //如果是系统自定义的，则查询是否能找到对应等级，找不到则不是分销员
                if (websiteModel.DistributionGetWay == 1 && result)
                {
                    var userLevel = bllDist.QueryUserLevel(userInfo.WebsiteOwner, "DistributionOnLine", userInfo.MemberLevel.ToString());
                    result = userLevel != null;
                }

                //test.ToBLLWeixinLog(string.Format("userid:{3} haveParent:{0} && havePay:{1} && haveSuccessOrder:{2}", haveParent, havePay, haveSuccessOrder, userInfo.AutoID));
            }
            catch (Exception ex)
            {
                test.ToBLLWeixinLog("IsDistributionMember EX: " + ex.Message);
            }
            //test.ToBLLWeixinLog("IsDistributionMember ennd：" + (result ? "T" : "F"));
            return result;
        }

        /// <summary>
        /// 生成登录cookie
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        public HttpCookie CreateLoginCookie(string userId, string wxOpenId, string wxNickname)
        {
            Model.Other.WxAutoLoginToken wxAutoLoginToken = new Model.Other.WxAutoLoginToken() { IsUAuth = 0 };
            wxAutoLoginToken.Uid = userId;
            wxAutoLoginToken.Oid = wxOpenId;
            wxAutoLoginToken.IsUAuth = string.IsNullOrWhiteSpace(wxNickname) ? 0 : 1;

            string cookieValue = "ltk:" + ZentCloud.Common.DEncrypt.ZCEncrypt(userId);
            HttpCookie loginCookie = new HttpCookie(ZentCloud.Common.SessionKey.LoginCookie, cookieValue);
            loginCookie.Expires = DateTime.Now.AddDays(30);
            RedisHelper.RedisHelper.StringSetSerialize(cookieValue, wxAutoLoginToken, new TimeSpan(30, 0, 0, 0));
            return loginCookie;
        }
        /// <summary>
        /// 查出当前设备的cookie是否有登录用户
        /// </summary>
        /// <returns></returns>
        public string GetUserInfoByLoginCookie()
        {
            HttpCookie loginCookie = HttpContext.Current.Request.Cookies[ZentCloud.Common.SessionKey.LoginCookie];
            if (loginCookie == null || string.IsNullOrWhiteSpace(loginCookie.Value)) return null;

            Model.Other.WxAutoLoginToken wxAutoLoginToken = RedisHelper.RedisHelper.StringGet<Model.Other.WxAutoLoginToken>(loginCookie.Value);
            if (wxAutoLoginToken != null && wxAutoLoginToken.IsUAuth == 1)
            {
                return wxAutoLoginToken.Uid;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 会员清理
        /// </summary>
        /// <param name="websiteOwner"></param>
        public int CleanUser(string websiteOwner)
        {
            BLLWeixin bllWeixin = new BLLWeixin();
            int pageIndex = 1;
            int pageSize = 100;
            int totalCount = GetCount<UserInfo>(string.Format(" WebsiteOwner='{0}' And WXOPENID!=''", websiteOwner));
            int pageCount = GetTotalPage(totalCount, pageSize);
            int successCount = 0;
            //string accessToken = bllWeixin.GetAccessToken(websiteOwner);
            for (int i = 1; i <= pageCount; i++)
            {
                var userList = GetLit<UserInfo>(pageSize, pageIndex, string.Format(" WebsiteOwner='{0}' And WXOPENID!=''", websiteOwner));
                foreach (var user in userList)
                {

                    var wxUserInfo = bllWeixin.GetWeixinUserInfo(bllWeixin.GetAccessToken(websiteOwner), user.WXOpenId);
                    if (wxUserInfo != null)
                    {
                        if (wxUserInfo.subscribe == 0)//未关注
                        {
                            if (user.IsWeixinFollower == 1)
                            {
                                if (Update(user, string.Format(" IsWeixinFollower=0"), string.Format(" AutoId={0}", user.AutoID)) > 0)
                                {
                                    successCount++;
                                }
                            }



                        }
                        else//已关注
                        {
                            if (user.IsWeixinFollower == 0)
                            {

                                if (Update(user, string.Format(" IsWeixinFollower=1,WXNickname='{0}',WXHeadimgurl='{1}'", wxUserInfo.nickname, wxUserInfo.headimgurl), string.Format(" AutoId={0}", user.AutoID)) > 0)
                                {
                                    successCount++;
                                }
                            }
                        }
                    }


                }
                pageIndex++;

            }
            return successCount;


        }

        /// <summary>
        /// 是否是分销渠道
        /// </summary>
        /// <returns></returns>
        public bool IsDistributionChannel(UserInfo userInfo)
        {

            if (userInfo.PermissionGroupID.HasValue)
            {

                ZentCloud.BLLPermission.Model.PermissionGroupInfo perGroupInfo = Get<ZentCloud.BLLPermission.Model.PermissionGroupInfo>(string.Format(" GroupID={0}", userInfo.PermissionGroupID));
                if (perGroupInfo != null && perGroupInfo.GroupType == 4)
                {
                    return true;
                }
            }
            return false;

        }
        /// <summary>
        /// 更新用户审核状态
        /// </summary>
        /// <param name="autoid"></param>
        /// <param name="memberApplyStatus"></param>
        /// <returns></returns>
        public bool UpdateUserMemberApplyStatus(int autoid, int memberApplyStatus)
        {
            return Update(new UserInfo(), string.Format(" MemberApplyStatus={0} ", memberApplyStatus), string.Format(" AutoID={0} ", autoid)) > 0;
        }
        ///// <summary>
        ///// 是否绑定宏巍
        ///// </summary>
        ///// <param name="userInfo"></param>
        ///// <returns></returns>
        //public bool IsBindHongWare(UserInfo userInfo)
        //{

        //    WebsiteInfo websiteInfo = Get<WebsiteInfo>(string.Format(" WebsiteOwner='{0}'", userInfo.WebsiteOwner));
        //    if (websiteInfo != null)
        //    {

        //        if (websiteInfo.IsUnionHongware == 1)
        //        {
        //            if (userInfo.IsPhoneVerify == 1)
        //            {
        //                return true;
        //            }
        //            var memberInfo = hongWareClient.GetMemberInfo(userInfo.WXOpenId);
        //            if (memberInfo.member != null)
        //            {
        //                Update(userInfo, string.Format(" IsPhoneVerify=1,Phone='{0}',TotalScore={1},AccountAmount={2}", memberInfo.member.mobile, memberInfo.member.point, memberInfo.member.balance), string.Format(" AutoId={0}", userInfo.AutoID));
        //                return true;

        //            }



        //        }




        //    }
        //    return false;

        //}
        /// <summary>
        /// 是否绑定宏巍
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <param name="websiteInfo">站点信息</param>
        /// <returns></returns>
        public bool IsBindHongWare(UserInfo userInfo, WebsiteInfo websiteInfo)
        {

            if (websiteInfo != null)
            {

                if (websiteInfo.IsUnionHongware == 1)
                {
                    //if (userInfo.IsPhoneVerify == 1)
                    //{
                    //    return true;
                    //}
                    Open.HongWareSDK.Client hongWareClient = new Open.HongWareSDK.Client(websiteInfo.WebsiteOwner);
                    var memberInfo = hongWareClient.GetMemberInfo(userInfo.WXOpenId);
                    if (memberInfo.member != null)
                    {
                        Update(userInfo, string.Format(" IsPhoneVerify=1,Phone='{0}',TotalScore={1},AccountAmount={2}", memberInfo.member.mobile, memberInfo.member.point, memberInfo.member.balance), string.Format(" AutoId={0}", userInfo.AutoID));
                        return true;
                    }

                }

            }
            return false;

        }
        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="openId"></param>
        /// <param name="showName"></param>
        /// <returns></returns>
        public UserInfo CreateNewUser(string websiteOwner, string openId, string showName = "")
        {
            UserInfo userInfo = new UserInfo();
            userInfo.UserID = string.Format("WXUser{0}", Guid.NewGuid().ToString());//Guid
            userInfo.Password = ZentCloud.Common.Rand.Str_char(12);
            userInfo.UserType = 2;
            userInfo.WebsiteOwner = websiteOwner;
            userInfo.Regtime = DateTime.Now;
            userInfo.WXOpenId = openId;
            userInfo.RegIP = ZentCloud.Common.MySpider.GetClientIP();
            userInfo.LastLoginIP = ZentCloud.Common.MySpider.GetClientIP();
            userInfo.LastLoginDate = DateTime.Now;
            userInfo.LoginTotalCount = 1;
            userInfo.TrueName = showName;
            if (Add(userInfo))
            {
                userInfo = GetUserInfo(userInfo.UserID, userInfo.WebsiteOwner);
                return userInfo;
            }

            return null;

        }
        /// <summary>
        /// 查询用户 
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="phone">手机</param>
        /// <param name="nickname">昵称</param>
        /// <param name="websiteOwner">站点</param>
        /// <param name="userType">用户类型</param>
        /// <returns></returns>
        public List<UserInfo> FindList(out int total, int rows, int page, string phone, string nickname, string websiteOwner,
            string userType = "2",
            string colName = "AutoID,UserID,TrueName,WXNickname,WXHeadimgurl,UserType,Phone,Avatar,ViewType",
            string ids = "")
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" IsDisable=0 ");//未被禁用
            sbWhere.AppendFormat(" And WebsiteOwner='{0}' ", websiteOwner);//站点

            if (!string.IsNullOrWhiteSpace(ids))
                sbWhere.AppendFormat(" And AutoID In ({0})", ids);//ids

            if (!string.IsNullOrWhiteSpace(userType)) sbWhere.AppendFormat(" And UserType In ({0}) ", userType);//用户类型
            if (!string.IsNullOrWhiteSpace(phone))
                sbWhere.AppendFormat(" And Phone='{0}' ", phone);//手机
            if (!string.IsNullOrWhiteSpace(nickname))
                sbWhere.AppendFormat(" And ( TrueName like '%{0}%' or WXNickname='%{0}%')", nickname);//昵称
            total = GetCount<UserInfo>(sbWhere.ToString());
            return GetColList<UserInfo>(rows, page, sbWhere.ToString(), colName);
        }

        /// <summary>
        /// 查询站点总积分（淘股币）
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public double GetSumScore(string websiteOwner)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" Select Sum(TotalScore) From  ZCJ_UserInfo ");
            sbSql.AppendFormat(" Where WebsiteOwner='{0}' ", websiteOwner);
            object result = GetSingle(sbSql.ToString(), "UserScoreDetailsInfo");
            return Convert.ToDouble(result);
        }
        //根据昵称查用户
        public List<UserInfo> GetUsersByLikeName(string likeName, string websiteOwner)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            if (!string.IsNullOrWhiteSpace(likeName))
                sbWhere.AppendFormat(" And ( TrueName like '{0}%' or WXNickname='{0}%')", likeName);//昵称
            return GetList<UserInfo>(sbWhere.ToString());
        }
        /// <summary>
        /// 获取推广人
        /// </summary>
        /// <param name="spreadid">推广编号（13位为手机号）</param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public UserInfo GetSpreadUser(string spreadid, string websiteOwner, int? minMemberLevel = null, bool likeName = false)
        {
            UserInfo mu = null;
            int id = 0;
            StringBuilder sbWhere = new StringBuilder();
            if (int.TryParse(spreadid, out id))
            {
                sbWhere = new StringBuilder();
                sbWhere.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
                sbWhere.AppendFormat(" And AutoID={0} ", id);
                if (minMemberLevel.HasValue) sbWhere.AppendFormat(" And MemberLevel>={0} ", minMemberLevel);
                mu = Get<UserInfo>(sbWhere.ToString());
            }
            if (mu != null) return mu;
            sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            sbWhere.AppendFormat(" And Phone='{0}' ", spreadid);
            if (minMemberLevel.HasValue) sbWhere.AppendFormat(" And MemberLevel>={0} ", minMemberLevel);
            mu = Get<UserInfo>(sbWhere.ToString());
            if (mu != null) return mu;

            sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            sbWhere.AppendFormat(" And UserID='{0}' ", spreadid);
            if (minMemberLevel.HasValue) sbWhere.AppendFormat(" And MemberLevel>={0} ", minMemberLevel);
            mu = Get<UserInfo>(sbWhere.ToString());
            if (mu != null) return mu;

            if (likeName)
            {
                sbWhere = new StringBuilder();
                sbWhere.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
                sbWhere.AppendFormat(" And TrueName Like '{0}%' ", spreadid);
                if (minMemberLevel.HasValue) sbWhere.AppendFormat(" And MemberLevel>={0} ", minMemberLevel);
                mu = Get<UserInfo>(sbWhere.ToString());
            }
            return mu;
        }
        public List<UserInfo> GetSpreadUsers(string member, string websiteOwner, string colName, int? minMemberLevel = null)
        {
            UserInfo mu = GetSpreadUser(member, websiteOwner, minMemberLevel);
            if (mu != null)
            {
                return new List<UserInfo>() { mu };
            }
            else
            {
                StringBuilder sbWhere = new StringBuilder();
                sbWhere.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
                sbWhere.AppendFormat(" And TrueName Like '{0}%' ", member);
                if (minMemberLevel.HasValue) sbWhere.AppendFormat(" And MemberLevel>={0} ", minMemberLevel);
                return GetColList<UserInfo>(int.MaxValue, 1, sbWhere.ToString(), colName);
            }
        }
        public string GetSpreadUserIds(string member, string websiteOwner, int? minMemberLevel = null, int? maxCount = null)
        {
            UserInfo mu = GetSpreadUser(member, websiteOwner, minMemberLevel);
            if (mu != null)
            {
                return mu.UserID;
            }
            else
            {
                StringBuilder sbWhere = new StringBuilder();
                sbWhere.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
                sbWhere.AppendFormat(" And TrueName Like '{0}%' ", member);
                if (minMemberLevel.HasValue) sbWhere.AppendFormat(" And MemberLevel>={0} ", minMemberLevel);
                int rows = maxCount.HasValue ? maxCount.Value : int.MaxValue;
                List<UserInfo> uList = GetColList<UserInfo>(rows, 1, sbWhere.ToString(), "AutoID,UserID");
                if (uList.Count > 0)
                {
                    return ZentCloud.Common.MyStringHelper.ListToStr(uList.Select(p => p.UserID).ToList(), "", ",");
                }
                else
                {
                    return "-1";
                }
            }
        }
        /// <summary>
        /// 添加积分明细 （返回ID）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="score"></param>
        /// <param name="note"></param>
        /// <param name="scoreType"></param>
        /// <param name="totalScore"></param>
        /// <param name="relationId"></param>
        /// <param name="scoreEvent"></param>
        /// <param name="openId"></param>
        /// <param name="serialNumber"></param>
        /// <param name="eventScore"></param>
        /// <param name="deductScore"></param>
        /// <param name="eventRelationUserID"></param>
        /// <returns></returns>
        public int AddScoreDetail(string userId, string websiteOwner, double score, string note, string scoreType, double totalScore, string relationId,
            string scoreEvent, string openId = "", string serialNumber = "", double? eventScore = null, double? deductScore = null,
            string eventRelationUserID = "", BLLTransaction trans = null, string ex1 = "", string ex2 = "", string ex3 = "",
            string ex4 = "", DateTime? addtime = null, long? fromId = null, string ex5 = "", int isPrint = 0)
        {
            UserScoreDetailsInfo detail = new UserScoreDetailsInfo();
            detail.WebSiteOwner = websiteOwner;
            detail.UserID = userId;
            detail.Score = score;
            detail.AddNote = note;
            detail.ScoreType = scoreType;
            detail.TotalScore = totalScore;
            detail.IsPrint = isPrint;
            if (addtime.HasValue)
            {
                detail.AddTime = addtime.Value;
            }
            else
            {
                detail.AddTime = DateTime.Now;
            }
            if (fromId.HasValue)
            {
                detail.FromId = fromId.Value;
            }
            if (!string.IsNullOrWhiteSpace(relationId)) detail.RelationID = relationId;
            if (!string.IsNullOrWhiteSpace(scoreEvent)) detail.ScoreEvent = scoreEvent;
            if (!string.IsNullOrWhiteSpace(openId)) detail.OpenId = openId;
            if (!string.IsNullOrWhiteSpace(serialNumber)) detail.SerialNumber = serialNumber;
            if (eventScore.HasValue) detail.EventScore = eventScore.Value;
            if (deductScore.HasValue) detail.DeductScore = deductScore.Value;
            if (!string.IsNullOrWhiteSpace(eventRelationUserID)) detail.RelationUserID = eventRelationUserID;
            if (!string.IsNullOrWhiteSpace(ex1)) detail.Ex1 = ex1;
            if (!string.IsNullOrWhiteSpace(ex2)) detail.Ex2 = ex2;
            if (!string.IsNullOrWhiteSpace(ex3)) detail.Ex3 = ex3;
            if (!string.IsNullOrWhiteSpace(ex4)) detail.Ex4 = ex4;
            if (!string.IsNullOrWhiteSpace(ex5)) detail.Ex5 = ex5;
            object result = AddReturnID(detail, trans);
            return Convert.ToInt32(result);
        }
        /// <summary>
        /// 获取登录会员信息
        /// </summary>
        /// <returns></returns>
        public string GetLoginUserJsonString()
        {
            return GetUserJsonString(WebsiteOwner, GetCurrUserID());
        }
        /// <summary>
        /// 获取会员信息
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserJsonString(string websiteOwner, string userId)
        {
            dynamic userInfo = GetUserJson(websiteOwner, userId);
            return JsonConvert.SerializeObject(userInfo);
        }
        /// <summary>
        /// 获取会员信息
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public dynamic GetUserJson(string websiteOwner, string userId)
        {
            BLLDistribution bll = new BLLDistribution();
            UserInfo curUser = GetUserInfo(userId, websiteOwner);
            UserLevelConfig userLevel = bll.QueryUserLevel(websiteOwner, "DistributionOnLine", curUser.MemberLevel.ToString());
            string area = !string.IsNullOrWhiteSpace(curUser.City) ? curUser.City : curUser.Province;
            JToken imgJtoken = GetLevelImg(curUser.MemberLevel.ToString());
            return new
            {
                id = curUser.AutoID,
                uid = curUser.UserID,
                name = GetUserDispalyName(curUser),
                avatar = GetUserDispalyAvatar(curUser),
                phone = curUser.Phone,
                totalscore = curUser.TotalScore,
                amount = curUser.AccountAmount,
                totalamount = curUser.TotalAmount,
                lockamount = curUser.AccountAmountEstimate,
                accumfund = curUser.AccumulationFund,
                stock = curUser.Stock,
                level = curUser.MemberLevel,
                levelerror = userLevel == null ? true : false,
                levelname = userLevel == null ? "" : userLevel.LevelString,
                levelamount = userLevel == null ? 0 : userLevel.FromHistoryScore,
                levelico = imgJtoken == null ? "" : imgJtoken["ico"].ToString(),
                levelnameimg = imgJtoken == null ? "" : imgJtoken["name_img"].ToString(),
                area = area,
                wxbind = !string.IsNullOrWhiteSpace(curUser.WXOpenId)
            };
        }

        /// <summary>
        /// 获取等级图片
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public JToken GetLevelImg(string level)
        {
            StringBuilder imgJsonString = new StringBuilder();
            imgJsonString.Append("{");
            imgJsonString.AppendFormat("l10: {0} ico: '', name_img: '/App/Wap/img/level10name.png' {1},", "{", "}");
            imgJsonString.AppendFormat("l20: {0} ico: '/App/Wap/img/level20.png', name_img: '/App/Wap/img/level20name.png' {1},", "{", "}");
            imgJsonString.AppendFormat("l21: {0} ico: '/App/Wap/img/level20.png', name_img: '/App/Wap/img/level20name.png' {1},", "{", "}");
            imgJsonString.AppendFormat("l30: {0} ico: '/App/Wap/img/level30.png', name_img: '/App/Wap/img/level30name.png' {1},", "{", "}");
            imgJsonString.AppendFormat("l31: {0} ico: '/App/Wap/img/level30.png', name_img: '/App/Wap/img/level30name.png' {1},", "{", "}");
            imgJsonString.AppendFormat("l40: {0} ico: '/App/Wap/img/level40.png', name_img: '/App/Wap/img/level40name.png' {1},", "{", "}");
            imgJsonString.AppendFormat("l50: {0} ico: '/App/Wap/img/level50.png', name_img: '/App/Wap/img/level50name.png' {1},", "{", "}");
            imgJsonString.Append("}");
            JToken imgJtoken = JToken.Parse(imgJsonString.ToString());
            return imgJtoken["l" + level];
        }
        /// <summary>
        /// 微信客服列表
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<WXKeFu> GetWeixinKefuList(string websiteOwner)
        {
            return GetList<WXKeFu>(string.Format(" WebsiteOwner='{0}'", websiteOwner));
        }
        /// <summary>
        /// 检查用户是否是客服
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool IsWeixinKefu(UserInfo userInfo)
        {

            if (string.IsNullOrEmpty(userInfo.WXOpenId))
            {
                return false;
            }
            var kefuList = GetWeixinKefuList(userInfo.WebsiteOwner);
            if (kefuList.Count > 0)
            {
                if (kefuList.Count(p => p.WeiXinOpenID == userInfo.WXOpenId) > 0)
                {
                    return true;
                }
            }

            return false;

        }

        /// <summary>
        /// 查询商户列表
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<UserInfo> GetSupplierList(string websiteOwner, int pageIndex, int pageSize, string keyWord, string userId, out int totalCount)
        {
            StringBuilder sbWhere = new StringBuilder();
            try
            {

                sbWhere.AppendFormat(" WebsiteOwner='{0}' And UserType=7", websiteOwner);
                if (!string.IsNullOrEmpty(keyWord))
                {
                    sbWhere.AppendFormat(" And (TrueName like '%{0}%' Or Company like '%{0}%')", keyWord);

                }
                if (!string.IsNullOrEmpty(userId))
                {
                    sbWhere.AppendFormat(" And UserId='{0}'", userId);

                }

                totalCount = GetCount<UserInfo>(sbWhere.ToString());
                return GetList<UserInfo>(sbWhere.ToString());
            }
            catch (Exception ex)
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"D:\log.txt", true, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(string.Format("sett:{0}", ex.ToString()));
                    sw.WriteLine(string.Format("sett:{0}", sbWhere.ToString()));
                }
                totalCount = 0;
                return new List<UserInfo>();
            }


        }


        /// <summary>
        /// 查询商户渠道列表
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<UserInfo> GetSupplierChannelList(string websiteOwner, int pageIndex, int pageSize, string keyWord, string userId, out int totalCount)
        {
            StringBuilder sbWhere = new StringBuilder();
            try
            {

                sbWhere.AppendFormat(" WebsiteOwner='{0}' And UserType=8", websiteOwner);
                if (!string.IsNullOrEmpty(keyWord))
                {
                    sbWhere.AppendFormat(" And (TrueName like '%{0}%' Or Company like '%{0}%')", keyWord);

                }
                if (!string.IsNullOrEmpty(userId))
                {
                    sbWhere.AppendFormat(" And UserId='{0}'", userId);

                }

                totalCount = GetCount<UserInfo>(sbWhere.ToString());
                return GetList<UserInfo>(sbWhere.ToString());
            }
            catch (Exception ex)
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"D:\log.txt", true, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(string.Format("sett:{0}", ex.ToString()));
                    sw.WriteLine(string.Format("sett:{0}", sbWhere.ToString()));
                }
                totalCount = 0;
                return new List<UserInfo>();
            }


        }

        /// <summary>
        /// 添加商户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passWord"></param>
        /// <param name="passWordConfirm"></param>
        /// <param name="companyName"></param>
        /// <param name="trueName"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="description"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddSupplier(string userId, string passWord, string passWordConfirm, string companyName, string trueName, string phone, string email, string description, string permissionGroupId, string headImage, string image, string ex1, string ex2,string ex3,string ex4, out string msg,string address=""
            , string province = "", string provinceCode = "", string city = "", string cityCode = "", string district = "", string districtCode = "", string backDeposit = "", string backAccount = "")
        {
            msg = "";
            if (string.IsNullOrEmpty(userId))
            {
                msg = "账号名必填";
                return false;
            }
            if (GetCount<UserInfo>(string.Format(" WebsiteOwner='{0}' And UserId='{1}'", WebsiteOwner, userId)) > 0)
            {
                msg = "账号名重复,请换账号名";
                return false;
            }
            if (GetCount<UserInfo>(string.Format(" WebsiteOwner='{0}' And Company='{1}' And UserType=7", WebsiteOwner, companyName)) > 0)
            {
                msg = "商户名称重复";
                return false;
            }
            if (!string.IsNullOrEmpty(ex2))
            {
                if (GetCount<UserInfo>(string.Format(" WebsiteOwner='{0}' And Ex2='{1}'", WebsiteOwner, ex2)) > 0)
                {
                    msg = "商户代码重复";
                    return false;
                }
            }
            if (string.IsNullOrEmpty(passWord))
            {
                msg = "密码必填";
                return false;
            }
            if (passWord != passWordConfirm)
            {
                msg = "密码不一致";
                return false;
            }
            if (string.IsNullOrEmpty(permissionGroupId))
            {
                msg = "请选择角色";
                return false;
            }
            if (permissionGroupId == "0")
            {
                msg = "请选择角色";
                return false;
            }

            ZentCloud.ZCBLLEngine.BLLTransaction tran = new BLLTransaction();

            UserInfo userInfo = new UserInfo();
            userInfo.Description = description;
            userInfo.UserID = userId;
            userInfo.Password = passWordConfirm;
            userInfo.Company = companyName;
            userInfo.TrueName = trueName;
            userInfo.Phone = phone;
            userInfo.Email = email;
            userInfo.WebsiteOwner = WebsiteOwner;
            userInfo.Regtime = DateTime.Now;
            userInfo.LastLoginDate = DateTime.Now;
            userInfo.Birthday = DateTime.Now;
            userInfo.UserType = 7;
            userInfo.PermissionGroupID = long.Parse(permissionGroupId);
            userInfo.WXHeadimgurl = headImage;
            userInfo.Images = image;
            userInfo.Ex1 = ex1;
            userInfo.Ex2 = ex2;
            userInfo.Ex3 = ex3;
            userInfo.Ex4 = ex4;
            userInfo.Address = address;
            userInfo.Province = province;
            userInfo.City = city;
            userInfo.District = district;
            userInfo.ProvinceCode = provinceCode;
            userInfo.CityCode = cityCode;
            userInfo.DistrictCode = districtCode;
            ZentCloud.BLLPermission.Model.UserPmsGroupRelationInfo userPm = new BLLPermission.Model.UserPmsGroupRelationInfo();
            userPm.UserID = userInfo.UserID;
            userPm.GroupID = (long)userInfo.PermissionGroupID;

            if (!Add(userInfo, tran))
            {
                msg = "添加用户失败";
                return false;
            }
            if (GetCount<ZentCloud.BLLPermission.Model.UserPmsGroupRelationInfo>(string.Format("UserId='{0}' And GroupID={1}", userInfo.UserID, userInfo.PermissionGroupID)) == 0)
            {

                if (!Add(userPm, tran))
                {
                    msg = "添加用户权限组失败";
                    return false;
                }
            }
             BLLUserExpand bllUserExpand=new BLLUserExpand();
             if (!bllUserExpand.AddUserExpand(BLLJIMP.Enums.UserExpandType.BankInfo, userInfo.UserID, "", backDeposit, backAccount))
             {
                 return false;
             }
       
            tran.Commit();
            return true;

        }
        /// <summary>
        /// 修改商户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="compan yName"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="description"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool UpdateSupplier(string id, string companyName, string trueName, string phone, string email, string description, string permissionGroupId, string headImage, string image, string ex1, string ex2,string ex3,string ex4, out string msg,string address=""
            , string province = "", string provinceCode = "", string city = "", string cityCode = "", string district = "", string districtCode = "", string backDeposit = "", string backAccount = "")
        {
            msg = "";
            if (string.IsNullOrEmpty(id))
            {
                msg = "id必填";
                return false;
            }
            if (GetCount<UserInfo>(string.Format(" WebsiteOwner='{0}' And Company='{1}' And AutoId!={2}", WebsiteOwner, companyName, id)) > 0)
            {
                msg = "商户名称重复";
                return false;
            }
            if (!string.IsNullOrEmpty(ex2))
            {
                if (GetCount<UserInfo>(string.Format(" WebsiteOwner='{0}' And Ex2='{1}' And AutoId!={2}", WebsiteOwner, ex2, id)) > 0)
                {
                    msg = "商户代码重复";
                    return false;
                }
            }
            if (string.IsNullOrEmpty(permissionGroupId))
            {
                msg = "请选择角色";
                return false;
            }
            if (permissionGroupId == "0")
            {
                msg = "请选择角色";
                return false;
            }
            if (string.IsNullOrEmpty(companyName))
            {
                msg = "请填写名称";
                return false;
            }
            UserInfo userInfo = GetUserInfoByAutoID(int.Parse(id));
            userInfo.Description = description;
            userInfo.Company = companyName;
            userInfo.TrueName = trueName;
            userInfo.Phone = phone;
            userInfo.Email = email;
            userInfo.WXHeadimgurl = headImage;
            userInfo.Images = image;
            userInfo.Ex1 = ex1;
            userInfo.Ex2 = ex2;
            userInfo.Ex3 = ex3;
            userInfo.Ex4 = ex4;
            userInfo.Address = address;
            userInfo.Province = province;
            userInfo.City = city;
            userInfo.District = district;
            userInfo.ProvinceCode = provinceCode;
            userInfo.CityCode = cityCode;
            userInfo.DistrictCode = districtCode;
            //userInfo.PermissionGroupID = long.Parse(permissionGroupId);
            if (Update(userInfo))
            {
                if (GetCount<ZentCloud.BLLPermission.Model.UserPmsGroupRelationInfo>(string.Format("UserId='{0}' And GroupID={1}", userInfo.UserID, userInfo.PermissionGroupID)) == 0)
                {
                    ZentCloud.BLLPermission.Model.UserPmsGroupRelationInfo userPm = new BLLPermission.Model.UserPmsGroupRelationInfo();
                    userPm.UserID = userInfo.UserID;
                    userPm.GroupID = (long)userInfo.PermissionGroupID;
                    Add(userPm);
                }
                BLLUserExpand bllUserExpand=new BLLUserExpand();
                bllUserExpand.UpdateUserExpand(BLLJIMP.Enums.UserExpandType.BankInfo, userInfo.UserID, "", backDeposit, backAccount);
                return true;
            }
            return false;

        }

        /// <summary>
        /// 删除商户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteSupplier(string ids)
        {

            BLLUserExpand bllUserExpand = new BLLUserExpand();
            List<UserInfo> userList = GetList<UserInfo>(string.Format(" AutoId in({0}) And WebsiteOwner='{1}'", ids, WebsiteOwner));
            foreach (var item in userList)
            {
                bllUserExpand.DeleteUserExpand(BLLJIMP.Enums.UserExpandType.BankInfo, item.UserID, item.WebsiteOwner);
            }
            if (Delete(new UserInfo(), string.Format(" AutoId in({0}) And WebsiteOwner='{1}'", ids, WebsiteOwner)) > 0)
            {
                Update(new WXMallProductInfo(), string.Format("SupplierUserId=''"), string.Format(" SupplierUserId in(Select UserId From ZCJ_UserInfo Where AutoId in({0})) And WebsiteOwner='{1}'", ids, WebsiteOwner));
                Update(new WXMallOrderInfo(), string.Format("SupplierUserId=''"), string.Format(" SupplierUserId in(Select UserId From ZCJ_UserInfo Where AutoId in({0})) And WebsiteOwner='{1}'", ids, WebsiteOwner));
              
                return true;
            }

            
            return false;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberLevel"></param>
        /// <param name="levelType"></param>
        /// <returns></returns>
        public string GetMemberLevelName(int memberLevel, string levelType)
        {
            UserLevelConfig model = Get<UserLevelConfig>(string.Format(" WebsiteOwner='{0}' AND LevelType='{1}' AND LevelNumber={2}", WebsiteOwner, levelType, memberLevel));
            if (model != null && !string.IsNullOrEmpty(model.LevelString))
            {
                return model.LevelString;
            }
            return "";
        }

        /// <summary>
        /// 是否是商户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool IsSupplier(UserInfo userInfo)
        {
            if (userInfo.UserType == 7)
            {
                return true;
            }
            return false;
        }

    }
}
