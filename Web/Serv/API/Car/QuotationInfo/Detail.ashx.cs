using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Car.QuotationInfo
{
    /// <summary>
    /// Detail 的摘要说明
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

                var quotationId = Convert.ToInt32(context.Request["quotationId"]);

                var data = bll.GetCarQuotation(quotationId);

                BLLJIMP.Model.JuActivityInfo activity = new BLLJIMP.Model.JuActivityInfo();

                if (!string.IsNullOrWhiteSpace(data.ActivityId))
                {
                    activity = new BLLJIMP.BLLJuActivity().GetJuActivity(Convert.ToInt32(data.ActivityId));
                }

                resp.isSuccess = true;

                resp.returnObj = new
                {
                    quotationId = data.QuotationId,
                    activityId = activity.JuActivityID,
                    activityStartTime = activity.ActivityStartDate == null? "": activity.ActivityStartDate.Value.ToString(),
                    activity = activity,
                    carImg = data.CarImg,
                    carBrandName = data.CarBrandName,
                    carSeriesName = data.CarSeriesName,
                    carModelName = data.CarModelName,
                    guidePrice = data.GuidePrice,
                    stockDescription = data.StockDescription,
                    nationalSalesCount = data.NationalSalesCount,
                    increase = data.Increase,
                    discountPrice = data.DiscountPrice,
                    isShopInsurance = data.IsShopInsurance == 1,
                    status = data.Status,
                    sallerMemo = data.SallerMemo,
                    licensingFees = data.LicensingFees,
                    otherExpenses = data.OtherExpenses,
                    insuranceCost = data.InsuranceCost,
                    purchaseTaxCost = data.PurchaseTaxCost,
                    totalPrice = data.TotalPrice,

                    carBrandId = data.CarBrandId,
                    carSeriesCateId = data.CarSeriesCateId,
                    carSeriesId = data.CarSeriesId,
                    carModelId = data.CarModelId
                    
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