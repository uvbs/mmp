using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Score
{
    /// <summary>
    /// ApplyWithdrawCash 的摘要说明
    /// </summary>
    public class ApplyWithdrawCash : BaseHandlerNeedLoginNoAction
    {
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int score = Convert.ToInt32(context.Request["score"]);
            string moduleName = "积分";
            if (!string.IsNullOrWhiteSpace(context.Request["module_name"])) moduleName = context.Request["module_name"];
            string websiteOwner = bllKeyValueData.WebsiteOwner;
            if (score <= 0)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = moduleName + "不能为0";
                bllKeyValueData.ContextResponse(context, apiResp);
                return;
            }
            string rechargeValue = bllKeyValueData.GetDataVaule("Recharge", "100", websiteOwner);
            string minScore = bllKeyValueData.GetDataVaule("MinScore", "1", websiteOwner);
            string minWithdrawCashScore = bllKeyValueData.GetDataVaule("MinWithdrawCashScore", "1", websiteOwner);

            if (score < Convert.ToDecimal(minWithdrawCashScore))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "提现数额不能少于" + minWithdrawCashScore + moduleName;
                bllKeyValueData.ContextResponse(context, apiResp);
                return;
            }

            double curTotalScore = CurrentUserInfo.TotalScore;
            double sTotalScore = curTotalScore - score;
            double nScore = Convert.ToDouble(minScore)+score;
            if (sTotalScore< Convert.ToDouble(minScore))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = moduleName + "不足";
                bllKeyValueData.ContextResponse(context, apiResp);
                return;
            }
            decimal rechargeFee = Convert.ToDecimal(rechargeValue);
            decimal money = rechargeFee / 100 * score;


            WithdrawCash model = new WithdrawCash();
            model.Amount = money;
            model.UserId = CurrentUserInfo.UserID;
            model.TrueName = CurrentUserInfo.TrueName;
            model.WebSiteOwner = websiteOwner;
            model.WithdrawCashType = "ScoreOnLine";
            model.TransfersType = 1;
            model.Status = 0;
            model.Score = score;
            model.RealAmount = money;
            model.ServerFee = 0;
            model.Phone = CurrentUserInfo.Phone;
            model.LastUpdateDate = DateTime.Now;
            model.InsertDate = DateTime.Now;
            model.IsPublic = 2;

            //积分明细
            UserScoreDetailsInfo scoreModel = new UserScoreDetailsInfo();
            scoreModel.AddNote = string.Format("申请提现消耗{0}{1}", score, moduleName);
            scoreModel.AddTime = DateTime.Now;
            scoreModel.Score = 0-score;
            scoreModel.UserID = CurrentUserInfo.UserID;
            scoreModel.ScoreType = "WithdrawCash";
            scoreModel.TotalScore = sTotalScore;
            scoreModel.WebSiteOwner = websiteOwner;

            //通知
            BLLJIMP.Model.SystemNotice notice = new BLLJIMP.Model.SystemNotice();
            notice.Ncontent = scoreModel.AddNote;
            notice.UserId = CurrentUserInfo.UserID;
            notice.Receivers = CurrentUserInfo.UserID;
            notice.SendType = 2;
            notice.SerialNum = bllKeyValueData.GetGUID(TransacType.SendSystemNotice);
            notice.Title = "申请提现消耗淘股币";
            notice.NoticeType = 1;
            notice.WebsiteOwner = websiteOwner;
            notice.InsertTime = DateTime.Now;

            BLLTransaction tran = new BLLTransaction();
            if (bllUser.Update(CurrentUserInfo, 
                string.Format("TotalScore=TotalScore-{0}",score),
                string.Format("AutoID={0} AND WebsiteOwner='{1}' AND TotalScore>{2}", CurrentUserInfo.AutoID, websiteOwner, nScore), 
                tran)>0
                && bllUser.Add(scoreModel, tran)
                && bllUser.Add(notice, tran)
                && bllUser.Add(model, tran))
            {
                tran.Commit();
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = "申请成功";
                apiResp.status = true;
            }
            else
            {
                tran.Rollback();
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "申请失败";
            }
            bllKeyValueData.ContextResponse(context, apiResp);
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