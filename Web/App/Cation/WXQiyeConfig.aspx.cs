using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation
{
    public partial class WXQiyeConfig : System.Web.UI.Page
    {
        BLLJIMP.BLLWeixin bll = new BLLJIMP.BLLWeixin("");
        /// <summary>
        /// 微信企业号配置信息
        /// </summary>
        public BLLJIMP.Model.WXQiyeConfig model;
        protected void Page_Load(object sender, EventArgs e)
        {
            model = bll.Get<BLLJIMP.Model.WXQiyeConfig>(string.Format("WebsiteOwner='{0}'",bll.WebsiteOwner));
            if (model==null)
            {
                model = new BLLJIMP.Model.WXQiyeConfig();
            }

        }
    }
}