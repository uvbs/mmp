using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.SVCard
{
    /// <summary>
    /// 修改储值卡状态
    /// </summary>
    public class UpdateStatus : BaseHandlerNeedLoginAdminNoAction
    {
        BLLStoredValueCard bll = new BLLStoredValueCard();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];
            string status = context.Request["status"];
            if (bll.UpdateMultByKey<StoredValueCard>("AutoId", ids, "Status", status, null, true)>0)
            {
                apiResp.status = true;
                apiResp.msg = "批量修改储值卡状态成功";
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.msg = "批量修改储值卡状态失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bll.ContextResponse(context, apiResp);
        }
    }
}