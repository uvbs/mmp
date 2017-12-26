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
    public class OpenHandler : IHttpHandler, IRequiresSessionState
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
                    case "getTypeList":
                        result = getTypeList(context);
                        break;
                    case "addCate":
                        result = addCate(context);
                        break;
                    case "getOpenList":
                        result = getOpenList(context);
                        break;
                    case "EditOpenClass":
                        result = EditOpenClass(context);
                        break;
                    case "DelOpenClass":
                        result = DelOpenClass(context);
                        break;
                    case "EditOpenConfig":
                        result = EditOpenConfig(context);
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
        /// 类型列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getTypeList(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request["page"]),
                rows = Convert.ToInt32(context.Request["rows"]);
            string type = "OpenClass";
            int preId = int.Parse(context.Request["preId"]);

            if (page == 0) page = 1;
            if (rows == 0) rows = int.MaxValue;
            int totalCount = 0;
            var dataList = bllArticleCategory.GetCateList(out totalCount, type, preId, bll.WebsiteOwner, rows, page);

            return Common.JSONHelper.ObjectToJson(new {
                rows = dataList,
                total = totalCount
            });
        }
        /// <summary>
        /// 添加/修改类型
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string addCate(HttpContext context)
        {
            int autoId = int.Parse(context.Request["autoId"]);
            string type = "OpenClass";
            int preId = int.Parse(context.Request["preId"]);
            string name = context.Request["name"];
            int sort = int.Parse(context.Request["sort"]);
            

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
            cate.CategoryName = name;
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
        /// 公开课列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string getOpenList(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request["page"]),
                rows = Convert.ToInt32(context.Request["rows"]);
            string type = "OpenClass";
            string cateId = context.Request["cateId"];
            string keyword = context.Request["keyword"];

            if (page == 0) page = 1;
            if (rows == 0) rows = int.MaxValue;
            int totalCount = 0;

            //DataTable sourceData = this.bll.GetArticleNewList(
            //        out totalCount,
            //        page,
            //        rows,
            //        null,
            //        keyword,
            //        currentUserInfo.UserID,
            //        type,
            //        bll.WebsiteOwner,
            //        cateId,
            //        null,
            //        null,
            //        null,
            //        null,
            //        null,
            //        null,
            //        false
            //    );

            List<JuActivityInfo> sourceData = this.bll.GetJuActivityList(type,null,out totalCount,page,rows,null,currentUserInfo.UserID,cateId,bll.WebsiteOwner,keyword,null);

            List<dynamic> data = new List<dynamic>();
            foreach (var item in sourceData)
            {
                data.Add(new
                {
                    id = item.JuActivityID,
                    title = item.ActivityName,
                    type=item.CategoryName,
                    pv = item.PV,
                    hide = item.IsHide,
                    integral = item.IsFee == 1 ? 0 : item.ActivityIntegral,
                    favNum = item.FavoriteCount,
                    buyNum = bllCommRelation.GetRelationCount(CommRelationType.ViewOpenClass, item.JuActivityID.ToString(), null),
                    cmtNum = item.CommentCount,
                    img = item.ThumbnailsPath.ToString()
                });
            }
         
            return Common.JSONHelper.ObjectToJson(new
            {
                rows = data,
                total = totalCount
            });
        }
        /// <summary>
        /// 添加/修改类型
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string EditOpenClass(HttpContext context)
        {
            if (!bllUserExpand.ExistUserExpand(UserExpandType.UserOpenCreate, this.currentUserInfo.UserID))
            {
                resp.Status = -1;
                resp.Msg = "公开课基本设置不完整";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            JuActivityInfo model = new JuActivityInfo();
            model.JuActivityID = int.Parse(context.Request["JuActivityID"]);
            model.ActivityName = context.Request["ActivityName"];
            model.ArticleType = "OpenClass";
            model.ActivityDescription = context.Request["ActivityDescription"];
            model.CategoryId = context.Request["CategoryId"];
            model.Summary = context.Request["Summary"];
            if (string.IsNullOrWhiteSpace(model.Summary)) {
                model.Summary = MySpider.MyRegex.RemoveHTMLTags(model.ActivityDescription);
                if (model.Summary.Length > 300) model.Summary = model.Summary.Substring(0, 300) + "...";
            }
            model.ThumbnailsPath = context.Request["ThumbnailsPath"];
            model.ActivityAddress = context.Request["ActivityAddress"];
            model.ActivityWebsite = context.Request["ActivityWebsite"];
            model.ActivityIntegral = int.Parse(context.Request["ActivityIntegral"]);
            model.Tags = context.Request["Tags"];
            model.PV = int.Parse(context.Request["PV"]);
            model.IsFee = int.Parse(context.Request["IsFee"]);
            model.IsHide = Convert.ToInt32(context.Request["IsHide"]);
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
        /// 删除课程
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DelOpenClass(HttpContext context)
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
            string Creater = context.Request["Creater"];
            if (string.IsNullOrWhiteSpace(Creater))
            {
                resp.Status = (int)APIErrCode.PrimaryKeyIncomplete;
                resp.Msg = "创建人不能为空";
                return Common.JSONHelper.ObjectToJson(resp);
            }
            if (bllUserExpand.ExistUserExpand(UserExpandType.UserOpenCreate, this.currentUserInfo.UserID))
            {
                bllUserExpand.UpdateUserExpand(UserExpandType.UserOpenCreate, this.currentUserInfo.UserID, Creater);
            }
            else
            {
                bllUserExpand.AddUserExpand(UserExpandType.UserOpenCreate, this.currentUserInfo.UserID, Creater);
            }

            string OpenClassNotice = context.Request["OpenClassNotice"];
            if (!string.IsNullOrWhiteSpace(OpenClassNotice))
            {
                BLLKeyValueData bllKeyValueData = new BLLKeyValueData();

                KeyVauleDataInfo value = new KeyVauleDataInfo();
                value.DataType = "OpenClassNotice";
                value.DataKey="0";
                value.DataValue=OpenClassNotice;
                value.Creater=this.currentUserInfo.UserID;
                value.WebsiteOwner=bll.WebsiteOwner;
                value.CreateTime=DateTime.Now;
                bllKeyValueData.PutDataValue(value);
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