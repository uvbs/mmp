using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP
{
    public class BLLEfast : BLL
    {
        /// <summary>
        /// 颜色属性id
        /// </summary>
        readonly int ColorPropId = 2;
        /// <summary>
        /// 尺码属性id
        /// </summary>
        readonly int SizePropId = 1;
        readonly string CurrWebsiteOwner = "hf";
        readonly string EfastSourceBase = Common.ConfigHelper.GetConfigString("eFastBaseUrl");
        readonly int CurrEfastShopId = Common.ConfigHelper.GetConfigInt("eFastShopId");

        BLLMall bllMall = new BLLMall();
        Open.EfastSDK.Client client = new Open.EfastSDK.Client();

        public BLLEfast() {
            
        }

        //同步颜色和尺码档案
        
        #region 颜色档案同步
        /// <summary>
        /// 颜色档案同步
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public void ColorSync(out int newCount, out int updateCount)
        {

            newCount = 0;
            updateCount = 0;

            //读取efast数据
            var efastData = GetEfastColor(1, 20);

            //保存到数据库，根据名称判读，如果相同则入库，否则修改ID
            foreach (var item in efastData)
            {
                string strExists = string.Format(" PropID = {0} AND PropValueId = {1} ", ColorPropId, item.color_id);

                if (GetCount<Model.ProductPropertyValue>(strExists) > 0)
                {
                    updateCount += Update(
                            new Model.ProductPropertyValue(),
                            string.Format(" PropValueId={0},Modified='{1}',PropCode='{2}',PropDescription='{3}' ",
                                item.color_id,
                                DateTime.Now,
                                item.color_code,
                                item.color_note
                            ),
                            strExists
                        );
                }
                else
                {
                    Model.ProductPropertyValue model = new Model.ProductPropertyValue();
                    model.PropID = ColorPropId;
                    model.PropValueId = int.Parse(item.color_id);
                    model.PropValue = item.color_name;
                    model.WebSiteOwner = CurrWebsiteOwner;
                    model.InsertDate = DateTime.Now;
                    model.Modified = DateTime.Now;
                    model.PropCode = item.color_code;
                    model.PropDescription = item.color_note;

                    if (Add(model))
                        newCount++;

                }
            }

        }
        public List<Open.EfastSDK.Entity.ColorInfo> GetEfastColor(int pageIndex=1, int pageSize=20)
        {
            List<Open.EfastSDK.Entity.ColorInfo> result = new List<Open.EfastSDK.Entity.ColorInfo>();

            var resp = client.GetColorList(pageIndex, pageSize);

            result.AddRange(resp.list);

            var totalPage = GetTotalPage(resp.total_results, pageSize);

            if (pageIndex < totalPage)
            {
                result.AddRange(GetEfastColor(++pageIndex, pageSize));
            }

            return result;
        }

        #endregion

        #region 尺码档案同步
        public void SizeSync(out int newCount, out int updateCount)
        {

            newCount = 0;
            updateCount = 0;

            //读取efast数据
            var efastData = GetEfastSize(1, 20);

            //保存到数据库，根据名称判读，如果相同则入库，否则修改ID
            foreach (var item in efastData)
            {
                string strExists = string.Format(" PropID = {0} AND PropValueId = {1} ", SizePropId, item.size_id);


                if (GetCount<Model.ProductPropertyValue>(strExists) > 0)
                {
                    updateCount += Update(
                            new Model.ProductPropertyValue(),
                            string.Format(" PropValueId={0},Modified='{1}',PropCode='{2}',PropDescription='{3}' ",
                                item.size_id,
                                DateTime.Now,
                                item.size_code,
                                item.size_note
                            ),
                            strExists
                        );
                }
                else
                {
                    Model.ProductPropertyValue model = new Model.ProductPropertyValue();
                    model.PropID = SizePropId;
                    model.PropValueId = int.Parse(item.size_id);
                    model.PropValue = item.size_name;
                    model.WebSiteOwner = CurrWebsiteOwner;
                    model.InsertDate = DateTime.Now;
                    model.Modified = DateTime.Now;
                    model.PropCode = item.size_code;
                    model.PropDescription = item.size_note;

                    if (Add(model))
                        newCount++;

                }
                
            }

        }
        public List<Open.EfastSDK.Entity.SizeInfo> GetEfastSize(int pageIndex=1, int pageSize=20)
        {
            List<Open.EfastSDK.Entity.SizeInfo> result = new List<Open.EfastSDK.Entity.SizeInfo>();

            var resp = client.GetSizeList(pageIndex, pageSize);

            result.AddRange(resp.list);

            var totalPage = GetTotalPage(resp.total_results, pageSize);

            if (pageIndex < totalPage)
            {
                result.AddRange(GetEfastSize(++pageIndex, pageSize));
            }

            return result;
        }
        #endregion

        //TODO：同步商品档案
        /// <summary>
        /// 颜色档案同步
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public void GoodsSync(out int newCount, out int updateCount,string cateId = null)
        {

            newCount = 0;
            updateCount = 0;


            //获取最后同步时间
            var lastGoods = GetList<Model.WXMallProductInfo>(1,string.Format(""), " LastUpdate DESC ");

            DateTime? lastDate = null;

            if (lastGoods != null && lastGoods.Count > 0)
            {
                lastDate = lastGoods[0].LastUpdate;
            }

            //读取efast数据
            var efastData = GetEfastGoods(1, 20, lastDate, cateId);

            //保存到数据库，根据名称判读，如果相同则入库，否则修改ID
            foreach (var item in efastData)
            {
                string strExists = string.Format(" OutGoodsId = {0} ", item.goods_id);

                Model.WXMallProductInfo model = Get<Model.WXMallProductInfo>(strExists);

                if (model == null)
                {
                    model = new Model.WXMallProductInfo();
                }

                model.OutGoodsId = item.goods_id;
                model.ProductCode = item.goods_sn;
                model.PName = item.goods_name;
                model.WebsiteOwner = CurrWebsiteOwner;
                model.Summary = item.goods_brief;
                model.PDescription = item.goods_desc;
                model.PreviousPrice = Convert.ToDecimal(item.market_price);
                model.Price = Convert.ToDecimal(item.shop_price);
                model.UserID = "jubit";
                model.InsertDate = DateTime.Now;
                model.RecommendImg = EfastSourceBase + item.goods_thumb;
                model.ShowImage1 = EfastSourceBase + item.goods_thumb;
                model.IsOnSale = item.is_on_sale;
                model.LastUpdate = item.modified;

                model.OutCateId = item.cat_id;
                model.OutSeasonId = item.season_id;
                model.OutSeriesId = item.series_id;
                model.OutBrandId = item.brand_id;

                if (GetCount<Model.WXMallProductInfo>(strExists) > 0)
                {
                    updateCount += Update(
                            new Model.WXMallProductInfo(),
                            string.Format(" IsOnSale='{0}',LastUpdate='{1}',OutCateId='{2}',OutSeasonId='{3}',OutSeriesId='{4}',OutBrandId='{5}' ",
                                model.IsOnSale,
                                model.LastUpdate,
                                model.OutCateId,
                                model.OutSeasonId,
                                model.OutSeriesId,
                                model.OutBrandId
                            ),
                            strExists
                        );
                }
                else
                {
                    model.PID = GetGUID(TransacType.CommAdd);

                    if (Add(model))
                        newCount++;
                }
                
            }

        }
        public List<Open.EfastSDK.Entity.GoodsInfo> GetEfastGoods(int pageIndex, int pageSize,DateTime? startDate,string cateId = null)
        {
            List<Open.EfastSDK.Entity.GoodsInfo> result = new List<Open.EfastSDK.Entity.GoodsInfo>();

            var resp = client.GetGoodList(pageIndex, pageSize, startDate, cateId);

            result.AddRange(resp.list);

            var totalPage = GetTotalPage(resp.total_results, pageSize);

            if (pageIndex < totalPage)
            {
                result.AddRange(GetEfastGoods(++pageIndex, pageSize, startDate,cateId));
            }

            return result;
        }

        //TODO：同步SKU及库存
        public void SkuSync(out int newCount, out int updateCount)
        {

            newCount = 0;
            updateCount = 0;

            //读取现有商品数据，
            var goodsList = GetList<Model.WXMallProductInfo>(" OutGoodsId IS NOT NULL ");

            foreach (var goods in goodsList)
            {
                
                //读取efast数据
                var efastData = GetEfastSku(1, 20,goods.OutGoodsId, null);

                //保存到数据库，根据名称判读，如果相同则入库，否则修改ID
                foreach (var item in efastData)
                {
                    string strExists = string.Format(" OutBarCode = '{0}' ", item.barcode);

                    Model.ProductSku model = Get<Model.ProductSku>(strExists);

                    if (model == null)
                    {
                        model = new Model.ProductSku();
                    }

                    model.OutBarCode = item.barcode;
                    model.PropValueIdEx1 = item.size_id;
                    model.PropValueIdEx2 = item.color_id;
                    model.PropValueIdEx3 = item.cat_id;
                    model.WebSiteOwner = CurrWebsiteOwner;
                    model.Modified = Convert.ToDateTime(item.modified);
                    model.InsertDate = DateTime.Now;
                    model.ProductId = Convert.ToInt32(goods.PID);
                    model.SkuSN = item.goods_sn;
                    
                    //读取库存
                    var stock = client.GetSkuStock(CurrEfastShopId, item.barcode);
                    if (stock != null)
                    {
                        model.Stock = stock.sl;
                        model.Description = stock.msg;
                    }

                    if (GetCount<Model.ProductSku>(strExists) > 0)
                    {
                        updateCount += Update(
                                new Model.ProductSku(),
                                string.Format(" Stock={0},Description='{1}' ",
                                    model.Stock,
                                    model.Description
                                ),
                                strExists
                            );
                    }
                    else
                    {
                        model.SkuId = int.Parse(GetGUID(TransacType.CommAdd));

                        if (Add(model))
                            newCount++;
                    }

                }
            }
        }
        /// <summary>
        /// 获取sku
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="goodsId"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public List<Open.EfastSDK.Entity.SkuInfo> GetEfastSku(int pageIndex, int pageSize,string goodsId, DateTime? startDate)
        {
            List<Open.EfastSDK.Entity.SkuInfo> result = new List<Open.EfastSDK.Entity.SkuInfo>();

            var resp = client.GetSkuList(pageIndex, pageSize, goodsId, startDate);

            result.AddRange(resp.list);

            var totalPage = GetTotalPage(resp.total_results, pageSize);

            if (pageIndex < totalPage)
            {
                result.AddRange(GetEfastSku(++pageIndex, pageSize, goodsId, startDate));
            }

            return result;
        }


        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="outOrderId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool CreateOrder(string orderId,out string outOrderId,out string msg)
        {
            bool result = false;
            outOrderId = "";
            msg = "";

            var order = bllMall.GetOrderInfo(orderId);

            var efastOrder = GetMappingOrderInfo(order);

            var resp = client.CreateOrder(efastOrder);

            msg = resp.msg;
            result = resp.msg == "success";

            if (result)
            {
                //更新订单的外部id
                outOrderId = resp.oder_sn;

                var updateResult = Update(
                        new Model.WXMallOrderInfo(),
                        string.Format(" OutOrderId = '{0}' ", outOrderId),
                        string.Format(" OrderID = '{0}' ",orderId)
                    );

            }

            return result;
        }

        /// <summary>
        /// 根据本地订单映射出efast订单
        /// </summary>
        /// <param name="orderSrc"></param>
        /// <returns></returns>
        public Open.EfastSDK.Entity.TradeNewAddInfo GetMappingOrderInfo(Model.WXMallOrderInfo orderSrc)
        {
            
            Open.EfastSDK.Entity.TradeNewAddInfo order = new Open.EfastSDK.Entity.TradeNewAddInfo();

            order.oid = orderSrc.OrderID;
            order.sd_id = CurrEfastShopId;
            order.user_name = orderSrc.OrderUserID;
            order.shipping_name = "zto";
            order.pay_name = "weixinpay"; //weixinpay
            order.consignee = orderSrc.Consignee;
            order.address = orderSrc.Address;
            order.zipcode = string.IsNullOrWhiteSpace(orderSrc.ZipCode)? "000000": orderSrc.ZipCode;
            order.mobile = orderSrc.Phone;
            order.tel = "";
            order.email = "";
            order.postscript = "";
            order.to_buyer = "";//商家备注
            order.goods_amount = orderSrc.Product_Fee .ToString();
            order.goods_count = orderSrc.ProductCount.ToString();
            order.total_amount = orderSrc.TotalAmount.ToString();
            order.shipping_fee = orderSrc.Transport_Fee.ToString();
            order.money_paid = orderSrc.TotalAmount.ToString();
            order.order_amount = orderSrc.TotalAmount.ToString();
            order.add_time = DateTime.Now.ToString();
            order.province_name = orderSrc.ReceiverProvince;
            order.city_name = orderSrc.ReceiverCity;
            order.district_name = orderSrc.ReceiverDist;
            
            if (!string.IsNullOrWhiteSpace(orderSrc.ReceiverDist))
            {
                order.address = orderSrc.ReceiverDist + "  " + order.address;
            }
            if (!string.IsNullOrWhiteSpace(orderSrc.ReceiverCity))
            {
                order.address = orderSrc.ReceiverCity + "  " + order.address;
            }
            if (!string.IsNullOrWhiteSpace(orderSrc.ReceiverProvince))
            {
                order.address = orderSrc.ReceiverProvince + "  " + order.address;
            }

            List<Model.WXMallOrderDetailsInfo> detailList = bllMall.GetOrderDetailsList(orderSrc.OrderID);
            List<Open.EfastSDK.Entity.TradeNewAddOrdersInfo> dtls = new List<Open.EfastSDK.Entity.TradeNewAddOrdersInfo>();
            int totalCount = 0;
            int saleProdQty = 0;

            foreach (var item in detailList)
            {
                Open.EfastSDK.Entity.TradeNewAddOrdersInfo dtl = new Open.EfastSDK.Entity.TradeNewAddOrdersInfo();
                saleProdQty++;
                totalCount += item.TotalCount;
                var sku = bllMall.GetProductSku(item.SkuId.Value);
                //var product = bllMall.GetProduct(item.PID);

                dtl.goods_name = item.ProductName;
                dtl.goods_number = item.TotalCount;
                dtl.goods_price = (double)item.OrderPrice;
                dtl.is_gift = 0;
                dtl.outer_sku = sku.OutBarCode;

                if (item.PaymentFt != null && item.PaymentFt != 0)
                {
                    dtl.payment_ft = (double)item.PaymentFt;
                }
                
                dtls.Add(dtl);
            }
            
            order.orders = dtls;

            return order;
        }
        

        //TODO：获取订单详情

        //TODO：修改订单支付状态

        //TODO：取消订单
        public bool CancelOrder(string orderId)
        {
            //var order = bllMall.GetOrderInfo(orderId);
            return CancelOrderByOutId(orderId);//(order.OutOrderId);
        }
        public bool CancelOrderByOutId(string outOrderId)
        {
            var resp = client.TradeInvalid(outOrderId);
            return resp.msg == "success";
        }

        /// <summary>
        /// 发货状态快递查询
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <param name="expressCompanyCode">快递公司代码</param>
        /// <param name="expressNumber">快递号</param>
        /// <returns>是否已发货</returns>
        public bool GetOrderExpressInfo(string orderId,out string expressCompanyCode,out string expressNumber)
        {
            bool isSend = false;
           
            var outOrderId = bllMall.GetOrderInfo(orderId).OutOrderId;

            isSend = GetOrderExpressInfoByOutId(outOrderId,out expressCompanyCode,out expressNumber);

            return isSend;
        }

        public bool GetOrderExpressInfoByOutId(string outOrderId, out string expressCompanyCode, out string expressNumber)
        {
            bool isSend = false;
            expressCompanyCode = "";
            expressNumber = "";
            
            var resp = client.GetTradeDetail(outOrderId);
            if (resp != null)
            {
                expressNumber = resp.invoice_no;
                expressCompanyCode = resp.shipping_name;

                isSend = resp.shipping_status == "1";
            }

            return isSend;
        }


    }
}
