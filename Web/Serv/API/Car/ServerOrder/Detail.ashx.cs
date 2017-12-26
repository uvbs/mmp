using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Car.ServerOrder
{
    /// <summary>
    /// 服务订单详情
    /// </summary>
    public class Detail : CarBaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            resp.isSuccess = false;

            try
            {
                if (!bll.IsLogin)
                {
                    resp.errmsg = "未登录";
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                    bll.ContextResponse(context, resp);
                    return;
                }

                var orderId = Convert.ToInt32(context.Request["order_id"]);

                var data = bll.GetCarServerOrderDetail(orderId);

                var server = bll.GetServer(data.ServerId);
                
                resp.isSuccess = true;

                resp.returnObj = new
                {                    
                    id = data.OrderId,
                    order_id = data.OrderId,
                    server_name = server.ServerName,
                    
                    status = data.Status,
                    status_str = data.StatusStr,
                    book_arrvie_date = data.BookArrvieDate,
                    book_arrvie_starttime = data.BookArrvieStartTime,
                    book_arrvie_endtime = data.BookArrvieEndTime,
                    book_arrvie_date_Str = data.BookArrvieDateStr,
                    comment_status = data.CommentStatus,
                    createtime = data.CreateTime.ToString("yyyy-MM-dd HH:mm"),

                    total_price = data.TotalPrice,

                    car_brand_id = data.CarBrandId,
                    car_series_cate_id = data.CarSeriesCateId,
                    car_series_id = data.CarSeriesId,
                    car_model_id = data.CarModelId,

                    car_model = data.CarModel,
                    
                    owner_name = data.CarOwnerName,
                    owner_phone = data.CarOwnerPhone,

                    saller = data.Saller
                };

            }
            catch (Exception ex)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }

            bll.ContextResponse(context, resp);
        }

    }
}