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
    /// Start 的摘要说明
    /// </summary>
    public class Start : BaseHandlerNeedLoginAdminNoAction
    {
        BLLFlow bllFlow = new BLLFlow();
        BLLDistribution bll = new BLLDistribution();
        public void ProcessRequest(HttpContext context)
        {
            BLLJIMP.Model.API.Flow.PostAction requestPost = bllFlow.ConvertRequestToModel<BLLJIMP.Model.API.Flow.PostAction>(new BLLJIMP.Model.API.Flow.PostAction());
            string[] limit = new string[]{"OfflineRecharge","OfflineUpgrade"};
            if (!limit.Contains(requestPost.flow_key))
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = "该流程不支持";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            string websiteOwner = bllFlow.WebsiteOwner;
            BLLJIMP.Model.Flow flow = bllFlow.GetFlowByKey(requestPost.flow_key, websiteOwner);
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

            UserInfo member = bllUser.GetUserInfo(requestPost.member_userid, websiteOwner);
            if (requestPost.flow_key == "OfflineUpgrade") {
                if (member.MemberLevel >= Convert.ToInt32(requestPost.ex2))
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "所选级别低于会员当前级别";
                    bllFlow.ContextResponse(context, apiResp);
                    return;
                }
                UserLevelConfig mlevelConfig = bll.QueryUserLevel(websiteOwner, "DistributionOnLine", member.MemberLevel.ToString());
                UserLevelConfig toLevelConfig = bll.QueryUserLevel(websiteOwner, "DistributionOnLine", requestPost.ex2.ToString());
                if (toLevelConfig == null)
                {
                    apiResp.code = (int)APIErrCode.IsNotFound;
                    apiResp.msg = "等级未找到";
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                if (toLevelConfig.IsDisable == 1)
                {
                    apiResp.code = (int)APIErrCode.IsNotFound;
                    apiResp.msg = "级别禁止升级";
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                requestPost.amount = Convert.ToDecimal(toLevelConfig.FromHistoryScore) - Convert.ToDecimal(mlevelConfig.FromHistoryScore);
                if (requestPost.amount<0)
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "所选级别低于会员当前级别";
                    bllFlow.ContextResponse(context, apiResp);
                    return;
                }
                requestPost.ex3 = toLevelConfig.LevelString;
            }

            WebsiteInfo website = bllUser.GetWebsiteInfoModelFromDataBase(websiteOwner);
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
            if (requestPost.amount > 0) action.Amount = requestPost.amount;
            if (!string.IsNullOrWhiteSpace(requestPost.content)) action.StartContent = requestPost.content;
            if (!string.IsNullOrWhiteSpace(requestPost.ex1)) action.StartEx1 = requestPost.ex1;
            if (!string.IsNullOrWhiteSpace(requestPost.ex2)) action.StartEx2 = requestPost.ex2;
            if (!string.IsNullOrWhiteSpace(requestPost.ex3)) action.StartEx3 = requestPost.ex3;
            if (requestPost.select_date.HasValue) action.StartSelectDate = requestPost.select_date.Value;

            action.MemberAutoID = member.AutoID;
            action.MemberID = member.UserID;
            action.MemberName = bllUser.GetUserDispalyName(member);
            action.MemberAvatar = bllUser.GetUserDispalyAvatar(member);
            action.MemberPhone = member.Phone;

            ZentCloud.BLLJIMP.Model.UserLevelConfig levelConfig = bll.QueryUserLevel(websiteOwner, "DistributionOnLine", member.MemberLevel.ToString());
            action.MemberLevel = levelConfig.LevelNumber;
            action.MemberLevelName = levelConfig.LevelString;

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
            if (requestPost.select_date.HasValue) actionDetail1.HandleSelectDate = requestPost.select_date.Value;
            if (!string.IsNullOrWhiteSpace(action.StartEx1)) actionDetail1.Ex1 = action.StartEx1;
            if (!string.IsNullOrWhiteSpace(action.StartEx2)) actionDetail1.Ex2 = action.StartEx2;
            if (!string.IsNullOrWhiteSpace(action.StartEx3)) actionDetail1.Ex3 = action.StartEx3;

            List<BLLJIMP.Model.FlowActionFile> files = new List<BLLJIMP.Model.FlowActionFile>();
            if (!string.IsNullOrWhiteSpace(requestPost.files))
            {
                List<string> fileUrls = requestPost.files.Split(',').Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
                if (fileUrls.Count > 0)
                {
                    foreach (var item in fileUrls)
                    {
                        files.Add(new BLLJIMP.Model.FlowActionFile()
                        {
                            FlowID = flow.AutoID,
                            StepID = step1.AutoID,
                            WebsiteOwner = websiteOwner,
                            FilePath = item
                        });
                    }
                }
            }
            BLLTransaction tran = new BLLTransaction();
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
            foreach (var item in files)
            {
                item.ActionID = rId;
                if (!bllFlow.Add(item, tran))
                {
                    tran.Rollback();
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = flow.FlowName + "，保存附件失败";
                    bllFlow.ContextResponse(context, apiResp);
                    return;
                }
            }
            tran.Commit();
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = flow.FlowName + "提交成功";
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