using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Appointment
{
    /// <summary>
    /// 约会统计
    /// </summary>
    public class GetAppointmentStatistics : BaseHandlerNeedLoginNoAction
    {
        BLLJuActivity bll = new BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            //发布数
            int pubCount = bll.GetJuActivityCount("Appointment", null, CurrentUserInfo.UserID, true);
            //参与数
            int joinCount = 0;
            bll.GetRangeSignUpList(0, 0, null, bll.WebsiteOwner, null, null, null, "99", out joinCount);
            joinCount = joinCount + pubCount;

            //成功数
            int successCount = 0;
            bll.GetJuActivityList("Appointment", null, out successCount, 0, 0, CurrentUserInfo.UserID, null, null, bll.WebsiteOwner
                ,null,null,null,null,null,false,null,false,true,"2");
            int signSuccessCount = 0;
            bll.GetRangeSignUpList(0, 0, null, bll.WebsiteOwner, null, null, "2,3", "99", out signSuccessCount);
            successCount = successCount + signSuccessCount;

            //违约数
            int breachCount = 0;
            bll.GetJuActivityList("Appointment", null, out breachCount, 0, 0, CurrentUserInfo.UserID, null, null, bll.WebsiteOwner
                , null, null, null, null, null, false, null, false, true, "-1");

            int signbreachCount = 0;
            bll.GetRangeSignUpList(0, 0, null, bll.WebsiteOwner, null, null, "-3", "99", out signbreachCount);
            breachCount = breachCount + signbreachCount;

            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "查询完成";
            apiResp.status = true;
            apiResp.result = new
            {
                publish_count = pubCount,
                join_count = joinCount,
                success_count = successCount,
                breach_count = breachCount
            };
            bll.ContextResponse(context,apiResp);
        }
    }
}