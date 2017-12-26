using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Booking
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLMall bllMall = new BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            ProductModel productRequestModel = new ProductModel();//订单模型
            try
            {
                productRequestModel = bllMall.ConvertRequestToModel<ProductModel>(productRequestModel);
            }
            catch (Exception ex)
            {
                apiResp.msg = "提交格式错误";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllMall.ContextResponse(context, apiResp);
                return;
            }
            //数据检查
            if (string.IsNullOrEmpty(productRequestModel.product_title))
            {
                apiResp.msg = "商品名称必填";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllMall.ContextResponse(context, apiResp);
                return;
            }
            //if (productRequestModel.price == 0)
            //{
            //    apiResp.msg = "商品价格必填";
            //    apiResp.code = (int)APIErrCode.OperateFail;
            //    bllMall.ContextResponse(context, apiResp);
            //    return;
            //}
            //if (string.IsNullOrWhiteSpace(productRequestModel.show_imgs) && !productRequestModel.article_category_type.Contains("Added"))
            //{
            //    apiResp.msg = "请上传商品图片";
            //    apiResp.code = (int)APIErrCode.OperateFail;
            //    bllMall.ContextResponse(context, apiResp);
            //    return;
            //}
            WXMallProductInfo productModel = new WXMallProductInfo();
            productModel.PName = productRequestModel.product_title;
            productModel.PDescription = productRequestModel.product_desc;
            productModel.Price = productRequestModel.price;
            productModel.ArticleCategoryType = productRequestModel.article_category_type;
            productModel.CategoryId = productRequestModel.category_id;
            productModel.IsOnSale = productRequestModel.is_onsale.ToString();
            productModel.Stock = productRequestModel.totalcount;//（容纳人数）
            productModel.WebsiteOwner = bllMall.WebsiteOwner;
            productModel.Sort = productRequestModel.sort;
            productModel.UserID = currentUserInfo.UserID;
            productModel.PreviousPrice = productRequestModel.price;
            productModel.Summary = productRequestModel.product_summary;
            productModel.InsertDate = DateTime.Now;
            productModel.LastUpdate = DateTime.Now;
            productModel.RelationProductId = productRequestModel.relation_product_id;
            productModel.AccessLevel = productRequestModel.access_Level;
            productModel.Unit = productRequestModel.unit;
            if (!string.IsNullOrWhiteSpace(productRequestModel.show_imgs))
            {
                productModel.ShowImage = productRequestModel.show_imgs;
                List<string> imgs = productRequestModel.show_imgs.Split(',').Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
                productModel.RecommendImg = imgs[0];
                int max = imgs.Count > 5 ? 5 : imgs.Count;
                for (int i = 0; i < max; i++)
                {
                    if (i == 0) { productModel.ShowImage1 = imgs[0]; }
                    else if (i == 1) { productModel.ShowImage2 = imgs[1]; }
                    else if (i == 2) { productModel.ShowImage3 = imgs[2]; }
                    else if (i == 3) { productModel.ShowImage4 = imgs[3]; }
                    else if (i == 4) { productModel.ShowImage5 = imgs[4]; }
                }
            }
            //增加系统默认sku
            ProductSku productSkuDefault = new ProductSku();//
            productSkuDefault.InsertDate = DateTime.Now;
            productSkuDefault.Stock = 1;
            productSkuDefault.WebSiteOwner = bllMall.WebsiteOwner;
            productSkuDefault.ArticleCategoryType = productModel.ArticleCategoryType;

            List<ProductSku> skuList = new List<ProductSku>();
            if (productRequestModel.time_set_method == 0)
            {
                productSkuDefault.Price = productModel.Price;
                skuList.Add(productSkuDefault);
            }
            else if (productRequestModel.time_set_method == 1 || productRequestModel.time_set_method == 2)
            {
                if (string.IsNullOrWhiteSpace(productRequestModel.time_data))
                {
                    apiResp.msg = "请添加时间段";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllMall.ContextResponse(context, apiResp);
                    return;
                }

                List<timeModel> skuModelList = JSONHelper.JsonToModel<List<timeModel>>(productRequestModel.time_data);
                foreach (timeModel item in skuModelList)
                {
                    ProductSku nSku = (ProductSku)productSkuDefault.Clone();
                    nSku.PropValueIdEx1 = item.ex1;
                    nSku.PropValueIdEx2 = item.ex2;
                    nSku.PropValueIdEx3 = item.ex3;
                    nSku.Price = item.price;
                    skuList.Add(nSku);
                }
            }
            productModel.PID = bllMall.GetGUID(BLLJIMP.TransacType.AddWXMallProductID);
            BLLTransaction tran = new BLLTransaction();
            bool result = bllMall.Add(productModel, tran);
            if (!result)
            {
                tran.Rollback();
                apiResp.msg = "提交失败";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllMall.ContextResponse(context, apiResp);
                return;

            }
            int productId = int.Parse(productModel.PID);
            foreach (ProductSku item in skuList)
            {
                item.ProductId = productId;
                item.SkuId = int.Parse(bllMall.GetGUID(BLLJIMP.TransacType.AddProductSku));
                result = bllMall.Add(item, tran);
                if (!result)
                {
                    tran.Rollback();
                    apiResp.msg = "提交失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllMall.ContextResponse(context, apiResp);
                    return;
                }
            }
            tran.Commit();

            apiResp.msg = "提交完成";
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            bllMall.ContextResponse(context, apiResp);
        }
        /// <summary>
        /// 商品模型
        /// </summary>
        public class ProductModel
        {
            /// <summary>
            /// 商品编号
            /// </summary>
            public string product_id { get; set; }
            /// <summary>
            /// 商品名称
            /// </summary>
            public string product_title { get; set; }
            /// <summary>
            /// 类型
            /// </summary>
            public string article_category_type { get; set; }
            /// <summary>
            /// 分类ID
            /// </summary>
            public string category_id { get; set; }
            /// <summary>
            /// 现价
            /// </summary>
            public decimal price { get; set; }
            /// <summary>
            /// 总库存（容纳人数）
            /// </summary>
            public int totalcount { get; set; }
            /// <summary>
            ///在否上架 1上架 0下架
            /// </summary>
            public int is_onsale { get; set; }

            /// <summary>
            /// 商品简介
            /// </summary>
            public string product_summary { get; set; }
            /// <summary>
            /// 商品介绍
            /// </summary>
            public string product_desc { get; set; }
            /// <summary>
            /// 排序号
            /// </summary>
            public int sort { get; set; }
            /// <summary>
            /// 展示图片
            /// </summary>
            public string show_imgs { get; set; }
            /// <summary>
            /// 关联商品ID
            /// </summary>
            public string relation_product_id { get; set; }
            /// <summary>
            /// 访问等级
            /// </summary>
            public int access_Level { get; set; }
            /// <summary>
            /// 单位
            /// </summary>
            public string unit { get; set; }
            /// <summary>
            /// 时间方式 0日历时间 1设置时间 2设置周期
            /// </summary>
            public int time_set_method { get; set; }
            /// <summary>
            /// 时间段数据
            /// </summary>
            public string time_data { get; set; }
        }
        /// <summary>
        /// 时间model
        /// </summary>
        public class timeModel{
            /// <summary>
            /// sku的ID
            /// </summary>
            public int sku_id{ get; set; }
            /// <summary>
            /// 开始时间
            /// </summary>
            public string ex1 { get; set; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public string ex2 { get; set; }
            /// <summary>
            /// 星期几 1星期一 ~ 6星期六 0星期日
            /// </summary>
            public string ex3 { get; set; }
            /// <summary>
            /// 价格
            /// </summary>
            public decimal price { get; set; }
        }
    }
}