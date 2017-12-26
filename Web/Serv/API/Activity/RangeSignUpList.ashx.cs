using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Activity
{
    /// <summary>
    /// RangeSignUpList 的摘要说明
    /// </summary>
    public class RangeSignUpList : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLLJuActivity bll = new BLLJIMP.BLLJuActivity();

        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            int activityId = int.Parse(context.Request["activity_id"]);
            string keyWord = context.Request["keyword"];
            string sort = context.Request["sort"];
            string longitude = context.Request["longitude"];
            string latitude = context.Request["latitude"];
            string status = context.Request["status"];
            status = string.IsNullOrWhiteSpace(status) ? "0" : status;
            if (string.IsNullOrWhiteSpace(longitude) || string.IsNullOrWhiteSpace(latitude))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请传入当前经纬度";
                bll.ContextResponse(context, apiResp);
                return;
            }

            JuActivityInfo juInfo = bll.GetJuActivity(activityId, true);
            ResultModel result = new ResultModel();
            result.list = new List<RequestModel>();
            int total = 0;
            List<ActivityDataInfo> sourceData = bll.GetRangeSignUpList(pageSize, pageIndex, juInfo.SignUpActivityID, bll.WebsiteOwner, longitude
                , latitude, status, sort, out total);
            result.totalcount = total;

            foreach (var item in sourceData)
            {
                RequestModel requestModel = new RequestModel();
                requestModel.signup_uid = item.UID;
                requestModel.signup_status = item.Status;
                var userInfo = this.bllUser.GetUserInfo(item.UserId);
                if (userInfo == null)
                {
                    userInfo = this.bllUser.GetUserInfoByOpenId(item.WeixinOpenID);
                }

                requestModel.signup_name = item.Name;
                requestModel.signup_time = DateTimeHelper.DateTimeToUnixTimestamp(item.InsertDate);
                requestModel.signup_time_str = item.InsertDate.ToString();
                requestModel.signup_distance = item.Distance;

                requestModel.signup_headimg = bll.GetImgUrl("/img/europejobsites.png");

                if (userInfo != null)
                {
                    requestModel.signup_user_id = userInfo.UserID;
                    requestModel.signup_headimg = bll.GetImgUrl(this.bllUser.GetUserDispalyAvatar(userInfo));
                    requestModel.signup_birthday = DateTimeHelper.DateTimeToUnixTimestamp(userInfo.Birthday);
                    requestModel.signup_birthday_str = userInfo.Birthday.ToString();
                    requestModel.signup_gender = userInfo.Gender;
                    requestModel.signup_identification = userInfo.Ex5;
                }
                if (juInfo.ShowPersonnelListType.Equals(1))
                {
                    requestModel.signup_name = requestModel.signup_name.Substring(0, 1) + "**";
                }

                var activity = bll.GetJuActivityByActivityID(item.ActivityID);
                if (activity != null)
                {
                    requestModel.activity_id = activity.JuActivityID;
                    requestModel.activity_name = activity.ActivityName;
                }
                requestModel.signup_ex1 = item.K1;
                requestModel.guarantee_credit_acount = item.GuaranteeCreditAcount;

                result.list.Add(requestModel);
            }
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.result = result;
            apiResp.status = true;
            apiResp.msg = "查询完成";
            bll.ContextResponse(context, apiResp);
        }

        /// <summary>
        /// 已经报名人模型
        /// </summary>
        public class RequestModel
        {
            public int signup_uid { get; set; }
            /// <summary>
            /// 头像
            /// </summary>
            public string signup_headimg { get; set; }
            /// <summary>
            /// 显示名称
            /// </summary>
            public string signup_name { get; set; }

            public string signup_user_id { get; set; }
            /// <summary>
            /// 报名时间
            /// </summary>
            public double signup_time { get; set; }
            public string signup_time_str { get; set; }
            /// <summary>
            /// 你我有信地址
            /// </summary>
            public string signup_ex1 { get; set; }
            /// <summary>
            /// 报名距离
            /// </summary>
            public double signup_distance { get; set; }
            public int activity_id { get; set; }
            /// <summary>
            /// 活动名称
            /// </summary>
            public string activity_name { get; set; }
            /// <summary>
            /// 担保信用金
            /// </summary>
            public decimal guarantee_credit_acount { get; set; }
            /// <summary>
            /// 报名者生日
            /// </summary>
            public double signup_birthday { get; set; }
            public string signup_birthday_str { get; set; }
            /// <summary>
            /// 报名者性别
            /// </summary>
            public string signup_gender { get; set; }
            /// <summary>
            /// 报名者身份
            /// </summary>
            public string signup_identification { get; set; }
            /// <summary>
            /// 状态 -1未通过 -2已退信用金 0待确认 1通过
            /// </summary>
            public int signup_status { get; set; }
        }
        /// <summary>
        /// 已经报名人 api模型
        /// </summary>
        public class ResultModel
        {
            /// <summary>
            /// 总数
            /// </summary>
            public int totalcount { get; set; }
            /// <summary>
            /// 报名人集合
            /// </summary>
            public List<RequestModel> list { get; set; }
        }
    }
}