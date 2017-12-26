using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using System.Web;

namespace ZentCloud.BLLJIMP
{
    public class BLLApiLog : BLL
    {

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="websiteOwner">站点所有者</param>
        /// <param name="module">Api模块</param>
        /// <param name="remark">说明</param>
        /// <param name="openId">openId</param>
        /// <param name="userId">用户名</param>
        /// <param name="serialNumber">流水号</param>
        /// <returns></returns>
        public bool Add(string websiteOwner, EnumApiModule module, string remark, string openId = "", string userId = "", string serialNumber = "")
        {

            ApiLog model = new ApiLog();
            model.WebsiteOwner = websiteOwner;
            model.InsertDate = DateTime.Now;
            model.IP = Common.MySpider.GetClientIP();
            model.IPLocation = Common.MySpider.GetIPLocation(model.IP);
            model.Browser = HttpContext.Current.Request.Browser == null ? "" : HttpContext.Current.Request.Browser.ToString();
            model.BrowserID = HttpContext.Current.Request.Browser.Id;
            model.BrowserVersion = HttpContext.Current.Request.Browser.Version;
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
            model.UserAgent = HttpContext.Current.Request.UserAgent;
            model.HttpMethod = HttpContext.Current.Request.HttpMethod;
            model.Url = HttpContext.Current.Request.Url.ToString();
            model.Module = CommonPlatform.Helper.EnumStringHelper.ToString(module);
            StringBuilder sbPar = new StringBuilder();
            foreach (var item in GetRequestParameter())
            {
                sbPar.AppendFormat("参数名:{0}参数值:{1}", item.Key, item.Value);

            }
            model.Parameters = sbPar.ToString();
            model.Remark = remark;
            model.OpenId = openId;
            model.UserID = userId;
            model.SerialNumber = serialNumber;
            return Add(model);


        }

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="module"></param>
        /// <param name="totalCount"></param>
        /// <param name="openId"></param>
        /// <param name="userId"></param>
        /// <param name="fromTime"></param>
        /// <param name="toTime"></param>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public List<ApiLog> List(string websiteOwner, int pageIndex, int pageSize, string module, out int totalCount, string openId = "", string userId = "", string fromTime = "", string toTime = "", string serialNumber = "")
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("WebsiteOwner='{0}' ", websiteOwner);
            if (!string.IsNullOrEmpty(module))
            {
                sbWhere.AppendFormat(" And Module='{0}'", module);
            }
            if (!string.IsNullOrEmpty(openId))
            {
                sbWhere.AppendFormat(" And OpenId='{0}'", openId);
            }
            if (!string.IsNullOrEmpty(serialNumber))
            {
                sbWhere.AppendFormat(" And SerialNumber='{0}'", serialNumber);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                sbWhere.AppendFormat(" And UserId='{0}'", userId);
            }
            if (!string.IsNullOrEmpty(fromTime))
            {
                sbWhere.AppendFormat(" And InsertDate>='{0}'", fromTime);
            }
            if (!string.IsNullOrEmpty(toTime))
            {
                sbWhere.AppendFormat(" And InsertDate<='{0}'", toTime);
            }
            totalCount = GetCount<ApiLog>(sbWhere.ToString());
            return GetLit<ApiLog>(pageSize, pageIndex, sbWhere.ToString(), " AutoID DESC");
        }
    }
}
