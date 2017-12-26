using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Account
{
    /// <summary>
    /// 余额变动 增加或减少
    /// </summary>
    public class AddAmount : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 用户BLL
        /// </summary>
       // BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();

        public void ProcessRequest(HttpContext context)
        {
            string autoId=context.Request["autoid"];
            decimal amount=decimal.Parse(context.Request["amount"]);
            string addNote = context.Request["msg"];

            if (string.IsNullOrEmpty(autoId))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg= "autoId 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            
            //if (string.IsNullOrEmpty(amount))
            //{
            //    apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
            //    apiResp.msg = "amount 参数必传";
            //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
            //    return;
            //}
            string msg = "";
            if (bllUser.UpdateAccountAmount(currentUserInfo.UserID,autoId,amount, addNote, out msg))
            {
                apiResp.status = true;
                apiResp.msg = "ok";
            }
            else
            {
                apiResp.msg = msg;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));


        } 

    }
}