using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Expand
{
    /// <summary>
    /// 设置最后投保日期
    /// </summary>
    public class SetLastInsuranceDate : UserBaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            resp.isSuccess = false;

            try
            {
                if (!bll.IsLogin)
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                    bll.ContextResponse(context, resp);
                    return;
                }

                var userId = bll.GetCurrUserID();
                var type = BLLJIMP.Enums.UserExpandType.LastInsuranceDate;

                var value = context.Request["value"];

                resp.isSuccess = bllUserExpand.UpdateUserExpand(type, userId, value);
                resp.returnObj = resp.isSuccess;

            }
            catch (Exception ex)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }

            bll.ContextResponse(context, resp);
        }

    }
}