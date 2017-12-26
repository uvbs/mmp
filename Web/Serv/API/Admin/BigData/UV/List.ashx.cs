using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.BigData.UV
{
    /// <summary>
    /// List 的摘要说明  访客统计
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLUserStatistics bllUser = new BLLJIMP.BLLUserStatistics();

        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 20;
            string keyWords=context.Request["keyword"];
            string time=context.Request["times"];
            string sort=context.Request["sort"];
            string order=context.Request["order"];

            int total=0;
            List<UserStatistics> userList = bllUser.GetUserStatisticList(pageSize, pageIndex, keyWords, time, sort, order, out total);

            List<dynamic> returnList=new List<dynamic>();
            foreach (var item in userList)
	        {
                returnList.Add(new {
                    nick_name=item.WXNickName,
                    head_img=item.WXHeadimgurl,
                    true_name=item.TrueName,
                    data_type=item.DateType,
                    insert_date=item.UpdateDate,
                    VisitCount = item.VisitCount,
                    ArticleBrowseCount = item.ArticleBrowseCount,
                    ActivityBrowseCount = item.ActivityBrowseCount,
                    ActivitySignUpCount = item.ActivitySignUpCount,
                    OrderCount = item.OrderCount,
                    Score = item.Score,
                    OtherBrowseCount = item.OtherBrowseCount
                });
	        }

            var data = new
            {
                total=total,
                rows=returnList
            };

            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(data));

           

        }
    }
}