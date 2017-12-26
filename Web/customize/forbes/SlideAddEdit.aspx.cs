using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model.Forbes;

namespace ZentCloud.JubitIMP.Web.customize.forbes
{
    public partial class SlideAddEdit : System.Web.UI.Page
    {
        public string webAction = "add";
        BLL bll = new BLL();
        public Slide model = new Slide();
        public string HeadTitle = "添加幻灯片";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["id"] != null)
            {
                webAction = "edit";
                model = bll.Get<Slide>(string.Format("AutoID={0}", Request["id"]));
                if (model == null)
                {
                    Response.End();
                }
                else
                {
                    HeadTitle ="编辑幻灯片";

                }
            }

        }

    }
}