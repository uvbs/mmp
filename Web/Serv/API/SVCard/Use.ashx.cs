using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.SVCard
{
    /// <summary>
    /// 使用储值卡
    /// </summary>
    public class Use : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 
        /// </summary>
        BLLStoredValueCard bll = new BLLStoredValueCard();
        /// <summary>
        /// 
        /// </summary>
        BLLWeixin bllWeixin = new BLLWeixin();
        /// <summary>
        /// 
        /// </summary>
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            BLLTransaction tran = new BLLTransaction();
            try
            {

                string id = context.Request["id"];//id
                string useAmountStr = context.Request["useAmount"];
                decimal useAmount = 0;//使用金额
                string remark = context.Request["remark"];//备注
                if (!decimal.TryParse(useAmountStr, out useAmount))
                {
                    apiResp.msg = "使用金额不正确";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                if (useAmount <= 0)
                {
                    apiResp.msg = "使用金额需要大于0";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                if (!bllUser.IsWeixinKefu(CurrentUserInfo))
                {
                    apiResp.msg = "拒绝操作";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                StoredValueCardRecord record = bll.GetByKey<StoredValueCardRecord>("AutoId", id, websiteOwner: bll.WebsiteOwner);
                if (record == null)
                {
                    apiResp.msg = "储值卡未找到";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }

                if (record.ValidTo.HasValue && record.ValidTo.Value < DateTime.Now)
                {
                    apiResp.msg = "储值卡已过期";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                decimal maxUseAmount = (record.Amount - bll.GetUseRecordList(record.AutoId, record.UserId).Sum(p => p.UseAmount));
                if (useAmount > maxUseAmount)
                {
                    apiResp.msg = string.Format("储值卡最多可以使用{0}元", maxUseAmount);
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                record.UseDate = DateTime.Now;
                record.Status = 9;
                if (!bll.Update(record, tran))
                {
                    tran.Rollback();
                    apiResp.msg = "操作失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                StoredValueCardUseRecord storeValueCardUseRecord = new StoredValueCardUseRecord();
                storeValueCardUseRecord.UseAmount = useAmount;
                storeValueCardUseRecord.CardId = record.CardId;
                storeValueCardUseRecord.Remark = string.Format("{0}使用{1}元", remark, useAmount);
                storeValueCardUseRecord.UseDate = DateTime.Now;
                storeValueCardUseRecord.UserId = record.UserId;
                storeValueCardUseRecord.UseUserId = record.UserId;
                if (!string.IsNullOrEmpty(record.ToUserId))
                {
                    storeValueCardUseRecord.UseUserId = record.ToUserId;
                }
                storeValueCardUseRecord.WebsiteOwner = bll.WebsiteOwner;
                storeValueCardUseRecord.MyCardId = record.AutoId;
                if (!bll.Add(storeValueCardUseRecord, tran))
                {
                    tran.Rollback();
                    apiResp.msg = "操作失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                tran.Commit();
                apiResp.status = true;
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.msg = string.Format("储值卡已成功使用{0}元", useAmount);
                UserInfo userInfo=bllUser.GetUserInfo(storeValueCardUseRecord.UseUserId,storeValueCardUseRecord.WebsiteOwner);
                bllWeixin.SendTemplateMessageNotifyComm(userInfo, "储值卡消费通知", storeValueCardUseRecord.Remark, string.Format("http://{0}/App/SvCard/Wap/UseRecord.aspx?id={1}",context.Request.Url.Host,id));



            }
            catch (Exception ex)
            {
                tran.Rollback();
                apiResp.msg = ex.ToString();
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