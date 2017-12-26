using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    /// 购物车
    /// </summary>
    public class ShoppingCart : BaseHandlerNeedLogin
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLMall bllMall = new BLLMall();
        /// <summary>
        /// 用户
        /// </summary>
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 添加购物车
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {
            int skuId = int.Parse(context.Request["sku_id"]);
            int count = int.Parse(context.Request["count"]);
            string supplierId=context.Request["supplier_id"];//供应商 门店id
            string msg = "";
            if (bllMall.AddShoppingCart(currentUserInfo.UserID, skuId, count, out msg, supplierId))
            {
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = msg;
            }

            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 删除购物车
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {
            string cartIds = context.Request["cart_ids"];
            if (bllMall.DeleteShoppingCart(currentUserInfo.UserID,cartIds))
            {
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "删除失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 获取购物车列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {
            try
            {

          
            var sourceData = bllMall.GetShoppingCartList(currentUserInfo.UserID,true);
            //for (int i = 0; i < sourceData.Count; i++)
            //{
            //    if (bllMall.GetCount<ZentCloud.BLLJIMP.Model.WXMallProductInfo>(string.Format(" PID='{0}' And (IsDelete=1 Or IsOnSale='0')", sourceData[i].ProductId)) > 0)
            //    {
            //        sourceData.RemoveAt(i);//删除或下架的不显示
            //    }

            //}

            //TODO: 改造下，价格和积分再次拉取商品信息表数据
            List<BLLJIMP.Model.WXMallProductInfo> productList = new List<BLLJIMP.Model.WXMallProductInfo>();

            List<CartModel> resultDataList = new List<CartModel>();
            foreach (var item in sourceData)
            {
                int isInvalid = 0;
                var productSku = bllMall.GetProductSku(item.SkuId);
                if (productSku==null)
                {
                    isInvalid = 1;
                    //continue;
                }
                BLLJIMP.Model.WXMallProductInfo product = productList.SingleOrDefault(p => p.PID == item.ProductId.ToString());

                if (product == null)
                {
                    product = bllMall.GetProduct(item.ProductId);
                    if (product == null) continue;
                    productList.Add(product);
                }
                if (product.IsDelete==1||product.IsOnSale=="0")
                {
                    isInvalid = 1;
                }
                //dynamic resultDataItem = new
                //{
                //    cart_id = item.CartId,
                //    sku_id = item.SkuId,
                //    product_id = item.ProductId,
                //    title = item.Title,
                //    properties = item.SkuPropertiesName,
                //    img_url = bllMall.GetImgUrl(item.ImgUrl),
                //    quote_price = product.PreviousPrice,
                //    price = product.Score > 0? product.Price : bllMall.GetSkuPrice(productSku),
                //    score = product.Score,
                //    is_cashpay_only = product.IsCashPayOnly,
                //    total_fee = item.TotalFee,
                //    discount_fee = item.DiscountFee,
                //    count = item.Count,
                //    freight_terms = item.FreightTerms,
                //    freight = item.Freight,
                //    max_count = productSku.Stock,
                //    product_tags = item.Tags,
                //    is_no_express = product.IsNoExpress,
                //    supplier_name=bllMall.GetSuppLierByUserId(product.SupplierUserId,bllMall.WebsiteOwner).Company,
                //    is_invalid = isInvalid
                //};

                CartModel model = new CartModel();
                  model.cart_id = item.CartId;
                  model.sku_id = item.SkuId;
                  model.product_id = item.ProductId;
                  model.title = item.Title;
                  model.properties = item.SkuPropertiesName;
                  model.img_url = bllMall.GetImgUrl(item.ImgUrl);
                  model.quote_price = product.PreviousPrice;
                  model.price = product.Score > 0? product.Price : item.Price;
                  model.score = product.Score;
                  model.is_cashpay_only = product.IsCashPayOnly;
                  model.total_fee = item.TotalFee;
                  model.discount_fee = item.DiscountFee;
                  model.count = item.Count;
                  model.freight_terms = item.FreightTerms;
                  model.freight = item.Freight;
                  model.max_count = productSku.Stock;
                  model.product_tags = item.Tags;
                  model.is_no_express = product.IsNoExpress;
                  model.supplier_id = item.SupplierId;
                  //model.supplier_name=bllMall.GetSuppLierByUserId(product.SupplierUserId,bllMall.WebsiteOwner).Company;
                  model.supplier_name = item.SupplierName;
                  model.is_invalid = isInvalid;
                  model.store_address = item.StoreAddress; 
                resultDataList.Add(model);
            }
            resultDataList = resultDataList.OrderBy(p => p.is_invalid).ToList();
            var data = new
            {
                totalcount = resultDataList.Count(),
                list = resultDataList,//列表
            };

            return ZentCloud.Common.JSONHelper.ObjectToJson(data);

            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }

        /// <summary>
        /// 修改购物车
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {
            int skuId = int.Parse(context.Request["sku_id"]);
            int count = int.Parse(context.Request["count"]);
            string msg = "";
            if (bllMall.UpdateShoppingCart(currentUserInfo.UserID,skuId,count, out msg))
            {
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = msg;
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        private ZentCloud.BLLJIMP.Model.ProductSku GetProductSku(int skuId) {

           var sku= bllMall.GetProductSku(skuId);
           if (sku!=null)
           {
               return sku;
           }
           return new BLLJIMP.Model.ProductSku();
        
        }

        /// <summary>
        /// 购物车模型
        /// </summary>
        public class CartModel { 
        
                  public int  cart_id {get;set;}
                   public int  sku_id {get;set;}
                   public int  product_id {get;set;}
                   public string  title {get;set;}
                   public string  properties{get;set;}
                   public string  img_url{get;set;}
                   public decimal quote_price {get;set;}
                   public decimal price {get;set;}
                   public decimal score{get;set;}
                   public decimal is_cashpay_only {get;set;}
                   public decimal total_fee {get;set;}
                   public decimal discount_fee {get;set;}
                   public decimal count{get;set;}
                   public string freight_terms{get;set;}
                   public decimal freight{get;set;}
                   public decimal max_count { get; set; }
                   public string  product_tags{get;set;}
                   public int  is_no_express{get;set;}
                   public string supplier_id { get; set; }
                   public string  supplier_name {get;set;}
                   public int  is_invalid {get;set;}
                   public string store_address { get; set; }
                  


        }
    }
}