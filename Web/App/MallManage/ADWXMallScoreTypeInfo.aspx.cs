using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.MallManage
{
    public partial class ADWXMallScoreTypeInfo : System.Web.UI.Page
    {

        public string AutoId = "";
        public string currAction = "add";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AutoId = Request["AutoId"];
                if (!string.IsNullOrEmpty(AutoId))
                {
                    currAction = "edit";
                }

            }
        }
    }
}