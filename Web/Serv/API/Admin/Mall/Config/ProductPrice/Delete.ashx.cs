using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Config.ProductPrice
{
    /// <summary>
    ///删除商品价格配置
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {


        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];
            if (bllMall.Delete(new ProductPriceConfig(),string.Format(" AutoId in({0}) And WebsiteOwner='{1}'",ids,bllMall.WebsiteOwner))==ids.Split(',').Count())
            {
                apiResp.status = true;
                apiResp.msg = "ok";

            }
            else
            {
                apiResp.code = -1;
                apiResp.msg = "操作失败";

            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
        }


    }
}