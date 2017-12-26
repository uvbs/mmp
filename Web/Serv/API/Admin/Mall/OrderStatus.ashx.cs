using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public class OrderStatus : BaseHandlerNeedLoginAdmin
    {

        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 获取标签信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {

            var sourceData = bllMall.GetOrderStatuList();
            var list = from p in sourceData
                       select new
                       {
                           order_status_id=p.AutoID,
                           order_status=p.OrderStatu,
                           order_status_sort=p.Sort
                       };

            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = sourceData.Count,
                list = list

            });

        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {
            string orderStatus=context.Request["order_status"];
            string sort=context.Request["sort"];
            int sortInt = 0;
            if (!string.IsNullOrEmpty(sort))
            {
                sortInt = int.Parse(sort);
            }
            string msg="";
            var isSuccess = bllMall.AddOrderStatus(orderStatus, out msg, sortInt);
            if (isSuccess)
            {

                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = msg;
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {
            int orderStatusId=int.Parse(context.Request["order_status_id"]);
            string orderStatus = context.Request["order_status"];
            string sort = context.Request["sort"];
            int sortInt = 0;
            if (!string.IsNullOrEmpty(sort))
            {
                sortInt = int.Parse(sort);
            }
            string msg = "";
            var isSuccess = bllMall.UpdateOrderStatus(orderStatusId, orderStatus, out msg, sortInt);
            if (isSuccess)
            {

                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = msg;
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        ///删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {

            string orderStatusIds = context.Request["order_status_ids"];
            string msg = "";
            var isSuccess = bllMall.DeleteOrderStatus(orderStatusIds, out msg);
            if (isSuccess)
            {

                resp.errmsg = "ok";
            }
            else
            {
                resp.errcode = 1;
                resp.errmsg = msg;
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }




    }
}