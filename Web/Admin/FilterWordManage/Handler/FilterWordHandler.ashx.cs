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
    /// FilterWordHandler 的摘要说明
    /// </summary>
    public class FilterWordHandler : IHttpHandler, IRequiresSessionState, IReadOnlySessionState
    {
        AshxResponse resp = new AshxResponse();
        BLLUser bllUser = new BLLUser();
        UserInfo currentUserInfo;
        BLLFilterWord bllFilterWord = new BLLFilterWord();

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
                    case "QueryFilterWord":
                        result = QueryFilterWord(context);
                        break;
                    case "AddFilterWord":
                        result = AddFilterWord(context);
                        break;
                    case "UpdateFilterWord":
                        result = UpdateFilterWord(context);
                        break;
                    case "DeleteFilterWord":
                        result = DeleteFilterWord(context);
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

        #region 敏感词管理
        /// <summary>
        /// 查询敏感词管理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryFilterWord(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string filterWord = context.Request["FilterWord"], FilterType = context.Request["FilterType"];
            int totalCount = 0;
            List<FilterWord> dataList = bllFilterWord.GetFilterWordList(pageSize, pageIndex, FilterType, filterWord, bllFilterWord.WebsiteOwner, out totalCount);
            return Common.JSONHelper.ObjectToJson(
new
{
    total = totalCount,
    rows = dataList
});
        }

        /// <summary>
        /// 添加敏感词
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddFilterWord(HttpContext context)
        {
            string filterWord = context.Request["FilterWord"],
                   filterType = context.Request["FilterType"];

            if (string.IsNullOrEmpty(filterWord))
            {
                resp.Status = 0;
                resp.Msg = "请输入敏感词";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (string.IsNullOrEmpty(filterType))
            {
                resp.Status = 0;
                resp.Msg = "类型错误";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            FilterWord word = new FilterWord();
            word.FilterType = int.Parse(filterType);
            word.WebsiteOwner = bllFilterWord.WebsiteOwner;
            word.Word = filterWord;
            word.AutoID = 0;
            if (bllFilterWord.ExistsFilterWord(filterType, filterWord, bllFilterWord.WebsiteOwner))
            {
                resp.Status = 0;
                resp.Msg = "敏感词重复不能添加";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (bllFilterWord.PutFilterWord(word))
            {
                resp.Status = 1;
                resp.Msg = "添加成功";

            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);


        }
        /// <summary>
        /// 修改敏感词
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateFilterWord(HttpContext context)
        {
            string autoId = context.Request["AutoID"];
            string filterWord = context.Request["FilterWord"];
            FilterWord word = bllFilterWord.GetFilterWord(int.Parse(autoId));

            if (bllFilterWord.ExistsFilterWord(word.FilterType.ToString(), filterWord, word.WebsiteOwner, word.AutoID.ToString()))
            {
                resp.Status = 0;
                resp.Msg = "敏感词重复";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            word.Word = filterWord;
            if (bllFilterWord.PutFilterWord(word))
            {
                resp.Status = 1;
                resp.Msg = "修改成功";
            }
            else
            {
                resp.Msg = "修改失败";
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 删除敏感词
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteFilterWord(HttpContext context)
        {
            string ids = context.Request["ids"];
            if (string.IsNullOrWhiteSpace(ids)) {
                resp.Status = 0;
                resp.Msg = "请选择敏感词";
                return Common.JSONHelper.ObjectToJson(resp);
            }

            if (bllFilterWord.DeleteFilterWords(ids) > 0)
            {
                resp.Status = 1;
            }
            return Common.JSONHelper.ObjectToJson(resp);
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}