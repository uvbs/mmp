using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Activity
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNoAction
    {
        BLLJIMP.BLLJuActivity bll = new BLLJIMP.BLLJuActivity();
        /// <summary>
        /// 评论回复模块
        /// </summary>
        BLLReview bllReview = new BLLReview();

        BLLJIMP.BLLActivity bllTrueActivity = new BLLJIMP.BLLActivity("");
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string activityId = context.Request["activity_id"];
            if (string.IsNullOrEmpty(activityId))
            {
                resp.errcode = 1;
                resp.errmsg = "activityid 参数为空,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            ActivityDataInfo activityInfo = bllTrueActivity.GetActivityDataInfo(activityId,bll.GetCurrUserID());
            bool isEnroll = false;
            if (activityInfo == null)
            {
                 isEnroll = true;
            }
            JuActivityInfo model = bll.GetJuActivity(int.Parse(activityId), true);
            bll.Update(model, string.Format("PV+=1"), string.Format(" JuActivityID={0}", model.JuActivityID));

            RequestModel requestModel = new RequestModel();
            requestModel.signfield = new List<signfield>();
            requestModel.activity_id = model.JuActivityID;
            requestModel.activity_img_url = bll.GetImgUrl(model.ThumbnailsPath);
            requestModel.activity_name = model.ActivityName;
            requestModel.activity_address = model.ActivityAddress;
            requestModel.category_name = model.CategoryName;
            requestModel.activity_pv = model.PV;
            requestModel.activity_signcount = model.SignUpTotalCount;
            requestModel.activity_summary = model.Summary;
            requestModel.is_enroll = isEnroll;
            if (model.ActivityStartDate != null)
            {
                requestModel.activity_start_time = bll.GetTimeStamp((DateTime)model.ActivityStartDate);
            }
            if (model.IsHide == 1)
            {
                requestModel.activity_status = 1;
            }
            if ((model.MaxSignUpTotalCount > 0) && (model.SignUpTotalCount >= model.MaxSignUpTotalCount))
            {
                requestModel.activity_status = 2;
            }
            requestModel.activity_content = model.ActivityDescription;
            if (model.ActivityDescription.Contains("/FileUpload/"))
            {
                requestModel.activity_content = model.ActivityDescription.Replace("/FileUpload/", string.Format("http://{0}/FileUpload/", context.Request.Url.Host));
            }
            requestModel.activity_score = model.ActivityIntegral;

            requestModel.activity_commentcount = bllReview.GetReviewCount(BLLJIMP.Enums.ReviewTypeKey.ArticleComment, model.JuActivityID.ToString(), null);

            var fieldlist = bllTrueActivity.GetActivityFieldMappingList(model.SignUpActivityID);
            foreach (var item in fieldlist)
            {
                signfield signModel = new signfield();
                signModel.key = item.MappingName;
                signModel.value = item.FieldName;
                signModel.isnull = item.FieldIsNull;
                requestModel.signfield.Add(signModel);
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(requestModel));
        }

       
        public class RequestModel
        {
            /// <summary>
            /// 活动id编号
            /// </summary>
            public int activity_id { get; set; }
            /// <summary>
            /// 活动名称
            /// </summary>
            public string activity_name { get; set; }
            /// <summary>
            /// 活动图片
            /// </summary>
            public string activity_img_url { get; set; }
            /// <summary>
            /// 活动开始时间
            /// </summary>
            public double activity_start_time { get; set; }
            /// <summary>
            /// 阅读量
            /// </summary>
            public int activity_pv { get; set; }
            /// <summary>
            /// 报名人数
            /// </summary>
            public int activity_signcount { get; set; }

            /// <summary>
            /// 活动状态 0代表进行中 1代表已结束 2代表已满员
            /// </summary>
            public int activity_status { get; set; }
            /// <summary>
            /// 地址
            /// </summary>
            public string activity_address { get; set; }
            /// <summary>
            /// 分类名称 也就是标签名称
            /// </summary>
            public string category_name { get; set; }

            /// <summary>
            /// 活动详情
            /// </summary>
            public string activity_content { get; set; }
            /// <summary>
            /// 活动报名所需要的积分
            /// </summary>
            public int activity_score { get; set; }
            /// <summary>
            /// 报名字段集合
            /// </summary>
            public List<signfield> signfield { get; set; }
            /// <summary>
            /// 评论数量
            /// </summary>
            public int activity_commentcount { get; set; }

            /// <summary>
            /// 活动描述
            /// </summary>
            public string activity_summary { get; set; }

            /// <summary>
            /// 是否已经报名
            /// </summary>
            public bool is_enroll { get; set; }

        }



        /// <summary>
        ///报名字段模型
        /// </summary>
        public class signfield
        {
            /// <summary>
            /// 显示名称 如姓名
            /// </summary>
            public string key { get; set; }
            /// <summary>
            /// 如 K1
            /// </summary>
            public string value { get; set; }

            /// <summary>
            /// 是否为空
            /// </summary>
            public int isnull { get; set; }

        }
    }
}