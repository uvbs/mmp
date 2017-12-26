using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
    public partial class AddV : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL("");
        public StringBuilder sbCategory = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (var item in bll.GetList<WXAddVCategory>(string.Format("WebsiteOwner='{0}' order by CategoryValue ASC", bll.WebsiteOwner)))
            {
                sbCategory.AppendFormat("<option value=\"{0}\">{1}</option>",item.CategoryValue,item.CategoryKey);
            }
        }
    }
}