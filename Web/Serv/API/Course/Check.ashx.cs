using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Course
{
    /// <summary>
    /// 检查是否可以下单
    /// </summary>
    public class Check : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        public void ProcessRequest(HttpContext context)
        {

            int skuId = int.Parse(context.Request["sku_id"]);

            try
            {

           
            #region 课程检查

            //var productSku = bllMall.GetProductSku(int.Parse(skuId));
            var courseOrderList = bllMall.GetList<WXMallOrderInfo>(string.Format(" OrderUserId='{0}' And OrderType=7 And Status!='已取消'", CurrentUserInfo.UserID));
            if (courseOrderList.Count > 0)
            {
                foreach (var item in courseOrderList.Where(p => p.PaymentStatus == 1))
                {

                    var courseOrderDetailList = bllMall.GetOrderDetailsList(item.OrderID);
                    if (courseOrderDetailList.Where(p => p.SkuId == skuId).Count() > 0)
                    {

                        apiResp.msg = "证书已购买";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;
                    }
                }
                foreach (var item in courseOrderList.Where(p => p.PaymentStatus == 0))
                {
                    var courseOrderDetailList = bllMall.GetOrderDetailsList(item.OrderID);
                    if (courseOrderDetailList.Where(p => p.SkuId == skuId).Count() > 0)
                    {
                        apiResp.msg = "证书已购买,但未支付,请先支付";
                        context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
                        return;
                    }
                }


            }
            apiResp.status = true;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

            #endregion
            }
            catch (Exception ex)
            {
                apiResp.msg = ex.ToString();
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
            }



        }


    }
}