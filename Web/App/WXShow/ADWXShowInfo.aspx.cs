using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.WXShow
{
    public partial class ADWXShowInfo_ : System.Web.UI.Page
    {
        ///App/WXShow/ADWXShowInfo.aspx
        public string currAction = "add";
        public string AutoId = "0";
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