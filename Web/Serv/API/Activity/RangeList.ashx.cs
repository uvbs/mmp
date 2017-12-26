using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Activity
{
    /// <summary>
    /// 附近的活动
    /// </summary>
    public class RangeList : BaseHandlerNoAction
    {
        BLLUser bllUser = new BLLUser();
        BLLJuActivity bllActivity = new BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string category_id = context.Request["category_id"];
            string city = context.Request["city"];
            string user_name = context.Request["user_name"];
            string activity_ex1 = context.Request["activity_ex1"];
            string activity_ex2 = context.Request["activity_ex2"];
            string activity_ex3 = context.Request["activity_ex3"];
            string activity_ex4 = context.Request["activity_ex4"];
            string activity_ex5 = context.Request["activity_ex5"];
            string activity_ex6 = context.Request["activity_ex6"];
            string activity_ex7 = context.Request["activity_ex7"];
            string activity_ex8 = context.Request["activity_ex8"];
            string activity_ex9 = context.Request["activity_ex9"];
            string activity_ex10 = context.Request["activity_ex10"];
            string start_time = context.Request["start_time"];
            string stop_time = context.Request["stop_time"];
            string longitude = context.Request["longitude"];
            string latitude = context.Request["latitude"];
            string keyword = context.Request["keyword"];
            string tag = context.Request["tag"];
            string sort = context.Request["sort"];
            string status = context.Request["status"];
            status = string.IsNullOrWhiteSpace(status) ? "0" : status;
            int range = string.IsNullOrWhiteSpace(context.Request["range"]) ? 3 : Convert.ToInt32(context.Request["range"]);

            if (string.IsNullOrWhiteSpace(longitude) || string.IsNullOrWhiteSpace(latitude))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请传入当前经纬度";
                bllActivity.ContextResponse(context, apiResp);
                return;
            }

            int total = 0;
            List<JuActivityInfo> list = bllActivity.GetRangeUserList(rows, page, bllActivity.WebsiteOwner, user_name, "activity", category_id, city, longitude, latitude
                , activity_ex1, activity_ex2, activity_ex3, activity_ex4, activity_ex5, activity_ex6, activity_ex7, activity_ex8, activity_ex9, activity_ex10
                , keyword, tag, sort, start_time, stop_time, out total, range, status);
            List<dynamic> result = new List<dynamic>();
            foreach (var item in list)
            {
                 UserInfo userTemp = bllUser.GetUserInfo(item.UserID);
                 if (userTemp == null) continue;
                 result.Add(new
                 {
                     activity_id = item.JuActivityID,
                     activity_name = item.ActivityName,
                     activity_address = item.ActivityAddress,
                     activity_summary = item.Summary,
                     create_time = DateTimeHelper.DateTimeToUnixTimestamp(item.CreateDate),
                     activity_start_time = item.ActivityStartDate.HasValue ? DateTimeHelper.DateTimeToUnixTimestamp(item.ActivityStartDate.Value) : 0,
                     activity_pv = item.PV,
                     activity_commentcount = item.CommentCount,
                     activity_signcount = item.SignUpCount,
                     category_id = item.CategoryId,
                     category_name = item.CategoryName,
                     activity_ex1 = item.K1,
                     activity_ex2 = item.K2,
                     activity_ex3 = item.K3,
                     activity_ex4 = item.K4,
                     activity_ex5 = item.K5,
                     activity_ex6 = item.K6,
                     activity_ex7 = item.K7,
                     activity_ex8 = item.K8,
                     activity_ex9 = item.K9,
                     activity_ex10 = item.K10,
                     credit_acount = item.CreditAcount,
                     publish_user =new
                     {
                         id = userTemp.AutoID,
                         user_name = userTemp.UserID,
                         nick_name = userTemp.WXNickname,
                         gender = userTemp.Gender,
                         birthday = DateTimeHelper.DateTimeToUnixTimestamp(userTemp.Birthday),
                         birthday_str = userTemp.Birthday.ToString(),
                         identification = userTemp.Ex5,
                         avatar = userTemp.Avatar
                     },
                     status = item.TStatus,
                     distance = item.Distance
                 });
            }
            apiResp.status = true;
            apiResp.code = 0;
            apiResp.result = new
            {
                totalcount = total,
                list = result
            };
            apiResp.msg = "查询完成";
            bllActivity.ContextResponse(context, apiResp);
        }

    }
}