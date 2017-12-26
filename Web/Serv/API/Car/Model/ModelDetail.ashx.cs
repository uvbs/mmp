using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Car.Model
{
    /// <summary>
    /// ModelDetail 的摘要说明
    /// </summary>
    public class ModelDetail : CarBaseHandler
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

                var carModelId = Convert.ToInt32(context.Request["carModelId"]);

                var data = bll.GetCarModelInfo(carModelId);

                resp.isSuccess = true;

                resp.returnObj = new
                {
                    carModelId = data.CarModelId,
                    carBrandId = data.CarBrandId,
                    carBrandName = bll.GetBrand(data.CarBrandId).CarBrandName,
                    carSeriesCateId = data.CarSeriesCateId,
                    carSeriesId = data.CarSeriesId,
                    carSeriesName = bll.GetSeriesInfo(data.CarSeriesId).CarSeriesName,
                    carModelName = data.CarModelName,
                    showName = data.ShowName,
                    year = data.Year,
                    img = data.Img,
                    guidePrice = data.GuidePrice,
                    colors = string.IsNullOrWhiteSpace(data.Colors) ? null : JsonConvert.DeserializeObject<List<BLLJIMP.Model.CarModelColorInfo>>(data.Colors).Select(c => new { name = c.Name, value = c.Value })
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