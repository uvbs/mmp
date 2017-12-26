using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Flow
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginNoAction
    {
        BLLFlow bllFlow = new BLLFlow();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string websiteOwner = bllFlow.WebsiteOwner;
            FlowAction act = bllFlow.GetByKey<FlowAction>("AutoID", id, websiteOwner: websiteOwner);
            List<FlowActionFile> actFiles = bllFlow.GetActionFiles(websiteOwner, act.AutoID, act.FlowID);
            List<string> files = new List<string>();
            if(actFiles.Count>0){
                files = actFiles.Select(p=>p.FilePath).Distinct().ToList();
            }
            FlowActionDetail endStepAct = bllFlow.GetEndActionDetail(websiteOwner, act.AutoID, act.FlowID);
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.msg = "查询完成";
            apiResp.result = new
            {
                id = act.AutoID,
                flowname = act.FlowName,
                stepname = act.StepName,
                flow_key = act.FlowKey,
                amount = act.Amount,
                true_amount = act.TrueAmount,
                deduct_amount = act.DeductAmount,
                status = act.Status,
                ex1 = act.StartEx1,
                ex2 = act.StartEx2,
                ex3 = act.StartEx3,
                content = act.StartContent,
                select_date = act.StartSelectDate.ToString("yyyy/MM/dd HH:mm:ss"),
                start = act.CreateDate.ToString("yyyy/MM/dd HH:mm:ss"),
                end = act.EndDate.ToString("yyyy/MM/dd HH:mm:ss"),
                cancel_date = act.CancelDate.ToString("yyyy/MM/dd HH:mm:ss"),
                end_content = endStepAct!=null && !string.IsNullOrWhiteSpace(endStepAct.HandleContent) ?endStepAct.HandleContent:"",
                files = files
            };
            bllFlow.ContextResponse(context, apiResp);
        }
    }
}