using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace ZentCloud.JubitIMP.Web.App.Forward.wap
{
    public partial class WXForwardWap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    BLLJIMP.Model.UserInfo CurrentUserInfo = DataLoadTool.GetCurrUserModel();
                    int id = Convert.ToInt32(Request["id"]);
                    Convert.ToString(id, 16);
                    Response.Redirect(string.Format("http://{0}/{1}/{2}/details.chtml", this.Request.Url.Host + ":" + this.Request.Url.Port, Convert.ToString(id, 16), Convert.ToString(CurrentUserInfo.AutoID, 16)));
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }
    }
}