using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Appointment
{
    /// <summary>
    /// RangeGet 的摘要说明
    /// </summary>
    public class RangeGet : BaseHandlerNeedLoginNoAction
    {
        BLLUser bllUser = new BLLUser();
        BLLJuActivity bll = new BLLJuActivity();
        BLLCommRelation bllCommRelation = new BLLCommRelation();
        public void ProcessRequest(HttpContext context)
        {
            string activity_id = context.Request["activity_id"];
            string longitude = context.Request["longitude"];
            string latitude = context.Request["latitude"];
            if (string.IsNullOrWhiteSpace(longitude) || string.IsNullOrWhiteSpace(latitude))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请传入当前经纬度";
                bll.ContextResponse(context, apiResp);
                return;
            }

            int total = 0;
            List<JuActivityInfo> list = bll.GetRangeUserList(1, 1, bll.WebsiteOwner, null, "Appointment", null, null, longitude, latitude
                , null, null, null, null, null, null, null, null, null, null
                , null, null, "99", null, null, out total, int.MaxValue, null,activity_id);
            if (list.Count == 0)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "约会没有找到";
                bll.ContextResponse(context, apiResp);
                return;
            }

            UserInfo userTemp = bllUser.GetUserInfo(list[0].UserID);
            if (userTemp == null)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "发布人信息未找到";
                bll.ContextResponse(context, apiResp);
                return;
            }
            ActivityDataInfo actData = bll.GetActivityDataByUserId(list[0].SignUpActivityID, null, CurrentUserInfo.UserID
                , bll.WebsiteOwner);
            List<ActivityDataInfo> listSignupData = new List<ActivityDataInfo>();
            if (CurrentUserInfo.UserID == list[0].UserID)
            {
                listSignupData = bll.GetActivityDataListByUId(list[0].SignUpActivityID, bll.WebsiteOwner, "1");
            }
            apiResp.status = true;
            apiResp.code = 0;
            apiResp.result = new
            {
                activity_id = list[0].JuActivityID,
                activity_name = list[0].ActivityName,
                activity_address = list[0].ActivityAddress,
                activity_summary = list[0].Summary,
                create_time = DateTimeHelper.DateTimeToUnixTimestamp(list[0].CreateDate),
                create_time_str = DateTimeHelper.DateTimeToString(list[0].CreateDate),
                activity_start_time = list[0].ActivityStartDate.HasValue ? DateTimeHelper.DateTimeToUnixTimestamp(list[0].ActivityStartDate.Value) : 0,
                activity_start_time_str = list[0].ActivityStartDate.HasValue ? DateTimeHelper.DateTimeToString(list[0].ActivityStartDate.Value) : "",
                activity_pv = list[0].PV,
                activity_commentcount = list[0].CommentCount,
                activity_signcount = list[0].SignUpCount,
                category_id = list[0].CategoryId,
                category_name = list[0].CategoryName,
                activity_ex1 = list[0].K1,
                activity_ex2 = list[0].K2,
                activity_ex3 = list[0].K3,
                activity_ex4 = list[0].K4,
                activity_ex5 = list[0].K5,
                activity_ex6 = list[0].K6,
                activity_ex7 = list[0].K7,
                activity_ex8 = list[0].K8,
                activity_ex9 = list[0].K9,
                activity_ex10 = list[0].K10,
                credit_acount = list[0].CreditAcount,
                guarantee_credit_acount = list[0].GuaranteeCreditAcount,
                publish_user = new
                {
                    id = userTemp.AutoID,
                    user_name = userTemp.UserID,
                    nick_name = userTemp.WXNickname,
                    gender = userTemp.Gender,
                    birthday = DateTimeHelper.DateTimeToUnixTimestamp(userTemp.Birthday),
                    birthday_str = DateTimeHelper.DateTimeToString(userTemp.Birthday),
                    identification = userTemp.Ex5,
                    avatar = userTemp.Avatar
                },
                status = list[0].TStatus,
                isstop = (list[0].TStatus==2|| list[0].TStatus==-1)?true:false,
                distance = list[0].Distance,
                isauthor = list[0].UserID == CurrentUserInfo.UserID ? true : false,
                issignup = actData == null?false:true,
                signup_user_status = actData == null? -99: actData.Status,
                need_signin = listSignupData.Count>0? true: false,
                isfollow = bllCommRelation.ExistRelation(CommRelationType.FollowUser,list[0].UserID,CurrentUserInfo.UserID)
            };

            //阅读数+1
            list[0].PV++;
            bll.Update(list[0]);

            apiResp.msg = "查询完成";
            bll.ContextResponse(context, apiResp);
        }
    }
}