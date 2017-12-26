using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using System.Reflection;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// Summary description for ReviewHandler
    /// </summary>
    public class ReviewHandler : BaseHandler
    {
        /// <summary>
        /// 评论
        /// </summary>
        BLLReview bllReview = new BLLReview();
        /// <summary>
        /// 批量审核Review
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string BatchAudit(HttpContext context)
        {

            string[] ids = context.Request["ids"].Split(',');
            BLLReview.AuditStatus status = (BLLReview.AuditStatus)int.Parse(context.Request["status"]);
            if (bllReview.BatchAudit(status, ids))
            {
                resp.Status = 0;
                resp.Msg = "更新审核状态成功";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "更新审核状态失败！";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 查询文章评论后台
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryArticleReview(HttpContext context)
        {

            if ((!bllReview.WebsiteOwner.Equals(currUserInfo.UserID)) && (!currUserInfo.UserType.Equals(1)))
            {
                return "无权访问";
            }
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}' And ReviewType='文章评论'", bllReview.WebsiteOwner));
            string keyWord = context.Request["KeyWord"];
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" And ReplyContent like '%{0}%'", keyWord);
            }
            int totalCount = bllReview.GetCount<ReplyReviewInfo>(sbWhere.ToString());
            List<ReplyReviewInfo> data = bllReview.GetLit<ReplyReviewInfo>(pageSize, pageIndex, sbWhere.ToString(), " AutoId DESC");
            return Common.JSONHelper.ObjectToJson(
                 new
                 {
                     total = totalCount,
                     rows = data
                 });

        }

        /// <summary>
        /// 删除文章评论后台
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteArticleReview(HttpContext context)
        {
            if ((!bllReview.WebsiteOwner.Equals(currUserInfo.UserID)) && (!currUserInfo.UserType.Equals(1)))
            {
                return "无权访问";
            }
            string ids = context.Request["ids"];
            string sql = string.Format("AutoID in({0}) And WebSiteOwner='{1}'", ids, bllReview.WebsiteOwner);
            return bllReview.Delete(new ReplyReviewInfo(), sql).ToString();

        }
    }
}