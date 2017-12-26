using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.WanBang.Wap
{
    public partial class AttentionList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!DataLoadTool.CheckWanBangLogin())
            {
                Response.Redirect(string.Format("/App/WanBang/Wap/Login.aspx?redirecturl={0}", Request.FilePath));
            }
        }
    }
}