using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Flow
{
    /// <summary>
    /// StartCancelRegister 的摘要说明
    /// </summary>
    public class StartCancelRegister : BaseHandlerNeedLoginAdminNoAction
    {
        BLLFlow bllFlow = new BLLFlow();
        BLLUser bllUser = new BLLUser();
        BLLDistribution bll = new BLLDistribution();
        public void ProcessRequest(HttpContext context)
        {
            int id = Convert.ToInt32(context.Request["id"]);
            string websiteOwner = bll.WebsiteOwner;
            string flow_key = context.Request["flow_key"];
            string content = context.Request["content"];

            BLLJIMP.Model.Flow flow = bllFlow.GetFlowByKey(flow_key, websiteOwner);
            if (flow == null)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "流程未定义";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            if (flow.IsDelete == 1)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = flow.FlowName + "已停用";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            List<BLLJIMP.Model.FlowStep> steps = bllFlow.GetStepList(2, 1, websiteOwner, flow.AutoID);
            if (steps.Count == 0)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = flow.FlowName + "环节未设置";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }

            UserInfo tUser = bllUser.GetUserInfoByAutoID(id, websiteOwner);
            if (tUser == null)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "会员未找到";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (tUser.MemberLevel<=0)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "该会员已经撤单";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (bllFlow.ExistsMemberPhoneAction(websiteOwner, flow.FlowKey, "0", memberUserId: tUser.UserID))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "该会员正在申请撤单";
                bll.ContextResponse(context, apiResp);
                return;
            }

            BLLJIMP.Model.FlowStep step1 = steps[0];
            BLLJIMP.Model.FlowStep step2 = null;
            if (steps.Count == 2) step2 = steps[1];

            BLLJIMP.Model.FlowAction action = new BLLJIMP.Model.FlowAction();
            action.CreateDate = DateTime.Now;
            action.CreateUserID = currentUserInfo.UserID;
            action.WebsiteOwner = websiteOwner;
            action.StartStepID = step1.AutoID;
            action.FlowID = flow.AutoID;
            action.FlowKey = flow.FlowKey;
            if (!string.IsNullOrWhiteSpace(content)) action.StartContent = content;

            action.MemberAutoID = tUser.AutoID;
            action.MemberID = tUser.UserID;
            action.MemberName = tUser.TrueName;
            action.MemberPhone = tUser.Phone;
            action.MemberLevel = tUser.MemberLevel;
            action.TrueAmount = tUser.TotalAmount;
            action.Amount = tUser.AccountAmountEstimate;
            action.DeductAmount = tUser.AccumulationFund;
            UserLevelConfig levelConfig = bll.QueryUserLevel(websiteOwner, "DistributionOnLine", tUser.MemberLevel.ToString());
            action.MemberLevelName = levelConfig==null?"": levelConfig.LevelString;
            action.FlowName = flow.FlowName;

            if (step2 != null)
            {
                action.StepID = step2.AutoID;
                action.StepName = step2.StepName;
            }
            else
            {
                action.Status = 9;
                action.EndDate = DateTime.Now;
            }

            BLLJIMP.Model.FlowActionDetail actionDetail1 = new BLLJIMP.Model.FlowActionDetail();
            actionDetail1.WebsiteOwner = websiteOwner;
            actionDetail1.FlowID = flow.AutoID;
            actionDetail1.StepID = step1.AutoID;
            actionDetail1.StepName = step1.StepName;
            actionDetail1.HandleUserID = currentUserInfo.UserID;
            actionDetail1.HandleDate = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(action.StartContent)) actionDetail1.HandleContent = action.StartContent;

            BLLTransaction tran = new BLLTransaction();

            if(bllUser.Update(tUser,
                string.Format("IsDisable=1"),
                string.Format("WebsiteOwner='{0}' And AutoID={1}",websiteOwner,tUser.AutoID),
                tran) <= 0)
            {
                tran.Rollback();
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = flow.FlowName + "冻结失败";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            int rId = Convert.ToInt32(bllFlow.AddReturnID(action, tran));
            if (rId <= 0)
            {
                tran.Rollback();
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = flow.FlowName + "失败";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            actionDetail1.ActionID = rId;
            if (!bllFlow.Add(actionDetail1, tran))
            {
                tran.Rollback();
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = flow.FlowName + "，记录失败";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            tran.Commit();
            apiResp.msg = "撤单申请提交成功";
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllFlow.ContextResponse(context, apiResp);
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