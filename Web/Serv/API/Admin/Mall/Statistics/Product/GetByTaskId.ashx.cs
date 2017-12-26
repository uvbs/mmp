using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Mall.Statistics.Product
{

        /// <summary>
        /// 商品统计
        /// </summary>
        public class GetByTaskId : BaseHandlerNeedLoginAdminNoAction
        {
            BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
            public void ProcessRequest(HttpContext context)
            {
                string taskId = context.Request["task_id"];
                string sortField=context.Request["sort"];//排序字段
                string order=context.Request["order"];//asc desc
                string keyWord=context.Request["keyword"];
                string orderBy = " Order By AutoId ASC";
                System.Text.StringBuilder sbWhere = new System.Text.StringBuilder();
                sbWhere.AppendFormat("TaskId='{0}' And WebsiteOwner='{1}'",taskId, bllMall.WebsiteOwner);
                if (!string.IsNullOrEmpty(keyWord))
                {
                    sbWhere.AppendFormat(" And ProductName like '%{0}%'",keyWord);
                }
                if (!string.IsNullOrEmpty(sortField))
                {
                    switch (sortField)
                    {
                        case "order_total_count"://总订单数
                            orderBy = " Order By OrderTotalCount ";
                            break;
                        case "order_total_amount"://总订单金额
                            orderBy = " Order By OrderTotalAmount ";
                            break;
                        case "order_base_total_amount"://总基价
                            orderBy = " Order By OrderBaseTotalAmount ";
                            break;
                        case "profit"://总利润
                            orderBy = " Order By Profit ";
                            break;
                        case "order_total_order_price"://总售价
                            orderBy = " Order By OrderTotalOrderPrice ";
                            break;
                        case "product_total_count"://商品总件数
                            orderBy = " Order By ProductTotalCount ";
                            break;
                        case "product_name"://商品名称
                            orderBy = " Order By ProductName ";
                            break;
                        default:
                            break;
                    }
                    orderBy += order;


                }


                sbWhere.AppendFormat(" {0}", orderBy);
                List<BLLJIMP.Model.WXMallStatisticsProduct> list = bllMall.GetList<BLLJIMP.Model.WXMallStatisticsProduct>(sbWhere.ToString());
                apiResp.status = true;
                apiResp.msg = "ok";
                apiResp.result = new
                {
                    totalcount = list.Count,
                    list = from p in list
                           select new
                           {

                               insert_date = p.InsertDate.ToString(),
                               from_date = p.FromDate != null ? ((DateTime)p.FromDate).ToString("yyyy-MM-dd HH:mm:ss") : "",
                               to_date = p.ToDate != null ? ((DateTime)p.ToDate).ToString("yyyy-MM-dd HH:mm:ss") : "",
                               product_name=p.ProductName,
                               product_total_count=p.ProductTotalCount,
                               order_total_count = p.OrderTotalCount,
                               order_total_amount = p.OrderTotalAmount,
                               order_base_total_amount = p.OrderBaseTotalAmount,
                               order_total_order_price=p.OrderTotalOrderPrice,
                               profit = p.Profit,
                               total_refund_amount = p.TotalRefundAmount

                           }


                };
                bllMall.ContextResponse(context, apiResp);



            }


        }


    
}