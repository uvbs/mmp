using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.WanBang.Wap
{
    public partial class Index : System.Web.UI.Page
    {
        ///// <summary>
        ///// 基地数量
        ///// </summary>
        //public int BaseCount;
        ///// <summary>
        ///// 企业数量
        ///// </summary>
        //public int CompanyCount;
        ///// <summary>
        ///// 项目数量
        ///// </summary>
        public int ProjectCount;
        /// <summary>
        ///对接项目数量
        /// </summary>
        public int JointProjectCount;
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        protected void Page_Load(object sender, EventArgs e)
        {
           // BaseCount = bll.GetCount<WBBaseInfo>(string.Format("WebSiteOwner='{0}'",bll.WebsiteOwner));
           // CompanyCount = bll.GetCount<WBCompanyInfo>(string.Format("WebSiteOwner='{0}'", bll.WebsiteOwner));
            ProjectCount = bll.GetCount<WBProjectInfo>(string.Format("WebSiteOwner='{0}'", bll.WebsiteOwner));
            JointProjectCount = bll.GetCount<WBJointProjectInfo>(string.Format("WebSiteOwner='{0}'", bll.WebsiteOwner));
        }
    }
}