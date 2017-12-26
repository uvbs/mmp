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
    /// Update 的摘要说明
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        BLLMall bllMall = new BLLMall();
        public void ProcessRequest(HttpContext context)
        {
            Add.ProductModel productRequestModel = new Add.ProductModel();//订单模型
            try
            {
                productRequestModel = bllMall.ConvertRequestToModel<Add.ProductModel>(productRequestModel);
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
            WXMallProductInfo productInfo = bllMall.GetByKey<WXMallProductInfo>("PID", productRequestModel.product_id, true);
            if (productInfo == null)
            {
                apiResp.msg = "数据未找到";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bllMall.ContextResponse(context, apiResp);
                return;
            }
            productInfo.PName = productRequestModel.product_title;
            productInfo.PDescription = productRequestModel.product_desc;
            productInfo.Price = productRequestModel.price;
            productInfo.CategoryId = productRequestModel.category_id;
            productInfo.IsOnSale = productRequestModel.is_onsale.ToString();
            productInfo.Stock = productRequestModel.totalcount;//（容纳人数）
            productInfo.Sort = productRequestModel.sort;
            //productInfo.UserID = currentUserInfo.UserID; 
            productInfo.PreviousPrice = productRequestModel.price;
            productInfo.Summary = productRequestModel.product_summary;
            productInfo.LastUpdate = DateTime.Now;
            productInfo.RelationProductId = productRequestModel.relation_product_id;
            productInfo.AccessLevel = productRequestModel.access_Level;
            productInfo.Unit = productRequestModel.unit;
            if (!string.IsNullOrWhiteSpace(productRequestModel.show_imgs))
            {
                productInfo.ShowImage = productRequestModel.show_imgs;
                List<string> imgs = productRequestModel.show_imgs.Split(',').Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
                productInfo.RecommendImg = imgs[0];
                int max = imgs.Count > 5 ? 5 : imgs.Count;
                for (int i = 0; i < max; i++)
                {
                    if (i == 0) { productInfo.ShowImage1 = imgs[0]; }
                    else if (i == 1) { productInfo.ShowImage2 = imgs[1]; }
                    else if (i == 2) { productInfo.ShowImage3 = imgs[2]; }
                    else if (i == 3) { productInfo.ShowImage4 = imgs[3]; }
                    else if (i == 4) { productInfo.ShowImage5 = imgs[4]; }
                }
            }

            string dSkuIds = "";
            //默认第一条sku
            ProductSku productSku = bllMall.GetProductSku(productRequestModel.product_id);
            if (productSku == null)
            {
                //增加系统默认sku
                productSku = new ProductSku();//
                productSku.InsertDate = DateTime.Now;
                productSku.Stock = 1;
                productSku.WebSiteOwner = bllMall.WebsiteOwner;
                productSku.ArticleCategoryType = productInfo.ArticleCategoryType;
            }
            List<ProductSku> addSkuList = new List<ProductSku>();
            List<ProductSku> updateSkuList = new List<ProductSku>();
            if (productRequestModel.time_set_method == 0)
            {
                productSku.Price = productInfo.Price;
                updateSkuList.Add(productSku);

                List<ProductSku> dSkuList = bllMall.GetColList<ProductSku>(int.MaxValue, 1, string.Format("ProductId={0} AND SkuId!={1} ", productInfo.PID, productSku.SkuId), "SkuId");
                if (dSkuList.Count > 0)
                {
                    dSkuIds = ZentCloud.Common.MyStringHelper.ListToStr(dSkuList.Select(p=>p.SkuId).ToList(),"",",");
                }
            }
            else if (productRequestModel.time_set_method == 1 || productRequestModel.time_set_method == 2)
            {
                List<Add.timeModel> skuModelList = JSONHelper.JsonToModel<List<Add.timeModel>>(productRequestModel.time_data);
                string pSkuIds = ZentCloud.Common.MyStringHelper.ListToStr(skuModelList.Select(p=>p.sku_id).ToList(),"",",");
                if (string.IsNullOrWhiteSpace(pSkuIds)) pSkuIds = "0";
                List<ProductSku> dSkuList = bllMall.GetColList<ProductSku>(int.MaxValue, 1, string.Format("ProductId={0} AND SkuId Not In ({1}) ", productInfo.PID, pSkuIds), "SkuId");
                if (dSkuList.Count > 0)
                {
                    dSkuIds = ZentCloud.Common.MyStringHelper.ListToStr(dSkuList.Select(p=>p.SkuId).ToList(),"",",");
                }
                foreach (Add.timeModel item in skuModelList)
                {
                    ProductSku nSku = (ProductSku)productSku.Clone();
                    if (item.sku_id != 0)
                    {
                        nSku = bllMall.GetByKey<ProductSku>("SkuId", item.sku_id.ToString());
                    }
                    nSku.PropValueIdEx1 = item.ex1;
                    nSku.PropValueIdEx2 = item.ex2;
                    nSku.PropValueIdEx3 = item.ex3;
                    nSku.Price = item.price;
                    if (item.sku_id != 0)
                    {
                        updateSkuList.Add(nSku);
                    }
                    else
                    {
                        addSkuList.Add(nSku);
                    }
                }
            }

            BLLTransaction tran = new BLLTransaction();
            bool result = bllMall.Update(productInfo, tran);
            if (!result)
            {
                tran.Rollback();
                apiResp.msg = "更新失败";
                apiResp.code = (int)APIErrCode.OperateFail;
                bllMall.ContextResponse(context, apiResp);
                return;
            }
            if (!string.IsNullOrWhiteSpace(dSkuIds))
            {
                result = bllMall.DeleteMultByKey<ProductSku>("SkuId", dSkuIds,tran)>=0;
                if (!result)
                {
                    tran.Rollback();
                    apiResp.msg = "删除旧时间段失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllMall.ContextResponse(context, apiResp);
                    return;
                }
            }
            foreach (ProductSku item in updateSkuList)
            {
                result = bllMall.Update(item, tran);
                if (!result)
                {
                    tran.Rollback();
                    apiResp.msg = "修改Sku失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllMall.ContextResponse(context, apiResp);
                    return;
                }
            }
            int productId = int.Parse(productInfo.PID);
            foreach (ProductSku item in addSkuList)
            {
                item.ProductId = productId;
                item.SkuId = int.Parse(bllMall.GetGUID(BLLJIMP.TransacType.AddProductSku));
                result = bllMall.Add(item, tran);
                if (!result)
                {
                    tran.Rollback();
                    apiResp.msg = "新增Sku失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bllMall.ContextResponse(context, apiResp);
                    return;
                }
            }
            tran.Commit();
            apiResp.status = true;
            apiResp.msg = "更新完成";
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllMall.ContextResponse(context, apiResp);
        }
    }
}