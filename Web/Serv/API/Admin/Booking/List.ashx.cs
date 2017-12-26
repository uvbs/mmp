
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Booking
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLMall bllMall = new BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            string type = context.Request["type"];
            string cate_id = context.Request["cate_id"];
            string keyword = context.Request["keyword"];
            string sort = context.Request["sort"];
            string rows = context.Request["rows"];
            string page = context.Request["page"];
            int total = 0;
            List<WXMallProductInfo> data = bllMall.GetProductList(keyword, null, cate_id, sort,
                page, rows, null, null, null, "IsOnSale", null, null, null, null, null, null,
                null, null, null, null, null, out  total, 0, false, type);
            List<dynamic> list = new List<dynamic>();
            foreach (var item in data)
            {
                List<WXMallProductInfo> relList = new List<WXMallProductInfo>();
                if (!string.IsNullOrWhiteSpace(item.RelationProductId))
                {
                    string pIDStrings = "'" + item.RelationProductId.Replace(",","','") + "'";
                    relList = bllMall.GetColMultListByKey<WXMallProductInfo>(int.MaxValue, 1, "PID", pIDStrings, "PID,PName,Price,Unit",true);
                }
                list.Add(new
                       {
                           product_id = item.PID,
                           category_id = item.CategoryId,
                           category_name = bllMall.GetArticleCategoryName(item.CategoryId),
                           title = item.PName,
                           summary = item.Summary,
                           access_level = item.AccessLevel,
                           quote_price = item.PreviousPrice,
                           price = item.Price,
                           img = item.RecommendImg,
                           unit = item.Unit,
                           status = item.IsOnSale,
                           sort = item.Sort,
                           relation_products = from p in relList
                                               select new
                                               {
                                                    product_id = p.PID,
                                                    title = p.PName,
                                                    price = p.Price,
                                                    unit = p.Unit
                                               }
                       });
            }

            apiResp.result = new
            {
                totalcount = total,
                list = list
            };
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllMall.ContextResponse(context, apiResp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}