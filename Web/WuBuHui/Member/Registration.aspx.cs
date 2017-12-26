using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.WuBuHui.Member
{
    public partial class Registration : System.Web.UI.Page
    {
        public string Uid = "";
        public BLLJIMP.Model.UserInfo uinfo;
        public string Url = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Uid = Request["UserId"];
                Url = Request["from"];
                uinfo = DataLoadTool.GetCurrUserModel();
                if (!string.IsNullOrEmpty(uinfo.TrueName) && !string.IsNullOrEmpty(uinfo.Phone) && !string.IsNullOrEmpty(uinfo.Email) && !string.IsNullOrEmpty(uinfo.Company))
                {
                    Response.Redirect("/WuBuHui/MyCenter/MyCenter.aspx");
                }
            }
        }
    }
}