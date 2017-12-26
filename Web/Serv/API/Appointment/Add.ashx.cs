using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.Appointment
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginNoAction
    {
        BLLUser bllUser = new BLLUser();
        BLLJuActivity bllActivity = new BLLJuActivity();
        BLLSystemNotice bllSystemNotice = new BLLSystemNotice();
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
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "json格式错误,请检查";
                bllActivity.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(requestModel.activity_name))
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请输入名称";
                bllActivity.ContextResponse(context, apiResp);
                return;
            }
            if (requestModel.activity_start_time <= 0)
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.msg = "请输入开始时间";
                bllActivity.ContextResponse(context, apiResp);
                return;
            }
            if (requestModel.credit_acount > 0 && CurrentUserInfo.CreditAcount < requestModel.credit_acount)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "信用金不足";
                bllActivity.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrWhiteSpace(requestModel.user_longitude))
            {
                requestModel.user_longitude = CurrentUserInfo.LastLoginLongitude;
            }
            if (string.IsNullOrWhiteSpace(requestModel.user_latitude))
            {
                requestModel.user_latitude = CurrentUserInfo.LastLoginLatitude;
            }
            //string isReplaceN = context.Request["isReplaceN"];
            JuActivityInfo model = new JuActivityInfo();
            model.UserID = bllUser.GetCurrentUserInfo().UserID;
            model.JuActivityID = int.Parse(bllUser.GetGUID(BLLJIMP.TransacType.ActivityAdd));
            model.ActivityName = requestModel.activity_name;
            model.ActivityStartDate = DateTimeHelper.UnixTimestampToDateTime(requestModel.activity_start_time);
            
            if (requestModel.activity_stop_time == 0)
            {
                BLLKeyValueData bllKeyValue = new BLLKeyValueData();
                string hourStr = bllKeyValue.GetDataDefVaule("StartToEndHourNum", "Appointment", "3");
                model.ActivityEndDate = model.ActivityStartDate.Value.AddHours(Convert.ToDouble(hourStr));
            }
            else{
                model.ActivityEndDate = DateTimeHelper.UnixTimestampToDateTime(requestModel.activity_stop_time);
            }
            model.ActivityAddress = requestModel.activity_address;
            model.ActivityDescription = requestModel.activity_content;
            model.ThumbnailsPath = requestModel.activity_img_url;
            model.IsHide = requestModel.activity_status;
            model.Sort = requestModel.activity_sort;
            //model.IsFee = Convert.ToInt32(context.Request["IsFee"]);
            // model.ArticleTemplate = Convert.ToInt32(context.Request["ArticleTemplate"]);
            //model.TopImgPath = context.Request["TopImgPath"];

            //model.ActivityLecturer = context.Request["ActivityLecturer"];
            //model.ArticleTypeEx1 = context.Request["ArticleTypeEx1"];
            model.ActivityIntegral = requestModel.activity_score;
            //model.IsByWebsiteContent = Convert.ToInt32(context.Request["IsByWebsiteContent"]);
            model.LastUpdateDate = DateTime.Now;
            model.Summary = requestModel.acitvity_summary;
            if(model.ActivityDescription != null) model.ActivityDescription = model.ActivityDescription.Replace("\n", "<br />");
            model.CreateDate = DateTime.Now;
            //model.IsSpread = Convert.ToInt32(context.Request["IsSpread"]);
            model.ArticleType = "Appointment";
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
            model.Province = requestModel.province;
            model.CityCode = requestModel.city_code;
            model.City = requestModel.city;
            model.DistrictCode = requestModel.district_code;
            model.District = requestModel.district;
            model.RootCateId = requestModel.rootcate_id;
            model.CreditAcount = requestModel.credit_acount;
            model.GuaranteeCreditAcount = requestModel.guarantee_credit_acount;
            model.UserLongitude = requestModel.user_longitude;
            model.UserLatitude = requestModel.user_latitude;
            model.AccessLevel = requestModel.activity_access_level;
            if(string.IsNullOrWhiteSpace(requestModel.limit_signup_pass_count)){
                model.LimitSignUpPassCount = 1;
            }
            else
            { 
                model.LimitSignUpPassCount = Convert.ToInt32(requestModel.limit_signup_pass_count);
            }
            List<string> tagList = null;

            if (!string.IsNullOrWhiteSpace(model.Tags))
            {
                tagList = model.Tags.Split(',').ToList();
            }

            bllActivity.SetJuActivityContentTags(model.JuActivityID, tagList);

            model.PV = requestModel.activity_pv;
            model.IsSignUpJubit = 1;
            model.WebsiteOwner = bllActivity.WebsiteOwner;

            ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();

            try
            {
                if (model.IsSignUpJubit == 1)
                {
                    //自动创建报名活动
                    ActivityInfo signUpActivityModel = new ActivityInfo();
                    signUpActivityModel.ActivityID = bllActivity.GetGUID(ZentCloud.BLLJIMP.TransacType.ActivityAdd);
                    signUpActivityModel.UserID = bllUser.GetCurrUserID();
                    signUpActivityModel.ActivityName = model.ActivityName;
                    signUpActivityModel.ActivityDate = model.ActivityStartDate;
                    signUpActivityModel.ActivityAddress = model.ActivityAddress;
                    signUpActivityModel.ActivityWebsite = model.ActivityWebsite;
                    signUpActivityModel.ActivityStatus = requestModel.activity_status;
                    signUpActivityModel.LimitCount = 100;
                    signUpActivityModel.ActivityDescription = string.Format("该任务为活动{0}自动创建", model.JuActivityID);
                    signUpActivityModel.WebsiteOwner = model.WebsiteOwner;

                    //设置自动生成的ID
                    model.SignUpActivityID = signUpActivityModel.ActivityID;

                    if (!bllActivity.Add(signUpActivityModel, tran))
                    {
                        tran.Rollback();
                        apiResp.code = 1;
                        apiResp.msg = "添加活动出错!";
                        bllActivity.ContextResponse(context, apiResp);
                        return;
                    }

                    //添加默认字段
                    //添加自定义字段
                    List<ActivityFieldMappingInfo> fieldData = new List<ActivityFieldMappingInfo>();

                    string FieldNameListStr = context.Request["FieldNameList"];

                    if (!string.IsNullOrEmpty(FieldNameListStr))
                    {
                        List<string> FieldNameList = FieldNameListStr.Split(',').ToList();
                        if (FieldNameList.Count <= 60)
                        {
                            for (int i = 0; i < FieldNameList.Count; i++)
                            {
                                fieldData.Add(new ActivityFieldMappingInfo()
                                {
                                    ActivityID = model.SignUpActivityID,
                                    ExFieldIndex = (i + 1),
                                    FieldIsDefauld = 0,
                                    FieldType = 0,
                                    MappingName = FieldNameList[i]
                                });
                            }
                        }
                        else
                        {
                            tran.Rollback();
                            apiResp.code = 1;
                            apiResp.msg = "最多新增60个报名字段!";
                            bllActivity.ContextResponse(context, apiResp);
                            return;
                        }
                    }
                    else
                    {
                        fieldData = new List<ActivityFieldMappingInfo>()
                        {
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
                    }
                    if (!bllActivity.AddList(fieldData))
                    {
                        tran.Rollback();
                        apiResp.code = 1;
                        apiResp.msg = "添加报名字段失败!";
                        bllActivity.ContextResponse(context, apiResp);
                        return;
                    };
                }
                #region 推广
                //自动创建推广活动
                MonitorPlan monitorPlanModel = new MonitorPlan();
                monitorPlanModel.MonitorPlanID = int.Parse(bllActivity.GetGUID(ZentCloud.BLLJIMP.TransacType.MonitorPlanID));
                monitorPlanModel.PlanName = model.ActivityName;
                monitorPlanModel.PlanStatus = "1";
                monitorPlanModel.UserID = bllActivity.GetCurrUserID();
                monitorPlanModel.InsertDate = DateTime.Now;
                monitorPlanModel.Remark = "自动创建的监测任务";

                model.MonitorPlanID = monitorPlanModel.MonitorPlanID;
                #endregion

                if (bllActivity.Add(monitorPlanModel, tran) && bllActivity.Add(model, tran))
                {
                    tran.Commit();
                    
                    if(CurrentUserInfo != null && context.Request["notice_publisher"] =="1"){
                        bllSystemNotice.SendSystemMessage("你发起了一个新的约会消耗" + Convert.ToDouble(model.CreditAcount) + "信用金", model.ActivityName, BLLJIMP.BLLSystemNotice.NoticeType.FinancialNotice, BLLJIMP.BLLSystemNotice.SendType.Personal, CurrentUserInfo.UserID, model.JuActivityID.ToString());
                    }

                    try
                    {
                        bllUser.AddUserCreditAcountDetails(CurrentUserInfo.UserID, "PublishCost", bllUser.WebsiteOwner, 0 - model.CreditAcount
                        , string.Format("发布【{0}】消耗{1}信用金", model.ActivityName, Convert.ToDouble(model.CreditAcount)));
                    }
                    catch (Exception ex11)
                    {
                        apiResp.code = 1;
                        apiResp.msg = "扣除信用金出错!";
                        bllActivity.ContextResponse(context, apiResp);
                        return;
                    }

                    apiResp.code = (int)APIErrCode.IsSuccess;
                    apiResp.status = true;
                    apiResp.msg = "发布完成";
                    //ExObj = model;
                    //ExStr = model.JuActivityIDHex;//将16进制ID传回去
                }
                else
                {
                    tran.Rollback();
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "发布出错!";
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "发布出错";
            }
            bllActivity.ContextResponse(context, apiResp);
        }
        public class RequestModel
        {
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
            public string acitvity_summary { get; set; }

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
            /// 省
            /// </summary>
            public string province_code { get; set; }
            public string province { get; set; }

            /// <summary>
            /// 市
            /// </summary>
            public string city_code { get; set; }
            public string city { get; set; }

            /// <summary>
            /// 区
            /// </summary>
            public string district_code { get; set; }
            public string district { get; set; }

            /// <summary>
            /// 信用金
            /// </summary>
            public decimal credit_acount { get; set; }
            /// <summary>
            /// 担保信用金
            /// </summary>
            public decimal guarantee_credit_acount { get; set; }
            /// <summary>
            /// 发布人经度
            /// </summary>
            public string user_longitude { get; set; }
            /// <summary>
            /// 发布人纬度
            /// </summary>
            public string user_latitude { get; set; }

            /// <summary>
            //
            /// </summary>
            public string rootcate_id { get; set; }

            /// <summary>
            //报名可通过数
            /// </summary>
            public string limit_signup_pass_count { get; set; }
        }
    }
}