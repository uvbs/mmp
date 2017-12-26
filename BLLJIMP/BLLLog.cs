using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using System.Web;

namespace ZentCloud.BLLJIMP
{
    public class BLLLog : BLL
    {
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <returns></returns>
        public bool Add(EnumLogType logType, EnumLogTypeAction action, string userId, string remark, string orderID = "", string targetID="")
        {
            if (userId=="jubit")
            {
                return true;
            }
            Log model = new Log();
            model.WebsiteOwner = WebsiteOwner;
            model.InsertDate = DateTime.Now;
            model.Module = CommonPlatform.Helper.EnumStringHelper.ToString(logType);
            model.Action = CommonPlatform.Helper.EnumStringHelper.ToString(action);
            model.Remark = remark;
            model.UserID = userId;
            model.IP = Common.MySpider.GetClientIP();
            model.IPLocation = Common.MySpider.GetIPLocation(model.IP);
            model.Browser = HttpContext.Current.Request.Browser == null ? "" : HttpContext.Current.Request.Browser.ToString();
            model.BrowserID = HttpContext.Current.Request.Browser.Id;
            if (!string.IsNullOrEmpty(orderID))
            {
                model.OrderID = orderID;
            }
            if (!string.IsNullOrEmpty(targetID))
            {
                model.TargetID = targetID;
            }
            if (HttpContext.Current.Request.Browser.Beta)
            {
                model.BrowserIsBata = "测试版";
            }
            else
            {
                model.BrowserIsBata = "正式版";
            }
            model.BrowserVersion = HttpContext.Current.Request.Browser.Version;
            model.SystemByte = HttpContext.Current.Request.Browser.Platform;
            model.UserAgent = HttpContext.Current.Request.UserAgent;
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
            return Add(model);


        }

        /// <summary>
        /// 日志列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="type"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<Log> List(int pageIndex, int pageSize, string type, string action, string userid, string keyword, out int totalCount, string orderID = "", int logLimitDay = 0, string targetID="")
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("WebsiteOwner='{0}' And Module!='Weixin'", WebsiteOwner);
            if (!string.IsNullOrEmpty(orderID))
            {
                sbWhere.AppendFormat(" And OrderID='{0}'", orderID);
            }
            if (!string.IsNullOrEmpty(targetID))
            {
                sbWhere.AppendFormat(" And TargetID='{0}'", targetID);
            }
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat(" And Module='{0}'", type);
            }
            if (!string.IsNullOrEmpty(action))
            {
                sbWhere.AppendFormat(" And Action='{0}'", action);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                sbWhere.AppendFormat(" And Remark like '%{0}%'", keyword);
            }
            if (!string.IsNullOrEmpty(userid))
            {
                sbWhere.AppendFormat(" AND UserID='{0}' ",userid);
            }
            if (logLimitDay > 0)
            {
                sbWhere.AppendFormat(" AND InsertDate >getdate()-{0}",logLimitDay);
            }
            totalCount = GetCount<Log>(sbWhere.ToString());
            return GetLit<Log>(pageSize, pageIndex, sbWhere.ToString(), " AutoID DESC");
        }
    }
}
