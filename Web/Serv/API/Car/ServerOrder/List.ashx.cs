using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Car.ServerOrder
{
    /// <summary>
    /// 获取我的订单列表
    /// </summary>
    public class List : CarBaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            resp.isSuccess = false;

            try
            {
                if (!bll.IsLogin)
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                    bll.ContextResponse(context, resp);
                    return;
                }

                var pageSize = Convert.ToInt32(context.Request["page_size"]);
                var pageIndex = Convert.ToInt32(context.Request["page_index"]);
                var commentStatus = context.Request["comment_status"];

                int totalCount = 0;

                pageSize = pageSize == 0 ? 20 : pageSize;
                pageIndex = pageIndex == 0 ? 1 : pageIndex;

                var list = bll.GetCarServerOrderList(out totalCount, pageSize, pageIndex, bll.WebsiteOwner, "", bll.GetCurrUserID(),commentStatus);

                List<dynamic> returnList = new List<dynamic>();

                resp.isSuccess = true;

                foreach (var item in list)
                {
                    var server = bll.GetServer(item.ServerId);

                    returnList.Add(new
                    {
                        id = item.OrderId,
                        order_id = item.OrderId,
                        server_name = server.ServerName,
                        status = item.Status,
                        status_str = item.StatusStr,
                        book_arrvie_date = item.BookArrvieDate,
                        book_arrvie_starttime = item.BookArrvieStartTime,
                        book_arrvie_endtime = item.BookArrvieEndTime,
                        book_arrvie_date_Str = item.BookArrvieDateStr,
                        comment_status = item.CommentStatus,
                        createtime = item.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                        saller = item.Saller
                    });
                }

                resp.returnObj = new
                {
                    total_count = totalCount,
                    list = returnList
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