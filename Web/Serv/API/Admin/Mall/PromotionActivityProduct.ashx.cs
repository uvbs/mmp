using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall
{
    /// <summary>
    /// 限时特卖活动商品
    /// </summary>
    public class PromotionActivityProduct : BaseHandlerNeedLoginAdmin
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {


            int totalCount = 0;
            var sourceData = bllMall.GetPromotionProductList(context, out totalCount);
            var list = from p in sourceData
                       select new
                       {
                           product_id = p.PID,
                           category_id = p.CategoryId,
                           product_title = p.PName,
                           quote_price = p.PreviousPrice,
                           price = p.Price,
                           base_price=p.BasePrice,
                           promotion_price = bllMall.GetMinPrommotionPrice(int.Parse(p.PID)),
                           img_url = bllMall.GetImgUrl(p.RecommendImg),
                           is_onsale = (!string.IsNullOrEmpty(p.IsOnSale) && p.IsOnSale == "1") ? 1 : 0,
                           totalcount = bllMall.GetProductTotalStockPromotion(int.Parse(p.PID))

                       };

            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = totalCount,
                list = list
            });



        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {
            int promotionActivityId = int.Parse(context.Request["promotion_activity_id"]);
            string productIds = context.Request["product_ids"];

            BLLJIMP.Model.PromotionActivity promotionActivity = bllMall.GetPromotionActivity(promotionActivityId);
            if (promotionActivity == null)
            {
                resp.errcode = 1;
                resp.errmsg = "限时特卖活动不存在,请检查";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (bllMall.GetCount<BLLJIMP.Model.WXMallProductInfo>(string.Format(" PID in ({0}) And WebsiteOwner='{1}' And IsPromotionProduct=0 And (PromotionActivityId=0 Or PromotionActivityId Is NULL)", productIds, bllMall.WebsiteOwner)) != productIds.Split(',').Length)
            {
                resp.errcode = 1;
                resp.errmsg = "商品ID不存在或商品已经加入限时特卖，请检查";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }


            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                string sql = string.Format(" Update ZCJ_ProductSku Set PromotionStartTime={0},PromotionStopTime={1} where ProductId in({2})", promotionActivity.StartTime, promotionActivity.StopTime, productIds);//更新sku表
                sql += string.Format(" Update ZCJ_WXMallProductInfo Set IsPromotionProduct=1, PromotionStartTime={0},PromotionStopTime={1},PromotionActivityId={2} where PID in({3})", promotionActivity.StartTime, promotionActivity.StopTime, promotionActivity.ActivityId, productIds);//更新sku表
                ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sql, tran);
                resp.errmsg = "ok";
                tran.Commit();

                

            }
            catch (Exception ex)
            {
                resp.errcode = -1;
                resp.errmsg = ex.Message;
                tran.Rollback();

            }

            bllMall.ClearProductListCacheByWhere(string.Format(" PID in ({0}) ", productIds));

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {
            string data = context.Request["data"];
            ProcuctModel productRequestModel;
            try
            {
                productRequestModel = ZentCloud.Common.JSONHelper.JsonToModel<ProcuctModel>(data);

            }
            catch (Exception ex)
            {
                resp.errcode = 1;
                resp.errmsg = "JSON格式错误,请检查。错误信息:" + ex.Message;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }

            #region 检查

            foreach (var sku in productRequestModel.skus)
            {
                ProductSku productSku = bllMall.GetProductSku(sku.sku_id);
                if (productSku.ProductId != productRequestModel.product_id)
                {

                    resp.errcode = 1;
                    resp.errmsg = "sku product_id不匹配,请检查";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

                }
                if (sku.promotion_sale_count > productSku.Stock)
                {

                    resp.errcode = 1;
                    resp.errmsg = "特卖总库存不能大于" + productSku.Stock;
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (sku.promotion_count > sku.promotion_sale_count)
                {

                    resp.errcode = 1;
                    resp.errmsg = "特卖剩余库存不能大于" + sku.promotion_sale_count;
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (sku.promotion_sale_count > 0 && sku.promotion_price <= 0)
                {

                    resp.errcode = 1;
                    resp.errmsg = "有特卖总数量的特卖价格需大于0";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                //if (sku.promotion_price<=0)
                //{
                //    resp.errcode = 1;
                //    resp.errmsg = "特卖价格需大于0";
                //    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                //}
                if (sku.promotion_price > productSku.Price)
                {

                    resp.errcode = 1;
                    resp.errmsg = "特卖价格不能大于原价格";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

            }
            //检查 
            #endregion
            ProductSku temproductSku = bllMall.GetProductSku(productRequestModel.skus[0].sku_id);
            WXMallProductInfo tempProductInfo = bllMall.GetProduct(temproductSku.ProductId.ToString());
            BLLJIMP.Model.PromotionActivity promotionActivity = bllMall.GetPromotionActivity(tempProductInfo.PromotionActivityId);
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                foreach (var sku in productRequestModel.skus)
                {
                    ProductSku productSku = bllMall.GetProductSku(sku.sku_id);
                    productSku.PromotionPrice = sku.promotion_price;
                    productSku.PromotionStock = sku.promotion_count;
                    productSku.PromotionSaleStock = sku.promotion_sale_count;
                    if (productSku.PromotionSaleStock == 0)
                    {
                        productSku.PromotionSaleStock = sku.promotion_count;
                    }
                    if (sku.promotion_sale_count > 0)
                    {
                        productSku.PromotionSaleStock = sku.promotion_sale_count;
                    }
                    if (productSku.PromotionStartTime == 0 || productSku.PromotionStopTime == 0)
                    {
                        productSku.PromotionStartTime = promotionActivity.StartTime;
                        productSku.PromotionStopTime = promotionActivity.StopTime;
                    }
                    if (!bllMall.Update(productSku))
                    {
                        tran.Rollback();
                        resp.errcode = 1;
                        resp.errmsg = "修改失败";
                        return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                    }


                }
                WXMallProductInfo productInfo = bllMall.GetProduct(productRequestModel.product_id.ToString());
                productInfo.PromotionPrice = productRequestModel.skus.Min(p => p.promotion_price);
                if (!bllMall.Update(productInfo, tran))
                {
                    tran.Rollback();
                    resp.errcode = 1;
                    resp.errmsg = "操作失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                tran.Commit();
                resp.errmsg = "ok";
                
                
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resp.errcode = -1;
                resp.errmsg = ex.Message;

            }

            bllMall.ClearProductListCacheByWhere(string.Format(" PID='{0}' ", productRequestModel.product_id.ToString()));


            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 获取限时特卖商品详细
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Get(HttpContext context)
        {

            string productId = context.Request["product_id"];
            var skuSourceList = bllMall.GetProductSkuList(int.Parse(productId));//源SKU 
            //var defaultSku = skuSourceList.Where(p => string.IsNullOrEmpty(p.Props) && string.IsNullOrEmpty(p.PropValueIdEx1) && string.IsNullOrEmpty(p.PropValueIdEx2)).ToList();//默认sku
            ////if (defaultSku.Count > 0)
            ////{
            //    skuSourceList.Remove(defaultSku[0]);
            ////}
            skuSourceList = skuSourceList.OrderBy(p => p.PropValueIdEx1).ToList();
            var skus = from p in skuSourceList
                       select new
                       {
                           sku_id = p.SkuId,
                           properties = GetSkuProperties(bllMall.GetProductProperties(p.SkuId)),
                           show_properties = bllMall.GetProductShowProperties(p.SkuId),
                           price = p.Price,
                           count = p.Stock,
                           base_price=p.BasePrice,
                           sku_sn = p.SkuSN,
                           promotion_price = p.PromotionPrice,
                           promotion_count = p.PromotionStock,
                           promotion_sale_count = p.PromotionSaleStock
                       };
            if (skus.Count() > 0)
            {
                var properties = skus.FirstOrDefault().properties;
                if (properties != null)
                {
                    var tempSkus = skus.OrderBy(p => p.properties.FirstOrDefault(pi => pi.property_id == properties[0].property_id).property_value_id);
                    for (int i = 1; i < properties.Count; i++)
                    {
                        var pid = properties[i].property_id;
                        tempSkus = tempSkus.ThenBy(p => p.properties.FirstOrDefault(pi => pi.property_id == pid).property_value_id);
                    }
                    skus = tempSkus;
                }

            }

            var result = new
            {
                product_id = int.Parse(productId),
                have_property_list = GetHaveProperty(int.Parse(productId)),
                skus = skus
            };

            return ZentCloud.Common.JSONHelper.ObjectToJson(result);

        }



        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {
            int promotionActivityId = int.Parse(context.Request["promotion_activity_id"]);
            string productId = context.Request["product_id"];
            BLLJIMP.Model.PromotionActivity promotionActivity = bllMall.GetPromotionActivity(promotionActivityId);
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            try
            {
                string sql = string.Format(" Update ZCJ_ProductSku Set PromotionStartTime=0,PromotionStopTime=0,PromotionPrice=0,PromotionStock=0,PromotionSaleStock=0 where ProductId in ({0})", productId);
                sql += string.Format(" Update ZCJ_WXMallProductInfo Set PromotionStartTime=0,PromotionStopTime=0,IsPromotionProduct=0,PromotionActivityId=0 where PID in ({0})", productId);
                ZentCloud.ZCBLLEngine.BLLBase.ExecuteSql(sql, tran);
                resp.errmsg = "ok";
                tran.Commit();
                
            }
            catch (Exception ex)
            {
                resp.errcode = -1;
                resp.errmsg = ex.Message;
                tran.Rollback();

            }

            bllMall.ClearProductListCacheByWhere(string.Format(" PID in ({0}) ", productId));

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// sku属性模型
        /// </summary>
        public class SkuProperties
        {
            /// <summary>
            /// 特征量ID
            /// </summary>
            public int property_id { get; set; }
            /// <summary>
            /// 特征量名称
            /// </summary>
            public string property_name { get; set; }
            /// <summary>
            /// 特征量值ID
            /// </summary>
            public int property_value_id { get; set; }
            /// <summary>
            /// 特征量值名称
            /// </summary>
            public string property_value_name { get; set; }


        }

        /// <summary>
        /// 商品拥有的规格集合
        /// </summary>
        private class PropertyModel
        {
            // /// <summary>
            // /// 属性ID
            // /// </summary>
            //public int property_id { get; set; }
            // /// <summary>
            // /// 属性
            // /// </summary>
            //public string property_name { get; set; }
            public Property property { get; set; }
            /// <summary>
            /// 值
            /// </summary>
            public List<PropertyValueModel> property_value_list { get; set; }

        }

        private class Property
        {
            /// <summary>
            /// 属性ID
            /// </summary>
            public int property_id { get; set; }
            /// <summary>
            /// 属性
            /// </summary>
            public string property_name { get; set; }



        }


        /// <summary>
        /// 特征量值
        /// </summary>
        private class PropertyValueModel
        {
            /// <summary>
            /// 特征量值ID
            /// </summary>
            public int property_value_id { get; set; }
            /// <summary>
            /// 特征量值
            /// </summary>
            public string property_value_name { get; set; }

        }

        /// <summary>
        /// 转换sku属性字符串成对象 
        /// </summary>
        /// <param name="skuPropStr">示例 1:2:尺码:M;2:6:颜色:马其色 </param>
        /// <returns></returns>
        private List<SkuProperties> GetSkuProperties(string skuPropStr)
        {
            try
            {

                List<SkuProperties> list = new List<SkuProperties>();
                foreach (var item in skuPropStr.Split(';'))
                {
                    SkuProperties model = new SkuProperties();
                    string[] propArry = item.Split(':');
                    BLLJIMP.Model.ProductProperty property = bllMall.GetProductProperty(int.Parse(propArry[0]));
                    BLLJIMP.Model.ProductPropertyValue propValue = bllMall.GetProductPropertyValue(int.Parse(propArry[1]));
                    model.property_id = property.PropID;
                    model.property_name = property.PropName.Trim();
                    model.property_value_id = propValue.PropValueId;
                    model.property_value_name = propValue.PropValue.Trim();
                    list.Add(model);

                }
                return list;
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        /// <summary>
        /// 获取商品拥有的属性集合 如颜色 有几种颜色,尺寸有几种尺寸
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private List<PropertyModel> GetHaveProperty(int productId)
        {
            try
            {
                var skuList = bllMall.GetProductSkuList(productId);
                List<string> propIdList = new List<string>();//获取商品拥有的所有属性ID集合
                Dictionary<int, string> propKeyValue = new Dictionary<int, string>();
                foreach (var sku in skuList)
                {

                    if (!string.IsNullOrEmpty(sku.PropValueIdEx1) && !string.IsNullOrEmpty(sku.PropValueIdEx2))
                    {
                        sku.Props = bllMall.GetProductProperties(sku.SkuId);//兼容efast同步 无用
                    }

                    foreach (var item in sku.Props.Split(';'))
                    {
                        string propId = item.Split(':')[0];
                        if (!propIdList.Contains(propId))
                        {
                            propIdList.Add(propId);
                            string propName = item.Split(':')[2];
                            propKeyValue.Add(int.Parse(propId), propName);

                        }
                    }

                }


                List<PropertyModel> data = new List<PropertyModel>();
                foreach (var propId in propIdList)
                {
                    PropertyModel model = new PropertyModel();
                    Property prop = new Property();
                    prop.property_id = int.Parse(propId);
                    prop.property_name = propKeyValue[int.Parse(propId)].Trim();
                    model.property = prop;
                    model.property_value_list = new List<PropertyValueModel>();
                    foreach (var sku in skuList)
                    {

                        foreach (var item in sku.Props.Split(';'))
                        {
                            string propIdNew = item.Split(':')[0];
                            string propValue = item.Split(':')[1];
                            string propValueName = item.Split(':')[3];
                            if (propId == propIdNew)
                            {
                                PropertyValueModel valueModel = new PropertyValueModel();
                                valueModel.property_value_id = int.Parse(propValue);
                                valueModel.property_value_name = propValueName.Trim();
                                if (model.property_value_list.Where(p => p.property_value_id == valueModel.property_value_id).Count() == 0)
                                {
                                    model.property_value_list.Add(valueModel);
                                }


                            }
                        }

                    }

                    data.Add(model);

                }


                return data;
            }
            catch (Exception)
            {
                return new List<PropertyModel>();

            }

        }

        ///// <summary>
        ///// 限时特卖活动商品模型
        ///// </summary>
        //public class PromotionActivityProductModel
        //{
        //    /// <summary>
        //    /// 商品Id
        //    /// </summary>
        //    public int product_id { get; set; }
        //    /// <summary>
        //    /// 分类Id
        //    /// </summary>
        //    public string category_id { get; set; }
        //    /// <summary>
        //    /// 分类名称
        //    /// </summary>
        //    public string category_name { get; set; }
        //    /// <summary>
        //    /// 商品名称
        //    /// </summary>
        //    public string product_title { get; set; }
        //    /// <summary>
        //    /// 吊牌价
        //    /// </summary>
        //    public decimal quote_price { get; set; }
        //    /// <summary>
        //    /// 价格
        //    /// </summary>
        //    public decimal price { get; set; }
        //    /// <summary>
        //    /// 特卖最低价格
        //    /// </summary>
        //    public decimal promotion_price { get; set; }
        //    /// <summary>
        //    /// 图片
        //    /// </summary>
        //    public string img_url { get; set; }
        //    /// <summary>
        //    /// 是否上架
        //    /// </summary>
        //    public int is_onsale { get; set; }
        //    /// <summary>
        //    /// 浏览量
        //    /// </summary>
        //    public int pv { get; set; }
        //    /// <summary>
        //    /// 标签
        //    /// </summary>
        //    public string tags { get; set; }
        //    /// <summary>
        //    /// 日期
        //    /// </summary>
        //    public double create_time { get; set; }
        //    /// <summary>
        //    /// 商品编码
        //    /// </summary>
        //    public string product_code { get; set; }
        //    /// <summary>
        //    /// 总库存
        //    /// </summary>
        //    public int totalcount { get; set; }


        //}


        /// <summary>
        /// 商品模型
        /// </summary>
        public class ProcuctModel
        {
            /// <summary>
            /// 商品编号
            /// </summary>
            public int product_id { get; set; }
            /// <summary>
            /// sku 集合
            /// </summary>
            public List<SkuModel> skus { get; set; }

        }

        /// <summary>
        /// SKU 模型
        /// </summary>
        public class SkuModel
        {
            /// <summary>
            /// sku 编号
            /// </summary>
            public int sku_id { get; set; }
            /// <summary>
            /// 特卖价格
            /// </summary>
            public decimal promotion_price { get; set; }
            /// <summary>
            /// 特卖剩余库存
            /// </summary>
            public int promotion_count { get; set; }
            /// <summary>
            /// 特卖总库存
            /// </summary>
            public int promotion_sale_count { get; set; }


        }


    }
}