using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.App.Cation
{
    public partial class ArticleCompile : System.Web.UI.Page
    {
        ///// <summary>
        /////id
        ///// </summary>
        //public int aid = 0;
        /// <summary>
        /// add edit
        /// </summary>
        public string webAction = "add";
        /// <summary>
        /// 当前操作 添加或编辑
        /// </summary>
        public string actionStr = "";
        /// <summary>
        /// 字段映射
        /// </summary>
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJuActivity bllJuactivity = new BLLJuActivity(DataLoadTool.GetCurrUserID());
        /// <summary>
        /// 当前文章
        /// </summary>
        public JuActivityInfo model = new JuActivityInfo();
        //public bool Pms_Advanced;
        /// <summary>
        /// 分类
        /// </summary>
        public System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        /// <summary>
        /// 根分类ID
        /// </summary>
        public int cateRootId = 0;
        /// <summary>
        /// 
        /// </summary>
        public int isHideTag = 0;
        public int isHideCate = 0;
        public int isHideLevel = 0;
        public int isHideWeixin = 0;
        public int isHideArea = 0;
        public int isHideTemplate = 0;
        public int isHideRelationArticle = 0;
        public int isHideUpSort = 0;
        public int nickReplaceId = 0;
        public int summaryReplaceTitle = 0;
        public int isHideSummary = 0;
        public int isHideImg = 0;
        public int isHideFile = 0;
        public int isHideUrl = 0;
        public int isShowPraise = 0;
        public int isShowFavorite = 0;
        public int isShowReward = 0;
        public int isHideDelete = 0;
        public int isHideAdd = 0;
        /// <summary>
        /// 
        /// </summary>
        public string moduleName = "文章";
        public string type = "Article";
        public string WebsiteOwner = "";
        /// <summary>
        /// 
        /// </summary>
        public List<TableFieldMapping> tableFieldList = new List<TableFieldMapping>();
        /// <summary>
        /// 相关文章
        /// </summary>
        public List<JuActivityInfo> RelationArticle = new List<JuActivityInfo>();
        /// <summary>
        /// 附件
        /// </summary>
        protected List<JuActivityFiles> nFiles = new List<JuActivityFiles>();
        protected void Page_Load(object sender, EventArgs e)
        {
            int juActivityId = Convert.ToInt32(Request["aid"]);
            cateRootId = Convert.ToInt32(Request["cateRootId"]);
            isHideTag = Convert.ToInt32(Request["isHideTag"]);
            isHideCate = Convert.ToInt32(Request["isHideCate"]);
            isHideLevel = Convert.ToInt32(Request["isHideLevel"]);
            isHideWeixin = Convert.ToInt32(Request["isHideWeixin"]);
            isHideArea = Convert.ToInt32(Request["isHideArea"]);
            isHideTemplate = Convert.ToInt32(Request["isHideTemplate"]);
            isHideRelationArticle = Convert.ToInt32(Request["isHideRelationArticle"]);
            isHideUpSort = Convert.ToInt32(Request["isHideUpSort"]);
            nickReplaceId = Convert.ToInt32(Request["nickReplaceId"]);
            summaryReplaceTitle = Convert.ToInt32(Request["summaryReplaceTitle"]);
            isHideSummary = Convert.ToInt32(Request["isHideSummary"]);
            isHideImg = Convert.ToInt32(Request["isHideImg"]);
            isHideFile = Convert.ToInt32(Request["isHideFile"]);
            isHideUrl = Convert.ToInt32(Request["isHideUrl"]);
            isShowPraise = Convert.ToInt32(Request["isShowPraise"]);
            isShowFavorite = Convert.ToInt32(Request["isShowFavorite"]);
            isShowReward = Convert.ToInt32(Request["isShowReward"]);
            isHideDelete = Convert.ToInt32(Request["isHideDelete"]);
            isHideAdd = Convert.ToInt32(Request["isHideAdd"]);

            var moduleNameReq = Request["moduleName"];
            var typeReq = Request["type"];
            webAction = Request["Action"];
            if (!string.IsNullOrWhiteSpace(moduleNameReq))
            {
                moduleName = moduleNameReq;
            }
            if (!string.IsNullOrWhiteSpace(typeReq))
            {
                type = typeReq;
            }
            actionStr = webAction == "add" ? "添加" : "编辑";
            if (webAction == "edit")
            {
                model = this.bllJuactivity.GetJuActivity(juActivityId);

                if (model == null)
                {
                    Response.End();
                }
                else
                {
                    nFiles = bllJuactivity.GetColMultListByKey<JuActivityFiles>(int.MaxValue, 1, "JuActivityID", model.JuActivityID.ToString(), "AutoId,FileName,FilePath,FileClass");
                }
            }
            tableFieldList = bllTableFieldMap.GetTableFieldMap(bllTableFieldMap.WebsiteOwner, "JuActivityInfo", cateRootId.ToString(),null,false,"0",null,type);

            //Pms_Advanced = DataLoadTool.CheckCurrUserPms(ZentCloud.BLLPermission.PermissionKey.Pms_JuActivity_Advanced);

            //foreach (var item in juActivityBll.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='article'", juActivityBll.WebsiteOwner)))
            //{
            sbCategory.Append(new MySpider.MyCategories().GetSelectOptionHtml(bllJuactivity.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='{1}'", bllJuactivity.WebsiteOwner, type)), "AutoID", "PreID", "CategoryName", cateRootId, "ddlcategory", "width:200px", ""));

            // sbCategory.AppendFormat("<option value=\"{0}\">{1}</option>",item.AutoID,item.CategoryName)

            // }

            if (!string.IsNullOrEmpty(model.RelationArticles))
            {
                string ids = "'" + model.RelationArticles.Replace(",", "','") + "'";
                RelationArticle = bllJuactivity.GetList<JuActivityInfo>(string.Format(" JuActivityID in ({0}) And IsHide=0  And ArticleType in('article','activity')", ids));
            }
            WebsiteOwner = bllJuactivity.WebsiteOwner;
        }
    }
}