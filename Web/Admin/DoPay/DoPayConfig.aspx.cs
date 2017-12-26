using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.DoPay
{
    public partial class DoPayConfig : System.Web.UI.Page
    {
        protected PayConfig payConfig;
        protected void Page_Load(object sender, EventArgs e)
        {
            BllPay bllPay = new BllPay();
            payConfig = bllPay.GetPayConfig();
        }
    }
}