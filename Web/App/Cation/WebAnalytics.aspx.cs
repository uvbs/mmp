using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation
{
    public partial class WebAnalytics : System.Web.UI.Page
    {
        public BLLJIMP.Model.AnalyticsViewModel alyModel = new BLLJIMP.Model.AnalyticsViewModel();
        public BLLJIMP.BLL bll;

        protected void Page_Load(object sender, EventArgs e)
        {
            bll = new BLLJIMP.BLL();

            //alyModel = bll.GetAnalyticsViewModel();
        }
    }
}