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
    public partial class ActivityManage : System.Web.UI.Page
    {
        BLLJuActivity juActivityBll = new BLLJuActivity();
        public string WebsiteOwner = null;
        public System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        public System.Text.StringBuilder sbCategory1 = new System.Text.StringBuilder();
        //public string DoMain = "";
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        BLLPermission.BLLMenuPermission bllMenupermission = new BLLMenuPermission("");
       
        public bool isHide = false;
        BLLJIMP.BLLWebSite bllWeisite = new BLLWebSite();
         //微信绑定域名
        public string strDomain = string.Empty;
        //是否显示pv列
        public bool IsShowActivityPv = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            //WebsiteOwner = DataLoadTool.GetWebsiteInfoModel().WebsiteOwner;
            //foreach (var item in juActivityBll.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='activity'", WebsiteOwner)))
            //{
            //    sbCategory.AppendFormat("<option value=\"{0}\">{1}</option>", item.AutoID, item.CategoryName);

            //}


            //DoMain = Request.Url.Host;
            //if (!bllUser.GetUserInfo(bllUser.WebsiteOwner).WeixinIsAdvancedAuthenticate.Equals(1))
            //{
            //    DoMain = "xixinxian.comeoncloud.net";
            //}
            WebsiteInfo model = bllWeisite.GetWebsiteInfo();
            if (model != null && !string.IsNullOrEmpty(model.WeiXinBindDomain))
            {
                strDomain = model.WeiXinBindDomain;
            }

            sbCategory.Append(new MySpider.MyCategories().GetSelectOptionHtml(juActivityBll.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='{1}'", juActivityBll.WebsiteOwner, "activity")), "AutoID", "PreID", "CategoryName", 0, "ddlcategory", "width:200px", "全部"));


            sbCategory1.Append(new MySpider.MyCategories().GetSelectOptionHtml(juActivityBll.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='{1}'", juActivityBll.WebsiteOwner, "activity")), "AutoID", "PreID", "CategoryName", 0, "ddlsetcategory", "width:200px", ""));




            isHide = bllMenupermission.CheckPerRelationByaccount(bllUser.GetCurrUserID(), -1);
            IsShowActivityPv = bllMenupermission.CheckUserAndPmsKey(bllUser.GetCurrUserID(), BLLPermission.Enums.PermissionSysKey.IsShowActivityPv);

        }
    }
}