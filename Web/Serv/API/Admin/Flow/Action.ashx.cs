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
    /// Action 的摘要说明
    /// </summary>
    public class Action : BaseHandlerNeedLoginAdminNoAction
    {
        BLLFlow bllFlow = new BLLFlow();
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            BLLJIMP.Model.API.Flow.PostAction requestPost = bllFlow.ConvertRequestToModel<BLLJIMP.Model.API.Flow.PostAction>(new BLLJIMP.Model.API.Flow.PostAction());
            string websiteOwner = bllFlow.WebsiteOwner;
            FlowAction act = bllFlow.GetByKey<FlowAction>("AutoID", requestPost.action_id.ToString(), websiteOwner: websiteOwner);
            string handleUserId = currentUserInfo.UserID;
            string handleGroupId = currentUserInfo.PermissionGroupID.HasValue ? currentUserInfo.PermissionGroupID.Value.ToString() : "";
            bool isCanAction = bllFlow.IsCanAction(websiteOwner, act.AutoID, handleUserId, handleGroupId);
            if (!isCanAction)
            {
                apiResp.code = (int)APIErrCode.NoPms;
                apiResp.msg = "您没有权限审核！";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            BLLJIMP.Model.FlowStep step1 = null;
            BLLJIMP.Model.FlowStep step2 = null;
            if (requestPost.audit == 1 || requestPost.audit == 3)
            {
                List<BLLJIMP.Model.FlowStep> steps = bllFlow.GetStepList(int.MaxValue, 1, websiteOwner, act.FlowID);
                foreach (var itemStep in steps)
	            {
                    if (step1 != null)
                    {
                        step2 = itemStep;
                        break;
                    }
                    if(itemStep.AutoID == act.StepID) step1 = itemStep;
                }
            }
            else if (requestPost.audit == 2 || requestPost.audit == 4)
            {
                step1 = bllFlow.GetStep(websiteOwner, act.FlowID, act.StepID);
            }


            BLLJIMP.Model.FlowActionDetail actionDetail1 = new BLLJIMP.Model.FlowActionDetail();
            actionDetail1.ActionID = act.AutoID;
            actionDetail1.WebsiteOwner = websiteOwner;
            actionDetail1.FlowID = act.FlowID;
            actionDetail1.StepID = step1.AutoID;
            actionDetail1.StepName = step1.StepName;
            actionDetail1.HandleUserID = currentUserInfo.UserID;
            actionDetail1.HandleDate = DateTime.Now;
            actionDetail1.HandleContent = requestPost.content;
            if (requestPost.select_date.HasValue) actionDetail1.HandleSelectDate = requestPost.select_date.Value;
            if (!string.IsNullOrWhiteSpace(requestPost.ex1)) actionDetail1.Ex1 = requestPost.ex1;
            if (!string.IsNullOrWhiteSpace(requestPost.ex2)) actionDetail1.Ex2 = requestPost.ex2;

            List<BLLJIMP.Model.FlowActionFile> files = new List<BLLJIMP.Model.FlowActionFile>();
            if (!string.IsNullOrWhiteSpace(requestPost.files))
            {
                List<string> fileUrls = requestPost.files.Split(',').ToList();
                if (fileUrls.Count > 0)
                {
                    foreach (var item in fileUrls)
                    {
                        files.Add(new BLLJIMP.Model.FlowActionFile()
                        {
                            ActionID = act.AutoID,
                            FlowID = act.FlowID,
                            StepID = step1.AutoID,
                            WebsiteOwner = websiteOwner,
                            FilePath = item
                        });
                    }
                }
            }
            string msg = "";
            apiResp.status = bllFlow.Action(out msg, requestPost.audit, websiteOwner, act, actionDetail1, files, step2);
            if (apiResp.status && string.IsNullOrWhiteSpace(msg)) msg = "审核完成";
            apiResp.msg = msg;
            apiResp.code = apiResp.status ? (int)APIErrCode.IsSuccess : (int)APIErrCode.OperateFail;
            bllFlow.ContextResponse(context, apiResp);
        }
    }
}