using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Car.Model
{
    /// <summary>
    /// 车系列表
    /// </summary>
    public class SeriesList : CarBaseHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            var cateId = Convert.ToInt32(context.Request["cateId"]);
            var brandId = Convert.ToInt32(context.Request["brandId"]);

            var dataList = bll.GetSeriesList(cateId, brandId);

            dataList = dataList.Where(p => p.CarSeriesName.IndexOf("停售") == -1).ToList();

            resp.isSuccess = true;

            resp.returnObj = new
            {
                list = dataList.Select(p => new {
                    carSeriesId = p.CarSeriesId,
                    carSeriesName = p.CarSeriesName
                })
            };

            bll.ContextResponse(context, resp);
        }


    }
}