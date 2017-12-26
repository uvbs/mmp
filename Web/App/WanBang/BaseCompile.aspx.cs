using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.WanBang
{
    public partial class BaseCompile : System.Web.UI.Page
    {

       
        public string webAction = "add";
        public string actionStr = "";
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public WBBaseInfo model = new WBBaseInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            webAction = Request["Action"];
            actionStr = webAction == "add" ? "添加" : "编辑";
            if (webAction == "edit")
            {
                model = bll.Get<WBBaseInfo>(string.Format("AutoID={0}", Convert.ToInt32(Request["id"])));
                if (model == null)
                {
                    Response.End();
                }
                else
                {

                }
            }




        }
        
    }
}