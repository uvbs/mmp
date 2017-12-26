using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.WuBuHui.News
{
    public partial class MarketInterPreted : System.Web.UI.Page
    {
        public System.Text.StringBuilder sbCategory = new System.Text.StringBuilder();
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public string RootCategoryId = "240";
        public string MarketNewsIds = "237";
        public string MarketInterPretedids = "240";
        public string IsHaveUnReadMessage = "false";
        BLLJIMP.BLLSystemNotice bllNotice = new BLLJIMP.BLLSystemNotice();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //RootCategoryId = Request["id"];
                //if (Request.Url.Host.Equals("xixinxian.comeoncloud.net"))
                //{
                //    RootCategoryId = "169";
                //    MarketNewsIds = "165";
                //    MarketInterPretedids = "169";
                //}
                foreach (var item in bll.GetList<ArticleCategory>(string.Format("PreID={0}", RootCategoryId)))
                {
                    sbCategory.AppendLine(string.Format("<li class=\"catli\" categoryid=\"{0}\"><a href=\"javascript:void(0)\">{1}</a></li>", item.AutoID, item.CategoryName));
                }
                IsHaveUnReadMessage = bllNotice.IsHaveUnReadMessage(bllNotice.GetCurrentUserInfo().UserID).ToString();

            }
            catch (Exception)
            {

                Response.End();
            }
        }
    }
}