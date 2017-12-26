using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Booking
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNoAction
    {
        BLLMall bllMall = new BLLMall();
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        public void ProcessRequest(HttpContext context)
        {
            string type = context.Request["type"];
            string cate_id = context.Request["cate_id"];
            string keyword = context.Request["keyword"];
            string sort = context.Request["sort"];
            string rows = context.Request["rows"];
            string page = context.Request["page"];
            int total =0;
            var data = bllMall.GetProductList(keyword, cate_id, null, sort,
                page, rows, null, null, null, null, null, null, null, null, null, null,
                null, null, null, null, null, out  total, 0, false, type);
            
            bool isAdded = type.Contains("Added");
            ArticleCategoryTypeConfig nCategoryTypeConfig = bllArticleCategory.GetArticleCategoryTypeConfig(bllArticleCategory.WebsiteOwner, type);
            if(nCategoryTypeConfig == null) nCategoryTypeConfig = new ArticleCategoryTypeConfig();
            List<dynamic> list = new List<dynamic>();
            foreach (var item in data)
            {
                List<WXMallProductInfo> relList = new List<WXMallProductInfo>();
                if (!string.IsNullOrWhiteSpace(item.RelationProductId))
                {
                    string pIDStrings = "'" + item.RelationProductId.Replace(",", "','") + "'";
                    relList = bllMall.GetColMultListByKey<WXMallProductInfo>(int.MaxValue, 1, "PID", pIDStrings, "PID,PName,Price,Unit");
                }
                List<ProductSku> skuList = bllMall.GetColList<ProductSku>(int.MaxValue, 1, string.Format("ProductId={0}", item.PID), "SkuId,ProductId,PropValueIdEx1,PropValueIdEx2,PropValueIdEx3,Price");
                list.Add(new
                {
                    product_id = item.PID,
                    category_name = bllMall.GetArticleCategoryName(item.CategoryId),
                    title = item.PName,
                    summary = item.Summary,
                    access_level = item.AccessLevel,
                    count=item.Stock,
                    price = item.Price,
                    img = item.RecommendImg,
                    imgs = item.ShowImage,
                    unit = item.Unit,
                    relation_products = from p in relList
                                        select new {
                                            product_id =p.PID,
                                            title = p.PName,
                                            price = p.Price,
                                            unit =p.Unit,
                                            sku_id = bllMall.GetProductSkuId(p.PID)
                                        },
                    sku_list = from p in skuList.Where(p => checkEx(isAdded,nCategoryTypeConfig.TimeSetMethod,p)).OrderBy(p=>p.PropValueIdEx1)
                               select new
                               {
                                   sku_id = p.SkuId,
                                   product_id = p.ProductId,
                                   price = p.Price,
                                   ex1 = p.PropValueIdEx1,
                                   ex2 = p.PropValueIdEx2,
                                   ex3 = p.PropValueIdEx3
                               },
                    sku_id = skuList.Count ==0? 0: skuList[0].SkuId
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
        private bool checkEx(bool isAdded, int timeSetMethod, ProductSku sku)
        {
            if (isAdded) return false;
            if (timeSetMethod != 1 && timeSetMethod!=2) return false;
            if (timeSetMethod == 1)
            {
                return !string.IsNullOrWhiteSpace(sku.PropValueIdEx1) && DateTime.Parse(sku.PropValueIdEx1) > DateTime.Now;
            }
            return true;
        }
    }
}