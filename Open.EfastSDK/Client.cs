using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.Common;
using Newtonsoft.Json;
using System.IO;

namespace Open.EfastSDK
{
    public class Client
    {

        //app_key app_secret app_nick app_act
        public readonly string APIUrl = ConfigHelper.GetConfigString("eFastAPIUrl");
        public readonly string AppNick = ConfigHelper.GetConfigString("eFastAppNick");
        public readonly string AppKey = ConfigHelper.GetConfigString("eFastAppKey");
        public readonly string AppSecret = ConfigHelper.GetConfigString("eFastAppSecret");
        public readonly Encoding EncodingBase = Encoding.UTF8;

        private void Tolog(string msg)
        {
            if (File.Exists(@"D:\hzhtest2.txt"))
            {
                using (StreamWriter sw = new StreamWriter(@"D:\hzhtest2.txt", true, Encoding.GetEncoding("GB2312")))
                {
                    sw.WriteLine(DateTime.Now.ToString() + "  " + msg);
                } 
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public T GetCommand<T>(string data, string method = "get",string exKey = "")
        {
            HttpInterFace http = new HttpInterFace();

            data += string.Format("&app_key={0}&app_secret={1}&app_nick={2}", AppKey, AppSecret, AppNick);
            //Tolog(" efast接口请求： " + data);
            string result = string.Empty;

            try
            {
                if (method.Equals("get", StringComparison.OrdinalIgnoreCase) || method.Equals(""))
                    result = http.GetWebRequest(data, APIUrl, EncodingBase);
                else if (method.Equals("post", StringComparison.OrdinalIgnoreCase))
                    result = http.PostWebRequest(data, APIUrl, EncodingBase);

               // Tolog(" efast接口返回： " + result);

                //dynamic resp = JsonConvert.DeserializeObject(result);

                //var respList = JsonConvert.DeserializeObject<Entity.GoodsListResp>(resp["resp_data"].ToString());

                if (result == "[]" || string.IsNullOrWhiteSpace(result))
                {
                    return default(T);
                }

                dynamic respData = JsonConvert.DeserializeObject(result);
                var resp_data = respData["resp_data"].ToString();

                if (!string.IsNullOrWhiteSpace(exKey))
                {
                    resp_data = respData["resp_data"][exKey].ToString();
                }

                if (resp_data == "[]" || string.IsNullOrWhiteSpace(resp_data))
                {
                    return default(T);
                }

                return JsonConvert.DeserializeObject<T>(resp_data);
            }
            catch (Exception ex)
            {
                return default(T);
                //throw ex;
            }

        }


        #region 测试
        public List<Entity.GoodsInfo> GetGoodsList()
        {
            List<Entity.GoodsInfo> result = new List<Entity.GoodsInfo>();

            var resp = GetCommand<Entity.GoodsListResp>("app_act=efast.items.list.get&page_no=1");

            return result;
        }

        public string CreateOrder()
        {

            Entity.TradeNewAddInfo order = new Entity.TradeNewAddInfo();

            order.sd_id = 9;
            order.oid = Guid.NewGuid().ToString();
            order.user_name = "13888888888";

            order.shipping_name = "zto";            

            order.pay_name = "weixinpay"; //weixinpay
            order.consignee = "李小姐";
            order.address = "上海";
            order.zipcode = "538100";
            order.mobile = "13888888888";
            order.tel = "";
            order.email = "";
            order.postscript = "";//买家备注
            order.to_buyer = "";//商家备注
            order.goods_amount = "11";
            order.goods_count = "1";
            order.total_amount = "16";
            order.shipping_fee = "";//快递费
            order.order_amount = "16";
            order.money_paid = "0";

            order.add_time = DateTime.Now.ToString();

            order.province_name = "广西壮族自治区";
            order.city_name = "防城港市";
            order.district_name = "东兴市";

            order.orders = new List<Entity.TradeNewAddOrdersInfo>()
            {
                new Entity.TradeNewAddOrdersInfo()
                {
                    outer_sku = "YC159020TS000",
                    goods_price = 11,
                    goods_number = 1,
                    goods_name = "【测试】迷情巴黎时尚休闲连衣裙"
                }
            };

            var resp = GetCommand<Entity.TradeNewAddResp>(string.Format("app_act=efast.trade.new.add&info={0}", JsonConvert.SerializeObject(order)));

            return JsonConvert.SerializeObject(resp);

        }

        

        public List<Entity.PaymentInfo> GetPaymentList()
        {
            return GetCommand<Entity.PaymentListResp>("app_act=efast.payment.list.get").list;
        }
        
        
        #endregion

        /// <summary>
        /// 获取颜色列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="colorId"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public Entity.ColorInfoResp GetColorList(int pageIndex = 1, int pageSize = 20, string colorId = null, DateTime? startTime = null)
        {
            StringBuilder strArgs = new StringBuilder("app_act=efast.color.list.get");
            strArgs.AppendFormat("&page_no={0}", pageIndex);
            strArgs.AppendFormat("&page_size={0}", pageSize);
            if (!string.IsNullOrWhiteSpace(colorId))
            {
                strArgs.AppendFormat("&color_id={0}", colorId);
            }
            if (startTime != null)
            {
                strArgs.AppendFormat("&start_time={0}", startTime.Value.ToString());
            }
            return GetCommand<Entity.ColorInfoResp>(strArgs.ToString());
        }

        /// <summary>
        /// 获取尺寸列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sizeId"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public Entity.SizeListResp GetSizeList(int pageIndex = 1, int pageSize = 20, string sizeId = null, DateTime? startTime = null)
        {
            StringBuilder strArgs = new StringBuilder("app_act=efast.size.list.get");
            strArgs.AppendFormat("&page_no={0}", pageIndex);
            strArgs.AppendFormat("&page_size={0}", pageSize);
            if (!string.IsNullOrWhiteSpace(sizeId))
            {
                strArgs.AppendFormat("&size_id={0}", sizeId);
            }
            if (startTime != null)
            {
                strArgs.AppendFormat("&start_time={0}", startTime.Value.ToString());
            }
            return GetCommand<Entity.SizeListResp>(strArgs.ToString());
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public Entity.GoodsListResp GetGoodList(int pageIndex = 1, int pageSize = 20, DateTime? startTime = null,string cateId = null)
        {
            StringBuilder strArgs = new StringBuilder("app_act=efast.items.list.get");
            strArgs.AppendFormat("&page_no={0}", pageIndex);
            strArgs.AppendFormat("&page_size={0}", pageSize);
            if (startTime != null)
            {
                strArgs.AppendFormat("&start_time={0}", startTime.Value.ToString());
            }
            if (!string.IsNullOrWhiteSpace(cateId))
            {
                strArgs.AppendFormat("&cat_id={0}", cateId);
            }
            
            return GetCommand<Entity.GoodsListResp>(strArgs.ToString());
        }

        /// <summary>
        /// 获取sku列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="goodsId"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public Entity.SkuListResp GetSkuList(int pageIndex = 1, int pageSize = 20, string goodsId = null, DateTime? startTime = null)
        {
            StringBuilder strArgs = new StringBuilder("app_act=efast.sku.list.get");
            strArgs.AppendFormat("&page_no={0}", pageIndex);
            strArgs.AppendFormat("&page_size={0}", pageSize);
            if (startTime != null)
            {
                strArgs.AppendFormat("&start_time={0}", startTime.Value.ToString());
            }
            if (!string.IsNullOrWhiteSpace(goodsId))
            {
                strArgs.AppendFormat("&goods_id={0}", goodsId);
            }
            return GetCommand<Entity.SkuListResp>(strArgs.ToString());
        }

        /// <summary>
        /// 获取库存
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="sku"></param>
        /// <returns></returns>
        public Entity.SkuStockInfo GetSkuStock(int shopId, string sku)
        {
            StringBuilder strArgs = new StringBuilder("app_act=efast.sku.stock.get");
            strArgs.AppendFormat("&sd_id={0}", shopId);
            strArgs.AppendFormat("&sku={0}", sku);

            return GetCommand<Entity.SkuStockInfo>(strArgs.ToString(), "get", sku);
        }
        
        public List<Entity.ShippingInfo> GetShippingList()
        {
            return GetCommand<Entity.ShippingListResp>("app_act=efast.shipping.list.get").list;
        }

        public Entity.TradeNewAddResp CreateOrder(Entity.TradeNewAddInfo order)
        {
            return GetCommand<Entity.TradeNewAddResp>(string.Format("app_act=efast.trade.new.add&info={0}", JsonConvert.SerializeObject(order)));
        }

        public Entity.RespDataBase TradeInvalid(string oid)
        {
            StringBuilder strArgs = new StringBuilder("app_act=efast.trade.invalid&lylx=openshop");
            strArgs.AppendFormat("&oid={0}", oid);

            return GetCommand<Entity.RespDataBase>(strArgs.ToString());
        }

        //TODO：获取快递单、快递代码、发货状态
        public Entity.TradeDetailInfo GetTradeDetail(string oid)
        {
            StringBuilder strArgs = new StringBuilder("app_act=efast.trade.detail.get&feilds=order_sn,invoice_no,shipping_status,shipping_name&type=1");
            strArgs.AppendFormat("&oid={0}", oid);

            return GetCommand<Entity.TradeDetailInfo>(strArgs.ToString());
        }

    }
}
