using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.App.MallManage
{
    public partial class WXMallCategoryMgr : System.Web.UI.Page
    {
        public ZentCloud.BLLJIMP.Model.WebsiteInfo currWebSiteInfo;
        BLLMall bllMall = new BLLMall();
        public string WXMallIndex = "Index.aspx";//商城主页
        protected void Page_Load(object sender, EventArgs e)
        {
            currWebSiteInfo = bllMall.GetWebsiteInfoModel();
            if (currWebSiteInfo!=null)
            {

                    if (currWebSiteInfo.MallTemplateId.Equals(1))//外卖
                    {
                        WXMallIndex = "IndexV2.aspx";
                    }
                
            }


        }

    }
}