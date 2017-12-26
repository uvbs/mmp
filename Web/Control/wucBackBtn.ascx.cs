using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Control
{
    public partial class wucBackBtn : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    this.ViewState["ref"] = this.Request.UrlReferrer.ToString();
                }
                catch { }
            }

            if (this.ViewState["ref"] == null)
                this.btnBack.Visible = false;
            if((this.ViewState["ref"].ToString().EndsWith("/Main.aspx",StringComparison.OrdinalIgnoreCase)))
                this.btnBack.Visible = false;

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(this.ViewState["ref"].ToString());
        }
    }
}