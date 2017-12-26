using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Review
{
    public partial class ReviewList : System.Web.UI.Page
    {
        BLLJIMP.BLLWebSite bllWeisite = new BLLWebSite();
        //微信绑定域名
        public string strDomain = string.Empty;
        protected string ReviewName = "话题";
        protected string pFolder = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            WebsiteInfo model = bllWeisite.GetWebsiteInfo();
            if (model != null && !string.IsNullOrEmpty(model.WeiXinBindDomain))
            {
                strDomain = model.WeiXinBindDomain;
            }
            if (!string.IsNullOrEmpty(Request["Pfolder"])) ReviewName = "评论";
            if (!string.IsNullOrEmpty(Request["Pfolder"])) pFolder = Request["Pfolder"];
            
        }
    }
}