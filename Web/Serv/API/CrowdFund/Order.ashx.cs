using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.CrowdFund
{
    /// <summary>
    /// 众筹订单接口
    /// </summary>
    public class Order : BaseHandlerNeedLogin
    {
        /// <summary>
        /// 逻辑
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();

        /// <summary>
        /// 下单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {

            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
                CrowdFundItem itemInfo = bll.Get<CrowdFundItem>(string.Format(" ItemId={0} And Websiteowner='{1}' And CrowdFundID={2}", requestModel.crowdfund_item_id, bll.WebsiteOwner, requestModel.crowdfund_id));
                if (itemInfo == null)
                {

                    resp.errcode = 1;
                    resp.errmsg = " crowdfund_item_id,crowdfund_id 不存在,请检查";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                CrowdFundRecord model = new CrowdFundRecord();
                model.UserID = currentUserInfo.UserID;
                model.Amount = itemInfo.Amount;
                model.CrowdFundID = itemInfo.CrowdFundID;
                model.InsertDate = DateTime.Now;
                model.ItemId = itemInfo.ItemId;
                model.Name = requestModel.receiver_name;
                model.Phone = requestModel.receiver_phone;
                model.ReceiveAddress = requestModel.receiver_address;
                model.RecordID = int.Parse(bll.GetGUID(BLLJIMP.TransacType.CommAdd));
                model.WebsiteOwner = bll.WebsiteOwner;
                model.CrowdFundID = itemInfo.CrowdFundID;
                model.OrderStatus = "待付款";
                model.BuyerMemo = requestModel.buyer_memo;
                CrowdFundInfo crowdFundInfo = bll.Get<CrowdFundInfo>(string.Format(" CrowdFundID={0} ", requestModel.crowdfund_id));
                if (crowdFundInfo == null)
                {
                    resp.errcode = 1;
                    resp.errmsg = " 众筹不存在";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (crowdFundInfo.Status == 0)
                {
                    resp.errcode = 1;
                    resp.errmsg = " 众筹已停止";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (DateTime.Now>=crowdFundInfo.StopTime)
                {
                    resp.errcode = 1;
                    resp.errmsg = " 众筹已到截止时间";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                //检查必填项
                if (string.IsNullOrEmpty(model.Name))
                {
                    resp.errcode = 1;
                    resp.errmsg = " 收货人姓名不能为空";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (string.IsNullOrEmpty(model.Phone))
                {
                    resp.errcode = 1;
                    resp.errmsg = " 收货人电话不能为空";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (string.IsNullOrEmpty(model.ReceiveAddress))
                {
                    resp.errcode = 1;
                    resp.errmsg = " 收货地址不能为空";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                if (!bll.Add(model))
                {

                    resp.errcode = 1;
                    resp.errmsg = "下单失败";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }

                return ZentCloud.Common.JSONHelper.ObjectToJson(new
                {
                    errcode = 0,
                    errmsg = "ok",
                    order_id = model.RecordID
                });


            }
            catch (Exception ex)
            {

                resp.errcode = 1;
                resp.errmsg = "JSON格式错误,请检查。错误信息:" + ex.Message;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }



        }

        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string status = context.Request["order_status"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebSiteOwner='{0}' AND UserID='{1}'", bll.WebsiteOwner, currentUserInfo.UserID));
            if (!string.IsNullOrEmpty(status))
            {
                sbWhere.AppendFormat(" And OrderStatus ='{0}'", status);
            }
            int totalCount = bll.GetCount<CrowdFundRecord>(sbWhere.ToString());
            var sourceData = bll.GetLit<CrowdFundRecord>(pageSize, pageIndex, sbWhere.ToString());

            List<OrderListModel> list = new List<OrderListModel>();
            foreach (var item in sourceData)
            {
                CrowdFundInfo crowdFundInfo = bll.Get<CrowdFundInfo>(string.Format(" CrowdFundID={0}", item.CrowdFundID));
                OrderListModel order = new OrderListModel();
                order.order_id = item.RecordID;
                order.order_amount = item.Amount;
                order.order_status = item.OrderStatus;
                order.crowdfund_title = crowdFundInfo.Title;
                order.crowdfund_img_url = bll.GetImgUrl(crowdFundInfo.CoverImage);
                order.crowdfund_originator = crowdFundInfo.Originator;
                order.expree_company_name = item.ExpressCompanyName;
                order.express_company_code = item.ExpressCompanyCode;
                order.express_number = item.ExpressNumber;
                list.Add(order);
            }
            var data = new
            {
                totalcount = totalCount,//数量
                list = list,//列表
            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);
        }

        /// <summary>
        /// 众筹订单详情
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Get(HttpContext context)
        {
            string recordId = context.Request["order_id"];
            CrowdFundRecord crowdFundRecord = bll.Get<CrowdFundRecord>(string.Format(" RecordID={0}", recordId));
            if (crowdFundRecord == null)
            {
                resp.errmsg = "订单号错误";
                resp.errcode = 1;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (crowdFundRecord.UserID != currentUserInfo.UserID)
            {
                resp.errmsg = "拒绝查看";
                resp.errcode = 1;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            CrowdFundInfo crowdFundInfo = bll.Get<CrowdFundInfo>(string.Format(" CrowdFundID={0}", crowdFundRecord.CrowdFundID));
            var item = bll.Get<CrowdFundItem>(string.Format("ItemId={0}", crowdFundRecord.ItemId));
            var data = new
            {
                receiver_name = crowdFundRecord.Name,
                receiver_phone = crowdFundRecord.Phone,
                receiver_address = crowdFundRecord.ReceiveAddress,
                crowdfund_img_url = crowdFundInfo.CoverImage,
                crowdfund_title = crowdFundInfo.Title,
                order_amount = crowdFundRecord.Amount,
                order_id = crowdFundRecord.RecordID,
                order_status = crowdFundRecord.OrderStatus,
                crowdfund_originator = crowdFundInfo.Originator,
                order_time = crowdFundRecord.InsertDate != null ? bll.GetTimeStamp((DateTime)crowdFundRecord.InsertDate) : 0,
                order_pay_time = crowdFundRecord.PayTime != null ? bll.GetTimeStamp((DateTime)crowdFundRecord.PayTime) : 0,
                order_delivery_time = crowdFundRecord.DeliveryTime != null ? bll.GetTimeStamp((DateTime)crowdFundRecord.DeliveryTime) : 0,
                express_company_name = crowdFundRecord.ExpressCompanyName,
                express_number = crowdFundRecord.ExpressNumber,
                express_company_code = crowdFundRecord.ExpressCompanyCode,
                item_productname = item.ProductName
            };
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ReceiptConfirm(HttpContext context)
        {
            string orderId = context.Request["order_id"];
            if (string.IsNullOrEmpty(orderId))
            {
                resp.errmsg = "订单号错误";
                resp.errcode = 1;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            CrowdFundRecord orderInfo = bll.Get<CrowdFundRecord>(string.Format(" RecordID={0} AND UserID='{1}' AND WebsiteOwner='{2}'", orderId, currentUserInfo.UserID, bll.WebsiteOwner));
            if (orderInfo == null)
            {
                resp.errmsg = "订单不存在";
                resp.errcode = 1;
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            orderInfo.OrderStatus = "确认收货";
            if (bll.Update(orderInfo))
            {
                resp.errmsg = "ok";
                resp.errcode = 0;
            }
            else
            {
                resp.errcode = -1;
                resp.errmsg = "确认收货出错";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
        }



        /// <summary>
        /// 请求模型
        /// </summary>
        public class RequestModel
        {
            /// <summary>
            /// 众筹ID
            /// </summary>
            public int crowdfund_id { get; set; }
            /// <summary>
            /// 众筹选项ID
            /// </summary>
            public int crowdfund_item_id { get; set; }
            /// <summary>
            /// 收货人姓名
            /// </summary>
            public string receiver_name { get; set; }
            /// <summary>
            /// 收货人手机
            /// </summary>
            public string receiver_phone { get; set; }
            /// <summary>
            /// 收货地址
            /// </summary>
            public string receiver_address { get; set; }
            /// <summary>
            /// 买家留言
            /// </summary>
            public string buyer_memo { get; set; }
        }

        /// <summary>
        /// 订单列表
        /// </summary>
        public class OrderListModel
        {
            /// <summary>
            /// 订单编号
            /// </summary>
            public int order_id { get; set; }

            /// <summary>
            /// 订单状态
            /// </summary>
            public string order_status { get; set; }

            /// <summary>
            ///图片 
            /// </summary>
            public string crowdfund_img_url { get; set; }

            /// <summary>
            /// 众筹标题
            /// </summary>
            public string crowdfund_title { get; set; }

            /// <summary>
            /// 订单金额
            /// </summary>
            public decimal order_amount { get; set; }

            /// <summary>
            /// 发起人
            /// </summary>
            public string crowdfund_originator { get; set; }

            /// <summary>
            /// 快递公司名称
            /// </summary>
            public string expree_company_name { get; set; }

            /// <summary>
            /// 快递公司代码
            /// </summary>
            public string express_company_code { get; set; }

            /// <summary>
            /// 快递单号
            /// </summary>
            public string express_number { get; set; }
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        public class OrderDetailModel : OrderListModel
        {
            /// <summary>
            /// 收货人
            /// </summary>
            public string receiver_name { get; set; }

            /// <summary>
            /// 联系电话
            /// </summary>
            public string receiver_phone { get; set; }
            /// <summary>
            /// 收货地址
            /// </summary>
            public string receiver_address { get; set; }

            /// <summary>
            /// 下单时间
            /// </summary>
            public double order_time { get; set; }

            /// <summary>
            /// 付款时间
            /// </summary>
            public double order_pay_time { get; set; }

            /// <summary>
            /// 发货时间
            /// </summary>
            public double order_delivery_time { get; set; }


        }


    }
}