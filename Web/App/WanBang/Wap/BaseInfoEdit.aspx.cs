using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.WanBang.Wap
{
    public partial class BaseInfoEdit : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public WBBaseInfo model = new WBBaseInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!DataLoadTool.CheckWanBangLogin())
            {
                Response.Redirect(string.Format("/App/WanBang/Wap/Login.aspx?redirecturl={0}", Request.Url.PathAndQuery));
            }
            if (HttpContext.Current.Session[SessionKey.WanBangUserType].ToString().Equals("1"))
            {
                Response.Write("<script>alert('只有基地用户可以访问');window.location.href='Index.aspx';</script>");
                Response.End();//只有基地能访问
            }
            model = bll.Get<WBBaseInfo>(string.Format("UserId='{0}'", HttpContext.Current.Session[SessionKey.WanBangUserID].ToString()));


        }
    }
}