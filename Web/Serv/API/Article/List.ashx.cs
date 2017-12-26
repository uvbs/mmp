using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Article
{
    /// <summary>
    /// 文章列表接口
    /// </summary>
    public class List : BaseHandlerNoAction
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
                var articleList = bllJuActivity.QueryJuActivityData("search", out totalcount, null, null, null, null, keyWord, pageIndex, pageSize, null, null, "article", bllJuActivity.WebsiteOwner, null, categoryId, null, null, null, null, tags);
                resp.isSuccess = true;
                List<dynamic> list = new List<dynamic>();
                foreach (var item in articleList)
                {
                    list.Add(
                        new
                        {
                            article_id = item.JuActivityID,
                            article_name = item.ActivityName,
                            //articel_context = item.ActivityDescription,
                            article_img_url = item.ThumbnailsPath,
                            article_sort = item.Sort,
                            article_pv = item.PV,
                            article_summary = item.Summary,
                            article_tags = item.Tags,
                            article_access_level = item.AccessLevel,
                            category_name = item.CategoryName,
                            article_share_total_count = item.ShareTotalCount,
                            article_time=bllJuActivity.GetTimeStamp(item.CreateDate),
                            article_status = item.ActivityStatus
                        });
                }
                resp.returnObj = new
                {
                    totalcount = totalcount,
                    list = list
                };
            }
            catch (Exception ex)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }

    }
}