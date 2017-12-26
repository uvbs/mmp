using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.WuBuHui
{
    public class UserPage : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
           
           BLLJIMP.Model.UserInfo uinfo= DataLoadTool.GetCurrUserModel();
            if (string.IsNullOrEmpty(uinfo.TrueName) && string.IsNullOrEmpty(uinfo.Phone) && string.IsNullOrEmpty(uinfo.Email))
            {
                Response.Redirect("/WuBuHui/Member/Registration.aspx?from=" + Request.RawUrl);
            }
            base.OnInit(e);
        }
    }
}