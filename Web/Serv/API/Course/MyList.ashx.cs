using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Course
{
    /// <summary>
    ///我的课程列表
    /// </summary>
    public class MyList : BaseHandlerNeedLoginNoAction
    {
        /// <summary>
        /// 
        /// </summary>
        BLLMall bllMall = new BLLMall();
        public void ProcessRequest(HttpContext context)
        {
           
            int totalCount = 0;
            var orderList = bllMall.GetOrderList(10000, 1, "", out totalCount, "", CurrentUserInfo.UserID).Where(p => p.OrderType == 7).Where(p => p.PaymentStatus == 1).Where(p => p.Status != "已取消").OrderByDescending(p=>p.InsertDate);
            List<MyCourse> data = new List<MyCourse>();
            foreach (var item in orderList)
            {

               
                try
                {
                    var orderDetail = bllMall.GetOrderDetailsList(item.OrderID)[0];
                    MyCourse model = new MyCourse();
                    var productInfo = bllMall.GetProduct(orderDetail.PID);
                    model.course_img_url = productInfo.RecommendImg;
                    model.course_name = productInfo.PName + " " + orderDetail.SkuShowProp;

                    Questionnaire examInfo = bllMall.Get<Questionnaire>(string.Format("QuestionnaireID={0}", orderDetail.ExQuestionnaireID));

                    model.exam_id = examInfo.QuestionnaireID;
                    model.exam_minute = examInfo.ExamMinute;
                    model.status = bllMall.GetExamStatus(item, examInfo);
                    switch (model.status)
                    {

                        case 0://0 尚未到考试时间
                            model.day = (int)Math.Ceiling((item.InsertDate.AddDays(15) - DateTime.Now).TotalDays);
                            break;
                        case 1:// 1 已经到考试时间,可以正常考试
                            model.day = (int)Math.Ceiling((item.InsertDate.AddDays(90) - DateTime.Now).TotalDays);
                            break;
                        case 2:// 2 已经考过了
                            QuestionnaireRecord record = bllMall.Get<QuestionnaireRecord>(string.Format("QuestionnaireID={0}", examInfo.QuestionnaireID));
                            model.time = record.InsertDate.ToString("yyyy-MM-dd");
                            break;
                        case 3:// 3 缺考
                            model.time = item.InsertDate.AddDays(90).ToString("yyyy-MM-dd");
                            break;
                        default:
                            break;
                    }




                    data.Add(model);
                }
                catch (Exception)
                {

                    continue;
                }  
                

            }

            apiResp.status = true;
            apiResp.result = new
            {
                list = data
            };
            apiResp.msg = "ok";
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }


        /// <summary>
        /// 课程模型
        /// </summary>
        private class MyCourse
        {

            /// <summary>
            /// 状态
            /// 0 尚未到考试时间
            /// 1 已经到考试时间,可以正常考试
            /// 2 已经考过了
            /// 3 缺考
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 课程名称
            /// </summary>
            public string course_img_url { get; set; }

            /// <summary>
            /// 课程名称
            /// </summary>
            public string course_name { get; set; }


            /// <summary>
            /// 距离考试时间还剩下多少天
            /// status 1 还剩多少天
            /// status 0 还有多少天可以考试
            /// </summary>
            public int day { get; set; }
            /// <summary>
            /// 考试时间 分钟
            /// </summary>
            public int exam_minute { get; set; }
            /// <summary>
            /// 试卷Id
            /// </summary>
            public int exam_id { get; set; }
            /// <summary>
            /// status 2 表示考试日期
            /// status 3 表示考试结束日期
            /// </summary>
            public string time { get; set; }




        }


    }
}