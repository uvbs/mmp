using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Stock
{
    /// <summary>
    /// List 的摘要说明   缺货登记列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 商城逻辑层
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();

        public void ProcessRequest(HttpContext context)
        {

            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string productId = context.Request["product_id"];
            string productName = context.Request["product_name"];

            int total = 0;

            List<ProductStock> stockList = bllMall.GetProductStockList(pageIndex,pageSize,productId,productName,out total);

            var list = from p in stockList
                       select new
                       {
                           autoid=p.AutoId,
                           sku_id=p.SkuId,
                           pid=p.ProductId,
                           title=p.PName,
                           time=p.CreateDate,
                           count=p.Count,
                           head_img=p.WXHeadimgurl,
                           nick_name=p.WXNickname,
                           props = bllMall.GetProductStockProperties(p.Props)
                       };

            apiResp.msg = "查询完成";
            apiResp.status = true;

            apiResp.result = new
            {
                totalcount = total,
                list = list//列表
            };
            bllMall.ContextResponse(context, apiResp);



        }

    }
}