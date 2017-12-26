using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Mall
{
    /// <summary>
    /// 限时特卖活动
    /// </summary>
    public class PromotionActivity : BaseHandler
    {

        /// <summary>
        /// 商城BLL
        /// </summary>
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        /// <summary>
        /// 短信BLL
        /// </summary>
        BLLJIMP.BLLSMS bllSms = new BLLJIMP.BLLSMS("");
        /// <summary>
        /// KeyValue
        /// </summary>
        BLLJIMP.BLLKeyValueData bllKeyValue = new BLLJIMP.BLLKeyValueData();

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string List(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            int totalCount = bllMall.GetCount<BLLJIMP.Model.PromotionActivity>(string.Format(" WebsiteOwner='{0}'", bllMall.WebsiteOwner));
            var sourceData = bllMall.GetLit<BLLJIMP.Model.PromotionActivity>(pageSize, pageIndex, string.Format(" WebsiteOwner='{0}'", bllMall.WebsiteOwner), " Sort DESC,ActivityId ASC");
            var list = from p in sourceData
                       select new
                       {
                           promotion_activity_id = p.ActivityId,
                           promotion_activity_name = p.ActivityName,
                           promotion_activity_summary = p.ActivitySummary,
                           promotion_activity_image = p.ActivityImage,
                           promotion_activity_start_time = p.StartTime,
                           promotion_activity_stop_time = p.StopTime,
                           is_sms_remind = IsRemind(p.ActivityId.ToString(),bllMall.GetCurrUserID())
                          
                       };

            return ZentCloud.Common.JSONHelper.ObjectToJson(new
            {
                totalcount = totalCount,
                list = list
            });




        }

        /// <summary>
        /// 短信提醒
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SmsRemind(HttpContext context)
        {
            string promotionActivityId = context.Request["promotion_activity_id"];//限时特卖活动ID
            string phone = context.Request["phone"];
            string smsContent = context.Request["sms_content"];
            string sendTime = context.Request["send_time"];
            if (!bllMall.IsLogin)
            {
                resp.errcode = 1;
                resp.errmsg = "请先登录";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            var currentUserInfo=bllMall.GetCurrentUserInfo();
            if (string.IsNullOrEmpty(promotionActivityId))
            {
                resp.errcode = 1;
                resp.errmsg = "promotion_activity_id 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(phone))
            {
                resp.errcode = 1;
                resp.errmsg = "phone 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if ((!phone.StartsWith("1")) || (!phone.Length.Equals(11)))
            {
                resp.errcode = 2;
                resp.errmsg = "手机号格式不正确";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(smsContent))
            {
                resp.errcode = 1;
                resp.errmsg = "sms_content 不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(sendTime))
            {
                resp.errcode = 1;
                resp.errmsg = "send_time 参数不能为空";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            //检查是否已经有记录提醒
            if (IsRemind(promotionActivityId,currentUserInfo.UserID)>0)
            {
                resp.errcode = 1;
                resp.errmsg = "已经添加提醒";
                return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

            }
            bool isSuccess = false;
            string msg = "";
            string smsSignature = bllSms.GetWebsiteInfoModelFromDataBase().SmsSignature;//短信签名
            if (string.IsNullOrEmpty(smsSignature))
            {
                smsSignature = "至云";
            }
            bllSms.SendSmsMisson(phone, smsContent, sendTime, smsSignature, out isSuccess, out msg);
            if (isSuccess)
            {
                CommRelationInfo model = new CommRelationInfo();
                model.MainId = promotionActivityId;
                model.RelationId = currentUserInfo.UserID;
                model.RelationTime = DateTime.Now;
                model.RelationType = "PromotionSmsRemind";
                if (bllMall.Add(model))
                {
                    resp.errcode = 0;
                    resp.errmsg = "ok";
                }
                else
                {
                    resp.errcode = 4;
                    resp.errmsg = string.Format("提交提醒失败", msg);
                }


            }
            else
            {
                resp.errcode = 4;
                resp.errmsg = string.Format("提交失败,错误码{0}", msg);
            }
            return ZentCloud.Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 检查是否已经提醒过
        /// </summary>
        /// <param name="promotionActivityId"></param>
        /// <param name="phone"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private int IsRemind(string promotionActivityId,string userId)
        {

            if (string.IsNullOrEmpty(userId))
            {
                return 0;

            }

            string strWhere = string.Format("MainId='{0}' And RelationId='{1}' And RelationType='PromotionSmsRemind'", promotionActivityId, userId);
            int recordCount = bllMall.GetCount<CommRelationInfo>(strWhere);
            if (recordCount > 0)
            {
                return 1;
            }
            return 0;
        
        }

       


    }
}