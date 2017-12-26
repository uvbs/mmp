using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.CrowdFund
{
    /// <summary>
    /// 众筹订单
    /// </summary>
    public class Order : BaseHandlerNeedLoginAdmin
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        /// <summary>
        /// 众筹订单列表
        /// </summary>
        /// <returns></returns>
        private string List(HttpContext context) 
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebSiteOwner='{0}'", bll.WebsiteOwner));
            string status = context.Request["order_status"];
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
                if (crowdFundInfo == null)
                {
                    resp.errcode = 1;
                    resp.errmsg = "不存在订单";
                    return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
                OrderListModel order = new OrderListModel();
                order.order_id = item.RecordID;
                order.order_amount = item.Amount;
                order.order_status = item.OrderStatus;
                order.crowdfund_title = crowdFundInfo.Title;
                order.crowdfund_img_url = crowdFundInfo.CoverImage;
                order.crowdfund_originator = crowdFundInfo.Originator;
                order.receiver_name = item.Name;
                order.order_time =bll.GetTimeStamp(item.InsertDate);
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
        /// 获取众筹订单详情
        /// </summary>
        /// <returns></returns>
        private string Get(HttpContext context) 
        {
            string recordID = context.Request["order_id"];
            CrowdFundRecord crowdFundRecord = bll.Get<CrowdFundRecord>(string.Format(" RecordID={0}", recordID));
            CrowdFundInfo crowdFundInfo = bll.Get<CrowdFundInfo>(string.Format(" CrowdFundID={0}", crowdFundRecord.CrowdFundID));
            var item = bll.Get<CrowdFundItem>(string.Format("ItemId={0}", crowdFundRecord.ItemId));
            var data = new
            {
                receiver_name=crowdFundRecord.Name,
                receiver_phone=crowdFundRecord.Phone,
                receiver_address=crowdFundRecord.ReceiveAddress,
                crowdfund_img_url=bll.GetImgUrl(crowdFundInfo.CoverImage),
                crowdfund_title=crowdFundInfo.Title,
                order_amount= crowdFundRecord.Amount,
                order_id=crowdFundRecord.RecordID,
                order_status=crowdFundRecord.OrderStatus,
                crowdfund_originator=crowdFundInfo.Originator,
                order_time=crowdFundRecord.InsertDate != null?bll.GetTimeStamp((DateTime)crowdFundRecord.InsertDate):0,
                order_pay_time=crowdFundRecord.PayTime != null?bll.GetTimeStamp((DateTime)crowdFundRecord.PayTime):0,
                order_delivery_time=crowdFundRecord.DeliveryTime != null?bll.GetTimeStamp((DateTime)crowdFundRecord.DeliveryTime):0,
                express_company_name=crowdFundRecord.ExpressCompanyName,
                express_number = crowdFundRecord.ExpressNumber,
                express_company_code=crowdFundRecord.ExpressCompanyCode,
                item_productname = item.ProductName
            };
           


           
            return ZentCloud.Common.JSONHelper.ObjectToJson(data);
        }


        /// <summary>
        /// 编辑众筹订单状态接口
        /// </summary>
        /// <returns></returns>

        private string UpdateOrderStatus(HttpContext context) 
        {
            string orderId = context.Request["order_id"];
            string orderStatus = context.Request["order_status"];
            if (string.IsNullOrEmpty(orderStatus))
            {
                resp.errcode = -1;
                resp.errmsg = "order_status 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            int count=bll.Update(new CrowdFundRecord(), string.Format(" OrderStatus='{0}'", orderStatus), string.Format(" RecordID={0} ",orderId));
            if (count > 0)
            {
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "编辑众筹订单状态失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 众筹发货接口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateDelivery(HttpContext context) 
        {
            string orderId = context.Request["order_id"];
            string expressCompanyCode = context.Request["express_company_code"];
            string expressCompanyName = context.Request["express_company_name"];
            string expressNumber = context.Request["express_number"];
            if (string.IsNullOrEmpty(expressCompanyCode))
            {
                resp.errcode = -1;
                resp.errmsg = "express_company_code 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(expressCompanyName))
            {
                resp.errcode = -1;
                resp.errmsg = "express_company_name 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(expressNumber))
            {   
                resp.errcode = -1;
                resp.errmsg = "express_number 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            int count = bll.Update(new CrowdFundRecord(), string.Format(" ExpressCompanyCode='{0}', ExpressCompanyName='{1}' , ExpressNumber='{2}',DeliveryTime='{3}',OrderStatus='{4}' ", expressCompanyCode,expressCompanyName, expressNumber, DateTime.Now.ToString(), "已发货"), string.Format(" RecordID={0} ", orderId));
            if (count > 0)
            {
                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = "发货失败";
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
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
            public string crowdfund_title { get;set; }

            /// <summary>
            /// 订单金额
            /// </summary>
            public decimal order_amount { get; set; }

            /// <summary>
            /// 发起人
            /// </summary>
            public string crowdfund_originator { get; set; }

            /// <summary>
            /// 订单时间
            /// </summary>
            public double order_time { get; set; }

            /// <summary>
            /// 购买人姓名
            /// </summary>
            public string receiver_name { get; set; }

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
            /// <summary>
            /// 众筹选项列表
            /// </summary>
            public List<ItemModel> item_list { get; set; }

            /// <summary>
            /// 快递公司
            /// </summary>
            public string express_company_name { get; set; }

            /// <summary>
            /// 快递单号
            /// </summary>
            public string express_company_number { get; set; }

            /// <summary>
            /// 公司代码
            /// </summary>
            public string express_company_code { get; set; }
        }
        /// <summary>
        /// 众筹选项模型
        /// </summary>
        public class ItemModel
        {
            /// <summary>
            /// 选项Id
            /// </summary>
            public int item_id { get; set; }
            /// <summary>
            /// 选项金额
            /// </summary>
            public decimal item_amount { get; set; }
            /// <summary>
            /// 选项说明
            /// </summary>
            public string item_desc { get; set; }
            /// <summary>
            /// 商品名称
            /// </summary>
            public string item_productname { get; set; }
        }
    }
}