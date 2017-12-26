using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Activity
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNoAction
    {

        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity(); 
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            string sort = context.Request["activity_sort"];
            string cateId = context.Request["category_id"];
            string column = context.Request["column"];
            string is_forward = context.Request["is_forward"];
            string isFee=context.Request["isFee"];
            bool myActivity = false;//查看已经报过名的活动
            JuActivityInfoList data = new JuActivityInfoList();
            int total = 0;
            data.list = GetActivityData(pageSize, pageIndex, cateId, keyWord, myActivity, sort, out total, column, is_forward=="1",isFee);
            data.totalcount = total;
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(data));
        }

        public List<RequestModel> GetActivityData(int rows, int page, string cateId, string keyword, bool myActivity, string sort, out int total,
            string column = "", bool isForward = false,string isFee="")
        {
            //DateTime start = DateTime.Now;

            StringBuilder sbWhere = new StringBuilder();
            string orderBy = "";//默认排序
            switch (sort)
            {
                case "activity_tstatus":
                    orderBy = " TStatus DESC";
                    break;
                case "activity_start_time":
                    orderBy = " ActivityStartDate DESC";
                    break;
                case "activity_signcount ":
                    orderBy = " SignUpCount DESC";
                    break;
                case "activity_sort"://综合排序
                    orderBy = "  SignUpCount DESC, ActivityStartDate DESC";
                    break;
                case "activity_createDate":
                    orderBy = " CreateDate DESC ";
                    break;
                default:
                    orderBy = "";
                    break;
            }

            
            total = 0;

            string currUserId = "";
            if (myActivity)
            {
                currUserId = bll.GetCurrUserID();
                if (string.IsNullOrEmpty(currUserId))
                {
                    currUserId = "-1";
                }
            }
            BLLJIMP.Model.ActivityConfig activityConfig = new ActivityConfig();
            activityConfig = bllJuActivity.Get<BLLJIMP.Model.ActivityConfig>(string.Format(" WebsiteOwner='{0}'", bllJuActivity.WebsiteOwner));
            if (activityConfig == null)
            {
                activityConfig = new ActivityConfig();
            }
            bool showStopEndDateData = false;
            bool showHide = true;
            if (activityConfig.IsShowHideActivity == 1)
            {
                showStopEndDateData = true;
                showHide = false;
            }
            var juActivityData = bllJuActivity.QueryJuActivityData(null, out total, null, null, null, isFee, keyword, page, rows, currUserId, null, 
                "Activity", bll.WebsiteOwner, null, cateId, null, null, null, null, null, false, orderBy, false,showHide, false, null, null,
                showStopEndDateData, column, isForward);

            //DateTime dataend = DateTime.Now;
            
            //sbWhere.AppendFormat(" ArticleType='Activity' AND IsDelete=0 AND WebsiteOwner='{0}'", bll.WebsiteOwner);

            //if (!string.IsNullOrEmpty(cateId) && cateId != "0" && !cateId.Contains(","))
            //{
            //    cateId = new ZentCloud.BLLJIMP.BLLArticleCategory().GetCateAndChildIds(int.Parse(cateId));//获取下面的子分类
            //    if (string.IsNullOrEmpty(cateId)) cateId = "-1";
            //    sbWhere.AppendFormat(" AND ( CategoryId in ({0})  OR RootCateId IN ({0}))", cateId);
            //}
            //else if (!string.IsNullOrEmpty(cateId) && cateId.Contains(","))
            //{
            //    cateId = "'" + cateId.Replace(",", "','") + "'";
            //    sbWhere.AppendFormat(" AND CategoryId in ({0}) ", cateId);
            //}

            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    sbWhere.AppendFormat(" And (ActivityName like'%{0}%' Or ActivityAddress like'%{0}%')", keyword);
            //}
            //if (myActivity)
            //{
            //    sbWhere.AppendFormat(" And SignUpActivityID in(select ActivityID from ZCJ_ActivityDataInfo where UserId='{0}')", bll.GetCurrUserID());
            //}
            //sbWhere.Append(" AND IsSys = 0 ");
            List<RequestModel> data = new List<RequestModel>();

            //total = bll.GetCount<JuActivityInfo>(sbWhere.ToString());
            //var juActivityData = bll.GetLit<JuActivityInfo>(rows, page, sbWhere.ToString(), orderBy);
            foreach (JuActivityInfo p in juActivityData)
            {
                CrowdFundItem itemModel = bllJuActivity.Get<CrowdFundItem>(string.Format(" WebsiteOwner='{0}' AND CrowdFundID={1} order by ItemId asc ", bll.WebsiteOwner, p.JuActivityID));

                RequestModel requestModel = new RequestModel();
                requestModel.activity_id = p.JuActivityID;
                requestModel.activity_name = p.ActivityName;
                if (p.ActivityStartDate != null)
                {
                    requestModel.activity_start_time = bll.GetTimeStamp((DateTime)p.ActivityStartDate);
                }
                if (p.ActivityEndDate != null)
                {
                    requestModel.activity_end_time = bll.GetTimeStamp((DateTime)p.ActivityEndDate);
                }
                requestModel.activity_address = p.ActivityAddress;
                requestModel.category_name = p.CategoryName;
                requestModel.activity_img_url = bll.GetImgUrl(p.ThumbnailsPath);
                requestModel.activity_pv = p.PV;
                requestModel.activity_signcount = p.SignUpCount;
                requestModel.activity_score = p.ActivityIntegral;
                if (!string.IsNullOrEmpty(p.Tags))
                {
                    requestModel.activity_tags = p.Tags.Split(',').Take(5).ToList();
                }
                requestModel.activity_status = p.ActivityStatus;
                if (itemModel != null)
                {
                    requestModel.activity_price = itemModel.Amount;
                }
                data.Add(requestModel);
            }
            //DateTime dataStructure = DateTime.Now;

            //data.Add(new RequestModel {
            //    activity_address = start.ToString("yyyy-MM-dd hh:mm:ss.fff"),
            //    activity_img_url = dataend.ToString("yyyy-MM-dd hh:mm:ss.fff"),
            //    activity_name = dataStructure.ToString("yyyy-MM-dd hh:mm:ss.fff")
            //});
            return data;
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
            /// 活动开始时间
            /// </summary>
            public double activity_end_time { get; set; }

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

            //价钱
            public decimal activity_price { get; set; } 

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