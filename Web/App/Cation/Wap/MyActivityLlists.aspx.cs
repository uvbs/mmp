using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
    public partial class MyActivityLlists1 : System.Web.UI.Page
    {
        /// <summary>
        /// 分类
        /// </summary>
        public System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        /// <summary>
        /// 配置
        /// </summary>
        public BLLJIMP.Model.ActivityConfig aconfig = new BLLJIMP.Model.ActivityConfig();
        /// <summary>
        /// 底部导航
        /// </summary>
        public List<CompanyWebsite_ToolBar> ToolBarGroup = new List<CompanyWebsite_ToolBar>();
        /// <summary>
        /// 列宽
        /// </summary>
        public double ColumnWidth = 33.3;
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        /// <summary>
        /// 
        /// </summary>
        BLLJuActivity bllJuactivity = new BLLJuActivity();
        protected void Page_Load(object sender, EventArgs e)
        {
           
                aconfig = bllJuactivity.Get<BLLJIMP.Model.ActivityConfig>(string.Format(" WebsiteOwner='{0}'", bllUser.WebsiteOwner));
                if (aconfig == null)
                {
                    aconfig = new BLLJIMP.Model.ActivityConfig() { TheOrganizers = "" };
                }
                sbCategory.Append(" <li class=\"list current\"><a>全部</a></li>");
                foreach (var item in bllJuactivity.GetList<ArticleCategory>(string.Format("WebsiteOwner='{0}' And CategoryType='activity'", bllUser.WebsiteOwner)))
                {
                    sbCategory.AppendFormat(" <li class=\"list\" v=\"{0}\"><a>{1}</a></li>", item.AutoID, item.CategoryName);
                }
                if (!string.IsNullOrEmpty(aconfig.ToolBarGroups))
                {
                    ToolBarGroup = bllUser.GetList<CompanyWebsite_ToolBar>(string.Format(" WebsiteOwner='{0}' And KeyType='{1}'", bllUser.WebsiteOwner, aconfig.ToolBarGroups));
                    if (ToolBarGroup.Count > 0)
                    {
                        ColumnWidth = 100 / ToolBarGroup.Count;
                    }
                }
            
        }
    }
}