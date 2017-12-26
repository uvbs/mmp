using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Activity
{
    /// <summary>
    /// List 的摘要说明   查询活动数据
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            string sort = context.Request["activity_sort"];
            string cateId = context.Request["category_id"];
            bool myActivity = false;//查看已经报过名的活动
            StringBuilder sbWhere = new StringBuilder();
            string orderBy = "";//默认排序
            switch (sort)
            {
                case "activity_start_time":
                    orderBy = " ActivityStartDate DESC";
                    break;
                case "activity_signcount ":
                    orderBy = " SignUpCount DESC";
                    break;
                default:
                    orderBy = " Sort DESC";
                    break;
            }

            sbWhere.AppendFormat(" ArticleType='activity' AND IsDelete=0 AND WebsiteOwner='{0}'", bll.WebsiteOwner);
            if (!string.IsNullOrEmpty(cateId))
            {
                sbWhere.AppendFormat(" And CategoryId={0}", cateId);
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And (ActivityName like'%{0}%' Or ActivityAddress like'%{0}%')", keyWord);
            }
            if (myActivity)
            {
                sbWhere.AppendFormat(" And SignUpActivityID in(select ActivityID from ZCJ_ActivityDataInfo where UserId='{0}')", bll.GetCurrUserID());
            }
            sbWhere.Append(" AND IsSys = 0 ");
            JuActivityInfoList data = new JuActivityInfoList();
            data.list = new List<RequestModel>();
            data.totalcount = bll.GetCount<JuActivityInfo>(sbWhere.ToString());
            var juActivityData = bll.GetLit<JuActivityInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
            foreach (JuActivityInfo p in juActivityData)
	        {
                RequestModel requestModel = new RequestModel();
                requestModel.activity_id = p.JuActivityID;
                requestModel.activity_name=p.ActivityName;
                if (p.ActivityStartDate != null)
                {
                    requestModel.activity_start_time = bll.GetTimeStamp((DateTime)p.ActivityStartDate);
                }
                requestModel.activity_address=p.ActivityAddress;
                requestModel.category_name=p.CategoryName;
                requestModel.activity_img_url = bll.GetImgUrl(p.ThumbnailsPath);
                requestModel.activity_pv=p.PV;
                requestModel.activity_signcount = p.SignUpCount;
                requestModel.activity_score = p.ActivityIntegral;
                if (!string.IsNullOrEmpty(p.Tags))
                {
                    requestModel.activity_tags = p.Tags.Split(',').Take(5).ToList();
                }
                if (p.IsHide == 1)
                {
                    requestModel.activity_status = 1;
                }
                if ((p.MaxSignUpTotalCount > 0) && (p.SignUpTotalCount >= p.MaxSignUpTotalCount))
                {
                    requestModel.activity_status = 2;
                }
                data.list.Add(requestModel);
	        }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(data));
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
            /// 活动开始时间
            /// </summary>
            public double activity_start_time { get; set; }

            /// <summary>
            /// 活动地址
            /// </summary>
            public string activity_address { get; set; }

            /// <summary>
            /// 分类名称
            /// </summary>
            public string category_name { get; set; }

            /// <summary>
            /// 缩略图
            /// </summary>
            public string activity_img_url { get; set; }


            /// <summary>
            /// pv 阅读量
            /// </summary>
            public int activity_pv { get; set; }

            /// <summary>
            /// 报门总人数
            /// </summary>
            public int activity_signcount { get; set; }


            /// <summary>
            /// 报门所需积分
            /// </summary>
            public int activity_score { get; set; }

            /// <summary>
            /// 活动状态 0代表进行中 1代表已结束 2代表已满员
            /// </summary>
            public int activity_status { get; set; }

            //<summary>
            // 活动标签
            // </summary>
            public List<string> activity_tags { get; set; }

        }


        public class JuActivityInfoList 
        {
            /// <summary>
            /// 总数
            /// </summary>
            public int totalcount { get; set; }
            /// <summary>
            /// 集合
            /// </summary>
            public List<RequestModel> list { get; set; }
        }

    }
}