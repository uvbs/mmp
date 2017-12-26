using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace ZentCloud.JubitIMP.Web.Serv.API.DistributionOffLine.User
{
    /// <summary>
    ///申请活动信息
    /// </summary>
    public class ApplyActivity : BaseHandlerNeedLoginNoAction
    {

        /// <summary>
        /// 线下分销BLL
        /// </summary>
        BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLLActivity bllActivity = new BLLJIMP.BLLActivity("");
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                ToLog("已经进入——ApplyActivity");

                var userInfo = bll.GetCurrentUserInfo();
                ToLog("userInfo:" + JsonConvert.SerializeObject(userInfo));

                string activityId = bll.GetDistributionOffLineApplyActivityID();
                ToLog("activityId:" + activityId);

                ActivityDataInfo model = bllActivity.GetActivityDataInfo(activityId, bll.GetCurrUserID());

                bool isEnroll = model != null;//是否报过名
                
                int apply_status = 0;//0未申请  1待审核 2已通过 4001已拒绝 4002 微转发通过
                string remarks = string.Empty;
                List<SignField> signfieldList = new List<SignField>();
                List<ActivityDataRecord> applyRecord = new List<ActivityDataRecord>();
                var fieldList = bllActivity.GetActivityFieldMappingList(activityId).Where(p => p.IsHideInSubmitPage != "1");
                //检查当前是否已经是分销员
                if (!string.IsNullOrWhiteSpace(userInfo.DistributionOffLinePreUserId))
                {
                    apply_status = 2;
                }
                else
                {
                    if (isEnroll)
                    {
                        //判断是待审核还是审核不通过
                        if (model.Status == 4001)
                        {
                            apply_status = 4001;
                            remarks = model.Remarks;
                        }
                        //else if (model.Status==0&&(!string.IsNullOrEmpty(model.SpreadUserID)))//微转发待审核
                        //{
                        //    apply_status = 4002;
                        //    remarks = string.Format("您已报名过 {0}分享的活动 {1}，系统正在审核中，审核通过可以直接成为平台会员",model.K59,model.K60);
                        //}
                        //else if (model.Status == 4003)//微转发审核通过
                        //{
                        //    apply_status = 4003;
                        //    remarks = string.Format("您已报名过 {0}分享的活动 {1},可以直接成为平台会员", model.K59, model.K60);
                        //}
                        else
                        {
                            apply_status = 1;
                        }

                        Type type = model.GetType();
                        //数据记录
                        foreach (var item in fieldList)
                        {
                            ActivityDataRecord record = new ActivityDataRecord();
                            record.field = item.FieldName;
                            record.value = type.GetProperty(item.FieldName).GetValue(model, null).ToString();
                            applyRecord.Add(record);

                        }

                        //分销推荐id加上
                        applyRecord.Add(new ActivityDataRecord()
                        {
                            field = "DistributionOffLineRecommendCode",
                            value =model.DistributionOffLineRecommendCode,
                           
                        });

                    }
                    else
                    {
                        apply_status = 0;


                    }
                    //如果未报过名，把活动字段信息返回
                   
                    foreach (var item in fieldList)
                    {
                        SignField signModel = new SignField();
                        signModel.key = item.MappingName;
                        signModel.value = item.FieldName;
                        signModel.isnull = item.FieldIsNull;
                        signfieldList.Add(signModel);
                    }


                    //分销推荐id加上
                    signfieldList.Add(new SignField()
                    {
                        key = "推荐码",
                        value = "DistributionOffLineRecommendCode",
                        isnull = 0
                    });



                }

                apiResp.result = new
                {
                    activity_id = activityId,
                    is_enroll = isEnroll,
                    signfield = signfieldList,
                    apply_record=applyRecord,
                    apply_status = apply_status,
                    remarks = remarks
                };
                apiResp.status = true;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));
            }
            catch (Exception ex)
            {
                ToLog("活动状态异常：" + ex.Message);
                throw ex;
            }
        }



        /// <summary>
        ///报名字段模型
        /// </summary>
        public class SignField
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

        /// <summary>
        ///报名字段模型
        /// </summary>
        public class ActivityDataRecord
        {
            /// <summary>
            /// 参数名称如K1
            /// </summary>
            public string field { get; set; }
            /// <summary>
            /// 如 公司名称
            /// </summary>
            public string value { get; set; }


        }


        private void ToLog(string log)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"D:\log_fx.txt", true, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString(), log));
                }
            }
            catch { }
        }


    }
}