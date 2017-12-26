using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Log
{
    /// <summary>
    /// MemberLogList 的摘要说明
    /// </summary>
    public class MemberLogList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLLog bllLog = new BLLJIMP.BLLLog();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string type = context.Request["type"];
            string action = context.Request["action"];
            string target_id = context.Request["target_id"];
            string websiteOwner = bllUser.WebsiteOwner;

            int totalCount = 0;
            List<BLLJIMP.Model.Log> logList = bllLog.List(pageIndex, pageSize, type, action, "", "", out totalCount,
                "", 0, target_id);
            UserInfo userModel = null;
            if (!string.IsNullOrWhiteSpace(target_id)) userModel = bllUser.GetColByKey<UserInfo>("UserID", target_id, 
                "AutoID,UserID,TrueName", websiteOwner: websiteOwner);
            List<UserInfo> targetUserList = new List<UserInfo>();
            List<dynamic> returnList = new List<dynamic>();
            foreach (var item in logList)
            {
                UserInfo cuserModel = null;
                if (!string.IsNullOrWhiteSpace(item.TargetID))
                {
                    cuserModel = targetUserList.FirstOrDefault(p => p.UserID == item.UserID);
                    if (cuserModel == null){
                        cuserModel = bllUser.GetColByKey<UserInfo>("UserID", item.UserID, "AutoID,UserID,TrueName",
                            websiteOwner: websiteOwner);
                        targetUserList.Add(cuserModel);
                    }
                }

                returnList.Add(new
                {
                    id = item.AutoID,
                    cuser = cuserModel == null ? null : new
                    {
                        id = cuserModel.AutoID,
                        uid = cuserModel.UserID,
                        name = bllUser.GetUserDispalyName(cuserModel),
                    },
                    time = item.InsertDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    remark = item.Remark,
                    action = item.Action,
                });
            }
            apiResp.result = new
            {
                totalcount = totalCount,
                list = returnList,
                member = new
                {
                    id =userModel.AutoID,
                    uid = userModel.UserID,
                    name = bllUser.GetUserDispalyName(userModel)
                }
            };
            bllLog.ContextResponse(context, apiResp);
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