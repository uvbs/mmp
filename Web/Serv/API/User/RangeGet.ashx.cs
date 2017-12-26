using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.User
{
    /// <summary>
    /// 获取用户信息带距离
    /// </summary>
    public class RangeGet : BaseHandlerNeedLoginNoAction
    {

        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string autoId = context.Request["id"];
            string longitude = context.Request["longitude"];
            string latitude = context.Request["latitude"];
            if (string.IsNullOrEmpty(autoId))
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "id 为必填项,请检查";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrWhiteSpace(longitude) || string.IsNullOrWhiteSpace(latitude))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请传入当前经纬度";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            UserInfo model = bllUser.GetRangeUserInfoByAutoID(int.Parse(autoId), bllUser.WebsiteOwner, longitude, latitude);
            if (model == null)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                apiResp.msg = "用户不存在";
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            apiResp.status = true;
            apiResp.result = new
            {
                nick_name = model.WXNickname,
                avatar = model.Avatar,
                head_img_url = model.WXHeadimgurl,
                last_login_date = DateTimeHelper.DateTimeToUnixTimestamp(model.LastLoginDate),
                last_login_date_str = DateTimeHelper.DateTimeToString(model.LastLoginDate),
                birthday = DateTimeHelper.DateTimeToUnixTimestamp(model.Birthday),
                birthday_str = DateTimeHelper.DateTimeToString(model.Birthday),
                identification = model.Ex5,
                gender = model.Gender,
                describe = model.Description,
                city = model.City,
                province = model.Province,
                salary = model.Salary,
                ex2 = model.Ex2,
                ex3 = model.Ex3,
                ex4 = model.Ex4,
                credit_acount = CurrentUserInfo.CreditAcount
            };
            bllUser.ContextResponse(context, apiResp);
        }
    }
}