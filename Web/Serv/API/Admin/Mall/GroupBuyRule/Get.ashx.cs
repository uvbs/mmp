using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.GroupBuyRule
{
    /// <summary>
    /// 获取单个规则
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string ruleId = context.Request["rule_id"];
            if (string.IsNullOrEmpty(ruleId))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "rule_id 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }

            var data = bllMall.GetProductGroupBuyRule(int.Parse(ruleId));
            if (data==null)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "规则不存在";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return; 
            }
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                rule_id = data.RuleId,
                rule_name = data.RuleName,
                head_discount = data.HeadDiscount,
                member_discount = data.MemberDiscount,
                people_count = data.PeopleCount,
                expire_day = data.ExpireDay,
                modify_time = bllMall.GetTimeStamp(data.ModifyDate),
                modify_time_formart = data.ModifyDate.ToString()
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }



    }
}