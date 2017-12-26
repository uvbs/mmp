using CommonPlatform.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Handler
{
    /// <summary>
    /// TutorApplyHander 的摘要说明
    /// </summary>
    public class IllegalHandler : IHttpHandler, IRequiresSessionState
    {
        AshxResponse resp = new AshxResponse();
        BLLCommRelation bllCommRelation = new BLLCommRelation();
        BLLUser bllUser = new BLLUser();
        BLLJuActivity bll = new BLLJuActivity();
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
        BLLReview bllReview = new BLLReview();
        BLLSystemNotice bllSystemNotice = new BLLSystemNotice();
        UserInfo currentUserInfo;
        
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                currentUserInfo = bllUser.GetCurrentUserInfo();

                if (currentUserInfo == null)
                {
                    resp.Status = (int)APIErrCode.UserIsNotLogin;
                    resp.Msg = "用户未登录";
                    result = Common.JSONHelper.ObjectToJson(resp);
                    return;
                }

                string Action = context.Request["Action"];
                switch (Action)
                {
                    case "getIllegalArticleList":
                        result = getIllegalArticleList(context);
                        break;
                    case "DelArticle":
                        result = DelArticle(context);
                        break;
                    case "getIllegalReviewList":
                        result = getIllegalReviewList(context);
                        break;
                    case "DelReview":
                        result = DelReview(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);
            }
            context.Response.Write(result);
        }
        /// <summary>
        /// 举报内容列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getIllegalArticleList(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request["page"]),
                rows = Convert.ToInt32(context.Request["rows"]);
            string keyword = context.Request["keyword"];
            if (page == 0) page = 1;
            if (rows == 0) rows = int.MaxValue;
            var dataList = bll.GetIllegalContentList(rows, page, keyword, bll.WebsiteOwner, false);
            int totalCount = Convert.ToInt32(dataList.Tables[1].Rows[0][0]);
            List<dynamic> data = new List<dynamic>();

            foreach (DataRow item in dataList.Tables[0].Rows)
            {
                data.Add(new
                {
                    id = item["JuActivityID"],
                    iname = bllUser.GetUserDispalyName(item["RelationId"].ToString()),
                    name = bllUser.GetUserDispalyName(item["UserID"].ToString()),
                    content = MySpider.MyRegex.RemoveHTMLTags(item["ActivityDescription"].ToString()),
                    datetm = Convert.ToDateTime(item["RelationTime"]).ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            return Common.JSONHelper.ObjectToJson(new
            {
                rows = data,
                total = totalCount
            });
        }

        /// <summary>
        /// 举报回答回复列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getIllegalReviewList(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request["page"]),
                rows = Convert.ToInt32(context.Request["rows"]);
            string keyword = context.Request["keyword"];
            if (page == 0) page = 1;
            if (rows == 0) rows = int.MaxValue;
            var dataList = bllReview.GetIllegalReviewList(rows, page, keyword, bll.WebsiteOwner);
            int totalCount = Convert.ToInt32(dataList.Tables[1].Rows[0][0]);
            List<dynamic> data = new List<dynamic>();

            foreach (DataRow item in dataList.Tables[0].Rows)
            {
                data.Add(new
                {
                    id = item["ReviewMainId"],
                    iname = bllUser.GetUserDispalyName(item["RelationId"].ToString()),
                    type = item["ReviewType"],
                    name = bllUser.GetUserDispalyName(item["UserID"].ToString()),
                    content = MySpider.MyRegex.RemoveHTMLTags(item["ReviewContent"].ToString()),
                    datetm = Convert.ToDateTime(item["RelationTime"]).ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            return Common.JSONHelper.ObjectToJson(new
            {
                rows = data,
                total = totalCount
            });
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelArticle(HttpContext context)
        {
            string ids = context.Request["ids"];
            List<string> IdList = ids.Split(',').ToList();
            for (int i = 0; i < IdList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(IdList[i])) continue;
                JuActivityInfo act = bll.GetJuActivity(int.Parse(IdList[i]));
                if (bll.DelArticle(IdList[i]))
                {
                    if (act.ArticleType == "Question") {
                        bllUser.AddUserScoreDetail(act.UserID, EnumStringHelper.ToString(ScoreDefineType.DelQuestions), bll.WebsiteOwner, null, null);
                    }
                    else if (act.ArticleType == "Article"){
                        bllUser.AddUserScoreDetail(act.UserID, EnumStringHelper.ToString(ScoreDefineType.DelArticle), bll.WebsiteOwner, null, null);
                    }
                }
            }
            resp.Status = 1;
            resp.Msg = "删除完成";
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 删除回复
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelReview(HttpContext context)
        {
            string ids = context.Request["ids"];
            List<string> IdList = ids.Split(',').Distinct().ToList();
            for (int i = 0; i < IdList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(IdList[i])) continue;
                ReviewInfo re = bllReview.GetReviewInfo(int.Parse(IdList[i]));
                if (bllReview.DelReview(IdList[i]))
                {
                    if (re.ReviewType == "Answer")
                    {
                        bllUser.AddUserScoreDetail(re.UserId, EnumStringHelper.ToString(ScoreDefineType.DelAnswer), bll.WebsiteOwner, null, null);
                    }
                    else if (re.ReviewType == "ArticleComment" || re.ReviewType == "CommentReply")
                    {
                        bllUser.AddUserScoreDetail(re.UserId, EnumStringHelper.ToString(ScoreDefineType.DelReview), bll.WebsiteOwner, null, null);
                        
                    }
                }
            }
            resp.Status = 1;
            resp.Msg = "删除完成";
            return Common.JSONHelper.ObjectToJson(resp);
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}