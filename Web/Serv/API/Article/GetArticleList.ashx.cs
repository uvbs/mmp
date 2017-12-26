using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Article
{
    /// <summary>
    /// GetArticleList 的摘要说明
    /// </summary>
    public class GetArticleList : BaseHandlerNoAction
    {
        /// <summary>
        /// 活动业务逻辑
        /// </summary>
        BLLJIMP.BLLJuActivity bll = new BLLJIMP.BLLJuActivity();
        /// <summary>
        /// 用户业务逻辑
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");

        pubapi papi = new pubapi();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        public void ProcessRequest(HttpContext context)
        {//DateTime start = DateTime.Now;
            int pageIndex = Convert.ToInt32(context.Request["pageIndex"]),
               pageSize = Convert.ToInt32(context.Request["pageSize"]),
               isGetNoCommentData = Convert.ToInt32(context.Request["isGetNoCommentData"]),
               isHasCommentAndReplayCount = Convert.ToInt32(context.Request["isHasCommentAndReplayCount"]);
                string cateId = context.Request["cateId"],
                   rootId=context.Request["root_id"],
                   keyword = context.Request["keyword"],
                   tags = context.Request["tags"],
                   cityCode = context.Request["city"],
                   provinceCode = context.Request["province"],
                   orderby = context.Request["orderby"],
                   type = context.Request["type"],
                   author = context.Request["author"],
                   keyType = context.Request["keyType"],
                   hasStatisticsStr = context.Request["hasStatistics"],
                   hasAuthorStr = context.Request["hasAuthor"],
                   column = context.Request["column"],
                   data_type = context.Request["data_type"],
                   create_start = context.Request["create_start"],
                   create_end = context.Request["create_end"],
                   keyword_author = context.Request["keyword_author"],
                   order_all = context.Request["order_all"],
                   chk_friend = context.Request["chk_friend"],
                   hide_subcount = context.Request["hide_subcount"],
                   hide_replyuser = context.Request["hide_replyuser"],
                   hide_province = context.Request["hide_province"],
                   is_hide=context.Request["is_hide"];

            if (orderby == "comment"){
                orderby = "CommentAndReplayCount desc";
            }
            bool hasStatistics = true;
            bool hasAuthor = true;
            bool chkFriend = false;
            bool hideSubCount = false;
            bool hideReplyUser = false;
            bool hideProvince = false;
            bool isHide = false;
            bool isForward = false;
            if (hasStatisticsStr == "0") hasStatistics = false;
            if (hasAuthorStr == "0") hasAuthor = false;
            if (chk_friend == "1") chkFriend = true;
            if (hide_subcount == "1") hideSubCount = true;
            if (hide_replyuser == "1") hideReplyUser = true;
            if (hide_province == "1") hideProvince = true;
            if (is_hide == "all") isHide = true;
            if (data_type == "1") isForward = true;
            if (!string.IsNullOrWhiteSpace(author)) author = bllUser.GetUserInfoByAutoID(int.Parse(author)).UserID;

            currentUserInfo = bll.GetCurrentUserInfo();

            var totalCount = 0;
            var sourceData = this.bll.GetJuActivityList(
                    type,
                    "",
                    out totalCount,
                    pageIndex,
                    pageSize,
                    author,
                    this.currentUserInfo == null ? "" : this.currentUserInfo.UserID,
                    cateId,
                    this.bll.WebsiteOwner,
                    keyword,
                    tags,
                    provinceCode,
                    cityCode,
                    null,
                    isGetNoCommentData > 0,
                    orderby,
                    isHasCommentAndReplayCount > 0,
                    isHide,
                    null,
                    false,
                    column,
                    hasStatistics,
                    hasAuthor,
                    isForward,
                    create_start,
                    create_end,
                    keyword_author == "1",
                    order_all,
                    hideSubCount,
                    hideReplyUser,
                    hideProvince,
                    rootId
                );
            //DateTime dataend = DateTime.Now;

            List<dynamic> returnList = new List<dynamic>();

            foreach (var item in sourceData)
            {
                returnList.Add(papi.StructureArticle(item, false, currentUserInfo, chkFriend));
            }
            //DateTime dataStructure = DateTime.Now;

            apiResp.status = true;
            apiResp.code = (int)ZentCloud.BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.result = new
            {
                totalcount = totalCount,
                list = returnList
                //,
                //start = start.ToString("yyyy-MM-dd hh:mm:ss.fff"),
                //dataend = dataend.ToString("yyyy-MM-dd hh:mm:ss.fff"),
                //dataStructure = dataStructure.ToString("yyyy-MM-dd hh:mm:ss.fff")
            };
            bll.ContextResponse(context, apiResp);
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