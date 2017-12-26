using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.WebPc
{
    public partial class Page : System.Web.UI.Page
    {
        /// <summary>
        /// 页面模型
        /// </summary>
        protected PcPage model = new PcPage();
        /// <summary>
        /// 中部模型
        /// </summary>
        protected List<ZentCloud.BLLJIMP.ModelGen.PcPage.MiddModel> middList = new List<BLLJIMP.ModelGen.PcPage.MiddModel>();
        /// <summary>
        /// 
        /// </summary>
       protected BLLJIMP.BLL bll = new BLLJIMP.BLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageId = Request["pageId"];
            if (string.IsNullOrEmpty(pageId))
            {
                Response.Write("pageId 参数必传");
                Response.End();
            }
            model = bll.Get<PcPage>(string.Format("WebsiteOwner='{0}' And PageId={1}", bll.WebsiteOwner, pageId));
            if (model == null)
            {
                Response.Write("页面不存在");
                Response.End();
            }
            middList = ZentCloud.Common.JSONHelper.JsonToModel<List<ZentCloud.BLLJIMP.ModelGen.PcPage.MiddModel>>(model.MiddContent);


        }
    }
}