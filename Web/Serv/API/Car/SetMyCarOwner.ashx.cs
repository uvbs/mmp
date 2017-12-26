using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Car
{
    /// <summary>
    /// 设置我的车主信息
    /// </summary>
    public class SetMyCarOwner : CarBaseHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            resp.isSuccess = false;

            try
            {
                if (!bll.IsLogin)
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.UserIsNotLogin;
                    resp.errmsg = "未登录";
                    bll.ContextResponse(context, resp);
                    return;
                }

                var carModelId = Convert.ToInt32(context.Request["carModelId"]);
                var carNumber = context.Request["carNumber"];//车牌
                var carNumberTimeStr = context.Request["carNumberTime"];//上牌时间
                var drivingLicenseType = context.Request["drivingLicenseType"];//驾照类型
                var drivingLicenseTimeStr = context.Request["drivingLicenseTime"];//驾照领取时间
                var VIN = context.Request["VIN"];

                //姓名 手机 地址  如果有更改则更新到用户信息
                var ownerName = context.Request["ownerName"];
                var ownerPhone = context.Request["ownerPhone"];
                var address = context.Request["address"];

                var currUser = bll.GetCurrentUserInfo();

                if (!string.IsNullOrWhiteSpace(ownerName))
                {
                    currUser.TrueName = ownerName;
                }
                if (!string.IsNullOrWhiteSpace(ownerPhone))
                {
                    currUser.Phone = ownerPhone;
                }
                if (!string.IsNullOrWhiteSpace(address))
                {
                    currUser.Address = address;
                }

                bll.Update(currUser);

                if (carModelId == 0)
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    resp.errmsg = "请选择车型";
                    bll.ContextResponse(context, resp);
                    return;
                }

                if (string.IsNullOrWhiteSpace(carNumber))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    resp.errmsg = "车牌不能为空";
                    bll.ContextResponse(context, resp);
                    return;
                }
                if (string.IsNullOrWhiteSpace(drivingLicenseType))
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    resp.errmsg = "驾照类型不能为空";
                    bll.ContextResponse(context, resp);
                    return;
                }
                DateTime carNumberTime = new DateTime(), drivingLicenseTime = new DateTime();

                if (DateTime.TryParse(carNumberTimeStr, out carNumberTime))
                {
                    
                }
                else
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    resp.errmsg = "上牌时间错误";
                    bll.ContextResponse(context, resp);
                    return;
                }
                if (DateTime.TryParse(drivingLicenseTimeStr, out drivingLicenseTime))
                {

                }
                else
                {
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                    resp.errmsg = "驾照领取时间错误";
                    bll.ContextResponse(context, resp);
                    return;
                }



                resp.isSuccess = bllUserEx.SetUserCarOwnerInfo(bll.GetCurrUserID(), carModelId, carNumber, carNumberTime, VIN, drivingLicenseType, drivingLicenseTime);

                resp.returnObj = resp.isSuccess;

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