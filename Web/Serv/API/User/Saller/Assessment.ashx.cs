using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.Saller
{
    /// <summary>
    /// 评价商家
    /// </summary>
    public class Assessment : UserBaseHandler
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

                var sallerId = context.Request["sallerId"];
                var reputationScore = Convert.ToDouble(context.Request["reputationScore"]);
                var serviceAttitudeScore = Convert.ToDouble(context.Request["serviceAttitudeScore"]);
                var comment = context.Request["comment"];
                //reputationScore serviceAttitudeScore comment

                resp.isSuccess = bllSaller.RateSaller(sallerId, bll.GetCurrUserID(), reputationScore, serviceAttitudeScore, comment);

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