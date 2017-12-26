using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// TotalAmountTransferAccounts 的摘要说明
    /// </summary>
    public class TotalAmountTransferAccounts : BaseHandlerNeedLoginNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            if (CurrentUserInfo.IsLock == 1)
            {
                apiResp.code = (int)APIErrCode.NoPms;
                apiResp.msg = "账号已被锁定";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (CurrentUserInfo.MemberApplyStatus != 9)
            {
                apiResp.code = (int)APIErrCode.NoPms;
                apiResp.msg = "您的账号正在审核中";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            decimal amount = Convert.ToDecimal(context.Request["amount"]);
            int spreadid = Convert.ToInt32(context.Request["spreadid"]);
            string websiteOwner = bllUser.WebsiteOwner;
            WebsiteInfo website = bllUser.GetWebsiteInfoModelFromDataBase(websiteOwner);
            if (amount <= 0)
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = string.Format("转账{0}必须大于0",website.TotalAmountShowName);
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (CurrentUserInfo.TotalAmount < amount)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = string.Format("您的{0}不足", website.TotalAmountShowName);
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            UserInfo spread = bllUser.GetUserInfoByAutoID(spreadid, websiteOwner);
            if (spread == null)
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "会员未找到";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (spread.AutoID == CurrentUserInfo.AutoID)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "禁止转账给自己";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            string spreadname = bllUser.GetUserDispalyName(spread);
            string curname = bllUser.GetUserDispalyName(CurrentUserInfo);
            BLLTransaction tran = new BLLTransaction();
            #region 消耗操作人金额
            if (bllUser.AddScoreDetail(CurrentUserInfo.UserID, websiteOwner, (double)(0-amount),
                string.Format("转给{0}[{1}]", spreadname, spread.Phone), 
                "TotalAmount",(double)(CurrentUserInfo.TotalAmount - amount),
                "", "转账", "", "", (double)amount, 0, spread.UserID,
                tran) <= 0)
            {
                tran.Rollback();
                apiResp.msg = "记录转账明细出错";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (bllUser.Update(CurrentUserInfo, string.Format("TotalAmount=ISNULL(TotalAmount,0)-{0},AccountAmountEstimate=ISNULL(AccountAmountEstimate,0)-{0}", amount),
                string.Format("AutoID={0} And WebsiteOwner='{1}' And ISNULL(TotalAmount,0)-{2}>=0 ",
                CurrentUserInfo.AutoID, websiteOwner, amount),
                tran) <= 0)
            {
                tran.Rollback();
                apiResp.msg = string.Format("消耗报单人可用{0}出错", website.TotalAmountShowName);
                apiResp.code = (int)APIErrCode.OperateFail;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            #endregion
            #region 指定会员获得金额
            if (bllUser.AddScoreDetail(spread.UserID, websiteOwner, (double)(amount),
                string.Format("{0}[{1}]转入", curname, CurrentUserInfo.Phone),
                "TotalAmount", (double)(spread.TotalAmount + amount),
                "", "获得转账", "", "", (double)amount, 0, CurrentUserInfo.UserID,
                tran) <= 0)
            {
                tran.Rollback();
                apiResp.msg = "记录获得转账明细出错";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (bllUser.Update(spread, string.Format("TotalAmount=ISNULL(TotalAmount,0)+{0},AccountAmountEstimate=ISNULL(AccountAmountEstimate,0)+{0}", amount),
                string.Format("AutoID={0} And WebsiteOwner='{1}'",
                spread.AutoID, websiteOwner),
                tran) <= 0)
            {
                tran.Rollback();
                apiResp.msg = string.Format("增加会员可用{0}出错", website.TotalAmountShowName);
                apiResp.code = (int)APIErrCode.OperateFail;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            #endregion
            tran.Commit();

            //异步计算金额
            Thread th1 = new Thread(delegate()
            {
                bll.CheckTotalAmount(spread.AutoID, websiteOwner, 7);
            });
            th1.Start();

            apiResp.msg = "转账成功";
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            bllUser.ContextResponse(context, apiResp);
        }
    }
}