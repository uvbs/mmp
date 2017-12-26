using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Wap.Mall
{
    public partial class List : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL("");
        public string WebsiteOwner = null;
        public System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        private string Width =null;
        protected void Page_Load(object sender, EventArgs e)
        {
            WebsiteOwner = Comm.DataLoadTool.GetWebsiteInfoModel().WebsiteOwner;
            var CategoryList=bll.GetLit<WXMallCategory>(3,1,string.Format("WebsiteOwner='{0}'", WebsiteOwner));
                if (CategoryList.Count.Equals(0))
                {
                    sbCategory.AppendFormat("<div style=\"width:100%;float:left;\" >全部</div>");
                }
                if (CategoryList.Count.Equals(1))
                {
                    Width = "100%";
                }
                if (CategoryList.Count.Equals(2))
                {
                    Width = "50%";
                }
                if (CategoryList.Count.Equals(3))
                {
                    Width = "33.33%";
                }
	        
       
                foreach (var item in CategoryList)
                {

                    sbCategory.AppendFormat("<div style=\"width:{0};float:left;\" data-categoryid=\"{1}\">{2}</div>", Width, item.AutoID, item.CategoryName);

                }
                

	        



        }
    }
}