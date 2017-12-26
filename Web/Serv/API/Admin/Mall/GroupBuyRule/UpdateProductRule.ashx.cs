using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.GroupBuyRule
{
    /// <summary>
    /// 更新商品团购规则
    /// </summary>
    public class UpdateProductRule : BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string ruleIds = context.Request["rule_ids"];
            string productId=context.Request["product_id"];
            //if (string.IsNullOrEmpty(ruleIds))
            //{
            //    apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
            //    apiResp.msg = "rule_ids 参数必传";
            //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
            //    return;
            //}
            if (string.IsNullOrEmpty(productId))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "product_id 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                return;
            }

            string msg = "";
            bool result = bllMall.UpdateProductGroupBuyRule(productId,ruleIds, out msg);
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