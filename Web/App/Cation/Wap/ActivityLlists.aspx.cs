using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using System.Text;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
    public partial class ActivityLlists : System.Web.UI.Page
    {

        /// <summary>
        /// 分类
        /// </summary>
        public System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        /// <summary>
        /// 活动配置
        /// </summary>
        public BLLJIMP.Model.ActivityConfig aconfig;
        /// <summary>
        /// 根分类
        /// </summary>
        public int cateRootId = 0;
        /// <summary>
        /// 底部导航
        /// </summary>
        public List<CompanyWebsite_ToolBar> ToolBarGroup=new List<CompanyWebsite_ToolBar>();
        /// <summary>
        /// 列宽
        /// </summary>
        public double ColumnWidth=33.3;
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJuActivity bllJuactivity = new BLLJuActivity();
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        protected void Page_Load(object sender, EventArgs e)
        {
            cateRootId = Convert.ToInt32(Request["cateRootId"]);

                aconfig = bllJuactivity.Get<BLLJIMP.Model.ActivityConfig>(string.Format(" WebsiteOwner='{0}'", bllJuactivity.WebsiteOwner));
                if (aconfig == null)
                {
                    aconfig = new BLLJIMP.Model.ActivityConfig() { TheOrganizers = "",ShowName="活动" };
                }
                sbCategory.Append(" <li class=\"list current\"><a>全部</a></li>");

                StringBuilder strCateWhere = new StringBuilder();
                strCateWhere.AppendFormat("WebsiteOwner='{0}' And CategoryType='activity'", bllJuactivity.WebsiteOwner);
                if (cateRootId > 0)
                {
                    strCateWhere.AppendFormat(" AND PreID = {0} ", cateRootId);
                }

                foreach (var item in bllJuactivity.GetList<ArticleCategory>(strCateWhere.ToString()))
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