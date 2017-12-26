using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.ScoreDefine
{
    /// <summary>
    /// PassWithdrawCash 的摘要说明
    /// </summary>
    public class PassWithdrawCash : BaseHandlerNeedLoginAdminNoAction
    {

        public void ProcessRequest(HttpContext context)
        {
            BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
            BllPay bllPay = new BllPay();
            BLLWeixin bllWeixin = new BLLWeixin();
            string ids = context.Request["ids"];
            string moduleName = "积分";
            if (!string.IsNullOrWhiteSpace(context.Request["module_name"])) moduleName = context.Request["module_name"];

            int total = 0;
            if (string.IsNullOrWhiteSpace(ids))
            {
                apiResp.msg = "请选中申请";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            var sourceList = bll.QueryWithdrawCashList(1, int.MaxValue, null, out total, "0", context.Request["type"], ids);
            if (sourceList.Count == 0)
            {
                apiResp.msg = "选中的申请无待审核申请";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }

            string websiteOwner = bll.WebsiteOwner;
            string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            int snum = 0;
            foreach (var p in sourceList)
            {
                UserInfo pu = bllUser.GetUserInfo(p.UserId, websiteOwner);
                if (pu == null || string.IsNullOrWhiteSpace(pu.WXOpenId))
                {
                    apiResp.status = snum>0;
                    apiResp.msg = string.Format("编号：{0}，审核出错，用户信息有误", p.AutoID);
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                //通知
                BLLJIMP.Model.SystemNotice notice = new BLLJIMP.Model.SystemNotice();
                notice.Ncontent = string.Format("申请提现通过审核，编号：{0}", p.AutoID);
                notice.UserId = currentUserInfo.UserID;
                notice.Receivers = pu.UserID;
                notice.SendType = 2;
                notice.Title = "申请提现通过审核";
                notice.NoticeType = 1;
                notice.WebsiteOwner = websiteOwner;
                notice.InsertTime = DateTime.Now;

                BLLTransaction tran = new BLLTransaction();
                bool result = bll.Update(p, string.Format("Status=2,LastUpdateDate=getdate()"), string.Format("AutoID={0}", p.AutoID), tran) > 0;
                if (!result)
                {
                    tran.Rollback();
                    apiResp.status = snum > 0;
                    apiResp.msg = string.Format("编号：{0}，审核出错", p.AutoID);
                    apiResp.code = snum > 0 ? (int)APIErrCode.IsSuccess : (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                string msg = "";
                if (bllPay.WeixinTransfers(p.AutoID.ToString(), p.RealAmount, pu.WXOpenId, ip, out msg, "提现"))
                {
                    //发送微信模板消息
                    bllWeixin.SendTemplateMessageNotifyComm(pu, "您提现的佣金已经到账", string.Format("提现金额:{0}元。请查看微信钱包", p.RealAmount));
                    //发送微信模板消息

                    //发送通知
                    notice.SerialNum = bllUser.GetGUID(TransacType.SendSystemNotice);
                    bll.Add(notice);
                    //发送通知
                }
                else//打款失败
                {
                    tran.Rollback();
                    apiResp.status = snum > 0;
                    apiResp.msg = string.Format("编号：{0}，微信打款出错 ： {1}", p.AutoID, msg);
                    apiResp.code = snum > 0 ? (int)APIErrCode.IsSuccess : (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }

                tran.Commit();
                snum++;
            }

            if (snum == 0)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "审核失败";
            }
            else
            {
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = string.Format("审核完成");
            }
            bll.ContextResponse(context, apiResp);
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