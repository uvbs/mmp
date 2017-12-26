using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Meifan.Train.My
{
    /// <summary>
    /// 我的培训详细
    /// </summary>
    public class Get : BaseHandlerNeedLoginNoAction
    {

        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {

            string id = context.Request["id"];
            var data = bll.GetActivityDataByOrderId(id);

            ActivityDataModel model = new ActivityDataModel();
            JuActivityInfo activity = bll.GetActivity(data.ActivityID);
            model.id = int.Parse(data.OrderId);
            model.activity_name = activity.ActivityName;
            model.activity_img = activity.ThumbnailsPath;

            model.amount = data.Amount.ToString();


            model.date_range = data.DateRange;
            model.group_type = data.GroupType;
            model.name = data.Name;
            model.status = bll.GetMyTrainStatus(data);
            model.comment = data.Remarks;
            model.main_points = activity.MainPoints;
            model.remark = data.UserRemark;
            model.phone = data.Phone;
            model.email = data.Email;
            model.sex = data.Sex;
            model.birthday = data.BirthDay;
            model.date_range = data.DateRange;
            model.group_type = data.GroupType;
            model.member_type = data.IsMember;
           



            apiResp.status = true;
            apiResp.msg = "ok";
            apiResp.result = model;

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
            /// 金额
            /// </summary>
            public string amount { get; set; }
            ///// <summary>
            ///// 时间范围
            ///// </summary>
            //public string date_range { get; set; }
            ///// <summary>
            ///// 团体类型
            ///// </summary>
            //public string group_type { get; set; }
            /// <summary>
            /// 状态
            /// 0 未开始
            /// 1 进行中
            /// 2 已结束
            /// </summary>
            public string status { get; set; }
            /// <summary>
            /// 评语
            /// </summary>
            public string comment { get; set; }
            /// <summary>
            /// 课程要点
            /// </summary>
            public string main_points { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string remark { get; set; }
            /// <summary>
            /// 联系人姓名
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 手机
            /// </summary>
            public string phone { get; set; }
            /// <summary>
            /// email
            /// </summary>
            public string email { get; set; }
            /// <summary>
            /// 性别
            /// </summary>
            public string sex { get; set; }
            /// <summary>
            /// 生日
            /// </summary>
            public string birthday { get; set; }
            /// <summary>
            /// 时间范围
            /// </summary>
            public string date_range { get; set; }
            /// <summary>
            /// 团体类型
            /// </summary>
            public string group_type { get; set; }
            /// <summary>
            /// 会员类型
            /// </summary>
            public string member_type { get; set; }


        }



    }
}