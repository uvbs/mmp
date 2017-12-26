using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.customize.Shop
{
    public partial class WxSmsPay : System.Web.UI.Page
    {
        /// <summary>
        /// 站点名称
        /// </summary>
        public string userId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["userid"]))
            {
                userId = Request["userid"];
            }
        }
    }
}