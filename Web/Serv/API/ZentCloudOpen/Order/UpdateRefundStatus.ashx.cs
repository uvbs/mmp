using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Order
{
    /// <summary>
    /// 订单退款/退押金申请启用接口
    /// </summary>
    public class UpdateRefundStatus : BaseHanderOpen
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
            string type = context.Request["type"];//类型 1 退款 2退押金
            string outSn = context.Request["out_sn"];//外部交易号
            string amountStr = context.Request["amount"];//金额
            string remark = context.Request["remark"];//备注
            //decimal amount = 0;

            string title = "";
            string content = string.Format("订单号:{0}\\n备注:",orderSn);
            if (string.IsNullOrEmpty(orderSn))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "order_sn 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(type))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "type 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(outSn))
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "out_sn 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            //if (string.IsNullOrEmpty(amountStr))
            //{
            //    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
            //    resp.msg = "amount 参数必传";
            //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            //    return;
            //}
            //if (string.IsNullOrEmpty(remark))
            //{
            //    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
            //    resp.msg = "remark 参数必传";
            //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            //    return;
            //}
            if (!string.IsNullOrEmpty(remark))
            {
                remark = HttpUtility.UrlDecode(remark);
            }
            //if (!decimal.TryParse(amountStr, out amount))
            //{
            //    resp.code = (int)APIErrCode.OperateFail;
            //    resp.msg = "amount 参数错误,请检查";
            //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            //    return;
            //}
            //if (amount <= 0)
            //{
            //    resp.code = (int)APIErrCode.OperateFail;
            //    resp.msg = "amount 参数不能小于等于0";
            //    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            //    return;
            //}
            var orderInfo = bllMall.GetOrderInfoByOutOrderId(orderSn);
            if (orderInfo==null)
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "order_sn 不存在,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            orderInfo.OutTranNo = outSn;
            orderInfo.Ex14 = remark;
            //orderInfo.Ex15 = amountStr;//
            orderInfo.LastUpdateTime = DateTime.Now;
            orderInfo.Ex8 ="0";
            orderInfo.Ex9 ="";
            switch (type)
            {
                case "1":
                    orderInfo.Ex12 = "1";//可以退款
                    //title = "订单" + orderInfo.OrderID + "可以进行退款啦";
                    break;
                case "2":
                    orderInfo.Ex13 = "1";//可以退押金
                    orderInfo.Status = "待退押金中";
                    title = "可以退押金啦";
                    break;
                default:
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "不合法的 type 值";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (bllMall.Update(orderInfo))
            {
                if (type == "2")
                {
                    var orderUserInfo = bllUser.GetUserInfo(orderInfo.OrderUserID);
                    if (orderUserInfo != null && !string.IsNullOrWhiteSpace(orderUserInfo.WXOpenId))
                    {
                        string tbMsg = "可退押金的订单号 " + orderInfo.OrderID;

                        //if (!string.IsNullOrWhiteSpace(amountStr))
                        //{
                        //    tbMsg += "\\n可退金额：" + amountStr;
                        //}
                        string url = string.Empty;

                        if (bllMall.WebsiteOwner == "jikuwifi")
                        {
                            url = string.Format("http://jikuwifi.comeoncloud.net/customize/jikuwifi/Index.aspx?v=1.0&ngroute=/orderDetail/{0}#/orderDetail/{0}", orderInfo.OrderID);
                        }

                        bllWeixin.SendTemplateMessageNotifyComm(orderUserInfo, title, tbMsg, url);

                    }
                }
                //
                resp.msg = "ok";
                resp.status = true;
            }
            else
            {
                resp.code = (int)APIErrCode.OperateFail;
                resp.msg = "操作失败";

            }
            //using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"D:\jikudevlog.txt", true, System.Text.Encoding.GetEncoding("gb2312")))
            //{

            //    sw.WriteLine(string.Format("orderSn{0}type{1}outSn{2}amountStr{3}remark{4}", orderSn, type, outSn, amountStr, remark));
            //    sw.WriteLine(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            //}
            
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            
        }

       
    }
}