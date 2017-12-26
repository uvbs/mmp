using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Home
{
    public partial class logout1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    string tmp = Request.UrlReferrer.ToString().ToLower();
            //    if (tmp.StartsWith("/main.aspx"))
            //    {
            //        string op = Request["op"];
            //        if (op.Equals("logout"))
            //        {
            //            if (this.Session != null)
            //                if (this.Session["login"] != null)
            //                    this.Session["login"] = 0;
            //        }
            //    }
            //}
            //catch { }

            string op = Request["op"];
            if (!string.IsNullOrWhiteSpace(op))
                if (op.Equals("logout"))
                {
                    if (this.Session != null)
                        if (this.Session["login"] != null)
                            this.Session["login"] = 0;
                }
            //清除session
            if (Session["WebsiteInfoModel"]!=null)
                Session.Remove("WebsiteInfoModel");
            bool isPhone = false;

            isPhone = !Request.Browser.Platform.ToLower().StartsWith("win");

            //判断如果是手机就返回到手机登录页面
            //if (isPhone)
            //    Response.Write(string.Format("<script type='text/javascript'> top.location = '{0}';</script>", Common.ConfigHelper.GetConfigString("wapLoginUrl")));
            //else
                //Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "logoutDirect", "<script type='text/javascript'> top.location = '/Home/Index.aspx';</script>");
                Response.Write(string.Format("<script type='text/javascript'> top.location = '{0}';</script>", Common.ConfigHelper.GetConfigString("loginUrl")));

            return;
        }
    }
}