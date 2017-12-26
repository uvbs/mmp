using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.PubMgr
{
    public partial class WXMassArticleAddEdit : System.Web.UI.Page
    {
        public string webAction = "add";
        BLL bll = new BLL();
        public WXMassArticle model = new WXMassArticle();
        public string HeadTitle = "添加群发图文素材";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["id"]!=null)
            {
                webAction ="edit";
                model = bll.Get<WXMassArticle>(string.Format("AutoID={0}",Request["id"]));
                if (model == null)
                {
                    Response.End();
                }
                else
                {
                    HeadTitle =  model.Title;

                }
            }

        }
    }
}