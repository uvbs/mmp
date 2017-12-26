using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Activity
{
    /// <summary>
    /// Get 的摘要说明
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
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
                resp.errmsg = "activity_id 为必填项,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            JuActivityInfo juInfo = bll.GetJuActivity(int.Parse(activityId), true);
            if (juInfo == null)
            {
                resp.errmsg = "没有找到该条活动信息,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            juInfo.PV++;
            bll.Update(juInfo);

            RequestModel requestModel = new RequestModel();
            requestModel.signfield = new List<signfield>();
            requestModel.activity_id = juInfo.JuActivityID;
            requestModel.activity_img_url = bll.GetImgUrl(juInfo.ThumbnailsPath);
            requestModel.activity_name = juInfo.ActivityName;
            requestModel.activity_address = juInfo.ActivityAddress;
            requestModel.category_name = juInfo.CategoryName;
            requestModel.activity_pv = juInfo.PV;
            requestModel.activity_signcount = juInfo.SignUpTotalCount;
            requestModel.activity_sort = juInfo.Sort;
            requestModel.category_name = juInfo.CategoryName;
            requestModel.activity_max_signup_count = juInfo.MaxSignUpTotalCount;
            requestModel.activity_tags = juInfo.Tags;

            requestModel.activity_ex1 = juInfo.K1;
            requestModel.activity_ex2 = juInfo.K2;
            requestModel.activity_ex3 = juInfo.K3;
            requestModel.activity_ex4 = juInfo.K4;
            requestModel.activity_ex5 = juInfo.K5;
            requestModel.activity_ex6 = juInfo.K6;
            requestModel.activity_ex7 = juInfo.K7;
            requestModel.activity_ex8 = juInfo.K8;
            requestModel.activity_ex9 = juInfo.K9;
            requestModel.activity_ex10 = juInfo.K10; ;
            requestModel.province_code = juInfo.ProvinceCode;
            requestModel.city_code = juInfo.CityCode;
            requestModel.district_code = juInfo.DistrictCode;
            requestModel.rootcate_id = juInfo.RootCateId;
            requestModel.activity_summary = juInfo.Summary;
            if (juInfo.ActivityStartDate != null)
            {
                requestModel.activity_start_time = bll.GetTimeStamp((DateTime)juInfo.ActivityStartDate);
            }
            if (juInfo.ActivityEndDate != null)
            {
                requestModel.activity_stop_time = bll.GetTimeStamp((DateTime)juInfo.ActivityEndDate);
            }
            if (juInfo.IsHide == 1)
            {
                requestModel.activity_status = 1;
            }
            if ((juInfo.MaxSignUpTotalCount > 0) && (juInfo.SignUpTotalCount >= juInfo.MaxSignUpTotalCount))
            {
                requestModel.activity_status = 2;
            }
            requestModel.activity_content = juInfo.ActivityDescription;
            if (juInfo.ActivityDescription.Contains("/FileUpload/"))
            {
                requestModel.activity_content = juInfo.ActivityDescription.Replace("/FileUpload/", string.Format("http://{0}/FileUpload/", context.Request.Url.Host));
            }
            requestModel.activity_score = juInfo.ActivityIntegral;

            requestModel.activity_commentcount = bllReview.GetReviewCount(BLLJIMP.Enums.ReviewTypeKey.ArticleComment, juInfo.JuActivityID.ToString(), null);

            var fieldlist = bllTrueActivity.GetActivityFieldMappingList(juInfo.SignUpActivityID);
            foreach (var item in fieldlist)
            {
                signfield model = new signfield();
                model.key = item.MappingName;
                model.value = item.FieldName;
                //TODO:增加是否可为空字段返回
                model.isnull = item.FieldIsNull;
                requestModel.signfield.Add(model);
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
            /// 活动结束时间
            /// </summary>
            public double activity_stop_time { get; set; }
            /// <summary>
            /// 阅读量
            /// </summary>
            public int activity_pv { get; set; }
            /// <summary>
            /// 最多报名人数
            /// </summary>
            public int activity_max_signup_count { get; set; }

            /// <summary>
            /// 活动标签
            /// </summary>
            public string activity_tags { get; set; }
            
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
            /// 分类名称 也就分类名称
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
            ///排序
            /// </summary>
            public int? activity_sort { get; set; }

            /// <summary>
            /// 活动描述
            /// </summary>
            public string activity_summary { get; set; }

            /// <summary>
            /// 扩展字段1
            /// </summary>
            public string activity_ex1 { get; set; }
            /// <summary>
            /// 扩展字段2
            /// </summary>
            public string activity_ex2 { get; set; }
            /// <summary>
            /// 扩展字段3
            /// </summary>
            public string activity_ex3 { get; set; }
            /// <summary>
            /// 扩展字段4
            /// </summary>
            public string activity_ex4 { get; set; }
            /// <summary>
            /// 扩展字段5
            /// </summary>
            public string activity_ex5 { get; set; }
            /// <summary>
            /// 扩展字段6
            /// </summary>
            public string activity_ex6 { get; set; }
            /// <summary>
            /// 扩展字段7
            /// </summary>
            public string activity_ex7 { get; set; }
            /// <summary>
            /// 扩展字段8
            /// </summary>
            public string activity_ex8 { get; set; }
            /// <summary>
            /// 扩展字段9
            /// </summary>
            public string activity_ex9 { get; set; }
            /// <summary>
            /// 扩展字段10
            /// </summary>
            public string activity_ex10 { get; set; }

            /// <summary>
            /// 省
            /// </summary>
            public string province_code { get; set; }

            /// <summary>
            /// 市
            /// </summary>
            public string city_code { get; set; }

            /// <summary>
            /// 区
            /// </summary>
            public string district_code { get; set; }

            /// <summary>
            //
            /// </summary>
            public string rootcate_id { get; set; }

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