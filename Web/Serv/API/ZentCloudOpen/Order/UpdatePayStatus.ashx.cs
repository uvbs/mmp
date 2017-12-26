using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Order
{
    /// <summary>
    /// 修改订单付款状态
    /// </summary>
    public class UpdatePayStatus : BaseHanderOpen
    {
        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 积分BLL
        /// </summary>
        BLLJIMP.BllScore bllScore = new BLLJIMP.BllScore();
        /// <summary>
        ///微信
        /// </summary>
        BLLJIMP.BLLWeixin bllWeixin = new BLLJIMP.BLLWeixin();
        /// <summary>
        /// 用户
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 卡券BLL
        /// </summary>
        BLLJIMP.BLLCardCoupon bllCardCoupon = new BLLJIMP.BLLCardCoupon();
        public void ProcessRequest(HttpContext context)
        {

            string orderSn = context.Request["order_sn"];//订单号
            string status = context.Request["pay_status"];// 0 1

            ToLog(string.Format("orderSn:{0}",orderSn));

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
                resp.msg = "pay_status 参数必传";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            var orderInfo = bllMall.GetOrderInfoByOutOrderId(orderSn);
            if (orderInfo == null)
            {
                resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.msg = "order_sn 不存在,请检查";

                ToLog(resp.msg + orderSn + context.Request.Url.Host + context.Request.Url.Port + bllMall.WebsiteOwner + ZentCloud.Common.ConfigHelper.GetConfigString("ConnectionString"));
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }

            orderInfo.Status = "待发货";
            orderInfo.PayTime = DateTime.Now;
            if (orderInfo.DeliveryType == 1)
            {
                orderInfo.Status = "待自提";
            }
            orderInfo.PaymentStatus = Convert.ToInt32(status);
            orderInfo.PayTime = DateTime.Now;

            bool result = bllMall.Update(orderInfo);
            if (result)
            {
                resp.status = true;
                resp.msg = "ok";
            }
            else
            {
                resp.msg = "更新订单失败";
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
            return;

           



        }


        private void ToLog(string msg)
        {
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"D:\log.txt", true, System.Text.Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), msg));
                }
            }
            catch { }
        }


    }

}