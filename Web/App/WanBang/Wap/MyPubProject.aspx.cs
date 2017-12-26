using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.WanBang.Wap
{
    public partial class MyPubProject : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!DataLoadTool.CheckWanBangLogin())
            {
                Response.Redirect(string.Format("/App/WanBang/Wap/Login.aspx?redirecturl={0}", Request.Url.PathAndQuery));
            }
            if (HttpContext.Current.Session[SessionKey.WanBangUserType].ToString().Equals("0"))
            {
                Response.Write("<script>alert('只有企业用户可以访问');window.location.href='Index.aspx';</script>");
                Response.End();//只有企业能访问
            }


        }
    }
}