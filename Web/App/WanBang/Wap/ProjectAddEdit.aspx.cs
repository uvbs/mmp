using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.App.WanBang.Wap
{
    public partial class ProjectAddEdit : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public WBProjectInfo model = new WBProjectInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!DataLoadTool.CheckWanBangLogin())
            {
                Response.Redirect(string.Format("/App/WanBang/Wap/Login.aspx?redirecturl={0}",Request.Url.PathAndQuery));
            }
            if (HttpContext.Current.Session[SessionKey.WanBangUserType].ToString().Equals("0"))
            {
                Response.Write("<script>alert('只有企业用户可以发布项目');window.location.href='Index.aspx';</script>");
                Response.End();
            }

            if (Request["action"]!=null&&Request["action"].Equals("edit"))
            {
                model = bll.Get<WBProjectInfo>(string.Format("AutoId={0}",Request["id"]));
                if (model!=null)
                {
                    if (!model.UserId.Equals(HttpContext.Current.Session[SessionKey.WanBangUserID].ToString()))
                    {
                        Response.End();
                    }
                    
                }

            }

        }
    }
}