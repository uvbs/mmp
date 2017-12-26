using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Flow
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginNoAction
    {
        BLLFlow bllFlow = new BLLFlow();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string flow_key = context.Request["flow_key"];
            string memberUserId = CurrentUserInfo.UserID;
            string websiteOwner = bllFlow.WebsiteOwner;
            List<BLLJIMP.Model.FlowAction> list = bllFlow.GetActionList(rows, page, websiteOwner, memberUserId: memberUserId, flowKey: flow_key);
            int totalCount = bllFlow.GetActionCount(websiteOwner, memberUserId:memberUserId, flowKey:flow_key);
            var resultList = from p in list
                             select new {
                                id = p.AutoID,
                                flowname = p.FlowName,
                                amount = p.Amount,
                                status = p.Status,
                                start = p.CreateDate.ToString("yyyy/MM/dd HH:mm:ss"),
                                ex1 = p.StartEx1
                             };
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