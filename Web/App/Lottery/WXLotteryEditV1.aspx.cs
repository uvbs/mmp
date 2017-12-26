using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Lottery
{
    public partial class WXLotteryEditV1 : System.Web.UI.Page
    {
        BLL bll = new BLL("");
        public WXLotteryV1 model = new WXLotteryV1();
        public string backUrl = "WXLotteryMgrV1.aspx";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            model = this.bll.Get<WXLotteryV1>(string.Format("LotteryID={0}", Request["id"]));
            if (model == null)
            {
                Response.End();
            }
            string reqBackUrl = Request["BackUrl"];
            if (!string.IsNullOrWhiteSpace(reqBackUrl))
            {
                backUrl = HttpUtility.UrlDecode(reqBackUrl);
            }

        }
    }
}