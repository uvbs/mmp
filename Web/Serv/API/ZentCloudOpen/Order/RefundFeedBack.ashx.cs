using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Order
{
    /// <summary>
    /// 申请退款退押金反馈接口
    /// </summary>
    public class RefundFeedBack : BaseHanderOpen
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        ///微信
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        /// <summary>
        /// 用户
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string orderSn = context.Request["order_sn"];//订单号
            string status = context.Request["status"];//状态 0不同意 1同意
            string remark = context.Request["remark"];//备注
            string type = context.Request["type"];//类型 1 退款 2退押金

            string tpMsgTitle = string.Empty, tpMsgContent = string.Empty;//模板消息

            if (string.IsNullOrEmpty(type))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "type 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(orderSn))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "order_sn 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(status))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "status 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (status != "1" && status != "0")
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "status 参数值不合法";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(remark))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "remark 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            var orderInfo = bllMall.GetOrderInfoByOutOrderId(orderSn);
            if (orderInfo == null)
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "order_sn 不存在,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            //using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"D:\wifilog.txt", true, System.Text.Encoding.GetEncoding("gb2312")))
            //{
            //    sw.WriteLine(string.Format("type:{0}", type));
            //}
            switch (type)
            {
                case "1"://退款
                    orderInfo.Ex18 = status;
                    if (status == "0")//拒绝后用户可申请退款
                    {
                        if (string.IsNullOrEmpty(orderInfo.ExpressNumber))//发货前可重新申请退款
                        {
                            orderInfo.Ex11 = "0";//退款
                            orderInfo.IsRefund = 0;
                        }
                        tpMsgTitle = "已经拒绝您的退款申请";
                    }
                    else
                    {
                        tpMsgTitle = "已经同意您的退款申请";
                    }
                    orderInfo.Ex19 =HttpUtility.UrlDecode(remark);
                    break;
                case "2"://退押金
                    orderInfo.Ex20 = status;
                    orderInfo.Ex21 = HttpUtility.UrlDecode(remark);

                    if (status == "0")
                    {
                        tpMsgTitle = "已经拒绝您的退押金申请";
                    }
                    else
                    {
                        tpMsgTitle = "已经同意您的退押金申请";
                    }


                    break;
                default:
                    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.msg = "不合法的 type 值";
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
            }


            orderInfo.LastUpdateTime = DateTime.Now;
            if (bllMall.Update(orderInfo))
            {
                resp.msg = "ok";
                resp.status = true;

                tpMsgContent = "订单号 " + orderInfo.OrderID;

                if (status == "0")
                {
                    tpMsgContent += "\\n" + HttpUtility.UrlDecode(remark);
                }

                var orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID);
                if (orderUserInfo != null && !string.IsNullOrWhiteSpace(orderUserInfo.WXOpenId))
                {
                    string url = string.Empty;

                    if (bllMall.WebsiteOwner == "jikuwifi")
                    {
                        url = string.Format("http://jikuwifi.comeoncloud.net/customize/jikuwifi/Index.aspx?v=1.0&ngroute=/orderDetail/{0}#/orderDetail/{0}", orderInfo.OrderID);
                    }

                    bllWeixin.SendTemplateMessageNotifyComm(orderUserInfo, tpMsgTitle, tpMsgContent, url);
                }

            }
            else
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "操作失败";

            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));

        }



    }
}