using CommonPlatform.Helper;
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
    public class AskHandler : IHttpHandler, IRequiresSessionState
    {
        AshxResponse resp = new AshxResponse();
        BLLCommRelation bllCommRelation = new BLLCommRelation();
        BLLUser bllUser = new BLLUser();
        BLLJuActivity bll = new BLLJuActivity();
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        BLLKeyValueData bllKeyValueData = new BLLKeyValueData();
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
                    case "getAskList":
                        result = getAskList(context);
                        break;
                    case "EditAsk":
                        result = EditAsk(context);
                        break;
                    case "DelAsk":
                        result = DelAsk(context);
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
        /// 新闻资讯列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getAskList(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request["page"]),
                rows = Convert.ToInt32(context.Request["rows"]);
            string cateId = context.Request["cateId"];
            string keyword = context.Request["keyword"];
            if (string.IsNullOrWhiteSpace(cateId)) cateId = "81,82";
            if (page == 0) page = 1;
            if (rows == 0) rows = int.MaxValue;
            int totalCount = 0;
            var dataList = bll.GetJuActivityList(null, null, out totalCount, page, rows, null, currentUserInfo.UserID, cateId, bll.WebsiteOwner, keyword);
            var data = from p in dataList
                       select new {
                           id=p.JuActivityID,
                           title = p.ActivityName,
                           type = p.CategoryName,
                           pv=p.PV,
                           hide = p.IsHide,
                           province = (p.ProvinceCode == "0" || string.IsNullOrWhiteSpace(p.ProvinceCode)) ? "全国" : bllKeyValueData.GetDataDefVaule("Province", p.ProvinceCode),
                           favNum = bllCommRelation.GetRelationCount(CommRelationType.JuActivityFavorite, p.JuActivityID.ToString(), null),
                           cmtNum = p.CommentCount
                       };

            return Common.JSONHelper.ObjectToJson(new
            {
                rows = data,
                total = totalCount
            });
        }
        /// <summary>
        /// 添加/修改
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditAsk(HttpContext context)
        {
            
            JuActivityInfo model = new JuActivityInfo();
            model.JuActivityID = int.Parse(context.Request["JuActivityID"]);
            model.ActivityName = context.Request["ActivityName"];
            model.ArticleType = "Article";
            model.ActivityDescription = context.Request["ActivityDescription"];
            model.CategoryId = context.Request["CategoryId"];
            if (model.CategoryId == "81") model.ArticleType = "Question";
            model.Summary = context.Request["Summary"];
            if (string.IsNullOrWhiteSpace(model.Summary)) {
                model.Summary = MySpider.MyRegex.RemoveHTMLTags(model.ActivityDescription);
                if (model.Summary.Length > 300) model.Summary = model.Summary.Substring(0, 300) + "...";
            }
            model.Tags = context.Request["Tags"];
            model.PV = int.Parse(context.Request["PV"]);
            model.IsHide = Convert.ToInt32(context.Request["IsHide"]);
            model.ProvinceCode = context.Request["ProvinceCode"];
            model.WebsiteOwner = bll.WebsiteOwner;
            model.CreateDate = DateTime.Now;
            model.UserID = currentUserInfo.UserID;
            if (bll.PutArticle(model))
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
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelAsk(HttpContext context)
        {
            string ids = context.Request["ids"];
            List<string> IdList = ids.Split(',').ToList();
            for (int i = 0; i < IdList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(IdList[i])) continue;
                if (bll.DelArticle(IdList[i]))
                {
                    JuActivityInfo act = bll.GetJuActivity(int.Parse(IdList[i]));
                    bllUser.AddUserScoreDetail(act.UserID, EnumStringHelper.ToString(ScoreDefineType.DelQuestions), bll.WebsiteOwner, null, null);
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