using System;
using System.Collections.Generic;
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
    public class StatusesHandler : IHttpHandler, IRequiresSessionState, IReadOnlySessionState
    {
        AshxResponse resp = new AshxResponse();
        BLLCommRelation bllCommRelation = new BLLCommRelation();
        BLLUser bllUser = new BLLUser();
        BLLUserExpand bllUserExpand = new BLLUserExpand();
        BLLJuActivity bll = new BLLJuActivity();
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
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

                string action = context.Request["Action"];
                switch (action)
                {
                    case "getStatusesList":
                        result = getStatusesList(context);
                        break;
                    case "EditStatuses":
                        result = EditStatuses(context);
                        break;
                    case "getStatusesArticleList":
                        result = getStatusesArticleList(context);
                        break;
                    case "DelStatusesArticle":
                        result = DelStatusesArticle(context);
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
        /// 社区列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getStatusesList(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request["page"]),
                rows = Convert.ToInt32(context.Request["rows"]);
            string type = "Community";
            int preId = int.Parse(context.Request["preId"]);

            if (page == 0) page = 1;
            if (rows == 0) rows = int.MaxValue;
            int totalCount = 0;
            var dataList = bllArticleCategory.GetCateList(out totalCount, type, preId, bll.WebsiteOwner, rows, page);
            var data = from p in dataList
                       select new
                       {
                           AutoID = p.AutoID,
                           CategoryName = p.CategoryName,
                           type = p.CategoryName,
                           Summary = p.Summary,
                           Sort = p.Sort,
                           actNum = bll.GetJuActivityCount("Statuses",p.AutoID.ToString(),null,true),
                           ImgSrc = p.ImgSrc
                       };
            return Common.JSONHelper.ObjectToJson(new {
                rows = data,
                total = totalCount
            });
        }
        /// <summary>
        /// 添加/修改类型
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditStatuses(HttpContext context)
        {
            string type = "Community";
            int autoId = int.Parse(context.Request["AutoID"]);
            string categoryName = context.Request["CategoryName"];
            string summary = context.Request["Summary"];
            int preId = int.Parse(context.Request["preId"]);
            string imgSrc = context.Request["ImgSrc"];
            string createTime = context.Request["CreateTime"];
            int sort = int.Parse(context.Request["Sort"]);
            

            ZentCloud.BLLJIMP.Enums.ArticleCategoryType ntype = new ZentCloud.BLLJIMP.Enums.ArticleCategoryType();
            if (!Enum.TryParse(type, out ntype))
            {
                resp.Status = (int)APIErrCode.OperateFail;
                resp.Msg = "类型错误";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            ArticleCategory cate = new ArticleCategory();
            cate.AutoID = autoId;
            cate.CategoryType = type;
            cate.PreID = preId;
            cate.CategoryName = categoryName;
            cate.Summary = summary;
            cate.SysType = 0;
            cate.ImgSrc = imgSrc;
            cate.CreateTime = DateTime.Parse(createTime);
            cate.Sort = sort;
            cate.WebsiteOwner = bllArticleCategory.WebsiteOwner;
            if (bllArticleCategory.PutArticleCategory(cate))
            {
                resp.Status = 1;
                resp.Msg = "提交成功";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "提交失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 动态列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getStatusesArticleList(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request["page"]),
                rows = Convert.ToInt32(context.Request["rows"]);
            string type = "Statuses";
            string cateId = context.Request["cateId"];
            string keyword = context.Request["keyword"];

            if (page == 0) page = 1;
            if (rows == 0) rows = int.MaxValue;
            int totalCount = 0;
            var dataList = bll.GetJuActivityList(type, null, out totalCount, page, rows, null, currentUserInfo.UserID, cateId, bll.WebsiteOwner, keyword);
            var data = from p in dataList
                       select new {
                           id = p.JuActivityID,
                           type = p.CategoryName,
                           name = bllUser.GetUserDispalyName(p.UserID),
                           summary = p.Summary,
                           cmtNum=p.CommentCount
                       };

            return Common.JSONHelper.ObjectToJson(new
            {
                rows = data,
                total = totalCount
            });
        }
        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelStatusesArticle(HttpContext context)
        {
            string ids = context.Request["ids"];
            List<string> IdList = ids.Split(',').ToList();
            for (int i = 0; i < IdList.Count; i++)
            {
                bll.DelArticle(IdList[i]);
            }
            resp.Status = 1;
            resp.Msg = "删除完成";
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 课程基本设置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditOpenConfig(HttpContext context)
        {
            string creater = context.Request["Creater"];
            if (string.IsNullOrWhiteSpace(creater))
            {
                resp.Status = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.Msg = "创建人不能为空";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (bllUserExpand.ExistUserExpand(UserExpandType.UserOpenCreate, this.currentUserInfo.UserID))
            {
                bllUserExpand.UpdateUserExpand(UserExpandType.UserOpenCreate, this.currentUserInfo.UserID, creater);
            }
            else
            {
                bllUserExpand.AddUserExpand(UserExpandType.UserOpenCreate, this.currentUserInfo.UserID, creater);
            }
            resp.Status = 1;
            resp.Msg = "修改完成";
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