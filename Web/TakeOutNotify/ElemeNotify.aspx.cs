using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.JubitIMP.Web.TakeOutNotify.Model;

namespace ZentCloud.JubitIMP.Web.TakeOutNotify
{
    /// <summary>
    /// 接受饿了么后台发送过来的订单信息  [type=10]触发饿了么后台
    /// </summary>
    public partial class ElemeNotify : System.Web.UI.Page
    {
        BLLJIMP.BLLWebSite bllWebsite = new BLLJIMP.BLLWebSite();
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        ResponseModel resp = new ResponseModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            string host = HttpContext.Current.Request.Url.Host;

            WebsiteDomainInfo domain = bllWebsite.Get<WebsiteDomainInfo>(string.Format(" WebsiteDomain='{0}'", host));

            var websiteModel = bllWebsite.Get<WebsiteInfo>(string.Format(" WebsiteOwner='{0}'",domain.WebsiteOwner));

            if (Request.RequestType.ToUpper()=="POST")
            {
                string body = PostInput();
                ElemeRequset reqEleme =Newtonsoft.Json.JsonConvert.DeserializeObject<ElemeRequset>(body);
                switch (reqEleme.type)
                {
                    case 10:
                        OrderEntry(reqEleme, websiteModel);//订单生效
                        break;
                    case 18:
                        ReceiptConfirm(reqEleme, websiteModel);//订单已完成
                        break;
                    case 20:
                        OrderAppyCancel(reqEleme, websiteModel);//用户申请取消订单
                        break;
                    case 21:
                        withdrawCelcel(reqEleme, websiteModel);//用户撤回取消
                        break;
                    case 22:
                        MerchantrefuseCencal(reqEleme, websiteModel);//商户拒绝取消
                        break;
                    case 23:
                        MerchantAgreeCencel(reqEleme, websiteModel);//商户同意取消
                        break;
                    default:
                        break;
                }
            }
            else
            {
                var getMsg = new
                {
                    message = "ok"
                };
                Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(getMsg));
            }

        }
        /// <summary>
        /// 订单生效type=10
        /// </summary>
        public void OrderEntry(ElemeRequset reqEleme, WebsiteInfo websiteModel)
        {
            var sign = reqEleme.signature;
            var value = GetSign(reqEleme, websiteModel);
            if (sign == value)
            {
                AddOrder(reqEleme.message, websiteModel.WebsiteOwner);
                resp.msg = "ok";
            }
            else
            {
                resp.msg = "验签错误";
            }
            Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        public void AddOrder(string oorderInfo,string websiteOwner)
        {
            OOrder oorder = ZentCloud.Common.JSONHelper.JsonToModel<OOrder>(oorderInfo);
            WXMallOrderInfo mallOrder = bllMall.Get<WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}' AND OutOrderId='{1}'", websiteOwner, oorder.orderId));
            if (mallOrder == null)
            {
                mallOrder = new WXMallOrderInfo();
                ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZentCloud.ZCBLLEngine.BLLTransaction();
                mallOrder.OrderID = bllMall.GetGUID(ZentCloud.BLLJIMP.TransacType.AddMallOrder);
                mallOrder.OutOrderId = oorder.orderId;
                mallOrder.Address = oorder.address;
                mallOrder.OrderType = 8;
                mallOrder.WebsiteOwner = websiteOwner;
                mallOrder.TakeOutType = "Eleme";
                mallOrder.OrderMemo = oorder.description;
                if (!string.IsNullOrEmpty(oorder.createdAt))
                {
                    mallOrder.InsertDate = GetDate(oorder.createdAt);
                }
                if (!string.IsNullOrEmpty(oorder.activeAt))
                {
                    mallOrder.PayTime = GetDate(oorder.activeAt);
                }
                if (oorder.deliverFee.HasValue) mallOrder.Transport_Fee = (decimal)oorder.deliverFee;
                if (!string.IsNullOrEmpty(oorder.deliverTime))
                {
                    mallOrder.Ex2 = GetDate(oorder.deliverTime).ToString();
                }
                mallOrder.Ex3 = oorder.invoice;
                mallOrder.Ex4 = oorder.book.ToString();
                mallOrder.Ex5 = oorder.onlinePaid.ToString();
                List<string> phoneList = oorder.phoneList;
                if (phoneList.Count > 0)
                {
                    for (int i = 0; i < phoneList.Count; i++)
                    {
                        if (i == 0) mallOrder.Phone = phoneList[i];
                        mallOrder.Ex6 += phoneList[i] + ",";
                    }
                    mallOrder.Ex6 = mallOrder.Ex6.Remove(mallOrder.Ex6.Length - 1, 1);
                }
                mallOrder.Ex7 = oorder.shopId.ToString();
                mallOrder.Ex9 = oorder.shopName;
                mallOrder.OrderUserID = oorder.userId.ToString();
                if (oorder.totalPrice.HasValue) mallOrder.TotalAmount = (decimal)oorder.totalPrice;
                if (oorder.originalPrice.HasValue) mallOrder.Ex10 = oorder.originalPrice.ToString();
                mallOrder.Consignee = oorder.consignee;
                mallOrder.Ex11 = oorder.deliveryGeo;
                mallOrder.Ex12 = oorder.deliveryPoiAddress;
                mallOrder.Ex13 = oorder.invoiced.ToString();
                if (oorder.income.HasValue) mallOrder.Ex14 = oorder.income.ToString();
                if (oorder.serviceRate.HasValue) mallOrder.Ex15 = oorder.serviceRate.ToString();
                if (oorder.serviceFee.HasValue) mallOrder.Ex16 = oorder.serviceFee.ToString();
                if (oorder.hongbao.HasValue) mallOrder.Ex17 = oorder.hongbao.ToString();
                if (oorder.packageFee.HasValue) mallOrder.Ex18 = oorder.packageFee.ToString();
                if (oorder.activityTotal.HasValue) mallOrder.Ex19 = oorder.activityTotal.ToString();
                if (oorder.shopPart.HasValue) mallOrder.Ex20 = oorder.shopPart.ToString();
                if (oorder.elemePart.HasValue) mallOrder.Ex21 = oorder.elemePart.ToString();
                if (mallOrder.TotalAmount > 0) mallOrder.PaymentStatus = 1;
                mallOrder.Ex22 = oorder.downgraded.ToString();
                mallOrder.OutOrderStatus = GetOrderStatus(oorder.status);
                mallOrder.Status = "待发货";
                mallOrder.OutRefundStatus = GetRefStatus(oorder.refundStatus);
                mallOrder.Ex8 = oorder.openId;
                if (oorder.vipDeliveryFeeDiscount.HasValue) mallOrder.Ex10 = oorder.vipDeliveryFeeDiscount.ToString();
                if (!bllWebsite.Add(mallOrder, tran))
                {
                    tran.Rollback();
                    resp.msg = "添加订单出错";
                    Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                List<OGoodsGroup> goodsGroupList = oorder.groups;
                foreach (var item in goodsGroupList)
                {
                    WXMallOrderDetailsInfo oorderDetail = new WXMallOrderDetailsInfo();
                    oorderDetail.Ex1 = item.name;
                    oorderDetail.Ex2 = item.type;
                    oorderDetail.OrderID = mallOrder.OrderID;
                    List<OGoodsItem> goodItems = item.items;
                    foreach (var good in goodItems)
                    {
                        oorderDetail.PID = good.id.ToString();
                        oorderDetail.Ex3 = good.id.ToString();
                        oorderDetail.Ex9 = good.skuId.ToString();
                        oorderDetail.ProductName = good.name;
                        oorderDetail.Ex4 = good.categoryId.ToString();
                        if (good.price.HasValue) oorderDetail.OrderPrice = (decimal)good.price;
                        oorderDetail.TotalCount = good.quantity;
                        if (good.total.HasValue) oorderDetail.TotalPrice = (decimal)good.total;
                        oorderDetail.Ex7 = good.extendCode;
                        oorderDetail.Ex8 = good.barCode;
                        if (good.userPrice.HasValue) oorderDetail.Ex10 = good.userPrice.ToString();
                        if (good.shopPrice.HasValue) oorderDetail.Ex11 = good.shopPrice.ToString();
                        if (good.weight.HasValue) oorderDetail.Wegith = (decimal)good.weight;
                        oorderDetail.Unit = "元";
                        if (good.newSpecs != null)
                        {
                            List<OGroupItemSpec> specList = good.newSpecs;
                            oorderDetail.Ex5 = ZentCloud.Common.JSONHelper.ListToJson<OGroupItemSpec>(specList);
                        }
                        if (good.attributes != null)
                        {
                            List<OGroupItemAttribute> attrList = good.attributes;
                            oorderDetail.Ex6 = ZentCloud.Common.JSONHelper.ListToJson<OGroupItemAttribute>(attrList);
                        }
                        if (!bllWebsite.Add(oorderDetail, tran))
                        {
                            tran.Rollback();
                            resp.msg = "添加商品出错";
                            Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                            return;
                        }
                    }
                }
                tran.Commit();
            }
        }

        /// <summary>
        /// 订单已完成 type=18
        /// </summary>
        /// <param name="oorderInfo"></param>
        /// <param name="websiteOwner"></param>
        public void ReceiptConfirm(ElemeRequset reqEleme, WebsiteInfo websiteModel)
        {
            var sign = reqEleme.signature;
            var value = GetSign(reqEleme, websiteModel);
            if (sign == value)
            {
                RespApplyCencel oorder = ZentCloud.Common.JSONHelper.JsonToModel<RespApplyCencel>(reqEleme.message);
                WXMallOrderInfo mallOrder = bllMall.Get<WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}' AND OutOrderId='{1}'", websiteModel.WebsiteOwner, oorder.orderId));
                if (mallOrder != null)
                {
                    mallOrder.Status = "交易成功";
                    mallOrder.ReceivingTime = DateTime.Now;
                    mallOrder.OutOrderStatus = GetRefStatus(oorder.refundStatus);
                    bllMall.Update(mallOrder);
                }
                resp.msg = "ok";
            }
            else
            {
                resp.msg = "验签错误";
            }
            Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }

        /// <summary>
        /// 用户申请取消 type=20
        /// </summary>
        public void OrderAppyCancel(ElemeRequset reqEleme, WebsiteInfo websiteModel)
        {
            var sign = reqEleme.signature;
            var value = GetSign(reqEleme, websiteModel);
            if (sign == value)
            {
                RespApplyCencel oorder = ZentCloud.Common.JSONHelper.JsonToModel<RespApplyCencel>(reqEleme.message);
                WXMallOrderInfo mallOrder = bllMall.Get<WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}' AND OutOrderId='{1}'", websiteModel.WebsiteOwner, oorder.orderId));
                if (mallOrder != null)
                {
                    mallOrder.IsRefund = 1;
                    mallOrder.OutOrderStatus = GetRefStatus(oorder.refundStatus);
                    bllMall.Update(mallOrder);
                }
                resp.msg = "ok";
            }
            else
            {
                resp.msg = "验签错误";
            }
            Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));

        }
        /// <summary>
        /// 用户撤回取消 type=21
        /// </summary>
        public void withdrawCelcel(ElemeRequset reqEleme, WebsiteInfo websiteModel)
        {
            var sign = reqEleme.signature;
            var value = GetSign(reqEleme, websiteModel);
            if (sign == value)
            {
                RespApplyCencel oorder = ZentCloud.Common.JSONHelper.JsonToModel<RespApplyCencel>(reqEleme.message);
                WXMallOrderInfo mallOrder = bllMall.Get<WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}' AND OutOrderId='{1}'", websiteModel.WebsiteOwner, oorder.orderId));
                if (mallOrder != null)
                {
                    mallOrder.IsRefund = 0;
                    mallOrder.OutOrderStatus = GetRefStatus(oorder.refundStatus);
                    bllMall.Update(mallOrder);
                }
                resp.msg = "ok";
            }
            else
            {
                resp.msg = "验签错误";
            }
            Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
        /// <summary>
        /// 商户拒绝取消申请 type=22
        /// </summary>
        public void MerchantrefuseCencal(ElemeRequset reqEleme, WebsiteInfo websiteModel)
        {
            ToLog("商户拒绝取消申请=" + reqEleme.message);
            var sign = reqEleme.signature;
            var value = GetSign(reqEleme, websiteModel);
            if (sign == value)
            {
                RespApplyCencel oorder = ZentCloud.Common.JSONHelper.JsonToModel<RespApplyCencel>(reqEleme.message);
                WXMallOrderInfo mallOrder = bllMall.Get<WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}' AND OutOrderId='{1}'", websiteModel.WebsiteOwner, oorder.orderId));
                if (mallOrder != null)
                {
                    mallOrder.IsRefund = 0;
                    mallOrder.OutOrderStatus = GetRefStatus(oorder.refundStatus);
                    bllMall.Update(mallOrder);
                }
                resp.msg = "ok";
            }
            else
            {
                resp.msg = "验签错误";
            }
            Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
        /// <summary>
        /// 商户同意取消申请 type=23
        /// </summary>
        public void MerchantAgreeCencel(ElemeRequset reqEleme, WebsiteInfo websiteModel)
        {
            ToLog("商户同意取消申请=" + reqEleme.message);
            var sign = reqEleme.signature;
            var value = GetSign(reqEleme, websiteModel);
            if (sign == value)
            {
                RespApplyCencel oorder = ZentCloud.Common.JSONHelper.JsonToModel<RespApplyCencel>(reqEleme.message);
                WXMallOrderInfo mallOrder = bllMall.Get<WXMallOrderInfo>(string.Format(" WebsiteOwner='{0}' AND OutOrderId='{1}'", websiteModel.WebsiteOwner, oorder.orderId));
                if (mallOrder != null)
                {
                    mallOrder.Status = "已取消";
                    mallOrder.IsRefund = 0;
                    mallOrder.OutOrderStatus = GetRefStatus(oorder.refundStatus);
                    bllMall.Update(mallOrder);
                }
                resp.msg = "ok";
            }
            else
            {
                resp.msg = "验签错误";
            }
            Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }



        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="reqEleme"></param>
        /// <param name="websiteModel"></param>
        /// <returns></returns>
        public string GetSign(ElemeRequset reqEleme, WebsiteInfo websiteModel)
        {
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Add("appId", reqEleme.appId.ToString());
            dic.Add("requestId", reqEleme.requestId);
            dic.Add("type", reqEleme.type.ToString());
            dic.Add("message", reqEleme.message);
            dic.Add("shopId", reqEleme.shopId.ToString());
            dic.Add("timestamp", reqEleme.timestamp.ToString());
            dic.Add("userId", reqEleme.userId.ToString());
            string signo = string.Empty;
            foreach (var item in dic)
            {
                signo += string.Format("{0}={1}", item.Key, item.Value);
            }
            if (websiteModel != null && !string.IsNullOrEmpty(websiteModel.ElemeAppSecret))
            {
                signo += websiteModel.ElemeAppSecret;
            }
            string value = ZentCloud.Common.DEncrypt.GetMD5(signo, "UTF-8").ToUpper();
            return value;
        }








        /// <summary>
        /// 获取订单状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private string GetOrderStatus(string status)
        {
            if (string.IsNullOrEmpty(status)) return "";
            string strStatus = string.Empty;
            switch (status)
            {
                case "pending":
                    strStatus = "未生效订单";
                    break;
                case "unprocessed":
                    strStatus = "未处理订单";
                    break;
                case "refunding":
                    strStatus = "退单处理中";
                    break;
                case "valid":
                    strStatus = "已处理的有效订单";
                    break;
                case "invalid":
                    strStatus = "无效订单";
                    break;
                case "settled":
                    strStatus = "已完成订单";
                    break;
                default:
                    break;
            }
            return strStatus;
        }
       /// <summary>
       /// 获取退单状态
       /// </summary>
       /// <param name="status"></param>
       /// <returns></returns>
        private string GetRefStatus(string status)
        {
            if (string.IsNullOrEmpty(status)) return "";
            string refStatus = string.Empty;
            switch (status)
            {
                case "noRefund":
                    status = "未申请退单";
                    break;
                case "applied":
                    status = "用户申请退单";
                    break;
                case "rejected":
                    status = "店铺拒绝退单";
                    break;
                case "arbitrating":
                    status = "客服仲裁中";
                    break;
                case "failed":
                    status = "退单失败";
                    break;
                case "successful":
                    status = "退单成功";
                    break;
                default:
                    break;
            }
            return status;
        }








        //获取GET返回来的数据
        private NameValueCollection GETInput()
        {
            return Request.QueryString;
        }
        // 获取POST返回来的数据
        private string PostInput()
        {
            try
            {
                System.IO.Stream s = Request.InputStream;
                int count = 0;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();
                while ((count = s.Read(buffer, 0, 1024)) > 0)
                {
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                }
                s.Flush();
                s.Close();
                s.Dispose();
                return builder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// 替换掉为T的时间
        public DateTime GetDate(string date)
        {
            var time = date.Replace("T", " ");
            return DateTime.Parse(time);
        }
        /// 响应饿了么实体
        public class ResponseModel
        {
            public string msg { get; set; }
        }
        /// 打印日志 测试
        public void ToLog(string message)
        {
            try
            {
                //using (StreamWriter sw = new StreamWriter("D:\\WebSite\\CommonPlatformDev\\log.txt", true, Encoding.UTF8))
                //{
                //    sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), message));
                //}

            }
            catch { }
        }

    }
}