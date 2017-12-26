using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{

    public partial class MyPub : System.Web.UI.Page
    {
        BLLJIMP.BLLJuActivity bll = new BLLJIMP.BLLJuActivity();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["oid"]))
            {
                var currentUserInfo=bll.Get<ZentCloud.BLLJIMP.Model.UserInfo>(string.Format("WXOpenId='{0}'",Request["oid"]));
                if (currentUserInfo!=null)
                {
                    Session[SessionKey.UserID] = currentUserInfo.UserID;
                    Session[SessionKey.LoginStatu] = 1;
                    Session[SessionKey.UserType] = currentUserInfo.UserType;

                }
            }
            Response.Write(bll.GetTemplateSource(bll.WebsiteOwner,"mypub"));
        }
    }
} 