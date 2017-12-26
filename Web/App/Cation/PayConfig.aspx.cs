using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation
{
    public partial class PayConfig : System.Web.UI.Page
    {
        BLLJIMP.BllPay bll = new BLLJIMP.BllPay();
        public ZentCloud.BLLJIMP.Model.PayConfig model;
        protected void Page_Load(object sender, EventArgs e)
        {
            model = bll.GetPayConfig();
            if (model==null)
            {
                model = new BLLJIMP.Model.PayConfig();
            }


        }
    }
}