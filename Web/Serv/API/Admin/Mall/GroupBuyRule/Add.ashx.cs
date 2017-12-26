using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.GroupBuyRule
{
    /// <summary>
    /// 添加拼团规则
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string ruleName = context.Request["rule_name"];
            string headDiscount = context.Request["head_discount"];
            string memberDiscount = context.Request["member_discount"];
            string peopleCount = context.Request["people_count"];
            string expireDay = context.Request["expire_day"];
            if (string.IsNullOrEmpty(headDiscount))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "head_discount 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (string.IsNullOrEmpty(memberDiscount))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "member_discount 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (string.IsNullOrEmpty(peopleCount))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "people_count 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            if (string.IsNullOrEmpty(expireDay))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "expire_day 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }
            string msg = "";
            bool result = bllMall.AddProductGroupBuyRule(ruleName, decimal.Parse(headDiscount), decimal.Parse(memberDiscount), int.Parse(peopleCount), int.Parse(expireDay), currentUserInfo.UserID, out msg);
            if (result)
            {

                apiResp.status = true;
                apiResp.msg = "ok";

            }
            else
            {
                apiResp.msg = msg;
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));



        }
    }
}