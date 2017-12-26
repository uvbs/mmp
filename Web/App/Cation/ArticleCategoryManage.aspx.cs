using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation
{
    public partial class ArticleCategoryManage : System.Web.UI.Page
    {
        public string CategoryType;
        public int cateRootId = 0;
        public int isNoPreSelect = 0;
        public int selectMaxDepth = 10000;
        public BLLJIMP.Model.UserInfo currUser = new BLLJIMP.Model.UserInfo();
        public string currShowName = "分类";
        public string hasKeyValue = "";
        public ArticleCategoryTypeConfig nCategoryTypeConfig;

        //权限
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        BLLJIMP.BLLArticleCategory bllArticleCategory = new BLLJIMP.BLLArticleCategory();
        BLLPermission.BLLMenuPermission bllMenupermission = new BLLPermission.BLLMenuPermission("");
        public bool isHide = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            currUser = DataLoadTool.GetCurrUserModel();
            cateRootId = Convert.ToInt32(Request["cateRootId"]);
            isNoPreSelect = Convert.ToInt32(Request["isNoPreSelect"]);
            selectMaxDepth = Convert.ToInt32(Request["selectMaxDepth"]);
            if (selectMaxDepth.Equals(0)) selectMaxDepth = int.MaxValue;
            isHide = bllMenupermission.CheckPerRelationByaccount(bllUser.GetCurrUserID(), -1);

            //是否传入了名称
            var inputShowName = Request["currShowName"];
            hasKeyValue = Request["hasKeyValue"];
            if (!string.IsNullOrWhiteSpace(inputShowName))
            {
                currShowName = inputShowName;
                nCategoryTypeConfig = new ArticleCategoryTypeConfig();
                return;
            }

            //检查配置取名称
            CategoryType = Request["type"];
            nCategoryTypeConfig = bllArticleCategory.GetArticleCategoryTypeConfig(bllArticleCategory.WebsiteOwner, CategoryType);
            if (nCategoryTypeConfig != null) { currShowName = nCategoryTypeConfig.CategoryTypeExDispalyName; }
            else { currShowName = "分类"; nCategoryTypeConfig = new ArticleCategoryTypeConfig(); }
        }
    }
}