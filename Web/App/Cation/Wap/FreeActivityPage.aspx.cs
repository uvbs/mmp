using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
    public partial class FreeActivityPage : System.Web.UI.Page
    {
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        protected BLLJIMP.Model.JuActivityInfo model = new BLLJIMP.Model.JuActivityInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            model = bllJuActivity.GetJuActivity(int.Parse(Request["aid"]));
        }
    }
}