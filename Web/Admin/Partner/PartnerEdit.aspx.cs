using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Partner
{
    public partial class PartnerEdit : System.Web.UI.Page
    {
        protected JuActivityInfo model = new JuActivityInfo();
        BLLJuActivity bllJuActivity = new BLLJuActivity();
        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request["id"]);
            if (id == 0)
            {
                model.JuActivityID = 0;
            }
            else
            {
                model = bllJuActivity.GetJuActivity(id);
            }
        }
    }
}