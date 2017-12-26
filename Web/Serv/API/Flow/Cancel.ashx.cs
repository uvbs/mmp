using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Flow
{
    /// <summary>
    /// Cancel 的摘要说明
    /// </summary>
    public class Cancel : BaseHandlerNeedLoginNoAction
    {
        BLLFlow bllFlow = new BLLFlow();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string content = context.Request["content"];
            if (string.IsNullOrWhiteSpace(content))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请填写申请取消原因";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            string websiteOwner = bllFlow.WebsiteOwner;
            FlowAction act = bllFlow.GetByKey<FlowAction>("AutoID", id, websiteOwner: websiteOwner);
            if (act.Status  == 11)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "您已经申请取消";
                bllFlow.ContextResponse(context, apiResp);
                return;
            } 
            if (act.Status != 0)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "已经处理完成不能申请取消";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            if (CurrentUserInfo.UserID != act.CreateUserID)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "仅能申请人申请取消";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            act.CancelDate = DateTime.Now;
            act.Status = 11;

            BLLJIMP.Model.FlowActionDetail actionDetail1 = new BLLJIMP.Model.FlowActionDetail();
            actionDetail1.ActionID = act.AutoID;
            actionDetail1.WebsiteOwner = websiteOwner;
            actionDetail1.FlowID = act.FlowID;
            actionDetail1.StepID = 0;
            actionDetail1.StepName = "申请取消";
            actionDetail1.HandleUserID = CurrentUserInfo.UserID;
            actionDetail1.HandleDate = DateTime.Now;
            actionDetail1.HandleContent = content;

            BLLTransaction tran = new BLLTransaction();
            if (!bllFlow.Update(act, tran))
            {
                tran.Rollback();
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "申请取消失败";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            if (!bllFlow.Add(actionDetail1, tran))
            {
                tran.Rollback();
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "记录申请取消失败";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            tran.Commit();
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "申请取消成功";
            bllFlow.ContextResponse(context, apiResp);
        }
    }
}