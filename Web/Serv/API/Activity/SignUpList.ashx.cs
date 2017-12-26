using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Activity
{
    /// <summary>
    /// SignUpList 的摘要说明
    /// </summary>
    public class SignUpList : BaseHandlerNoAction
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
            JuActivityInfo juInfo = bll.GetJuActivity(activityId, true);
            ResultModel result = new ResultModel();
            result.list = new List<RequestModel>();
            result.totalcount = bll.GetCount<ActivityDataInfo>(string.Format("ActivityID='{0}' And WebsiteOwner='{1}' AND IsDelete=0", juInfo.SignUpActivityID, bll.WebsiteOwner));
            var sourceData = bll.GetLit<ActivityDataInfo>(pageSize, pageIndex, string.Format("ActivityID='{0}' And WebsiteOwner='{1}' AND IsDelete=0", juInfo.SignUpActivityID, bll.WebsiteOwner), " InsertDate DESC");
            foreach (var item in sourceData)
            {
                var userInfo = this.bllUser.GetUserInfo(item.UserId);
                if (userInfo == null)
                {
                    userInfo = this.bllUser.GetUserInfoByOpenId(item.WeixinOpenID);
                }
                RequestModel requestModel = new RequestModel();

                requestModel.signup_name = item.Name;
                requestModel.signup_time = DateTimeHelper.DateTimeToUnixTimestamp(item.InsertDate);
                requestModel.signup_time_str = item.InsertDate.ToString();

                requestModel.signup_headimg = bll.GetImgUrl("/img/europejobsites.png");

                if (userInfo != null)
                {
                    requestModel.signup_user_id = userInfo.UserID;
                    requestModel.signup_headimg = this.bllUser.GetUserDispalyAvatar(userInfo);
                }

                if (juInfo.ShowPersonnelListType.Equals(1))
                {
                    requestModel.signup_name = requestModel.signup_name.Substring(0, 1) + "**";
                }
                result.list.Add(requestModel);
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(result));
        }

 /// <summary>
    /// 已经报名人模型
    /// </summary>
    public class RequestModel
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string signup_headimg { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string signup_name { get; set; }
        /// <summary>
        /// 报名时间
        /// </summary>
        public double signup_time { get; set; }

        public string signup_user_id { get; set; }
        public string signup_time_str { get; set; }
    }
    /// <summary>
    /// 已经报名人 api模型
    /// </summary>
        public class ResultModel {
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