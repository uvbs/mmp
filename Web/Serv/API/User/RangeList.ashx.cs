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
    /// 附近的用户
    /// </summary>
    public class RangeList : BaseHandlerNoAction
    {

        BLLUser bllUser = new BLLUser("");
        UserInfo CurrentUserInfo;
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string user_type = context.Request["user_type"];
            string province = context.Request["province"];
            string city = context.Request["city"];
            string district = context.Request["district"];
            string last_login_city = context.Request["last_login_city"];
            string gender = context.Request["gender"];
            string longitude = context.Request["longitude"];
            string latitude = context.Request["latitude"];
            string ex1 = context.Request["ex1"];
            string ex2 = context.Request["ex2"];
            string ex3 = context.Request["ex3"];
            string ex4 = context.Request["ex4"];
            string ex5 = context.Request["ex5"];
            string keyword = context.Request["keyword"];
            string tag = context.Request["tag"];
            int range = string.IsNullOrWhiteSpace(context.Request["range"]) ? 3 : Convert.ToInt32(context.Request["range"]);
            string sort = context.Request["sort"];

            if (string.IsNullOrWhiteSpace(longitude) || string.IsNullOrWhiteSpace(latitude))
            {
                CurrentUserInfo = bllUser.GetCurrentUserInfo();
                if (CurrentUserInfo != null && !string.IsNullOrWhiteSpace(CurrentUserInfo.LastLoginLongitude)
                    && !string.IsNullOrWhiteSpace(CurrentUserInfo.LastLoginLatitude))
                {
                    longitude = CurrentUserInfo.LastLoginLongitude;
                    latitude = CurrentUserInfo.LastLoginLatitude;
                }
            }
            int total = 0;
            List<UserInfo> list = bllUser.GetRangeUserList(rows, page, bllUser.WebsiteOwner, user_type, province, city, district, last_login_city
                , gender, longitude, latitude, ex1, ex2, ex3, ex4, ex5, keyword, tag, sort, out total, range);

            dynamic result = from p in list
                             select new
                             {
                                 id = p.AutoID,
                                 username = p.UserID,
                                 nick_name = p.WXNickname,
                                 avatar = p.Avatar,
                                 gender = p.Gender,
                                 identification = p.Ex5,
                                 describe = p.Description,
                                 last_login_date = DateTimeHelper.DateTimeToUnixTimestamp(p.LastLoginDate),
                                 last_login_date_str = DateTimeHelper.DateTimeToString(p.LastLoginDate),
                                 birthday = DateTimeHelper.DateTimeToUnixTimestamp(p.Birthday),
                                 birthday_str = DateTimeHelper.DateTimeToString(p.Birthday),
                                 last_login_city = p.LastLoginCity,
                                 distance = p.Distance,
                                 img=p.Images
                             };

            apiResp.status = true;
            apiResp.code = 0;
            apiResp.result = new
            {
                totalcount = total,
                list = result
            };
            apiResp.msg = "查询完成";
            bllUser.ContextResponse(context, apiResp);
        }

    }
}