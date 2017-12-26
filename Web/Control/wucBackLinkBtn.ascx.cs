using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Control
{
    public partial class wucBackLinkBtn : System.Web.UI.UserControl
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
                this.lbtnBack.Visible = false;
        }

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(this.ViewState["ref"].ToString());
        }
    }
}