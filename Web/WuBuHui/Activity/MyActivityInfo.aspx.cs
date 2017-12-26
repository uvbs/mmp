using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.WuBuHui.Activity
{
    public partial class MyActivityInfo : UserPage
    {
        BLLJuActivity juActivityBll = new BLLJuActivity();
        public string WebsiteOwner = null;
        public System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        public string DoMain = "";
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        public string websiteOwner; public BLLJIMP.Model.ActivityConfig aconfig;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                WebsiteOwner = DataLoadTool.GetWebsiteInfoModel().WebsiteOwner;
                aconfig = juActivityBll.Get<BLLJIMP.Model.ActivityConfig>(string.Format(" WebsiteOwner='{0}'", WebsiteOwner));
                if (aconfig == null)
                {
                    aconfig = new BLLJIMP.Model.ActivityConfig() { TheOrganizers = "" };
                }
                sbCategory.Append(" <li class=\"catli current\"><a>全部</a></li>");
                foreach (var item in juActivityBll.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='activity'", WebsiteOwner)))
                {
                    sbCategory.AppendFormat(" <li class=\"catli\" v=\"{0}\"><a>{1}</a></li>", item.AutoID, item.CategoryName);
                }
            }
        }
    }
}