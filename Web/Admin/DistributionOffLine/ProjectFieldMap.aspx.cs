using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Admin.DistributionOffLine
{
    public partial class ProjectFieldMap : System.Web.UI.Page
    {
        protected string moduleName = "商机";
        protected string moduleType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["moduleType"]))
            {
                moduleType = Request["moduleType"];
            }
            if (!string.IsNullOrEmpty(Request["moduleName"]))
            {
                moduleName = Request["moduleName"];
            }
        }
    }
}