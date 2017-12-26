using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Gift.Giving
{
    /// <summary>
    /// 没有使用
    /// </summary>
    public class ReceiveGift : IHttpHandler,IReadOnlySessionState
    {

        /// <summary>
        /// 响应实体
        /// </summary>
        DefaultResponse resp = new DefaultResponse();


        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            if (bllUser.IsLogin == false)
            {
                resp.errcode = -1;
                resp.errmsg = "你还没有登录,请先登录";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            string data = context.Request["data"];
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {

                resp.errcode = -1;
                resp.errmsg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            //检查必填项
            if (requestModel.order_id <= 0)
            {
                resp.errcode = 1;
                resp.errmsg = "订单号出错";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.receiver_name))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入收货人姓名";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.receiver_phone))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入收货人电话";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.receiver_province))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入收货人省份名称";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.receiver_province_code))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入收货人省份代码";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.receiver_city))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入收货人城市名称";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.receiver_city_code))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入收货人城市代码";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.receiver_dist))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入收货人区域名称";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.receiver_dist_code))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入收货人区域代码";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.receiver_address))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入收货人街道地址";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.receiver_zip))
            {
                resp.errcode = 1;
                resp.errmsg = "请输入收货人邮政编码";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            WXMallGiftOrderInfo giftorder = bllUser.Get<WXMallGiftOrderInfo>(string.Format(" OrderId={0} AND UserId='{1}' AND WebsiteOwner='{2}'", requestModel.order_id, bllUser.GetCurrUserID(), bllUser.WebsiteOwner));
            //检查是否已经领取
            if (giftorder != null)
            {
                resp.errcode = -1;
                resp.errmsg = "你已经领取过了";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            //查看订单详情
            WXMallOrderDetailsInfo orderdetail = bllUser.Get<WXMallOrderDetailsInfo>(string.Format(" OrderID={0} ", requestModel.order_id));
            int OrderCount = bllUser.GetCount<WXMallGiftOrderInfo>(string.Format(" OrderId={0} ", requestModel.order_id));

            if (OrderCount >= orderdetail.TotalCount)
            {
                resp.errcode = -1;
                resp.errmsg = "已经领完";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            WXMallGiftOrderInfo mallGiftOrderInfo = new WXMallGiftOrderInfo();
            mallGiftOrderInfo.GiftOrderId = int.Parse(bllUser.GetGUID(BLLJIMP.TransacType.CommAdd));
            mallGiftOrderInfo.OrderId = requestModel.order_id;
            mallGiftOrderInfo.ReceiveName = requestModel.receiver_name;
            mallGiftOrderInfo.ReceivePhone = requestModel.receiver_phone;
            mallGiftOrderInfo.InsertDate = DateTime.Now;
            mallGiftOrderInfo.WebsiteOwner = bllUser.WebsiteOwner;
            mallGiftOrderInfo.UserId = bllUser.GetCurrUserID();
            mallGiftOrderInfo.ReceiverProvince = requestModel.receiver_province;
            mallGiftOrderInfo.ReceiverProvinceCode = requestModel.receiver_province_code;
            mallGiftOrderInfo.ReceiverCity = requestModel.receiver_city;
            mallGiftOrderInfo.ReceiverCityCode = requestModel.receiver_city_code;
            mallGiftOrderInfo.ReceiverDist = requestModel.receiver_dist;
            mallGiftOrderInfo.ReceiverDistCode = requestModel.receiver_dist_code;
            mallGiftOrderInfo.Address = requestModel.receiver_address;
            mallGiftOrderInfo.ZipCode = requestModel.receiver_zip;
            if (bllUser.Add(mallGiftOrderInfo))
            {
                resp.errcode = 0;
                resp.errmsg = "领取成功";
            }
            else
            {
                resp.errcode = -1;
                resp.errmsg = "领取出错";
            }

            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            return;

        }

        public class RequestModel
        {
            /// <summary>
            /// 订单编号
            /// </summary>
            public int order_id { get; set; }

            /// <summary>
            /// 收货人姓名
            /// </summary>
            public string receiver_name { get; set; }

            /// <summary>
            /// 收货人省份
            /// </summary>
            public string receiver_province { get; set; }
            /// <summary>
            /// 收货人省份代码
            /// </summary>
            public string receiver_province_code { get; set; }
            /// <summary>
            /// 收货人城市名称
            /// </summary>
            public string receiver_city { get; set; }
            /// <summary>
            /// 收货人城市代码
            /// </summary>
            public string receiver_city_code { get; set; }

            /// <summary>
            /// 收货人区域名称
            /// </summary>
            public string receiver_dist { get; set; }
            /// <summary>
            /// 收货人区域代码
            /// </summary>
            public string receiver_dist_code { get; set; }
            /// <summary>
            /// 街道地址
            /// </summary>
            public string receiver_address { get; set; }
            /// <summary>
            /// 收货人邮编
            /// </summary>
            public string receiver_zip { get; set; }
            /// <summary>
            /// 收货人电话
            /// </summary>
            public string receiver_phone { get; set; }


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