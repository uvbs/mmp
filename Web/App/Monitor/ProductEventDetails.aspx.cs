using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Monitor
{
    public partial class ProductEventDetails : System.Web.UI.Page
    {
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        protected string pid = string.Empty;
        protected WXMallProductInfo productModel;
        protected string spreadUserID = string.Empty;
        protected string pv = string.Empty;
        protected string type = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            pid=Request["pid"];
            pv=Request["pv"];
            spreadUserID = Request["spreadUserID"];
            if (!string.IsNullOrEmpty(Request["type"])) type = Request["type"];
            productModel = bllMall.GetProduct(pid);
            if (productModel == null)
                return;
        }
    }
}