using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZCJson.Linq;

namespace ZentCloud.JubitIMP.Web.Serv.API.Car.Model
{
    /// <summary>
    /// ModelList 的摘要说明
    /// </summary>
    public class ModelList : CarBaseHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            var seriesId = Convert.ToInt32(context.Request["seriesId"]);
            int totalCount = 0;
            var dataList = bll.GetModelList(out totalCount, 10000, 1, 1, seriesId);
            
            resp.isSuccess = true;
            
            resp.returnObj = new
            {
                list = dataList.Select(p => new {
                    carModelId = p.CarModelId,
                    carModelName = p.CarModelName,
                    year = p.Year,
                    guidePrice = p.GuidePrice,
                    colors = string.IsNullOrWhiteSpace(p.Colors)?  null:JsonConvert.DeserializeObject<List<BLLJIMP.Model.CarModelColorInfo>>(p.Colors).Select(c => new { name = c.Name, value = c.Value }),
                    img = p.Img,
                    showName = p.ShowName,
                    carBrandName = bll.GetBrand(p.CarBrandId).CarBrandName,
                    carSeriesName = bll.GetSeriesInfo(p.CarSeriesId).CarSeriesName
                })
            };

            bll.ContextResponse(context, resp);
        }
        
    }
}