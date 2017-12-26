using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Car.QuotationInfo
{
    /// <summary>
    /// 提交购车需求（新建报价单）
    /// </summary>
    public class Add : CarBaseHandler
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

                if (string.IsNullOrWhiteSpace(context.Request["carModelId"]))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                    resp.errmsg = "车型id不能为空";
                    bll.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrWhiteSpace(context.Request["buyTime"]))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                    resp.errmsg = "购车时间不能为空";
                    bll.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrWhiteSpace(context.Request["preference"]))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                    resp.errmsg = "购车偏好不能为空";
                    bll.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrWhiteSpace(context.Request["purchaseWay"]))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                    resp.errmsg = "购车方式不能为空";
                    bll.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrWhiteSpace(context.Request["licensePlate"]))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                    resp.errmsg = "牌照不能为空";
                    bll.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrWhiteSpace(context.Request["carOwnerName"]))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                    resp.errmsg = "车主姓名不能为空";
                    bll.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrWhiteSpace(context.Request["carOwnerPhone"]))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                    resp.errmsg = "车主手机不能为空";
                    bll.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrWhiteSpace(context.Request["city"]))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                    resp.errmsg = "城市不能为空";
                    bll.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrWhiteSpace(context.Request["district"]))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                    resp.errmsg = "地区不能为空";
                    bll.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrWhiteSpace(context.Request["area"]))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                    resp.errmsg = "区域不能为空";
                    bll.ContextResponse(context, resp);
                    return;
                }

                var carModelId = Convert.ToInt32(context.Request["carModelId"]);

                var carModel = bll.GetCarModelInfo(carModelId);

                if (carModel == null)
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    resp.errmsg = "该车型未找到";
                    bll.ContextResponse(context, resp);
                    return;
                }

                var carBrand = bll.GetBrand(carModel.CarBrandId);
                var carSeries = bll.GetSeriesInfo(carModel.CarSeriesId);

                var quotaation = new BLLJIMP.Model.CarQuotationInfo();
                
                quotaation.UserId = bll.GetCurrUserID();
                quotaation.CarBrandId = carModel.CarBrandId;
                quotaation.CarSeriesCateId = carModel.CarSeriesCateId;
                quotaation.CarSeriesId = carModel.CarSeriesId;
                quotaation.CarModelId = carModelId;
                quotaation.CarColor = context.Request["carColor"];
                quotaation.BuyTime = context.Request["buyTime"];
                quotaation.PurchaseWay = context.Request["purchaseWay"];
                quotaation.LicensePlate = context.Request["licensePlate"];

                quotaation.CarOwnerName = context.Request["carOwnerName"];
                quotaation.CarOwnerPhone = context.Request["carOwnerPhone"];

                quotaation.City = context.Request["city"];
                quotaation.District = context.Request["district"];
                quotaation.Area = context.Request["area"];

                quotaation.Preference = context.Request["preference"];

                quotaation.CreateTime = DateTime.Now;
                quotaation.WebSiteOwner = bll.WebsiteOwner;

                quotaation.CarBrandName = carBrand.CarBrandName;
                quotaation.CarSeriesName = carSeries.CarSeriesName;
                quotaation.CarModelName = carModel.CarModelName;
                quotaation.GuidePrice = carModel.GuidePrice;

                quotaation.Status = 0;

                if (!ZentCloud.Common.ValidatorHelper.PhoneNumLogicJudge(quotaation.CarOwnerPhone))
                {
                    resp.isSuccess = false;
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PhoneFormatError;
                    resp.errmsg = "手机格式错误";
                    bll.ContextResponse(context, resp);
                    return;
                }

                quotaation.QuotationId = Convert.ToInt32(bll.GetGUID(BLLJIMP.TransacType.CommAdd));


                //判断账户姓名手机是否为空，为空则更新
                var currUser = bll.GetCurrentUserInfo();

                if ( string.IsNullOrWhiteSpace(currUser.TrueName))
                {
                    bll.Update(
                            new BLLJIMP.Model.UserInfo(),
                            string.Format(" TrueName = '{0}' ", quotaation.CarOwnerName),
                            string.Format(" UserId = '{0}' ",currUser.UserID)
                        );
                }
                if (string.IsNullOrWhiteSpace(currUser.Phone))
                {
                    bll.Update(
                            new BLLJIMP.Model.UserInfo(),
                            string.Format(" Phone = '{0}' ", quotaation.CarOwnerPhone),
                            string.Format(" UserId = '{0}' ", currUser.UserID)
                        );
                }
                
                if (bll.Add(quotaation))
                {
                    resp.isSuccess = true;
                }
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