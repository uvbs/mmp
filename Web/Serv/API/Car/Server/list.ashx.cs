using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Car.Server
{
    /// <summary>
    /// 获取服务列表
    /// </summary>
    public class list : CarBaseHandler
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

                var pageSize = Convert.ToInt32(context.Request["pageSize"]);
                var pageIndex = Convert.ToInt32(context.Request["pageIndex"]);
                var carModelId = Convert.ToInt32(context.Request["carModelId"]);
                var cateId = Convert.ToInt32(context.Request["cateId"]);
                var serverType = context.Request["serverType"];
                var shopType = context.Request["shopType"];

                int totalCount = 0;

                pageSize = pageSize == 0 ? 20000 : pageSize;
                pageIndex = pageIndex == 0 ? 1 : pageIndex;

                var list = bll.GetServerList(out totalCount, bll.WebsiteOwner, pageSize, pageIndex, cateId, 0, 0, 0, carModelId, serverType, shopType);

                List<ReturnServer> resultList = new List<ReturnServer>();

                //拆分每个商户一个数据
                foreach (var item in list)
                {
                    var serverSallerList = bll.GetServerSallerList(item.ServerId);

                    foreach (var saller in serverSallerList)
                    {
                        ReturnServer data = new ReturnServer();

                        data.saller_id = saller.UserID;
                        data.saller_name = saller.Company;
                        data.server_id = item.ServerId;
                        data.server_name = item.ServerName;

                        

                        //计算未来七天日期
                        var dayDataList = bll.GetCarServerNext7DayPrice(item.ServerId, saller.UserID);

                        //取出最低价格
                        data.lowest_price = dayDataList.Min(p => p.Price);
                        
                        ////获取七天详情
                        //data.day_data_list = dayDataList.Select(p => new
                        //{
                        //    date = p.Date,
                        //    start_time = p.StartTime,
                        //    end_time = p.EndTime,
                        //    price = p.Price,
                        //    week = p.Week,
                        //    workhours = p.Workhours,
                        //    workhours_price = p.WorkhoursPrice,
                        //    workhours_rate = p.WorkhoursRate,
                        //    parts_rate = p.PartsRate,
                        //    parts_detail = p.PartsDetail == null? null:p.PartsDetail.Select(parts => new
                        //    {
                        //        parts_id = parts.PartsId,
                        //        parts_name = parts.PartsName,
                        //        count = parts.Count,
                        //        price = parts.Price
                        //    })
                        //});

                        var dayGroup = dayDataList.GroupBy(p => p.Date).Select(p => p.Key).ToList();

                        List<dynamic> dayResult = new List<dynamic>();

                        foreach (var day in dayGroup)
                        {
                           
                            dayResult.Add(new
                            {
                                day = day,
                                timeList = dayDataList.Where(o => o.Date == day).Select(p => new
                                {
                                    tempid = Guid.NewGuid().ToString(),
                                    server_id = item.ServerId,
                                    saller_id = saller.UserID,
                                    saller_name = saller.Company,
                                    server_name = item.ServerName,
                                    saller_presale_linker = saller.Ex3,
                                    saller_presale_linker_phone = saller.Ex5,
                                    date = p.Date,
                                    start_time = p.StartTime,
                                    end_time = p.EndTime,
                                    price = p.Price,
                                    week = p.Week,
                                    workhours = p.Workhours,
                                    workhours_price = p.WorkhoursPrice,
                                    workhours_rate = p.WorkhoursRate,
                                    parts_rate = p.PartsRate,
                                    parts_detail = p.PartsDetail == null ? null : p.PartsDetail.Select(parts => new
                                    {
                                        parts_id = parts.PartsId,
                                        parts_name = parts.PartsName,
                                        count = parts.Count,
                                        price = parts.Price,
                                        total_price = parts.TotalPrice
                                    })
                                })
                            });
                        }

                        data.day_data_list = dayResult;
                        
                        resultList.Add(data);
                    }

                }
                
                resp.isSuccess = true;

                resp.returnObj = new
                {
                    list = resultList
                };
            }
            catch (Exception ex)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }

            bll.ContextResponse(context, resp);
        }

        /// <summary>
        /// 返回服务列表
        /// </summary>
        public struct ReturnServer
        {
            /// <summary>
            /// 服务id
            /// </summary>
            public int server_id { get; set; }
            /// <summary>
            /// 服务名称
            /// </summary>
            public string server_name { get; set; }
            /// <summary>
            /// 商户id
            /// </summary>
            public string saller_id { get; set; }
            /// <summary>
            /// 商户名称
            /// </summary>
            public string saller_name { get; set; }            
            /// <summary>
            /// 最低价格
            /// </summary>
            public double lowest_price { get; set; }

            public dynamic day_data_list { get; set; }
        }

    }
}