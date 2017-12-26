using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Course
{
    /// <summary>
    /// 课程列表
    /// </summary>
    public class List :BaseHandlerNoAction
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        
        {

            int totalCount = 0;
            var sourceData = bllMall.GetProductList(context, out totalCount);
            var list = from p in sourceData
                       select new
                       {
                           product_id = p.PID,
                           title = p.PName,
                           quote_price = p.PreviousPrice,
                           price = p.Price,
                           category_id = p.CategoryId,
                           category_name = bllMall.GetWXMallCategoryName(p.CategoryId),
                           img_url = bllMall.GetImgUrl(p.RecommendImg),
                           is_onsale = (!string.IsNullOrEmpty(p.IsOnSale) && p.IsOnSale == "1") ? 1 : 0,
                           pv = p.PV,
                           tags = p.Tags,
                           sale_count = p.SaleCount,



                       };
            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                totalcount = totalCount,
                list = list,//列表
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

            
        }

        
    }
}