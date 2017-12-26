using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLPermission;
namespace ZentCloud.JubitIMP.Web.App.Cation
{
    public partial class ArticleManage : System.Web.UI.Page
    {
        BLLJuActivity juActivityBll = new BLLJuActivity();
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        public string WebsiteOwner = null;
        public System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        public System.Text.StringBuilder sbCategory1 = new System.Text.StringBuilder();
        //public string DoMain = "";
        public string TutorStr;

        public int cateRootId = 0;
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
        public int isHideShareMonitorId = 0;
        public int isHideCommentCount = 0;

        public string moduleName = "文章";
        public string type = "Article";
        public bool isHide = false;
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        public List<TableFieldMapping> tableFieldList = new List<TableFieldMapping>();
        BLLPermission.BLLMenuPermission bllMenupermission = new BLLMenuPermission("");
        BLLJIMP.BLLWebSite bllWeisite = new BLLWebSite();
        /// <summary>
        /// 是否显示pv列
        /// </summary>
        public bool isArticlePv = false;
        //微信绑定域名
        public string strDomain = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
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
            isHideShareMonitorId = Convert.ToInt32(Request["isHideShareMonitorId"]);
            isHideCommentCount = Convert.ToInt32(Request["isHideCommentCount"]);
            var moduleNameReq = Request["moduleName"];
            var typeReq = Request["type"];
            
            WebsiteInfo model = bllWeisite.GetWebsiteInfo();
            if (model != null && !string.IsNullOrEmpty(model.WeiXinBindDomain))
            {
                strDomain = model.WeiXinBindDomain;
            }
            if (!string.IsNullOrWhiteSpace(moduleNameReq))
            {
                moduleName = moduleNameReq;
            }
            if (!string.IsNullOrWhiteSpace(typeReq))
            {
                type = typeReq;
            }
            tableFieldList = bllTableFieldMap.GetTableFieldMap(bllTableFieldMap.WebsiteOwner, "JuActivityInfo", cateRootId.ToString(), null, false, "0", null, type);
            //foreach (var item in juActivityBll.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='article'", WebsiteOwner)))
            //{
            //    sbCategory.AppendFormat("<option value=\"{0}\">{1}</option>", item.AutoID, item.CategoryName);

            //}
            //GetTutor();
            sbCategory.Append(new MySpider.MyCategories().GetSelectOptionHtml(juActivityBll.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='{1}'", juActivityBll.WebsiteOwner, type)), "AutoID", "PreID", "CategoryName", cateRootId, "ddlcategory", "width:200px", "全部"));
            sbCategory1.Append(new MySpider.MyCategories().GetSelectOptionHtml(juActivityBll.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='{1}'", juActivityBll.WebsiteOwner, type)), "AutoID", "PreID", "CategoryName", cateRootId, "ddlsetcategory", "width:200px", ""));

            isHide = bllMenupermission.CheckPerRelationByaccount(bllUser.GetCurrUserID(), -1);

            isArticlePv = bllMenupermission.CheckUserAndPmsKey(bllUser.GetCurrUserID(),BLLPermission.Enums.PermissionSysKey.IsShowArticlePv);
            WebsiteOwner = bllMenupermission.WebsiteOwner;

        }

        //private void GetTutor()
        //{
        //    List<BLLJIMP.Model.TutorInfo> tInfos = juActivityBll.GetList<BLLJIMP.Model.TutorInfo>(string.Format(" websiteOwner='{0}'", bllUser.WebsiteOwner));

        //    if (tInfos != null)
        //    {
        //        foreach (BLLJIMP.Model.TutorInfo item in tInfos)
        //        {
        //            TutorStr += "<option value=\"" + item.UserId + "\">" + item.TutorName + "</option>";
        //        }
        //    }


        //}
    }
}