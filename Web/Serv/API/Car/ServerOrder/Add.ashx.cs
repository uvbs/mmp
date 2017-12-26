using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Serv.API.Car.ServerOrder
{
    /// <summary>
    /// 添加养车服务订单
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

                var serverOrder = new BLLJIMP.Model.CarServerOrderInfo();

                serverOrder.CarOwnerName = context.Request["carOwnerName"];
                serverOrder.CarOwnerPhone = context.Request["carOwnerPhone"];
                serverOrder.UserId = bll.GetCurrUserID();
                serverOrder.ServerId = Convert.ToInt32(context.Request["serverId"]);

                serverOrder.SallerId = context.Request["sallerId"];
                serverOrder.CreateTime = DateTime.Now;
                serverOrder.Score = 0;
                serverOrder.TotalPrice = Convert.ToDouble(context.Request["totalPrice"]);
                serverOrder.WebsiteOwner = bll.WebsiteOwner;

                var server = bll.GetServer(serverOrder.ServerId);

                serverOrder.ShopType = server.ShopType;
                serverOrder.ServerType = server.ServerType;

                serverOrder.CarBrandId = carModel.CarBrandId;
                serverOrder.CarModelId = carModel.CarModelId;
                serverOrder.CarSeriesCateId = carModel.CarSeriesCateId;
                serverOrder.CarSeriesId = carModel.CarSeriesId;

                serverOrder.IsDesignatedDriving = Convert.ToInt32(context.Request["isDesignatedDriving"]);

                serverOrder.BookArrvieDate = context.Request["bookArrvieDate"];
                serverOrder.BookArrvieEndTime = context.Request["bookArrvieEndTime"];
                serverOrder.BookArrvieStartTime = context.Request["bookArrvieStartTime"];

                if (!ZentCloud.Common.ValidatorHelper.PhoneNumLogicJudge(serverOrder.CarOwnerPhone))
                {
                    resp.isSuccess = false;
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.PhoneFormatError;
                    resp.errmsg = "手机格式错误";
                    bll.ContextResponse(context, resp);
                    return;
                }

                serverOrder.OrderId = Convert.ToInt32(bll.GetGUID(BLLJIMP.TransacType.CommAdd));

                //判断账户姓名手机是否为空，为空则更新
                var currUser = bll.GetCurrentUserInfo();

                if (string.IsNullOrWhiteSpace(currUser.TrueName))
                {
                    bll.Update(
                            new BLLJIMP.Model.UserInfo(),
                            string.Format(" TrueName = '{0}' ", serverOrder.CarOwnerName),
                            string.Format(" UserId = '{0}' ", currUser.UserID)
                        );
                }
                if (string.IsNullOrWhiteSpace(currUser.Phone))
                {
                    bll.Update(
                            new BLLJIMP.Model.UserInfo(),
                            string.Format(" Phone = '{0}' ", serverOrder.CarOwnerPhone),
                            string.Format(" UserId = '{0}' ", currUser.UserID)
                        );
                }

                if (bll.Add(serverOrder))
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