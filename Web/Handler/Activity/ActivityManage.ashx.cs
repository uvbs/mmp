using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Handler.Activity
{
    /// <summary>
    /// ActivityManage 的摘要说明
    /// </summary>
    public class ActivityManage : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 基本响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse();
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJIMP.BLLActivity bllActivity=new BLLActivity("");
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            this.currentUserInfo = bllUser.GetCurrentUserInfo();
            string action = context.Request["Action"];
            string result = "false";
            try
            {
                switch (action)
                {
                    case "AddActivity":
                        result = AddActivity(context);
                        break;
                    case "EditActivity":
                        result = EditActivity(context);
                        break;
                    case "DeleteActivity":
                        result = DeleteActivity(context);
                        break;
                    case "QueryActivity":
                        result = QueryActivity(context);
                        break;
                    case "BatChangState":
                        result = BatChangState(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg =ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);
            }

            context.Response.Write(result);

        }


        private string BatChangState(HttpContext context)
        {
            var ids = Common.StringHelper.ListToStr<string>(context.Request["ids"].Split(',').ToList(), "'", ",");
            var activityStatus = context.Request["ActivityStatus"];

            if (this.bllActivity.Update(new ActivityInfo(), string.Format(" ActivityStatus = {0} ", activityStatus), string.Format(" ActivityID in ({0})", ids)) > 0)
            {
                return "true";
            }

            return "false";

        }

        
        private string QueryActivity(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string activityName = context.Request["ActivityName"];
            string activityId = context.Request["ActivityId"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format("IsDelete = 0 And WebsiteOwner='{0}'",bllUser.WebsiteOwner));

            if (!string.IsNullOrEmpty(activityName))
            {

                sbWhere.AppendFormat("And ActivityName like '%{0}%'", activityName); ;

            }
            if (!string.IsNullOrEmpty(activityId))
            {

                sbWhere.AppendFormat("And ActivityID  ='{0}'", activityId); ;

            }
            List<ActivityInfo> dataList = bllActivity.GetLit<ActivityInfo>(pageSize, pageIndex, sbWhere.ToString(), "ActivityStatus  DESC,ActivityID DESC");

            int totalCount = bllActivity.GetCount<ActivityInfo>(sbWhere.ToString());

            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = dataList
    });
        }

        private string DeleteActivity(HttpContext context)
        {
            string ids = context.Request["ids"];

            if (!string.IsNullOrWhiteSpace(ids))
            {
                return this.bllActivity.DeleteActivity(ids.Split(',').ToList()).ToString().ToLower();
            }
            return "false";
        }

        private string AddActivity(HttpContext context)
        {
            string activityName = context.Request["ActivityName"];
            string activityDate = context.Request["ActivityDate"];
            string activityAddress = context.Request["ActivityAddress"];
            string activityWebsite = context.Request["ActivityWebsite"];
            string activityDescription = context.Request["ActivityDescription"];
            string confirmSMSContent = context.Request["ConfirmSMSContent"];
            string activityStatus = context.Request["ActivityStatus"];
            string limitCount = context.Request["LimitCount"];
            string adminPhone = context.Request["AdminPhone"];
            string adminSMSContent = context.Request["AdminSMSContent"];
            string activityNoticeKeFuId = context.Request["ActivityNoticeKeFuId"];

            if (!string.IsNullOrWhiteSpace(adminPhone) && (string.IsNullOrWhiteSpace(adminSMSContent)))
            {
                resp.Status = 0;
                resp.Msg = "请输入管理员通知短信内容";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (string.IsNullOrWhiteSpace(adminPhone) && (!string.IsNullOrWhiteSpace(adminSMSContent)))
            {
                resp.Status = 0;
                resp.Msg = "请输入管理员通知手机号，多个手机号码用,分隔";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            //手机号不为空，检查手机号格式
            if (!string.IsNullOrWhiteSpace(adminPhone))
            {
                var phoneList = adminPhone.Split(',');

                foreach (var item in phoneList)
                {
                    if (!PageValidate.IsMobile(item))
                    {
                        resp.Status = 0;
                        resp.Msg = "手机号码无效";
                        return Common.JSONHelper.ObjectToJson(resp);
                       
                    }
                }
            }

            ActivityInfo model = new ActivityInfo();
            model.ActivityName = activityName;
            if (!string.IsNullOrEmpty(activityDate))
            {
                model.ActivityDate = DateTime.Parse(activityDate);
            }

           
            model.ActivityAddress = activityAddress;
            model.ActivityWebsite = activityWebsite;
            model.ActivityDescription = activityDescription;
            model.ConfirmSMSContent = confirmSMSContent;
            model.ActivityStatus = int.Parse(activityStatus);
            model.LimitCount = int.Parse(limitCount);
            model.AdminPhone = adminPhone;
            model.AdminSMSContent = adminSMSContent;
            //设置用户ID和活动ID
            model.UserID = this.currentUserInfo.UserID;
            model.ActivityID = this.bllActivity.GetGUID(ZentCloud.BLLJIMP.TransacType.ActivityAdd);
            model.WebsiteOwner = bllUser.WebsiteOwner;
            model.ActivityNoticeKeFuId = activityNoticeKeFuId;
            if (this.bllActivity.Add(model))
            {

                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";

            }
            return Common.JSONHelper.ObjectToJson(resp);

        }

        private string EditActivity(HttpContext context)
        {

            string activityName = context.Request["ActivityName"];
            string activityDate = context.Request["ActivityDate"];
            string activityAddress = context.Request["ActivityAddress"];
            string activityWebsite = context.Request["ActivityWebsite"];
            string activityDescription = context.Request["ActivityDescription"];
            string confirmSMSContent = context.Request["ConfirmSMSContent"];
            string activityStatus = context.Request["ActivityStatus"];
            string limitCount = context.Request["LimitCount"];
            string adminPhone = context.Request["AdminPhone"];
            string adminSMSContent = context.Request["AdminSMSContent"];
            string activityId = context.Request["ActivityID"];
            string activityNoticeKeFuId = context.Request["ActivityNoticeKeFuId"];
            if (!string.IsNullOrWhiteSpace(adminPhone) && (string.IsNullOrWhiteSpace(adminSMSContent)))
            {
                resp.Status = 0;
                resp.Msg = "请输入管理员通知短信内容";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            if (string.IsNullOrWhiteSpace(adminPhone) && (!string.IsNullOrWhiteSpace(adminSMSContent)))
            {
                resp.Status = 0;
                resp.Msg = "请输入管理员通知手机号，多个手机号码用,分隔";
                return Common.JSONHelper.ObjectToJson(resp);

            }
            //手机号不为空，检查手机号格式
            if (!string.IsNullOrWhiteSpace(adminPhone))
            {
                var phoneList = adminPhone.Split(',');
                foreach (var item in phoneList)
                {
                    if (!PageValidate.IsMobile(item))
                    {
                        resp.Status = 0;
                        resp.Msg = "手机号码无效";
                        return Common.JSONHelper.ObjectToJson(resp);
                       
                    }
                }
            }

            ActivityInfo model = new ActivityInfo();
            model.ActivityName = activityName;
            if (!string.IsNullOrEmpty(activityDate))
            {
                model.ActivityDate = DateTime.Parse(activityDate);
            }
            model.ActivityAddress = activityAddress;
            model.ActivityWebsite = activityWebsite;
            model.ActivityDescription = activityDescription;
            model.ConfirmSMSContent = confirmSMSContent;
            model.ActivityStatus = int.Parse(activityStatus);
            model.LimitCount = int.Parse(limitCount);
            model.AdminPhone = adminPhone;
            model.AdminSMSContent = adminSMSContent;
            //设置用户ID和活动ID
            model.ActivityID = activityId;
            model.ActivityNoticeKeFuId = activityNoticeKeFuId;
            if (this.bllActivity.Update(model))
            {

                resp.Status = 1;
                resp.Msg = "保存成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "保存失败";

            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

       



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}