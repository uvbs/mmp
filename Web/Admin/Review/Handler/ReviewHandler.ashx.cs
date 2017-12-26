using CommonPlatform.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.JubitIMP.Web.Serv;

namespace ZentCloud.JubitIMP.Web.Admin.Handler
{
    /// <summary>
    /// 话题
    /// </summary>
    public class ReviewHandler : BaseHandlerNeedLoginAdmin
    {
        AshxResponse resp = new AshxResponse();
        BLLCommRelation bllCommRelation = new BLLCommRelation();
        BLLUser bllUser = new BLLUser();
        BLLJuActivity bll = new BLLJuActivity();
        BLLReview bllReview = new BLLReview();
        //UserInfo currentUserInfo;

        //public void ProcessRequest(HttpContext context)
        //{
        //    context.Response.ContentType = "text/plain";
        //    context.Response.Expires = 0;
        //    string result = "false";
        //    try
        //    {
        //        currentUserInfo = bllUser.GetCurrentUserInfo();

        //        if (currentUserInfo == null)
        //        {
        //            resp.Status = (int)APIErrCode.UserIsNotLogin;
        //            resp.Msg = "用户未登录";
        //            result = Common.JSONHelper.ObjectToJson(resp);
        //            return;
        //        }

        //        string action = context.Request["Action"];
        //        switch (action)
        //        {
        //            case "GetReviewList":
        //                result = GetReviewList(context);
        //                break;
        //            case "DelReview":
        //                result = DelReview(context);
        //                break;
        //            case "Add":
        //                result = Add(context);
        //                break;
        //            case "ReplyList":
        //                result = ReplyList(context);
        //                break;
        //            case "DeleteReply":
        //                result = DeleteReply(context);
        //                break;
        //            case "UpdateReplyStatus":
        //                result = UpdateReplyStatus(context);
        //                break;
        //            case "UpdateReviewConfig":
        //                result = UpdateReviewConfig(context);
        //                break;
        //            case "Delete":
        //                result = Delete(context);
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resp.Status = -1;
        //        resp.Msg = ex.Message;
        //        result = Common.JSONHelper.ObjectToJson(resp);

        //    }
        //    context.Response.Write(result);
        //}


        /// <summary>
        /// 评论列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetReviewList(HttpContext context)
        {
            try
            {

           
            int pageIndex = Convert.ToInt32(context.Request["page"]),

                pageSize = Convert.ToInt32(context.Request["rows"]);

            string actId = context.Request["actId"];

            string keyword = context.Request["keyword"];

            string Pfolder = context.Request["Pfolder"];

            string status = context.Request["status"];

            string foreignkeyId = context.Request["foreignkeyId"];

            string type=context.Request["type"];

            if (string.IsNullOrEmpty(Pfolder))
            {
                type = "话题";
            }
            else
            {
                if (Pfolder == "ArticleManage") type = "";
                if (Pfolder == "OrderComment") type = "OrderComment";
            }
      
            if (pageIndex == 0) pageIndex = 1;

            if (pageSize == 0) pageSize = int.MaxValue;
            int totalCount = 0;
            List<ReviewInfo> dataList = bllReview.GetActReviewList(out totalCount, pageIndex, pageSize, foreignkeyId, keyword, null, type, actId, status);

            List<ResponModel> returnList = new List<ResponModel>();

            foreach (var item in dataList)
            {
                ResponModel model = new ResponModel();
                model.id = item.ReviewMainId;
                model.title = item.ReviewTitle;
                model.content = item.ReviewContent;
                model.comment_img = item.CommentImg;
                model.foreignkey_id = item.ForeignkeyId;
                model.status = item.AuditStatus;
                model.reply_count = bll.GetCount<ReplyReviewInfo>(string.Format(" ReviewID={0}", item.AutoId));
                model.review_id = item.AutoId;
                model.ex1 = item.Expand1;
                if (type == "OrderComment")
                {
                    model.name = item.UserName;
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.UserId))
                    {
                        model.name = bllUser.GetUserDispalyName(item.UserId);
                    }
                }
                if (type != "OrderComment") model.rpyNum = bllReview.GetReviewCount(item.ReviewMainId.ToString());
                model.type = item.ReviewType;
                returnList.Add(model);
            }
            return Common.JSONHelper.ObjectToJson(new
            {
                rows = returnList,
                total = totalCount
            });
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// 
        public class ResponModel
        {
            public int id { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// 内容
            /// </summary>
            public string content { get; set; }
            /// <summary>
            /// 文章、商品
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 状态
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 评论人
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 回复数
            /// </summary>

            public int rpyNum { get; set; }
            /// <summary>
            /// 回复数量
            /// </summary>
            public int reply_count { get; set; }
            /// <summary>
            /// 评论id
            /// </summary>
            public int review_id { get; set; }
            /// <summary>
            /// 评论图片
            /// </summary>
            public string comment_img { get; set; }
            /// <summary>
            /// 关系id
            /// </summary>
            public string foreignkey_id { get; set; }
            /// <summary>
            /// 商品id
            /// </summary>
            public string ex1 { get; set; }
        }
        private string DelReview(HttpContext context)
        {
            string ids = context.Request["ids"];
            List<string> IdList = ids.Split(',').ToList();
            for (int i = 0; i < IdList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(IdList[i])) continue;
                if (bllReview.DelReview(IdList[i]))
                {
                    ReviewInfo reviewInfo = bllReview.GetReviewInfo(int.Parse(IdList[i]));
                    if (reviewInfo.ReviewType == "Answer")
                    {
                        bllUser.AddUserScoreDetail(reviewInfo.UserId, EnumStringHelper.ToString(ScoreDefineType.DelAnswer), bll.WebsiteOwner, null, null);
                    }
                    else if (reviewInfo.ReviewType == "ArticleComment" || reviewInfo.ReviewType == "CommentReply")
                    {
                        bllUser.AddUserScoreDetail(reviewInfo.UserId, EnumStringHelper.ToString(ScoreDefineType.DelReview), bll.WebsiteOwner, null, null);
                    }
                }
            }
            resp.Status = 1;
            resp.Msg = "删除完成";
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Pass(HttpContext context)
        {
            string ids = context.Request["ids"];
            List<string> IdList = ids.Split(',').ToList();
            for (int i = 0; i < IdList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(IdList[i])) continue;
                bllReview.UpdateByKey<ReviewInfo>("AutoId", IdList[i], "AuditStatus", "1");
            }
            BLLRedis.ClearProductList(bllReview.WebsiteOwner);
            resp.Status = 1;
            resp.Msg = "审核通过";
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string NoPass(HttpContext context)
        {
            string ids = context.Request["ids"];
            List<string> IdList = ids.Split(',').ToList();
            for (int i = 0; i < IdList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(IdList[i])) continue;
                bllReview.UpdateByKey<ReviewInfo>("AutoId", IdList[i], "AuditStatus", "2");
            }
            BLLRedis.ClearProductList(bllReview.WebsiteOwner);
            resp.Status = 1;
            resp.Msg = "审核不通过";
            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 发表话题
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Add(HttpContext context)
        {
            string title = context.Request["title"];
            string content = context.Request["content"];
            if (string.IsNullOrEmpty(title))
            {
                resp.Msg = "标题不能为空";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(content))
            {
                resp.Msg = "内容不能为空";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            BLLJIMP.Model.ReviewInfo model = new BLLJIMP.Model.ReviewInfo
            {
                ForeignkeyId = bll.WebsiteOwner,
                ForeignkeyName = bll.WebsiteOwner,
                UserId = this.currentUserInfo.UserID,
                UserName = this.currentUserInfo.TrueName,
                ReviewPower = 0,
                InsertDate = DateTime.Now,
                ReviewTitle = title,
                ReviewContent = content,
                WebsiteOwner = bll.WebsiteOwner,
                PraiseNum = 0,
                StepNum = 0,
                AuditStatus = 1,
                ReviewType = "话题",
                CategoryType = "",
                ReplyDateTiem = DateTime.Now
            };
            if (bll.Add(model))
            {
                resp.Status = 1;
                resp.Msg = "发布成功";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 评论回复列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ReplyList(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]),
                pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyword = context.Request["keyword"];
            string reviewId = context.Request["reviewId"];
            string status = context.Request["status"];
            if (pageIndex == 0) pageIndex = 1;
            if (pageSize == 0) pageSize = int.MaxValue;
            int totalCount = 0;
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' And ReviewID={1}", bll.WebsiteOwner, reviewId);
            if (!string.IsNullOrEmpty(keyword))
            {
                sbWhere.AppendFormat(" And ReplyContent like '%{0}%'", keyword);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sbWhere.AppendFormat(" And Status ='{0}'", status);
            }
            totalCount = bll.GetCount<ReplyReviewInfo>(sbWhere.ToString());
            var list = bll.GetLit<ReplyReviewInfo>(pageSize, pageIndex, sbWhere.ToString(), " AutoID DESC");
            return Common.JSONHelper.ObjectToJson(new
            {
                rows = list,
                total = totalCount
            });
        }


        /// <summary>
        /// 删除评论回复 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteReply(HttpContext context)
        {
            string ids = context.Request["ids"];
            if (bll.Delete(new ReplyReviewInfo(), string.Format(" AutoId in ({0}) And WebsiteOwner='{1}'", ids, bll.WebsiteOwner)) > 0)
            {
                resp.Status = 1;
                resp.Msg = "删除成功";

            }
            else
            {
                resp.Msg = "删除失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 更新审核状态
        /// </summary>
        private string UpdateReplyStatus(HttpContext context)
        {

            string ids = context.Request["ids"];
            string status = context.Request["Status"];
            if (bll.Update(new ReplyReviewInfo(), string.Format(" Status={0}", status), string.Format(" AutoId in ({0}) And WebsiteOwner='{1}'", ids, bll.WebsiteOwner)) > 0)
            {
                resp.Status = 1;
                resp.Msg = "操作成功";

            }
            else
            {
                resp.Msg = "操作失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);

        }
        /// <summary>
        /// 更新话题配置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateReviewConfig(HttpContext context)
        {

            string isEnableUserReleaseReview = context.Request["IsEnableUserReleaseReview"];
            WebsiteInfo currentWebsiteInfo = bll.GetWebsiteInfoModelFromDataBase();
            currentWebsiteInfo.IsEnableUserReleaseReview = int.Parse(isEnableUserReleaseReview);
            if (bll.Update(currentWebsiteInfo))
            {
                resp.Status = 1;
                resp.Msg = "操作成功";

            }
            else
            {
                resp.Msg = "操作失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 删除话题
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Delete(HttpContext context)
        {
            //TODO：
            //删除前得判断，如果是评论的，查出评论的回复，并减去文章表里的  评论字段以及  评论和回复字段
            //如果是回复的，则只减去文章表里的  评论和回复字段

            //目前先把评论和回复的删除功能去掉

            string ids = context.Request["ids"];
            if (bll.Delete(new ReviewInfo(), string.Format(" AutoId in ({0}) And WebsiteOwner='{1}'", ids, bll.WebsiteOwner)) > 0)
            {
                bll.Delete(new ReplyReviewInfo(), string.Format(" ReviewID in ({0}) And WebsiteOwner='{1}'", ids, bll.WebsiteOwner));
                resp.Status = 1;
                resp.Msg = "删除成功";

            }
            else
            {
                resp.Msg = "删除失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }


    }
}