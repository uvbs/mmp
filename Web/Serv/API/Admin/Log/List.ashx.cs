using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Log
{
    /// <summary>
    /// 操作日志列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLLog bllLog = new BLLJIMP.BLLLog();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLLWebSite bllWeisite = new BLLJIMP.BLLWebSite();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWords = context.Request["keyword"];
            string type = context.Request["type"];
            string action = context.Request["action"];
            string useId = context.Request["user_id"];
            WebsiteInfo webSite = bllWeisite.GetWebsiteInfo(bllWeisite.WebsiteOwner);
            int totalCount = 0;
            List<BLLJIMP.Model.Log> logList = bllLog.List(pageIndex, pageSize, type, action,useId, keyWords, out totalCount,
                "", webSite.LogLimitDay);
            List<dynamic> returnList = new List<dynamic>();
            foreach (var item in logList)
            {
                UserInfo userModel = bllUser.GetUserInfo(item.UserID);

                var remark = item.Remark;

                if (!string.IsNullOrWhiteSpace(remark))
                {
                    if (remark.IndexOf("用户[") > -1)
                    {
                        var dealUserId = remark.Substring(remark.IndexOf("用户[") + 3);
                        dealUserId = dealUserId.Substring(0, dealUserId.IndexOf("]"));

                        var dealUser = bllUser.GetUserInfo(dealUserId);

                        var replaceStr = string.Empty;

                        if (dealUser != null)
                        {
                            replaceStr = string.Format("用户[id:{0} {1} {2}]",
                                    dealUser.AutoID,
                                    string.IsNullOrWhiteSpace(dealUser.WXNickname) ? "" : ",微信昵称:" + dealUser.WXNickname,
                                    string.IsNullOrWhiteSpace(dealUser.TrueName) ? "" : ",姓名:" + dealUser.TrueName
                                );
                        }
                        else
                        {
                            replaceStr = "用户[用户不存在，可能已删除]";
                        }

                        remark = remark.Replace("用户[" + dealUserId + "]", replaceStr);

                    }
                }

                returnList.Add(new
                {
                    id = item.AutoID,
                    user_id = item.UserID,
                    ip = item.IP,
                    time = item.InsertDate,
                    time_str = item.InsertDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    browser = item.Browser,
                    browser_id = item.BrowserID,
                    module = item.Module,
                    remark = remark,
                    action = item.Action,
                    true_name = userModel.TrueName
                });
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                total=totalCount,
                rows=returnList
            }));

        }

        
    }
}