using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Activity
{
    /// <summary>
    /// MyList 的摘要说明
    /// </summary>
    public class MyList : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJuActivity bll = new BLLJuActivity();
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string longitude = context.Request["longitude"];
            string latitude = context.Request["latitude"];
            string sort = context.Request["sort"];//3开始时间升序，4开始时间降序
            string status = context.Request["status"];
            status = string.IsNullOrWhiteSpace(status) ? "0,1" : status;
            string signup_status = context.Request["signup_status"];
            signup_status = string.IsNullOrWhiteSpace(signup_status) ? "0,1" : signup_status;
            string ctype = context.Request["ctype"];//查询类型 0我发布和报名的 1我发布的 2我报名的

            List<int> juActIdlist = new List<int>();
            if (ctype == "0" || ctype == "1")
            {
                int tempTotal = 0;
                List<JuActivityInfo> tempList = bll.GetRangeUserList(int.MaxValue, 1, bll.WebsiteOwner, CurrentUserInfo.UserID, "activity", null, null, null, null
                    , null, null, null, null, null, null, null, null, null, null
                    , null, null, "99", null, null, out tempTotal, int.MaxValue, status, null, null, "JuActivityID");
                if (tempList.Count > 0)
                {
                    juActIdlist.AddRange(tempList.Select(p => p.JuActivityID));
                }
            }
            if (ctype == "0" || ctype == "2")
            {
                int signupTotal = 0;
                List<ActivityDataInfo> signupList = bll.GetRangeSignUpList(int.MaxValue, 1, null, bll.WebsiteOwner, null, null
                    , signup_status, "99", out signupTotal, CurrentUserInfo.UserID, "ActivityID,UID");
                if (signupList.Count > 0)
                {
                    string signUpActivityIds = MyStringHelper.ListToStr(signupList.Select(p => p.ActivityID).Distinct().ToList(), "'", ",");
                    int tempTotal = 0;
                    List<JuActivityInfo> tempList = bll.GetRangeUserList(int.MaxValue, 1, bll.WebsiteOwner, null, "activity", null, null, null, null
                        , null, null, null, null, null, null, null, null, null, null
                        , null, null, "99", null, null, out tempTotal, int.MaxValue, status, null, signUpActivityIds, "JuActivityID");
                    if (tempList.Count > 0)
                    {
                        juActIdlist.AddRange(tempList.Select(p => p.JuActivityID));
                    }
                }
            }
            int total = 0;
            string juActIds = MyStringHelper.ListToStr(juActIdlist.Distinct().ToList(), "", ",");
            List<JuActivityInfo> list = bll.GetRangeUserList(rows, page, bll.WebsiteOwner, null, "activity", null, null, longitude, latitude
                , null, null, null, null, null, null, null, null, null, null
                , null, null, status, null, null, out total, int.MaxValue, status, juActIds);


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
                    publish_user = new
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
                    distance = item.Distance,
                    type = item.UserID == CurrentUserInfo.UserID?0:1
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
            bll.ContextResponse(context, apiResp);


        }
    }
}