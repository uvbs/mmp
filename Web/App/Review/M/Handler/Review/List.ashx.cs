using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Review.M.Handler.Review
{
    /// <summary>
    /// 话题列表
    /// </summary>
    public class List : ZentCloud.JubitIMP.Web.Serv.BaseHandlerNeedLoginNoAction
    {

        BLLReview bllReview = new BLLReview();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            string reviewTitle = context.Request["Title"];
            string type = context.Request["type"];
            string haveReply = context.Request["HaveReply"];//是否有回复
            string sort = context.Request["Sort"];
            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbOrderBy = new StringBuilder();
            sbWhere.AppendFormat(" ReviewType='话题' AND websiteOwner='{0}'", bllReview.WebsiteOwner);
            if (!string.IsNullOrEmpty(reviewTitle))
            {
                sbWhere.AppendFormat(" AND ReviewTitle like '%{0}%'", reviewTitle);
            }
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat(" AND CategoryType LIKE '%{0}%'", type);
            }
            if (!string.IsNullOrEmpty(haveReply))
            {
                sbWhere.Append(" AND NumCount>0");

            }
            sbOrderBy.Append(" AutoId DESC ");
            if (sort.Equals("Newhf"))
            {
                sbOrderBy.Clear();
                sbOrderBy.Append(" ReplyDateTiem DESC");
            }
            if (sort.Equals("Mosthf"))
            {
                sbOrderBy.Clear();
                sbOrderBy.Append(" NumCount DESC");
            }

            if (sort.Equals("Mosthp"))
            {
                sbOrderBy.Clear();
                sbOrderBy.Append(" PraiseNum DESC, ReplyDateTiem DESC");
            }
            List<ReviewInfo> list = bllReview.GetLit<ReviewInfo>(pageSize, pageIndex, sbWhere.ToString(), sbOrderBy.ToString());
            apiResp.status = true;
            apiResp.result = list;
            context.Response.Write(Common.JSONHelper.ObjectToJson(apiResp));
        }



    }
}