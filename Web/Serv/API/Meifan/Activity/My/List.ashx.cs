using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Meifan.Activity.My
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginNoAction
    {

        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {

            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            int totalCount;
            var data = bll.ActivityDataList(pageIndex, pageSize, out totalCount, "", CurrentUserInfo.UserID, "activity");

            List<ActivityDataModel> list = new List<ActivityDataModel>();
            foreach (var item in data)
            {
                ActivityDataModel model = new ActivityDataModel();
                JuActivityInfo activity = bll.GetActivity(item.ActivityID);
                model.id = int.Parse(item.OrderId);
                model.activity_name = activity.ActivityName;
                model.activity_img = activity.ThumbnailsPath;
                model.amount = item.Amount.ToString();
                model.date_range = item.DateRange;
                model.group_type = item.GroupType;
                model.name = item.Name;
                model.status = bll.GetActivityStatus(activity);
                list.Add(model);
            }

            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = new
            {
                totalcount = totalCount,
                list = list
            };

            bll.ContextResponse(context, apiResp);

        }

        public class ActivityDataModel
        {
            /// <summary>
            /// id
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 名称
            /// </summary>
            public string activity_name { get; set; }
            /// <summary>
            /// 图片
            /// </summary>
            public string activity_img { get; set; }
            /// <summary>
            /// 名字
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 金额
            /// </summary>
            public string amount { get; set; }
            /// <summary>
            /// 时间范围
            /// </summary>
            public string date_range { get; set; }
            /// <summary>
            /// 团体类型
            /// </summary>
            public string group_type { get; set; }
            /// <summary>
            /// 状态
            /// 0 未开始
            /// 1 进行中
            /// 2 已结束
            /// </summary>
            public string status { get; set; }


        }
    }
}