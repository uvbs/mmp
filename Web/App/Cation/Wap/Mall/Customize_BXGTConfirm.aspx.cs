using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class Customize_BXGTConfirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!new ZentCloud.BLLJIMP.BLL("").IsLogin)
            {
                Response.End();
            }
        }
    }
}