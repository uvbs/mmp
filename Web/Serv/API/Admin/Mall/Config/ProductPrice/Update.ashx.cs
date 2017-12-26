using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Config.ProductPrice
{
    /// <summary>
    /// 更新商品价格配置
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string productId=context.Request["product_id"];
            string date=context.Request["date"];
            string price=context.Request["price"];
            ProductPriceConfig model = bllMall.Get<ProductPriceConfig>(string.Format(" AutoId={0} And WebsiteOwner='{1}'",id,bllMall.WebsiteOwner));
            model.ProductId = productId;
            model.Date = date;
            model.Price =decimal.Parse(price);
            if (bllMall.Update(model))
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