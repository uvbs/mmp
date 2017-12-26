using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class WxMallScoreBelowLine : System.Web.UI.Page
    {
        public int TypeId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TypeId = Convert.ToInt32(Request["TypeId"]);
            }
        }
    }
}