using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Flow
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLFlow bllFlow = new BLLFlow();
        BLLUser bllUser = new BLLUser();
        BLLDistribution bllDistribution = new BLLDistribution();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string flow_key = context.Request["flow_key"];
            string status = context.Request["status"];
            string isactme = context.Request["isactme"];
            string inactme = context.Request["inactme"];

            string handleUserId = "";
            string handleGroupId = "";
            if (isactme == "1")
            {
                handleUserId = currentUserInfo.UserID;
            }
            else if (inactme == "1")
            {
                handleUserId = currentUserInfo.UserID;
                handleGroupId = currentUserInfo.PermissionGroupID.HasValue ? currentUserInfo.PermissionGroupID.Value.ToString() : "";
            }
            string websiteOwner = bllFlow.WebsiteOwner;

            string memberUserId = "";
            string memberKey = "";
            string member = context.Request["member"];
            if (!string.IsNullOrWhiteSpace(member))
            {
                UserInfo mu = bllUser.GetSpreadUser(member, websiteOwner);
                if (mu == null)
                {
                    memberKey = member;
                }
                if(mu == null){
                    memberKey = member;
                }else{
                    memberUserId = mu.UserID;
                }
            }

            List<FlowAction> list = bllFlow.GetActionList(rows, page, websiteOwner, memberUserId: memberUserId, flowKey: flow_key,
                status: status, handleUserId: handleUserId, handleGroupId: handleGroupId, isActionMe: isactme, memberKey: memberKey);
            int totalCount = bllFlow.GetActionCount(websiteOwner, memberUserId: memberUserId, flowKey: flow_key,
                status: status, handleUserId: handleUserId, handleGroupId: handleGroupId, isActionMe: isactme, memberKey: memberKey);
            List<dynamic> resultList = new List<dynamic>();
            Dictionary<string, UserInfo> nameDir = new Dictionary<string, UserInfo>();
            foreach (var p in list)
            {
                UserInfo userInfo = new UserInfo();
                if (!nameDir.ContainsKey(p.CreateUserID))
                {
                    userInfo = bllUser.GetColByKey<UserInfo>("UserID", p.CreateUserID, 
                        "AutoID,TrueName,WXNickname", 
                        websiteOwner: websiteOwner);
                    nameDir.Add(p.CreateUserID, userInfo);
                }
                else{
                    userInfo = nameDir[p.CreateUserID];
                }

                resultList.Add(new
                {
                    id = p.AutoID,
                    flowname = p.FlowName,
                    amount = p.Amount,
                    status = p.Status,
                    member_id = p.MemberAutoID == 0 ? "" : p.MemberAutoID.ToString(),
                    member_name = p.MemberName,
                    member_phone = p.MemberPhone,
                    creater_name = userInfo ==null?"":bllUser.GetUserDispalyName(userInfo.WXNickname, userInfo.TrueName),
                    start = p.CreateDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    end = p.EndDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    lv = p.MemberLevel,
                    lvname = p.MemberLevelName,
                    ex1 = p.StartEx1,
                    ex2 = p.StartEx2,
                    content = p.StartContent,
                    select_date = p.StartSelectDate.ToString("yyyy/MM/dd HH:mm:ss")
                });
            }
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.msg = "查询完成";
            apiResp.result = new
            {
                list = resultList,
                totalcount = totalCount
            };
            bllFlow.ContextResponse(context, apiResp);
        }
    }
}