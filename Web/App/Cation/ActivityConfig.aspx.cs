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
    public partial class ActivityConfig : System.Web.UI.Page
    {
        public BLLJIMP.Model.ActivityConfig activityConfig;
        public List<String> Groups;
        BLLCompanyWebSite bll = new BLLCompanyWebSite();
        protected void Page_Load(object sender, EventArgs e)
        {
            //txtActivities.Value = Request.Url.Host + "/App/Cation/Wap/ActivityLlists.aspx";
            //txtMyRegistration.Value = Request.Url.Host + "/App/Cation/Wap/MyActivityLlists.aspx";
            List<CompanyWebsite_ToolBar> dataList = bll.GetColList<CompanyWebsite_ToolBar>(int.MaxValue, 1, string.Format(" WebsiteOwner = '{0}'", bll.WebsiteOwner), "AutoID,KeyType,BaseID");
            Groups = dataList.OrderBy(p => p.KeyType).Select(p => p.KeyType).Distinct().ToList();
            activityConfig = bll.Get<BLLJIMP.Model.ActivityConfig>(string.Format(" WebsiteOwner='{0}'",bll.WebsiteOwner));
            if (activityConfig==null)
            {
                activityConfig = new BLLJIMP.Model.ActivityConfig();
            }


        }
    }
}