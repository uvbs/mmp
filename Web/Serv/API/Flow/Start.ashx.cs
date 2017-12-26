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
    /// Start 的摘要说明
    /// </summary>
    public class Start : BaseHandlerNeedLoginNoAction
    {
        BLLFlow bllFlow = new BLLFlow();
        BLLUser bllUser = new BLLUser();
        BLLDistribution bllDistribution = new BLLDistribution();
        public void ProcessRequest(HttpContext context)
        {
            BLLJIMP.Model.API.Flow.PostAction requestPost = bllFlow.ConvertRequestToModel<BLLJIMP.Model.API.Flow.PostAction>(new BLLJIMP.Model.API.Flow.PostAction());
            
            if (requestPost.flow_key == "Withdraw")
            {
                if (CurrentUserInfo.IsLock == 1)
                {
                    apiResp.code = (int)APIErrCode.NoPms;
                    apiResp.msg = "账号已被锁定";
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }
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
            if( flow.IsDelete == 1)
            {
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = flow.FlowName + "已停用";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            List<BLLJIMP.Model.FlowStep> steps = bllFlow.GetStepList(2, 1, websiteOwner, flow.AutoID);
            if(steps.Count ==0){
                apiResp.code = (int)APIErrCode.IsNotFound;
                apiResp.msg = flow.FlowName + "环节未设置";
                bllFlow.ContextResponse(context, apiResp);
                return;
            }
            WebsiteInfo website = bllUser.GetWebsiteInfoModelFromDataBase(websiteOwner);
            BLLJIMP.Model.FlowStep step1 = steps[0];
            BLLJIMP.Model.FlowStep step2 = null;
            if (steps.Count == 2) step2 = steps[1];
            
            BLLJIMP.Model.FlowAction action = new BLLJIMP.Model.FlowAction();
            action.CreateDate = DateTime.Now;
            action.CreateUserID = CurrentUserInfo.UserID;
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
            if (requestPost.flow_key == "Withdraw")
            {
                if (CurrentUserInfo.TotalAmount < requestPost.amount)
                {
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = string.Format("消耗会员可用{0}不足", website.TotalAmountShowName);
                    bllFlow.ContextResponse(context, apiResp);
                    return;
                }
                action.TrueAmount = Math.Round(action.Amount * 0.9322M,2);
                action.DeductAmount = action.Amount - action.TrueAmount;
                var card = bllFlow.GetColByKey<BLLJIMP.Model.BindBankCard>("AutoID", action.StartEx3, "AutoID,BankName,AccountName,BankAccount", websiteOwner: websiteOwner);
                action.StartEx1 = card.BankName;
                action.StartEx2 = card.AccountName;
                action.StartEx3 = card.BankAccount;
            }
            else if (requestPost.flow_key == "PerformanceReward")
            {
                TeamPerformance myPerformance = bllFlow.GetByKey<TeamPerformance>("AutoID", requestPost.rel_id, websiteOwner: websiteOwner);
                action.RelationId = Convert.ToInt32(requestPost.rel_id);
                action.Amount = myPerformance.Reward;
                action.DeductAmount = (myPerformance.Reward * 20 / 100);
                action.TrueAmount = (myPerformance.Reward * 80 / 100);
                action.StartContent = string.Format("管理业绩：{0}，奖金比例：{1}，管理奖金：{2}，其他扣款：{3}。<br />公积金：{4}，开票金额：{5}",
                    Convert.ToDouble(myPerformance.Performance),Convert.ToDouble(myPerformance.Rate),Convert.ToDouble(myPerformance.TotalReward),
                    Convert.ToDouble(myPerformance.ChildReward),Convert.ToDouble((myPerformance.Reward * 20 / 100)),Convert.ToDouble((myPerformance.Reward * 80 / 100)));
                string ym = myPerformance.YearMonth.ToString();
                action.StartEx1 = ym.Substring(0, 4) + "年" + ym.Substring(4, 2) + "月";
            }
            action.MemberAutoID = CurrentUserInfo.AutoID;
            action.MemberID = CurrentUserInfo.UserID;
            action.MemberName = bllUser.GetUserDispalyName(CurrentUserInfo);
            action.MemberAvatar = bllUser.GetUserDispalyAvatar(CurrentUserInfo);
            action.MemberPhone = CurrentUserInfo.Phone;

            ZentCloud.BLLJIMP.Model.UserLevelConfig levelConfig = bllDistribution.QueryUserLevel(websiteOwner, "DistributionOnLine", CurrentUserInfo.MemberLevel.ToString());
            action.MemberLevel = levelConfig.LevelNumber;
            action.MemberLevelName = levelConfig.LevelString;

            action.FlowName = flow.FlowName;
            if(step2!=null) {
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
            actionDetail1.HandleUserID = CurrentUserInfo.UserID;
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
            if (requestPost.flow_key == "Withdraw")
            {
                #region 消耗操作人金额

                if (bllUser.AddScoreDetail(CurrentUserInfo.UserID, websiteOwner, (double)(0 - action.Amount),
                    string.Format("申请提现{0}元", (double)action.Amount),
                    "TotalAmount", (double)(CurrentUserInfo.TotalAmount - action.Amount),
                    rId.ToString(), "申请提现", "", "", (double)action.Amount, (double)action.DeductAmount, "",
                    tran, ex1: action.StartEx1, ex2: action.StartEx2, 
                    ex3: action.StartEx3,
                    ex5:"bank") <= 0)
                {
                    tran.Rollback();
                    apiResp.msg = "记录转账明细出错";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }
                if (bllUser.Update(CurrentUserInfo, string.Format("TotalAmount=ISNULL(TotalAmount,0)-{0},AccountAmountEstimate=ISNULL(AccountAmountEstimate,0)-{0}", action.Amount),
                    string.Format("AutoID={0} And WebsiteOwner='{1}' And ISNULL(TotalAmount,0)-{2}>=0 ",
                    CurrentUserInfo.AutoID, websiteOwner, action.Amount),
                    tran) <= 0)
                {
                    tran.Rollback();
                    apiResp.msg = string.Format("消耗会员可用{0}出错", website.TotalAmountShowName);
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }
                #endregion
            }
            else if (requestPost.flow_key == "PerformanceReward")
            {
                if (bllFlow.Update(new TeamPerformance(),
                    string.Format("FlowActionId={0},FlowActionStatus=0,Status=2",rId),
                    string.Format("AutoID={0}", requestPost.rel_id), tran) <= 0)
                {
                    tran.Rollback();
                    apiResp.msg = "业绩关联出错";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllUser.ContextResponse(context, apiResp);
                    return;
                }
            }
            tran.Commit();
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = flow.FlowName + "提交成功";
            bllFlow.ContextResponse(context, apiResp);
        }
    }
}