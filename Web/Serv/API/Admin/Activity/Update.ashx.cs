using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Activity
{
    /// <summary>
    /// Update 的摘要说明   编辑活动
    /// </summary>
    public class Update : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLLJuActivity bllActivity = new BLLJIMP.BLLJuActivity("");

        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string data = context.Request["data"];
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"]);
            }
            catch (Exception)
            {

                resp.errcode = -1;
                resp.errmsg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (requestModel.activity_id<=0)
            {
                resp.errmsg = "activity_id 为必填项,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.activity_name))
            {
                resp.errmsg = "activity_name 为必填项,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.activity_address))
            {
                resp.errmsg = "activity_address 为必填项,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (requestModel.activity_start_time <= 0)
            {
                resp.errmsg = "activity_start_time 为必填项,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (requestModel.activity_stop_time <= 0)
            {
                resp.errmsg = "activity_stop_time 为必填项,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.activity_img_url))
            {
                resp.errmsg = "activity_img_url 为必填项,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.activity_content))
            {
                resp.errmsg = "activity_content 为必填项,请检查";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            BLLJIMP.Model.JuActivityInfo model = bllActivity.Get<BLLJIMP.Model.JuActivityInfo>(string.Format(" JuActivityID={0} ",requestModel.activity_id));
            if (model == null)
            {
                resp.errmsg = "活动不存在";
                resp.errcode = 1;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            BLLJIMP.Model.UserInfo userInfo =bllUser.GetUserInfo(bllUser.GetCurrUserID());
            model.ActivityName = requestModel.activity_name;
            model.ActivityStartDate = bllUser.GetTime(requestModel.activity_start_time);
            model.ActivityEndDate = bllUser.GetTime(requestModel.activity_stop_time);
           //model.ActivityEndDate = new DateTime(1970, 1, 1);

            model.ActivityAddress = requestModel.activity_address;
            model.ActivityWebsite = bllUser.WebsiteOwner;
            model.ActivityDescription = requestModel.activity_content;
            model.ThumbnailsPath = requestModel.activity_img_url;

            //model.RecommendCate = context.Request["RecommendCate"];
            model.IsHide = requestModel.activity_status ;
            model.Sort = requestModel.activity_sort;
           // model.IsFee = Convert.ToInt32(context.Request["IsFee"]);
            //model.ArticleTemplate = Convert.ToInt32(context.Request["ArticleTemplate"]);
            //model.ActivityLecturer = context.Request["ActivityLecturer"];
           // model.IsByWebsiteContent = Convert.ToInt32(context.Request["IsByWebsiteContent"]);

           // model.TopImgPath = context.Request["TopImgPath"];
            model.ActivityIntegral = requestModel.activity_score;
            //model.SignUpActivityID = context.Request["SignUpActivityID"];活动ID默认创建，不给编辑了
           // model.IsSpread = Convert.ToInt32(context.Request["IsSpread"]);

            bool isAddSignUpplan = false;
           // model.ArticleType = context.Request["ArticleType"];

            //model.ArticleTypeEx1 = context.Request["ArticleTypeEx1"];
            model.LastUpdateDate = DateTime.Now;
            model.Summary = requestModel.activity_summary
;
            //string IsHideRecommend = context.Request["IsHideRecommend"];
            //if (!string.IsNullOrEmpty(IsHideRecommend))
            //{
            //    model.IsHideRecommend = IsHideRecommend;
            //}
            //model.ActivityNoticeKeFuId = context.Request["ActivityNoticeKeFuId"];
            model.CategoryId = requestModel.category_id.ToString();
           //model.IsShowPersonnelList = int.Parse(string.IsNullOrEmpty(context.Request["IsShowPersonnelList"]) ? "0" : context.Request["IsShowPersonnelList"]);
            //model.ShowPersonnelListType = int.Parse(string.IsNullOrEmpty(context.Request["ShowPersonnelListType"]) ? "0" : context.Request["ShowPersonnelListType"]);
            model.MaxSignUpTotalCount = requestModel.activity_max_signup_count;




            model.Tags = requestModel.activity_tags;
            model.K1 = requestModel.activity_ex1;
            model.K2 = requestModel.activity_ex2;
            model.K3 = requestModel.activity_ex3;
            model.K4 = requestModel.activity_ex4;
            model.K5 = requestModel.activity_ex5;
            model.K6 = requestModel.activity_ex6;
            model.K7 = requestModel.activity_ex7;
            model.K8 = requestModel.activity_ex8;
            model.K9 = requestModel.activity_ex9;
            model.K10 = requestModel.activity_ex10;
            model.ProvinceCode = requestModel.province_code;
            model.CityCode = requestModel.province_code;
            model.DistrictCode = requestModel.province_code;
            model.RootCateId = requestModel.rootcate_id;

            model.AccessLevel = requestModel.activity_access_level;

            List<string> tagList = null;

            if (!string.IsNullOrWhiteSpace(model.Tags))
            {
                tagList = model.Tags.Split(',').ToList();
            }

            bllActivity.SetJuActivityContentTags(model.JuActivityID, tagList);

            model.PV = requestModel.activity_pv;

            if (model.ActivityEndDate <= model.ActivityStartDate)
            {
                resp.errcode = -1;
                resp.errmsg = "结束时间要大于开始时间";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            //如果ArticleType类型是article，则IsSignUpJubit都为0;
            if (model.ArticleType == "article")
                model.IsSignUpJubit = 0;
            else
            {
                //如果由其他状态编辑更改为自动报名状态，则重新自动创建任务
                int isSignUpJubit = requestModel.activity_issignupjubit;
                if (model.IsSignUpJubit != 1 && isSignUpJubit == 1)
                {
                    isAddSignUpplan = true;
                }

                model.IsSignUpJubit = isSignUpJubit;
            }
            if (isAddSignUpplan)
            {
                ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

                try
                {
                    ActivityInfo signUpActivityModel =bllActivity.CreateSignUpActivityModelByJuActivity(model, userInfo.UserID);
                    model.SignUpActivityID = signUpActivityModel.ActivityID;
                    signUpActivityModel.WebsiteOwner = bllActivity.WebsiteOwner;

                    //添加默认字段
                    List<ActivityFieldMappingInfo> fieldData = new List<ActivityFieldMappingInfo>() {
                         new ActivityFieldMappingInfo()
                        { 
                            ActivityID = model.SignUpActivityID, 
                            ExFieldIndex = 1, 
                            FieldIsDefauld = 0,
                            FieldType = 0,
                            FormatValiFunc = "email",
                            MappingName = "邮箱"
                        },
                        new ActivityFieldMappingInfo()
                        { 
                            ActivityID = model.SignUpActivityID, 
                            ExFieldIndex = 2, 
                            FieldIsDefauld = 0,
                            FieldType = 0,
                            MappingName = "公司"
                        },
                        new ActivityFieldMappingInfo()
                        { 
                            ActivityID = model.SignUpActivityID, 
                            ExFieldIndex = 3, 
                            FieldIsDefauld = 0,
                            FieldType = 0,
                            MappingName = "职位"
                        }
                    };
                    if (!bllActivity.AddList(fieldData))
                    {
                        tran.Rollback();
                    }
                    else
                    {

                        if (bllActivity.Update(model, tran) && bllActivity.Add(signUpActivityModel, tran))
                        {
                            tran.Commit();
                            resp.errmsg = "ok";
                            resp.errcode = 0;
                            resp.isSuccess = true;
                        }
                        else
                            tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    resp.errmsg = ex.Message;
                }

            }
            else
            {

                if (this.bllActivity.Update(model))
                {
                    resp.errcode = 0;
                    resp.errmsg = "ok";
                    resp.isSuccess = true;
                }
            }



            if (bllActivity.Update(model))
            {
                resp.errmsg = "ok";
                resp.errcode = 0;
                resp.isSuccess = true;
            }
            else
            {
                resp.errmsg = "修改活动信息出错";
                resp.errcode = -1;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }

        public class RequestModel
        {
            /// <summary>
            /// 活动id
            /// </summary>
            public int activity_id { get; set; }
            /// <summary>
            /// 活动名称
            /// </summary>
            public string activity_name { get; set; }

            /// <summary>
            /// 地点
            /// </summary>
            public string activity_address { get; set; }

            /// <summary>
            /// 活动报名所需要的积分
            /// </summary>
            public int activity_score { get; set; }

            /// <summary>
            /// 最多报名人数
            /// </summary>
            public int activity_max_signup_count { get; set; }

            /// <summary>
            /// 活动开始时间
            /// </summary>
            public long activity_start_time { get; set; }

            /// <summary>
            /// 活动开始时间
            /// </summary>
            public long activity_stop_time { get; set; }

            /// <summary>
            /// 缩略图
            /// </summary>
            public string activity_img_url { get; set; }

            /// <summary>
            /// 活动详情
            /// </summary>
            public string activity_content { get; set; }

            /// <summary>
            /// 活动状态 0代表进行中 1代表已结束 2代表已满员
            /// </summary>
            public int activity_status { get; set; }

            /// <summary>
            /// 是否报名到平台：2自定义报名1自动报名0否
            /// </summary>
            public int activity_issignupjubit { get; set; }

            /// <summary>
            /// 分类id
            /// </summary>
            public int category_id { get; set; }

            /// <summary>
            /// 活动标签
            /// </summary>
            public string activity_tags { get; set; }

            /// <summary>
            /// pv 阅读量
            /// </summary>
            public int activity_pv { get; set; }

            /// <summary>
            /// 访问级别
            /// </summary>
            public int activity_access_level { get; set; }

            /// <summary>
            /// 活动描述
            /// </summary>
            public string activity_summary { get; set; }

            /// <summary>
            ///排序
            /// </summary>
            public int activity_sort { get; set; }


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
            /// 市区
            /// </summary>
            public string province_code { get; set; }

            /// <summary>
            /// 城市
            /// </summary>
            public string city_code { get; set; }

            /// <summary>
            /// 地区
            /// </summary>
            public string district_code { get; set; }

            /// <summary>
            //
            /// </summary>
            public string rootcate_id { get; set; }
        }

    }
}