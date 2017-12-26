using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCAppPush;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    public class BLLAppManage:BLL
    {
        /// <summary>
        /// 应用查询语句
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public string GetAppParam(string websiteOwner, string keyword, string appId, string alipayAppId="", string wxAppId="")
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbSql.AppendFormat(" And WebsiteOwner='{0}' ", websiteOwner);
            if (!string.IsNullOrWhiteSpace(keyword)) sbSql.AppendFormat(" And (AppName Like '{0}%' Or AppId Like '{0}%') ", keyword);
            if (!string.IsNullOrWhiteSpace(appId)) sbSql.AppendFormat(" And AppId='{0}' ", appId);
            if (!string.IsNullOrWhiteSpace(alipayAppId)) sbSql.AppendFormat(" And AlipayAppId='{0}' ", alipayAppId);
            if (!string.IsNullOrWhiteSpace(wxAppId)) sbSql.AppendFormat(" And WxAppId='{0}' ", wxAppId);
            return sbSql.ToString();
        }
        /// <summary>
        /// 应用数量
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public int GetAppCount(string websiteOwner,string keyword)
        {
            return GetCount<AppManage>(GetAppParam(websiteOwner, keyword,""));
        }
        /// <summary>
        /// 应用列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<AppManage> GetAppList(int rows, int page, string websiteOwner, string keyword)
        {
            return GetLit<AppManage>(rows, page, GetAppParam(websiteOwner, keyword,""));
        }

        public AppManage GetApp(string websiteOwner, string appId,string alipayAppId="",string wxAppId="")
        {
            return Get<AppManage>(GetAppParam(websiteOwner, "", appId, alipayAppId, wxAppId));
        }

        /// <summary>
        /// 版本查询语句
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="os"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public string GetVersionParam(string websiteOwner, string os, string keyword)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(websiteOwner)) sbSql.AppendFormat(" And WebsiteOwner='{0}' ", websiteOwner);
            if (!string.IsNullOrWhiteSpace(os)) sbSql.AppendFormat(" And AppOS='{0}' ", os.ToLower());
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sbSql.AppendFormat(" And (AppVersion Like '{0}%' Or AppVersionInfo Like '%{0}%') ", keyword);
            }
            return sbSql.ToString();
        }
        /// <summary>
        /// 版本数量
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="os"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public int GetVersionCount(string websiteOwner, string os, string keyword)
        {
            return GetCount<AppManageVersion>(GetVersionParam(websiteOwner, os, keyword));
        }
        /// <summary>
        /// 版本列表，倒序排
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="os"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<AppManageVersion> GetVersionList(int rows, int page, string websiteOwner, string os, string keyword)
        {
            return GetLit<AppManageVersion>(rows, page, GetVersionParam(websiteOwner, os, keyword),"AutoID Desc");
        }

        /// <summary>
        /// 查询版本，id为0查最新
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="os"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public AppManageVersion GetVersion(string websiteOwner, string os, string id)
        {
            if (!string.IsNullOrWhiteSpace(id) && id != "0")
            {
                return GetByKey<AppManageVersion>("AutoID", id, websiteOwner: websiteOwner);
            }
            else
            {
                List<AppManageVersion> list = GetVersionList(1, 1, websiteOwner, os, "");
                if (list.Count == 0) return null;
                return list[0];
            }
        }

        public bool HaveGetuiAppPush(WebsiteInfo website)
        {
            if (website.AppPushType != "getui") return false;
            if (!string.IsNullOrWhiteSpace(website.AppPushAppId) &&
                !string.IsNullOrWhiteSpace(website.AppPushAppKey) &&
                !string.IsNullOrWhiteSpace(website.AppPushAppSecret) &&
                !string.IsNullOrWhiteSpace(website.AppPushMasterSecret)) 
                return true;
            return false;
        }

        public string AppPushClientParam(string websiteOwner, string clientId, string appId, string uuId, string userId, string userIds = "", string hasClientId ="")
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner = '{0}' ", websiteOwner);
            if (!string.IsNullOrWhiteSpace(clientId)) sbWhere.AppendFormat(" And PushClientId = '{0}' ", clientId);
            if (!string.IsNullOrWhiteSpace(appId)) sbWhere.AppendFormat(" And PushAppId = '{0}' ", appId);
            if (!string.IsNullOrWhiteSpace(uuId)) sbWhere.AppendFormat(" And UUId = '{0}' ", uuId);
            if (!string.IsNullOrWhiteSpace(userId)) sbWhere.AppendFormat(" And UserID = '{0}' ", userId);
            if (!string.IsNullOrWhiteSpace(userIds)) {
                userIds = "'" + userIds.Replace(",", "','") + "'";
                sbWhere.AppendFormat(" And UserID In ({0}) ", userIds);
            }
            if (hasClientId == "1") sbWhere.AppendFormat(" And PushClientId>'' ");
            return sbWhere.ToString();
        }
        public List<AppPushClient> GetAppPushClientList(int rows, int page, string websiteOwner, string clientId, string appId, string uuId, string userId, 
            string userIds, string hasClientId)
        {
            return GetLit<AppPushClient>(rows, page, AppPushClientParam(websiteOwner, clientId, appId, uuId, userId, userIds, hasClientId));
        }
        public AppPushClient GetAppPushClient(string websiteOwner, string clientId, string appId, string uuId, string userId, string hasClientId = "")
        {
            return Get<AppPushClient>(AppPushClientParam(websiteOwner, clientId, appId, uuId, userId, "", hasClientId));
        }
        public bool ExistsAppPushClient(string websiteOwner, string clientId, string appId, string uuId, string userId)
        {
            AppPushClient uClient = GetAppPushClient(websiteOwner, clientId, appId, uuId, userId);
            return uClient != null;
        }

        public bool PushMassage(WebsiteInfo website, string title, string text, string link, List<UserInfo> users, out string msg)
        {
            msg = "";
            if (website.AppPushType == "getui")
            {
                if (!HaveGetuiAppPush(website)){
                    msg = "推送配置不完整";
                    return false;
                }
                if (title.Length > 40)
                {
                    msg = "个推标题最多支持40个字";
                    return false;
                }
                if (text.Length > 600){
                    msg = "个推标题最多支持600个字";
                    return false;
                }
                string userIds = users==null?"": ZentCloud.Common.MyStringHelper.ListToStr(users.Select(p=>p.UserID).Distinct().ToList(),"",",");
                List<AppPushClient> clientList = GetAppPushClientList(int.MaxValue,1,website.WebsiteOwner,"",website.AppPushAppId,"","",userIds,"1");
                if(clientList.Count ==0){
                    msg = "推送对象为空";
                    return false;
                }
                List<string> clientIds = clientList.Select(p=>p.PushClientId).ToList();
                GetuiPushHelper getui = new GetuiPushHelper(website.AppPushAppKey, website.AppPushMasterSecret);
                string resultString = getui.PushMessageToList(website.AppPushAppId, website.AppPushAppKey, title, text, link, clientIds);
                return true;
            }
            else
            {
                msg = "推送类型不支持";
                return false;
            }
        }

        public bool PushMassage(WebsiteInfo website, string title, string text, string link, UserInfo user, out string msg)
        {
            msg = "";
            if (website.AppPushType == "getui")
            {
                if (!HaveGetuiAppPush(website))
                {
                    msg = "推送配置不完整";
                    return false;
                }
                if (title.Length > 40)
                {
                    msg = "个推标题最多支持40个字";
                    return false;
                }
                text = text.Replace("\n", "，");
                if (text.Length > 600)
                {
                    msg = "个推标题最多支持600个字";
                    return false;
                }
                List<AppPushClient> clientList = GetAppPushClientList(int.MaxValue, 1, website.WebsiteOwner, "",website.AppPushAppId, "", user.UserID, "", "1");
                if (clientList.Count == 0)
                {
                    msg = "推送对象为空";
                    return false;
                }
                List<string> clientIds = clientList.Select(p => p.PushClientId).ToList();
                GetuiPushHelper getui = new GetuiPushHelper(website.AppPushAppKey, website.AppPushMasterSecret);
                msg = getui.PushMessageToList(website.AppPushAppId, website.AppPushAppKey, title, text, link, clientIds);
                JToken jto = JToken.Parse(msg);
                if (jto["result"] != null && jto["result"].ToString() == "ok")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                msg = "推送类型不支持";
                return false;
            }
        }
        /// <summary>
        /// app支付宝开放平台
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public bool IsAppAlipay(AppManage app)
        {
            if (!string.IsNullOrEmpty(app.AlipayAppId) && !string.IsNullOrEmpty(app.AlipayPrivatekey)
                && !string.IsNullOrEmpty(app.AlipayPublickey) && !string.IsNullOrEmpty(app.AlipaySignType))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// app微信开放平台
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public bool IsAppWeixin(AppManage app)
        {
            if (!string.IsNullOrEmpty(app.WxAppId) && !string.IsNullOrEmpty(app.WxAppSecret))
            {
                return true;
            }
            return false;
        }
    }
}
