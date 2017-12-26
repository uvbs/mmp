using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    /// 快递
    /// </summary>
    public class Express : BaseHandler
    {
        /// <summary>
        /// 快递100
        /// </summary>
        BLLJIMP.BllKuaidi100 bll = new BLLJIMP.BllKuaidi100();
        /// <summary>
        /// 查询快递状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Get(HttpContext context)
        {

            string expressCompanyCode = context.Request["express_company_code"];
            string expressNumber = context.Request["express_number"];
            if (string.IsNullOrEmpty(expressCompanyCode) || string.IsNullOrEmpty(expressNumber))
            {
                resp.errcode = 1;
                resp.errmsg = " express_company_code,express_number参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            var map = bll.expressCompanyMap.SingleOrDefault(p => p.Key == expressCompanyCode);
            if (!string.IsNullOrEmpty(map.Key))
            {
                expressCompanyCode = map.Value;

            }
            var result = bll.GetExpressResult(expressNumber, expressCompanyCode);
            if (result!=null)
            {
                return "{\"errcode\":0,\"errmsg\":\"ok\",\"express_info\":" + result.ExpressContent+"}";
            }
            else
            {

                var queryResult = bll.Query(expressCompanyCode, expressNumber);
                if (queryResult!="[]")
                {
                    ExpressResult model = new ExpressResult();
                    model.ExpressCompanyCode = expressCompanyCode;
                    model.ExpressNumber = expressNumber;
                    model.WebsiteOwner = bll.WebsiteOwner;
                    model.ExpressContent = queryResult;
                    model.InsertDate = DateTime.Now;
                    model.LastUpdateDate = DateTime.Now;
                    bll.Add(model);
                    return "{\"errcode\":0,\"errmsg\":\"ok\",\"express_info\":" + queryResult + "}";
                }
                else
                {
                    return "{\"errcode\":1,\"errmsg\":\"没有物流信息\",\"express_info\":" + queryResult + "}";
                }
                

            }

        }








    }
}