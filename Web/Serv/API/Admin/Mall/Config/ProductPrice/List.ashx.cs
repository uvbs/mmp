using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Config.ProductPrice
{
    /// <summary>
    /// 商品价格配置
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string productId=context.Request["product_id"];

            List<ProductPriceConfig> sourceList = bllMall.GetList<ProductPriceConfig>(string.Format(" WebsiteOwner='{0}' And ProductId='{1}'",bllMall.WebsiteOwner,productId));
            var list = from p in sourceList
                       select new
                       {
                           id=p.AutoId,
                           product_id=p.ProductId,
                           date=p.Date,
                           price=p.Price,
                          
                       };
            var result = new
            {
                totalcount = sourceList.Count,
                list = list

            };
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = result;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));


           
        }





    }
}