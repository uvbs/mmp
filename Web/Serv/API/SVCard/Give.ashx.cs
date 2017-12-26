using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.SVCard
{
    /// <summary>
    /// Give 的摘要说明
    /// </summary>
    public class Give : BaseHandlerNeedLoginNoAction
    {
        BLLStoredValueCard bll = new BLLStoredValueCard();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string websiteOwner = bll.WebsiteOwner;
            StoredValueCardRecord record = bll.GetByKey<StoredValueCardRecord>("AutoId", id, websiteOwner: websiteOwner);
            if (record == null)
            {
                apiResp.msg = "储值卡未找到";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }
            //if (record.Status == 9)
            //{
            //    apiResp.msg = "储值卡已使用";
            //    apiResp.code = (int)APIErrCode.OperateFail;
            //    bll.ContextResponse(context, apiResp);
            //    return;
            //}
            if (record.Status == 1)
            {
                apiResp.msg = "储值卡已转赠他人";
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
            if (record.UserId == CurrentUserInfo.UserID)
            {
                apiResp.msg = " 不能领取自己分享的储值卡";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (bll.Update(record, string.Format("Status=1,ToUserId='{0}',ToDate=GetDate()", CurrentUserInfo.UserID), string.Format("AutoId={0} And WebsiteOwner='{1}'  And (ValidTo Is Null Or ValidTo > GetDate()) ", record.AutoId, websiteOwner)) <= 0)
            {

                apiResp.msg = "领取好友转赠的储值卡失败";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }
            apiResp.msg = "领取好友转赠的储值卡成功";
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            bll.ContextResponse(context, apiResp);
        }

        //public bool IsReusable
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}
    }
}