using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.GroupBuyRule
{
    /// <summary>
    /// 拼团规则列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {

            var sourceData = bllMall.GetProductGroupBuyRuleList();
            var list = from p in sourceData
                       select new
                       {
                           rule_id = p.RuleId,
                           rule_name=p.RuleName,
                           head_discount = p.HeadDiscount,
                           member_discount = p.MemberDiscount,
                           people_count = p.PeopleCount,
                           expire_day=p.ExpireDay,
                           modify_time=bllMall.GetTimeStamp(p.ModifyDate),
                           modify_time_formart=p.ModifyDate.ToString()
                       };
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                totalcount = sourceData.Count,
                list = list,//列表
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }



    }
}