using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Car.Model
{
    /// <summary>
    /// 车系类别列表
    /// </summary>
    public class SeriesCateList : CarBaseHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            var brandId = Convert.ToInt32(context.Request["brandId"]);

            var dataList = bll.GetSeriesCateList(brandId);

            resp.isSuccess = true;

            resp.returnObj = new
            {
                list = dataList.Select(p => new {
                    carSeriesCateId = p.CarSeriesCateId,
                    carSeriesCateName = p.CarSeriesCateName
                })
            };

            bll.ContextResponse(context, resp);

        }
        
    }
}