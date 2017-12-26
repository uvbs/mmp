using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Article
{
    /// <summary>
    /// List 的摘要说明 文章列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
                int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
                string keyWord = context.Request["keyword"];
                string tags = context.Request["article_tags"];
                string categoryId = context.Request["category_id"];
                string sort = context.Request["article_sort"];
                int totalcount = 0;
                var articleList = bllJuActivity.QueryJuActivityData("search", out totalcount, null, null, null, null, keyWord, pageIndex, pageSize, null, null, "article", bllJuActivity.WebsiteOwner,null, categoryId, null, null, null, null, tags);
                resp.isSuccess = true;
                List<dynamic> list = new List<dynamic>();
                foreach (var item in articleList)
                {
                    list.Add(
                        new 
                        {
                            article_id=item.JuActivityID,
                            article_name=item.ActivityName,
                            article_img_url=item.ThumbnailsPath,
                            article_sort=item.Sort,
                            article_pv=item.PV,
                            article_summary=item.Summary,
                            article_tags=item.Tags,
                            article_access_level=item.AccessLevel,
                            category_name=item.CategoryName,
                            article_share_total_count=item.ShareTotalCount,
                            article_ishide=item.IsHide,
                            article_user_id=item.UserID
                        });
                }
                resp.returnObj=new 
                {
                    totalcount = totalcount,
                    list=list
                };
            }
            catch (Exception ex)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }


        public class RequestModel 
        {
            /// <summary>
            /// 文章id
            /// </summary>
            public int article_id { get; set; }

            /// <summary>
            /// 文章名称
            /// </summary>
            public string article_name { get; set; }

            /// <summary>
            /// 发布人
            /// </summary>
            public string article_user_id { get; set; }


            /// <summary>
            /// 文章详情
            /// </summary>
            public string articel_context { get; set; }

            /// <summary>
            /// 文章缩略图
            /// </summary>
            public string article_img_url { get; set; }

            /// <summary>
            /// 文章排序
            /// </summary>
            public int article_sort { get; set; }

            /// <summary>
            /// 文章浏览量
            /// </summary>
            public int article_pv { get; set; }

            /// <summary>
            /// 文章描述
            /// </summary>
            public string article_summary { get; set; }

            /// <summary>
            /// 文章标签
            /// </summary>
            public string article_tags { get; set; }

            /// <summary>
            /// 文章访问等级
            /// </summary>
            public int article_access_level { get; set; }

            /// <summary>
            /// 分类名称
            /// </summary>
            public string category_name { get; set; }

            /// <summary>
            /// 分享数
            /// </summary>
            public int article_share_total_count{get;set;}

            /// <summary>
            /// 文章状态  0代表进行中 1代表已结束 2代表已满员
            /// </summary>
            public int article_status { get; set; }

            /// <summary>
            /// 0 显示 1隐藏
            /// </summary>
            public int article_ishide { get; set; }

        }

    }
}