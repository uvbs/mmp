using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Meifan.Match.SignUp
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {


        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        public void ProcessRequest(HttpContext context)
        {
            string data = context.Request["data"];
            if (string.IsNullOrEmpty(data))
            {
                apiResp.msg = " data 参数必传";
                bll.ContextResponse(context, apiResp);
                return;
            }
            //模型
            ActivityDataModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<ActivityDataModel>(data);
            }
            catch (Exception ex)
            {

                apiResp.msg = "格式错误,请检查。错误信息:" + ex.Message;
                bll.ContextResponse(context, apiResp);
                return;

            }
            if (string.IsNullOrEmpty(requestModel.activity_id))
            {
                apiResp.msg = " activity_id 必传";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(requestModel.name))
            {
                apiResp.msg = " 请填写联系人姓名";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(requestModel.phone))
            {
                apiResp.msg = " 请填写联系人手机号";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (bll.GetCount<ActivityDataInfo>(string.Format(" ActivityId={0} And Name='{1}' And Phone='{2}'   And GroupType='{3}' And IsMember='{4}'", requestModel.activity_id, requestModel.name, requestModel.phone, requestModel.group_type, requestModel.member_type)) > 0)
            {

                apiResp.msg = " 重复添加";
                bll.ContextResponse(context, apiResp);
                return;

            }
            JuActivityInfo activity = bll.GetActivity(requestModel.activity_id);
            ActivityDataInfo model = new ActivityDataInfo();
            var newActivityUID = 1001;
            var lastActivityDataInfo = bll.Get<ActivityDataInfo>(string.Format("ActivityID='{0}' order by UID DESC", requestModel.activity_id));
            if (lastActivityDataInfo != null)
            {
                newActivityUID = lastActivityDataInfo.UID + 1;
            }
            model.UID = newActivityUID;
            model.ActivityID = requestModel.activity_id;
            model.ActivityName = bll.GetActivity(model.ActivityID).ActivityName;
            model.InsertDate = DateTime.Now;
            model.Phone = requestModel.phone;
            model.OrderId = bll.GetGUID(BLLJIMP.TransacType.CommAdd);
            if (!string.IsNullOrEmpty(requestModel.amount))
            {
                model.Amount = decimal.Parse(requestModel.amount);
            }


            model.BirthDay = requestModel.birthDay;
            model.DateRange = requestModel.date_range;
            model.Email = requestModel.email;
            model.GroupType = requestModel.group_type;
            model.IsMember = requestModel.member_type;
            model.Name = requestModel.name;
            model.Phone = requestModel.phone;
            model.Sex = requestModel.sex;
            model.UserId = requestModel.user_id;
            model.UserRemark = requestModel.remark;
            model.ActivityType = "match";
            model.PaymentStatus = requestModel.is_pay;
            model.WebsiteOwner = bll.WebsiteOwner;
            if (bll.Add(model))
            {
                apiResp.status = true;
                apiResp.msg = "ok";
                apiResp.result = new
                {
                    order_id = model.OrderId
                };
            }
            else
            {
                apiResp.msg = "提交失败";
            }
            bll.ContextResponse(context, apiResp);



        }

        /// <summary>
        /// 模型
        /// </summary>
        public class ActivityDataModel
        {
            /// <summary>
            /// 活动id
            /// </summary>
            public string activity_id { get; set; }
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
            public string birthDay { get; set; }
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
            /// <summary>
            /// 金额
            /// </summary>
            public string amount { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string remark { get; set; }
            /// <summary>
            /// 用户账号
            /// </summary>
            public string user_id { get; set; }
            /// <summary>
            /// 付款状态
            /// </summary>
            public int is_pay { get; set; }

        }


    }
}